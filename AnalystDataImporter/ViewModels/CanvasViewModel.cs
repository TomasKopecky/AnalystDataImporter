using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AnalystDataImporter.Factories;
using AnalystDataImporter.Globals;
using AnalystDataImporter.Managers;
using AnalystDataImporter.Models;
using AnalystDataImporter.Services;
using AnalystDataImporter.Utilities;

namespace AnalystDataImporter.ViewModels
{
    /// <summary>
    /// ViewModel pro plátno, které může obsahovat více prvků a poskytuje funkcionalitu pro přidání nových prvků.
    /// </summary>
    //TODO: If ElementViewModel has logic for moving the element on the canvas, consider adding bounds checking to ensure elements don't go outside the canvas or overlap with other elements.
    public class CanvasViewModel : INotifyPropertyChanged
    {
        private IMouseHandlingService _mouseHandlingService;

        public ICommand ChangeCursorWhenOperatingElementCommand { get; private set; }

        public ICommand FinishDrawingElemmentCommand { get; private set; }

        private object _selectedSingleItem;

        private RelationViewModel _selectedSingleRelation;

        public ICommand DeleteSelectionCommand { get; }

        public ObservableCollection<object> CanvasItems { get; } = new ObservableCollection<object>();

        public ObservableCollection<object> SelectedItems { get; } = new ObservableCollection<object>();

        private bool _isAddingElementOutsideCanvas = false;

        private bool _isMultipleSelectionActivated = false;

        private Canvas _canvasReference;

        private string _canvasCursor;

        private readonly bool _testingMode = true;

        private bool _mouseOnElement = false;

        private ElementViewModel _fromElement;
        private ElementViewModel _toElement;

        private RelationViewModel _tempRelation;

        private ElementViewModel _tempElement;

        // uvodni řešení, zasílání zráv mezi třídami
        //private readonly IMessageService _messageService;

        //private readonly IDrawnItemsModeService _elementModeService;

        // aktivace enabled vlastnosti obou tlačítek na začátku
        private bool _isRelationDrawingModeActive;

        private bool _isDrawingElementModeActive;

        private bool _isDraggingElementModeActive;

        //public RelayCommand<Point> MouseMovedDuringRelationDrawingCommand { get; private set; }
        public RelationViewModel TemporaryRelation { get; private set; }

        public ICommand CanvasMouseLeftButtonDownCommand { get; }
        public ICommand CanvasMouseMoveCommand { get; }
        public ICommand CanvasMouseLeftButtonUpCommand { get; private set; }

        private bool _isDrawingElement = false;

        //public enum DrawingRelationState
        //{
        //    NotDrawing,
        //    SelectingStart,
        //    SelectingEnd
        //}

        //public DrawingRelationState CurrentState { get; private set; } = DrawingRelationState.NotDrawing;

        public ElementViewModel StartingElement { get; private set; }

        /// <summary>
        /// Factory pro vytváření ViewModelů prvků element.
        /// </summary>
        private readonly IElementViewModelFactory _elementViewModelFactory;

        /// <summary>
        /// Factory pro vytváření ViewModelů prvků relation - vazba.
        /// </summary>
        private readonly IRelationViewModelFactory _relationViewModelFactory;

        /// <summary>
        /// Správce prvků pro manipulaci a získání informací o prvcích.
        /// </summary>
        private readonly IElementManager _elementManager;

        /// <summary>
        /// Správce prvků pro manipulaci a získání informací o prvcích.
        /// </summary>
        private readonly IRelationManager _relationManager;

        /// <summary>
        /// Kolekce všech ViewModelů prvků na plátně.
        /// </summary>
        public ObservableCollection<ElementViewModel> Elements => (ObservableCollection<ElementViewModel>)_elementManager.Elements;

        /// <summary>
        /// Kolekce vazeb na plátně.
        /// </summary>
        public ObservableCollection<RelationViewModel> Relations => (ObservableCollection<RelationViewModel>)_relationManager.Relations;

        public RelayCommand StartAddingElementCommand { get; private set; }

        public RelayCommand<object> CanvasClickedCommand { get; private set; }

        /// <summary>
        /// Příkaz pro zahájení kreslení vazby.
        /// </summary>
        public ICommand StartRelationCreatingCommand { get; private set; }

        /// <summary>
        /// Příkaz pro zahájení kreslení vazby.
        /// </summary>
        public ICommand AddRelationCommand { get; private set; }

        /// <summary>
        /// Konstruktor třídy CanvasViewModel. Přijímá závislosti potřebné pro tuto třídu.
        /// </summary>
        /// <param name="elementViewModelFactory">Factory pro vytváření ViewModelů prvků.</param>
        /// <param name="elementManager">Správce prvků pro manipulaci a získání informací o prvcích.</param>
        /// <param name="relationViewModelFactory"></param>
        /// <param name="relationManager"></param>
        public CanvasViewModel(IElementViewModelFactory elementViewModelFactory, IElementManager elementManager, IRelationViewModelFactory relationViewModelFactory, IRelationManager relationManager, IMouseHandlingService mouseHandlingService/*IDrawnItemsModeService elementModeService*/ /*IMessageService messageService*/)
        {

            _elementViewModelFactory = elementViewModelFactory ?? throw new ArgumentNullException(nameof(elementViewModelFactory));
            _elementManager = elementManager ?? throw new ArgumentNullException(nameof(elementManager));
            _relationViewModelFactory = relationViewModelFactory ?? throw new ArgumentNullException(nameof(relationViewModelFactory));
            _relationManager = relationManager ?? throw new ArgumentNullException(nameof(relationManager));
            _mouseHandlingService = mouseHandlingService ?? throw new ArgumentNullException(nameof(mouseHandlingService));

            StartAddingElementCommand = new RelayCommand(StartAddingElement, CanStartAddingElement);
            StartRelationCreatingCommand = new RelayCommand(ToggleRelationDrawingMode, CanToggleRelationDrawingMode);
            CanvasMouseLeftButtonDownCommand = new RelayCommand<object>(CanvasMouseLeftButtonDownExecute);
            CanvasMouseMoveCommand = new RelayCommand<object>(CanvasMouseMoveExecute);
            CanvasMouseLeftButtonUpCommand = new RelayCommand<object>(CanvasMouseLeftButtonUpExecute);

            DeleteSelectionCommand = new RelayCommand(DeleteSelectionExecute);

            // uvodní řešení ro sdílení aktivních modu v samostatne service třídě, tedy kdy je aktivní mod kreslení vazby, atd.
            //_elementModeService = elementModeService;
            //_elementModeService.ElementOrRelationModeChanged += OnElementModeChanged;

            Elements.CollectionChanged += (s, e) => OnCollectionChanged(e, CanvasItems);
            Relations.CollectionChanged += (s, e) => OnCollectionChanged(e, CanvasItems);
            ChangeCursorWhenOperatingElementCommand = new RelayCommand<string>(ChangeCursorByElemment);
            FinishDrawingElemmentCommand = new RelayCommand(FinishElementDrawing);

            // uvodni řešení, zasílání zráv mezi třídami
            //_messageService = messageService;
            //_messageService.Register<ElementMessage>(this, OnElementViewModelReceived);

            if (_testingMode)
                AddTestingElementsAndRelation();
        }

        private void FinishElementDrawing()
        {
            Debug.WriteLine("CanvasViewModel: MouseButtoDown - Adding new element to canvas");
            _tempElement.FinishTempElement();
            _tempElement = null;
            IsDrawingElement = false;
            IsDrawingElementModeActive = false;
            IsDraggingElementModeActive = true;
            OnPropertyChanged(nameof(CanvasCursor));
        }

        private void ChangeCursorByElemment(string operation)
        {
            if (operation == "EllipseDraggingCursor")
            {
                _isAddingElementOutsideCanvas = false;
                _mouseOnElement = true;
            }

            else if (operation == "EllipseDrawingInsideCanvasCursor")
                _isAddingElementOutsideCanvas = false;

            else if (operation == "EllipseDrawingOutsideCanvasCursor")
                _isAddingElementOutsideCanvas = true;

            OnPropertyChanged(nameof(CanvasCursor));
        }

        private void DeleteSelectionExecute()
        {
            if (!_isMultipleSelectionActivated)
            {
                if (SelectedSingleItem != null)
                {

                    if (SelectedSingleItem is ElementViewModel element)
                    {
                        IList<RelationViewModel> connectedRelations = _relationManager.GetRelationsConnectedToElement(element);
                        foreach (RelationViewModel connectedRelation in connectedRelations)
                        {
                            _relationManager.Relations.Remove(connectedRelation);
                        }
                        _elementManager.Elements.Remove(element);
                        // TODO: Linq, když je tento element v objetfrom nebo objectto na nějake vazbě, tak ji taky smaž
                    }
                    if (SelectedSingleItem is RelationViewModel relation)
                    {
                        _relationManager.Relations.Remove(relation);
                        // TODO: Linq, když je tento element v objetfrom nebo objectto na nějake vazbě, tak ji taky smaž
                    }
                    SelectedSingleItem = null;
                }
            }
        }



        public object SelectedSingleItem
        {
            get => _selectedSingleItem;
            set
            {
                if (_selectedSingleItem != value)
                {
                    _selectedSingleItem = value;
                    OnPropertyChanged(nameof(SelectedSingleItem));
                }
            }
        }

        public RelationViewModel SelectedSingleRelation
        {
            get => _selectedSingleRelation;
            set
            {
                if (_selectedSingleRelation != value)
                {
                    _selectedSingleRelation = value;
                    OnPropertyChanged(nameof(SelectedSingleRelation));
                }
            }
        }

        private void ReleaseSelection()
        {
            foreach (var item in CanvasItems)
            {
                if (item is ElementViewModel element)
                {
                    element.IsSelected = false;
                }

                if (item is RelationViewModel relation)
                {
                    relation.IsSelected = false;
                }

                SelectedSingleItem = null;
            }
        }

        private void RelationOrElementViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsSelected") return;

            if (!(sender is BaseDiagramItemViewModel viewModel)) return;

            // Handle the IsSelected property change
            if (viewModel.IsSelected)
            {
                HandleSelection(viewModel);
            }
            else
            {
                HandleDeselection(viewModel);
            }
        }

        private void HandleSelection(BaseDiagramItemViewModel viewModel)
        {
            if (!SelectedItems.Contains(viewModel))
            {
                if (!_isMultipleSelectionActivated && SelectedItems.Any())
                {
                    DeselectAll();
                }
                SelectedItems.Add(viewModel);
                SelectedSingleItem = viewModel;
            }
        }

        private void HandleDeselection(BaseDiagramItemViewModel viewModel)
        {
            if (SelectedItems.Contains(viewModel))
            {
                SelectedItems.Remove(viewModel);
                if (SelectedSingleItem == viewModel)
                {
                    SelectedSingleItem = null;
                }
            }
        }

        private void DeselectAll()
        {
            foreach (var item in SelectedItems.ToList())
            {
                if (item is BaseDiagramItemViewModel baseViewModel)
                {
                    baseViewModel.IsSelected = false;
                }
            }
            SelectedItems.Clear();
            SelectedSingleItem = null;
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e, ObservableCollection<object> canvasItems)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        if (item is BaseDiagramItemViewModel baseDiagramItemViewModel)
                        {
                            baseDiagramItemViewModel.PropertyChanged += RelationOrElementViewModel_PropertyChanged;
                        }
                        canvasItems.Add(item);
                        Debug.WriteLine("Adding object to the canvasCollection: " + item.ToString());
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        if (item is BaseDiagramItemViewModel baseDiagramItemViewModel)
                        {
                            baseDiagramItemViewModel.PropertyChanged -= RelationOrElementViewModel_PropertyChanged;
                        }
                        canvasItems.Remove(item);
                    }
                    break;

                    // Handle other cases if necessary
            }
        }

        private void AddTestingElementsAndRelation()
        {
            ElementViewModel fromElement = AddTestingElementToCanvas(new Point(200, 200), "First element", "identita není nastavena");
            ElementViewModel toElement = AddTestingElementToCanvas(new Point(400, 100), "Second element", "identita není");
            AddTestingElementToCanvas(new Point(2, 2), "Third element", "identita");
            AddTestingRelation(fromElement, toElement);
            IsDraggingElementModeActive = true;
        }

        public Canvas CanvasReference
        {
            get { return _canvasReference; }
            set
            {
                _canvasReference = value;
                OnPropertyChanged(nameof(CanvasReference));
            }
        }

        private bool IsTheOperationInsideCanvas(Point mousePosition, Canvas canvas)
        {
            // Perform the hit test.
            var hitTestResult = VisualTreeHelper.HitTest(canvas, mousePosition);
            Debug.WriteLine("hitresult first: " + hitTestResult?.VisualHit);

            // If nothing was hit, we're not over the canvas.
            if (hitTestResult == null)
                return false;

            // Check if the hit visual is the canvas itself.
            if (hitTestResult.VisualHit == canvas)
                return true;

            // Walk up the visual tree to find if the canvas is a parent of the hit visual.
            DependencyObject current = hitTestResult.VisualHit;
            while (current != null)
            {
                //Debug.WriteLine("hitresult second: " + current);
                if (current == canvas)
                    return true;

                current = VisualTreeHelper.GetParent(current);
            }

            // If we walked up the tree and didn't find the canvas, we're not over it.
            return false;
        }

        private ElementViewModel GetElementUnderMousePointerNew(Point mousePosition, Canvas canvas)
        {
            ElementViewModel foundElement = null;
            HitTestFilterBehavior FilterCallback(DependencyObject potentialHitTestTarget)
            {
                if (potentialHitTestTarget is Line)
                {
                    // Skip the line
                    return HitTestFilterBehavior.ContinueSkipSelf;
                }
                return HitTestFilterBehavior.Continue;
            }

            HitTestResultBehavior ResultCallback(HitTestResult result)
            {
                if (result.VisualHit is Ellipse ellipse)
                {
                    foundElement = ellipse.DataContext as ElementViewModel;
                    return HitTestResultBehavior.Stop;
                }
                return HitTestResultBehavior.Continue;
            }

            VisualTreeHelper.HitTest(
                canvas,
                new HitTestFilterCallback(FilterCallback),
                new HitTestResultCallback(ResultCallback),
                new PointHitTestParameters(mousePosition));

            Debug.WriteLine("New Hit test result: " + foundElement);
            return foundElement;
        }

        private (UIElement, ElementViewModel) GetElementUnderMousePointer(Point mousePosition, Canvas canvas)
        {
            var hitTestResult = VisualTreeHelper.HitTest(canvas, mousePosition);
            Debug.WriteLine("Hit Test coordinates: " + mousePosition);
            if (hitTestResult?.VisualHit is Ellipse ellipse)
            {
                ElementViewModel elementViewModel = ellipse.DataContext as ElementViewModel;
                Debug.WriteLine("Hit Test Ellise ");
                Debug.WriteLine("Starting coordinates: " + elementViewModel.XPosition + ", " + elementViewModel.YPosition);
                Debug.WriteLine("Ending coordinates: " + (elementViewModel.XPosition + ellipse.Width) + ", " + (elementViewModel.YPosition + ellipse.Height));
                return (ellipse, elementViewModel);
            }
            else
            {
                Debug.WriteLine("Hit Test Not Ellise: " + hitTestResult?.VisualHit);
            }

            return (null, null);
        }

        private void AddNewElementToCanvas(Point mousePosition)
        {
            Debug.WriteLine("CanvasViewModel: Adding new element to canvas");

            _tempElement = CreateNewElement(mousePosition); // A method that creates and configures the new element
            _elementManager.AddElement(_tempElement); // Assuming an element manager or similar mechanism
            //_mouseHandlingService.StartDragWhenDrawingOperation(mousePosition,_tempElement,true);
            //_tempElement = null;
            //SetAddingElementMode(false); // Consolidate state updates
        }

        private ElementViewModel CreateNewElement(Point position)
        {
            // Create and configure the new element
            var element = _elementViewModelFactory.Create();
            element.ConfigureTempElement(position.X, position.Y);
            return element;
        }


        public bool IsRelationDrawingModeActive
        {
            get => _isRelationDrawingModeActive;
            set
            {
                if (_isRelationDrawingModeActive != value)
                {
                    _isRelationDrawingModeActive = value;
                    OnPropertyChanged(nameof(IsRelationDrawingModeActive));
                }
            }
        }

        public bool IsDraggingElementModeActive
        {
            get => _isDraggingElementModeActive;
            set
            {
                if (_isDraggingElementModeActive != value)
                {
                    _isDraggingElementModeActive = value;
                    OnPropertyChanged(nameof(IsDraggingElementModeActive));
                }
            }
        }

        public bool IsDrawingElement
        {
            get => _isDrawingElement;
            set
            {
                if (_isDrawingElement != value)
                {
                    _isDrawingElement = value;
                    OnPropertyChanged(nameof(IsDrawingElement));
                }
            }
        }

        public bool IsDrawingElementModeActive
        {
            get => _isDrawingElementModeActive;
            set
            {
                if (_isDrawingElementModeActive != value)
                {
                    _isDrawingElementModeActive = value;
                    OnPropertyChanged(nameof(IsDrawingElementModeActive));
                }
            }
        }

        private void CanvasMouseLeftButtonDownExecute(object parameter)
        {
            Debug.WriteLine("CanvasViewModel: MouseButtoDown");
            if (!(parameter is Canvas canvas))
            {
                ReleaseSelection();
                return;
            }

            Point mousePosition = Mouse.GetPosition(canvas);

            bool isTheClickInsideWindow = IsTheOperationInsideCanvas(mousePosition, canvas);

            if (!isTheClickInsideWindow) return;

            else if (IsRelationDrawingModeActive)
            {
                Debug.WriteLine("CanvasViewModel: ButtonDown drawing");
                Debug.WriteLine("CanvasViewModel: ButtonDown drawing coordinates: " + mousePosition);
                //var hitTestResult = VisualTreeHelper.HitTest(canvas, mousePosition);
                ElementViewModel element = GetElementUnderMousePointer(mousePosition, canvas).Item2;
                if (element != null)
                {
                    _fromElement = element as ElementViewModel;
                    //AddStartRelation(mousePosition);
                    _tempRelation = _relationViewModelFactory.Create(mousePosition, mousePosition);
                    _tempRelation.IsFinished = false;
                    _tempRelation.ZIndex = 2;
                    _relationManager.AddRelation(_tempRelation);
                    //CanvasItems.Add(tempRelation);
                    Debug.WriteLine("CanvasViewModel: AddStartRelation");
                    canvas.CaptureMouse();
                }
                else
                {
                    IsRelationDrawingModeActive = false;
                    IsDraggingElementModeActive = true;
                    OnPropertyChanged(nameof(IsDraggingElementModeActive));
                    OnPropertyChanged(nameof(IsRelationDrawingModeActive));
                    OnPropertyChanged(nameof(CanvasCursor));
                }
            }
            else
            {
                ReleaseSelection();
            }
        }

        private void CanvasMouseMoveExecute(object parameter)
        {
            //Debug.WriteLine("CanvasViewModel: Mouse move");

            if (!(parameter is Canvas canvas)) return;

            //Canvas canvas = (Canvas)parameter;

            Point mousePosition = Mouse.GetPosition(canvas);

            if (IsDraggingElementModeActive)
            {
                _mouseOnElement = false;
                OnPropertyChanged(nameof(CanvasCursor));
            }

            // Dodána kontrola, jestli je mouse point v canvas, protože jinak zakládal elipsu (elementViewModel) i při pohybu přes jinou elipsu, která přečnívá přes okraj canvas 
            if (IsDrawingElementModeActive && _mouseHandlingService.IsMouseInCanvas(mousePosition,canvas))
            {
                if (_tempElement == null)
                {
                    Debug.WriteLine("CanvasViewModel: Mouse Move - Adding element");
                    IsDrawingElement = true;
                    OnPropertyChanged(nameof(CanvasCursor));
                    AddNewElementToCanvas(mousePosition);
                    Debug.WriteLine("CanvasViewModel: Now elements count: " + _elementManager.Elements.Count);
                }
            }

            if (IsRelationDrawingModeActive && canvas.IsMouseCaptured)
            {
                Debug.WriteLine("CanvasViewModel: Mouse move - getting _toElement under pointer");
                //_toElement = GetElementUnderMousePointer(mousePosition, canvas);
                //_toElement = GetElementUnderMousePointerNew(mousePosition, canvas);
                if (_fromElement != null && _tempRelation != null)
                {
                    // Update the endpoint of the current relation line to the current mouse position
                    _tempRelation.X2 = mousePosition.X;
                    _tempRelation.Y2 = mousePosition.Y;

                    // Notify the UI that the properties have changed so it can update the line's position
                    OnPropertyChanged(nameof(_tempRelation.X2));
                    OnPropertyChanged(nameof(_tempRelation.Y2));
                }

                //UpdateLineEndPoint(currentPoint); // Update the endpoint of the line being drawn
            }
        }

        public void CanvasMouseLeftButtonUpExecute(object parameter)
        {
            Debug.WriteLine("CanvasViewModel: MouseButtoUp");
            if ((IsDrawingElementModeActive == false && IsRelationDrawingModeActive == false) || !(parameter is Canvas canvas)) return;

            Point mousePosition = Mouse.GetPosition(canvas);

            if (IsDrawingElementModeActive)
            {
                //_mouseHandlingService.CheckDraggingDrawnElementOutsideCanvas();
            }

            if (IsRelationDrawingModeActive)
            {
                _toElement = GetElementUnderMousePointerNew(mousePosition, canvas);
                Debug.WriteLine("CanvasViewModel: ButtonUp drawing");
                //Point mousePosition = Mouse.GetPosition(canvas);
                //var hitTestResult = VisualTreeHelper.HitTest(canvas, mousePosition);
                //ElementViewModel element = GetElementUnderMousePointer(mousePosition, canvas);
                if (_fromElement != null && _toElement != null && _fromElement != _toElement && !RelationAlreadyExists(_fromElement, _toElement))
                {
                    //_toElement = element;
                    Debug.WriteLine("CanvasViewModel: FinishRelation");
                    _tempRelation.ObjectFrom = _fromElement;
                    _tempRelation.ObjectTo = _toElement;
                    _tempRelation.ZIndex = 0;
                    _tempRelation.Title = "vazba";
                    _tempRelation.IsFinished = true;
                }
                else
                {
                    _relationManager.Relations.RemoveAt(_relationManager.Relations.Count - 1);
                }
                IsRelationDrawingModeActive = false;
                IsDraggingElementModeActive = true;
                OnPropertyChanged(nameof(IsDraggingElementModeActive));
                OnPropertyChanged(nameof(IsRelationDrawingModeActive));
                OnPropertyChanged(nameof(CanvasCursor));
                _fromElement = null;
                _toElement = null;
                canvas.ReleaseMouseCapture();
            }
        }

        public bool RelationAlreadyExists(ElementViewModel fromElement, ElementViewModel toElement)
        {
            return Relations.Any(relation =>
                (relation.ObjectFrom == fromElement && relation.ObjectTo == toElement) ||
                (relation.ObjectFrom == toElement && relation.ObjectTo == fromElement)
            );
        }

        // Událost, která se vyvolá při změně vlastnosti
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Metoda pro oznamování změn vlastností.
        /// </summary>
        /// <param name="propertyName">Jméno vlastnosti, která se změnila.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string CanvasCursor
        {
            get
            {
                if (IsDrawingElement)
                {
                    if (_isAddingElementOutsideCanvas)
                        _canvasCursor = "Arrow";
                    else
                        _canvasCursor = "None";
                }
                else if (IsRelationDrawingModeActive)
                {
                    _canvasCursor = "Cross";
                }
                else if (_mouseOnElement)
                {
                    _canvasCursor = "SizeAll";
                }
                else
                    _canvasCursor = "Arrow";

                //Debug.WriteLine("Getting cursor: " + _canvasCursor);
                return _canvasCursor;
                //return IsRelationDrawingModeActive ? "Cross" : "Arrow";
            }
            set
            {
                _canvasCursor = value;
                OnPropertyChanged(nameof(CanvasCursor));
            }
        }

        private bool CanToggleRelationDrawingMode()
        {
            return true; // You can adjust this logic later based on your requirements
        }

        private bool CanStartAddingElement()
        {
            return true; // You can adjust this logic later based on your requirements
        }

        private void ToggleRelationDrawingMode()
        {
            // TODO: všechny metody IsRelationDrawingModeActive, IsAddingElementModeActive, atd. obsluhovat z _elementModeService
            //_elementModeService.IsAddingRelationModeActive = true;

            //releaseSelection();
            IsRelationDrawingModeActive = true; // Set to true when this method is called // Trigger OnPropertyChanged for all properties that depend on this mode
            IsDrawingElementModeActive = false;
            IsDraggingElementModeActive = false;
            //IsAddingElement = false;
            //OnPropertyChanged(nameof(IsRelationDrawingModeActive));
            //OnPropertyChanged(nameof(IsDrawingElementModeActive));
            //OnPropertyChanged(nameof(IsDraggingElementModeActive));
            OnPropertyChanged(nameof(CanvasCursor)); // Notify that the cursor might need to change
        }

        private void StartAddingElement()
        {
            //_elementModeService.IsAddingElementModeActive = true; // This will notify all subscribers
            //IsAddingElement = true;
            ReleaseSelection();
            IsDrawingElementModeActive = true;
            IsRelationDrawingModeActive = false;
            IsDraggingElementModeActive = false;
            //OnPropertyChanged(nameof(IsDrawingElementModeActive));
            //OnPropertyChanged(nameof(IsDraggingElementModeActive));
            //OnPropertyChanged(nameof(IsRelationDrawingModeActive));
            //OnPropertyChanged(nameof(CanvasCursor)); // Notify that the cursor might need to change
        }

        private ElementViewModel AddTestingElementToCanvas(Point position, string title, string label)
        {
            var newElement = _elementViewModelFactory.Create();
            //newElement.temporary = true;
            newElement.XPosition = position.X;
            newElement.YPosition = position.Y;
            newElement.Title = title;
            newElement.ZIndex = 1;
            newElement.Label = label;
            //newElement.temporary = false;
            _elementManager.AddElement(newElement);
            //CanvasItems.Add(newElement);
            return newElement;
        }

        public void AddTestingRelation(ElementViewModel fromElement, ElementViewModel toElement)
        {
            //RelationViewModel relationViewModel = _relationViewModelFactory.Create(_fromElement, _toElement);
            RelationViewModel relationViewModel = _relationViewModelFactory.Create(new Point(fromElement.XCenter, fromElement.YCenter), new Point(toElement.XCenter, toElement.YCenter));
            //relationViewModel.IsFinished = true;
            _relationManager.AddRelation(relationViewModel);
            relationViewModel.ZIndex = 0;
            relationViewModel.Title = "vazba";
            relationViewModel.ObjectFrom = fromElement;
            relationViewModel.ObjectTo = toElement;
            relationViewModel.IsFinished = true;

            // TODO: Po přidání vazby se nepřipojí do středu elipsy - někde bude problém v nastavení XCenter a YCenter ElementViewModelu
        }
    }
}