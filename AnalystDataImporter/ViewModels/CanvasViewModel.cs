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
using AnalystDataImporter.Factories;
using AnalystDataImporter.Managers;
using AnalystDataImporter.Services;
using AnalystDataImporter.Utilities;

namespace AnalystDataImporter.ViewModels
{
    /// <summary>
    ///     ViewModel pro plátno, které může obsahovat více prvků a poskytuje funkcionalitu pro přidání nových prvků.
    /// </summary>
    public class CanvasViewModel : INotifyPropertyChanged
    {
        #region Fields
        // Deklarace soukromých polí
        private readonly IElementManager _elementManager;
        private readonly IElementViewModelFactory _elementViewModelFactory;
        private readonly IRelationManager _relationManager;
        private readonly IRelationViewModelFactory _relationViewModelFactory;
        private readonly IMouseHandlingService _mouseHandlingService;
        private readonly MouseCursorService _mouseCursorService;
        private readonly SharedStatesService _sharedStateService;
        private readonly SharedCanvasPageItems _sharedCanvasPageItems;

        //private string _canvasCursor;
        private ElementViewModel _fromElement;
        //private bool _sharedStateService.IsAddingElementOutsideCanvas;
        //private bool _sharedStateService.IsDraggingElementModeActive;
        //private bool __sharedStateService.IsDrawingElement;
        //private bool __sharedStateService.IsDrawingElementModeActive;
        //private bool _isRelationDrawingModeActive;
        //private bool _sharedStateService.MouseOnElement;
        private object _selectedSingleItem;
        private readonly bool _isMultipleSelectionActivated;
        private ElementViewModel _tempElement;
        private RelationViewModel _tempRelation;
        #endregion

        #region Constructor
        // Konstruktor
        public CanvasViewModel(IElementViewModelFactory elementViewModelFactory, IElementManager elementManager,
            IRelationViewModelFactory relationViewModelFactory, IRelationManager relationManager,
            IMouseHandlingService mouseHandlingService, MouseCursorService mouseCursorService, SharedStatesService sharedStateService, SharedCanvasPageItems sharedCanvasPageItems)
        {
            // Inicializace proměnných a závislostí
            _elementViewModelFactory = elementViewModelFactory ?? throw new ArgumentNullException(nameof(elementViewModelFactory));
            _elementManager = elementManager ?? throw new ArgumentNullException(nameof(elementManager));
            _relationViewModelFactory = relationViewModelFactory ?? throw new ArgumentNullException(nameof(relationViewModelFactory));
            _relationManager = relationManager ?? throw new ArgumentNullException(nameof(relationManager));
            _mouseHandlingService = mouseHandlingService ?? throw new ArgumentNullException(nameof(mouseHandlingService));
            _mouseCursorService = mouseCursorService ?? throw new ArgumentNullException(nameof(mouseCursorService));
            _mouseCursorService = mouseCursorService;
            _sharedStateService = sharedStateService;
            _sharedCanvasPageItems = sharedCanvasPageItems;
            InitializeCommands();

            CanvasItems = new ObservableCollection<object>();
            SelectedItems = new ObservableCollection<object>();

            Elements.CollectionChanged += (s, e) => OnCollectionChanged(e, CanvasItems);
            Relations.CollectionChanged += (s, e) => OnCollectionChanged(e, CanvasItems);

            _isMultipleSelectionActivated = false;
            _sharedStateService.IsDraggingElementModeActive = true;
            TestingMode = false;
        }
        #endregion

        #region Properties
        // Vlastnosti
        public ICommand ChangeCursorWhenOperatingElementCommand { get; private set; }
        public ICommand FinishDrawingElementCommand { get; private set; }
        public ICommand GetRelationStartOrEndElementCommand { get; private set; }
        public ICommand DeleteSelectionCommand { get; private set; }
        public ObservableCollection<object> CanvasItems { get; }
        public ObservableCollection<object> SelectedItems { get; }
        public ICommand CanvasMouseLeftButtonDownCommand { get; private set; }
        public ICommand CanvasMouseMoveCommand { get; private set; }
        public ICommand RelationDeleteWhenOutsideCanvasCommand { get; private set; }
        public ICommand CanvasMouseLeftButtonUpCommand { get; private set; }
        public ICommand StartAddingElementCommand { get; private set; }
        public ICommand StartRelationCreatingCommand { get; private set; }
        public ICommand GetGridViewColumnDraggedSetCommand { get; private set; }
        public ObservableCollection<ElementViewModel> Elements => (ObservableCollection<ElementViewModel>)_elementManager.Elements;
        public ObservableCollection<RelationViewModel> Relations => (ObservableCollection<RelationViewModel>)_relationManager.Relations;
        public bool TestingMode { get; set; }
        #endregion

        private void InitializeCommands()
        {
            StartAddingElementCommand = new RelayCommand(StartAddingElement);
            StartRelationCreatingCommand = new RelayCommand(StartRelationCreation);
            CanvasMouseLeftButtonDownCommand = new RelayCommand<object>(CanvasMouseLeftButtonDownExecute);
            CanvasMouseMoveCommand = new RelayCommand<object>(CanvasMouseMoveExecute);
            CanvasMouseLeftButtonUpCommand = new RelayCommand<object>(CanvasMouseLeftButtonUpExecute);
            GetRelationStartOrEndElementCommand = new RelayCommand<object>(StartOrFinishDrawingRelation);
            DeleteSelectionCommand = new RelayCommand(DeleteSelectionExecute);
            ChangeCursorWhenOperatingElementCommand = new RelayCommand<string>(ChangeCursorByElement);
            FinishDrawingElementCommand = new RelayCommand(FinishElementDrawing);
            RelationDeleteWhenOutsideCanvasCommand = new RelayCommand(RemoveRelationWhenDrawnOutsideCanvas);
            GetGridViewColumnDraggedSetCommand = new RelayCommand<object>(DropGridColumnToElementInCanvas);
        }

        public object SelectedSingleItem
        {
            get => _selectedSingleItem;
            set
            {
                if (_selectedSingleItem == value) return;
                _selectedSingleItem = value;
                OnPropertyChanged(nameof(SelectedSingleItem));
            }
        }

        //public bool _sharedStateService.IsDrawingRelationModeActive
        //{
        //    get => _isRelationDrawingModeActive;
        //    set
        //    {
        //        if (_isRelationDrawingModeActive == value) return;
        //        _isRelationDrawingModeActive = value;
        //        OnPropertyChanged(nameof(_sharedStateService.IsDrawingRelationModeActive));
        //    }
        //}

        //public bool IsDraggingElementModeActive
        //{
        //    get => _sharedStateService.IsDraggingElementModeActive;
        //    set
        //    {
        //        if (_sharedStateService.IsDraggingElementModeActive == value) return;
        //        _sharedStateService.IsDraggingElementModeActive = value;
        //        OnPropertyChanged(nameof(IsDraggingElementModeActive));
        //    }
        //}

        //public bool _sharedStateService.IsDrawingElement
        //{
        //    get => __sharedStateService.IsDrawingElement;
        //    set
        //    {
        //        if (__sharedStateService.IsDrawingElement == value) return;
        //        __sharedStateService.IsDrawingElement = value;
        //        OnPropertyChanged(nameof(_sharedStateService.IsDrawingElement));
        //    }
        //}

        //public bool _sharedStateService.IsDrawingElementModeActive
        //{
        //    get => __sharedStateService.IsDrawingElementModeActive;
        //    set
        //    {
        //        if (__sharedStateService.IsDrawingElementModeActive == value) return;
        //        __sharedStateService.IsDrawingElementModeActive = value;
        //        OnPropertyChanged(nameof(_sharedStateService.IsDrawingElementModeActive));
        //    }
        //}

        public Cursor CanvasCursor
        {
            get => _mouseCursorService.CurrentCursor;
            private set
            {
                if (_mouseCursorService.CurrentCursor != value)
                    //{
                    _mouseCursorService.UpdateCursor();
                //_mouseCursorService.UpdateCursorForCanvas(_sharedStateService.IsDrawingElement, _sharedStateService.IsDrawingRelationModeActive, _sharedStateService.MouseOnElement, _sharedStateService.IsAddingElementOutsideCanvas);
                //CanvasCursor = null;
            }

            //get
            //{
            //    if (_sharedStateService.IsDrawingElement)
            //        _canvasCursor = _sharedStateService.IsAddingElementOutsideCanvas ? "Arrow" : "None";
            //    else if (_sharedStateService.IsDrawingRelationModeActive)
            //        _canvasCursor = "Cross";
            //    else if (_sharedStateService.MouseOnElement)
            //        _canvasCursor = "SizeAll";
            //    else if (_mouseOnGridView)
            //        _canvasCursor = "Hand";
            //    else
            //        _canvasCursor = "Arrow";

            //    //Debug.WriteLine("Getting cursor: " + _canvasCursor);
            //    return _canvasCursor;
            //}
            //set
            //{
            //    _canvasCursor = value;
            //    CanvasCursor = null;
            //}
        }

        // Událost, která se vyvolá při změně vlastnosti - pro binding
        public event PropertyChangedEventHandler PropertyChanged;

        #region Test metody

        /// <summary>
        /// Metoda pro přidání testovacích prvků a vazeb na plátno.
        /// </summary>
        public void AddTestingElementsAndRelation()
        {
            if (!TestingMode) return;
            var fromElement =
                AddTestingElementToCanvas(new Point(200, 200), "First element", "identita není nastavena");
            var toElement = AddTestingElementToCanvas(new Point(400, 100), "Second element", "identita není");
            AddTestingElementToCanvas(new Point(2, 2), "Third element", "identita");
            AddTestingRelationToCanvas(fromElement, toElement);
            _sharedStateService.IsDraggingElementModeActive = true;
        }

        /// <summary>
        /// Metoda pro přidání testovacího prvku na plátno.
        /// </summary>
        /// <param name="position">Pozice, kam prvek přidat.</param>
        /// <param name="title">Název prvku.</param>
        /// <param name="label">Popisek prvku.</param>
        /// <returns>Vytvořený testovací prvek.</returns>
        private ElementViewModel AddTestingElementToCanvas(Point position, string title, string label)
        {
            var newElement = _elementViewModelFactory.Create();
            newElement.XPosition = position.X;
            newElement.YPosition = position.Y;
            newElement.Title = title;
            newElement.ZIndex = 1;
            newElement.Label = label;
            _elementManager.AddElement(newElement);
            return newElement;
        }

        /// <summary>
        /// Metoda pro přidání testovací vazby mezi prvky na plátno.
        /// </summary>
        /// <param name="fromElement">Prvek, od kterého vazba začíná.</param>
        /// <param name="toElement">Prvek, ke kterému vazba směřuje.</param>
        private void AddTestingRelationToCanvas(ElementViewModel fromElement, ElementViewModel toElement)
        {
            var relationViewModel = _relationViewModelFactory.Create(
                new Point(fromElement.XCenter, fromElement.YCenter), new Point(toElement.XCenter, toElement.YCenter));
            relationViewModel.ZIndex = 0;
            relationViewModel.Title = "vazba";
            relationViewModel.ObjectFrom = fromElement;
            relationViewModel.ObjectTo = toElement;
            relationViewModel.IsFinished = true;
            _relationManager.AddRelation(relationViewModel);
        }

        #endregion

        /// <summary>
        /// Metoda pro ukončení kreslení prvku.
        /// </summary>
        private void FinishElementDrawing()
        {
            _tempElement.FinishTempElement();
            _tempElement = null;
            _sharedStateService.IsDrawingElement = false;
            _sharedStateService.IsDrawingElementModeActive = false;
            _sharedStateService.IsDraggingElementModeActive = true;

            //CanvasCursor = null;
        }

        private void FinishElementDrawingFromGridColumn()
        {
            _tempElement.FinishElementFromGridView();
            _tempElement = null;
            _sharedStateService.IsDrawingElement = false;
            _sharedStateService.IsDrawingElementModeActive = false;
            _sharedStateService.IsDraggingGridColumnModeActive = false;
            _sharedStateService.IsDraggingElementModeActive = true;
            _sharedCanvasPageItems.TableColumn = null;

            //CanvasCursor = null;
        }

        /// <summary>
        /// Metoda pro změnu kurzoru při operaci s prvkem.
        /// </summary>
        /// <param name="operation">Typ operace, která určuje, jaký kurzor zobrazit.</param>

        private void ChangeCursorByElement(string operation)
        {
            switch (operation)
            {
                case "EllipseDraggingCursor":
                    _sharedStateService.IsAddingElementOutsideCanvas = false;
                    _sharedStateService.MouseOnElement = true;
                    break;
                case "EllipseDrawingInsideCanvasCursor":
                    _sharedStateService.IsAddingElementOutsideCanvas = false;
                    _sharedStateService.MouseOnElement = false;
                    break;
                case "EllipseDrawingOutsideCanvasCursor":
                    _sharedStateService.IsAddingElementOutsideCanvas = true;
                    _sharedStateService.MouseOnElement = false;
                    break;
            }

            CanvasCursor = null;
            //CanvasCursor = null;
        }

        /// <summary>
        /// Metoda pro odstranění vybraných prvků.
        /// </summary>
        private void DeleteSelectionExecute()
        {
            if (_isMultipleSelectionActivated) return;
            switch (SelectedSingleItem)
            {
                case null:
                    return;
                case ElementViewModel element:
                    {
                        IList<RelationViewModel> connectedRelations = _relationManager.GetRelationsConnectedToElement(element);
                        foreach (RelationViewModel connectedRelation in connectedRelations)
                            _relationManager.Relations.Remove(connectedRelation);

                        _elementManager.Elements.Remove(element);
                        break;
                    }
                case RelationViewModel relation:
                    {
                        _relationManager.Relations.Remove(relation);
                        break;
                    }
            }
            SelectedSingleItem = null;
        }

        /// <summary>
        /// Metoda pro zrušení výběru všech prvků.
        /// </summary>
        private void ReleaseSelection()
        {
            if (SelectedSingleItem == null) return;
            foreach (var item in SelectedItems.ToList())
                if (item is BaseDiagramItemViewModel baseViewModel)
                    baseViewModel.IsSelected = false;
            SelectedItems.Clear();
            SelectedSingleItem = null;
        }

        /// <summary>
        /// Metoda pro zpracování změny vlastnosti 'IsSelected' u prvku nebo vazby.
        /// </summary>
        private void RelationOrElementViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsSelected") return;

            if (!(sender is BaseDiagramItemViewModel viewModel)) return;

            // Handle the IsSelected property change
            if (viewModel.IsSelected)
                HandleSelection(viewModel);
            else
                HandleDeselection(viewModel);
        }

        /// <summary>
        /// Metoda pro zpracování výběru prvku nebo vazby.
        /// </summary>
        private void HandleSelection(BaseDiagramItemViewModel viewModel)
        {
            if (SelectedItems.Contains(viewModel)) return;
            if (!_isMultipleSelectionActivated && SelectedItems.Any()) ReleaseSelection();//DeselectAll();
            SelectedItems.Add(viewModel);
            SelectedSingleItem = viewModel;
        }

        /// <summary>
        /// Metoda pro zpracování zrušení výběru prvku nebo vazby.
        /// </summary>
        private void HandleDeselection(BaseDiagramItemViewModel viewModel)
        {
            if (!SelectedItems.Contains(viewModel)) return;
            SelectedItems.Remove(viewModel);
            if (SelectedSingleItem == viewModel) SelectedSingleItem = null;
        }

        /// <summary>
        /// Metoda pro zpracování změn kolekce prvků nebo vazeb.
        /// </summary>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e, ICollection<object> canvasItems)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        if (item is BaseDiagramItemViewModel baseDiagramItemViewModel)
                            baseDiagramItemViewModel.PropertyChanged += RelationOrElementViewModel_PropertyChanged;
                        canvasItems.Add(item);
                        Debug.WriteLine("Adding object to the canvasCollection: " + item);
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        if (item is BaseDiagramItemViewModel baseDiagramItemViewModel)
                            baseDiagramItemViewModel.PropertyChanged -= RelationOrElementViewModel_PropertyChanged;
                        canvasItems.Remove(item);
                    }

                    break;

                // Případné další změny v kolekci
                //case NotifyCollectionChangedAction.Replace:
                //    break;
                //case NotifyCollectionChangedAction.Move:
                //    break;
                //case NotifyCollectionChangedAction.Reset:
                //    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Metoda pro přidání nového prvku na plátno.
        /// </summary>
        private void AddNewElementToCanvas(Point mousePosition)
        {
            Debug.WriteLine("CanvasViewModel: Adding new element to canvas");

            _tempElement = CreateNewElement(mousePosition); // A method that creates and configures the new element
            //ReleaseSelection();
            _elementManager.AddElement(_tempElement); // Assuming an element manager or similar mechanism
        }

        /// <summary>
        /// Metoda pro vytvoření nového prvku.
        /// </summary>
        private ElementViewModel CreateNewElement(Point position)
        {
            var element = _elementViewModelFactory.Create();
            if (_sharedStateService.IsDraggingGridColumnModeActive)
                element.ConfigureElementFromGridView(position.X, position.Y, _sharedCanvasPageItems.TableColumn);
            else
                element.ConfigureTempElement(position.X, position.Y);
            return element;
        }


        #region MouseEvents
        /// <summary>
        /// Metoda volaná při kliknutí levým tlačítkem myši na plátno. Zbylé operace s myší na objektech plátna (elipsy, lines), jsou v samostatných třídách
        /// </summary>
        private void CanvasMouseLeftButtonDownExecute(object parameter)
        {
            //Debug.WriteLine("CanvasViewModel: MouseButtonDown");
            if (!(parameter is Canvas canvas)) return;

            if (_sharedStateService.IsDrawingRelationModeActive)
            {
                var mousePosition = Mouse.GetPosition(canvas);
                if (!_mouseHandlingService.IsMouseInCanvas(mousePosition, canvas)) return;
                RemoveDrawnRelation();
                FinishRelationDrawing();
            }
            else
            {
                ReleaseSelection();
            }
        }

        /// <summary>
        /// Metoda pro zpracování pohybu myši na plátně. Zbylé operace s myší na objektech plátna (elipsy, lines), jsou v samostatných třídách
        /// </summary>
        private void CanvasMouseMoveExecute(object parameter)
        {
            //Debug.WriteLine("CanvasViewModel: Mouse move");

            if (!(parameter is Canvas canvas)) return;

            var mousePosition = Mouse.GetPosition(canvas);

            if (_sharedStateService.IsDraggingElementModeActive)
            {
                //Debug.WriteLine("CanvasViewModel: IsDraggingElementModeActive ");
                _sharedStateService.MouseOnElement = false;
                CanvasCursor = null;
                //CanvasCursor = null;
            }

            if (!_sharedStateService.IsDrawingElementModeActive) return;
            if (_tempElement != null) return;

            _sharedStateService.IsDrawingElement = true;
            CanvasCursor = null;
            //CanvasCursor = null;
            AddNewElementToCanvas(mousePosition);
            //Debug.WriteLine("CanvasViewModel: Mouse Move - Adding element");
            //Debug.WriteLine("CanvasViewModel: Now elements count: " + _elementManager.Elements.Count);
        }

        /// <summary>
        /// Metoda pro zpracování události uvolnění levého tlačítka myši na plátně. Zbylé operace s myší na objektech plátna (elipsy, lines), jsou v samostatných třídách
        /// </summary>
        public void CanvasMouseLeftButtonUpExecute(object parameter)
        {
            //Debug.WriteLine("CanvasViewModel: MouseButtonUp");
            //if ((_sharedStateService.IsDrawingElementModeActive == false && _sharedStateService.IsDrawingRelationModeActive == false) ||
            //    !(parameter is Canvas canvas)) return;
            if (!_sharedStateService.IsDrawingRelationModeActive && !_sharedStateService.IsDraggingGridColumnModeActive) return;

            Canvas canvas = parameter as Canvas;
            var mousePosition = Mouse.GetPosition(canvas);
            if (_sharedStateService.IsDrawingRelationModeActive)
            {
                if (!_mouseHandlingService.IsMouseInCanvas(mousePosition, canvas))
                    return;
                else
                {
                    RemoveDrawnRelation();
                    FinishRelationDrawing();
                }
            }

            else if (_sharedStateService.IsDraggingGridColumnModeActive)
            {
                AddNewElementToCanvas(mousePosition);
                FinishElementDrawingFromGridColumn();
            }
        }

        #endregion

        /// <summary>
        /// Metoda pro zpracování začátku nebo konce kreslení vazby.
        /// </summary>
        private void StartOrFinishDrawingRelation(object parameters)
        {
            if (!(parameters is List<object> parameter)) return;
            if (!(parameter[1] is Point mousePoint) || !(parameter[0] is ElementViewModel elementViewModel)) return;
            var command = parameter[2] as string;
            switch (command)
            {
                case "start":
                    _fromElement = elementViewModel;
                    _tempRelation = _relationViewModelFactory.Create(mousePoint, mousePoint);
                    _tempRelation.IsFinished = false;
                    _tempRelation.ZIndex = 2;
                    _relationManager.AddRelation(_tempRelation);
                    break;
                case "end":
                    {
                        if (_fromElement == elementViewModel || RelationAlreadyExists(_fromElement, elementViewModel))
                        {
                            RemoveDrawnRelation();
                        }
                        else
                        {
                            _tempRelation.ZIndex = 0;
                            _tempRelation.ObjectFrom = _fromElement;
                            _tempRelation.ObjectTo = elementViewModel;
                            _tempRelation.IsFinished = true;
                        }

                        FinishRelationDrawing();
                        break;
                    }
            }
        }

        /// <summary>
        /// Metoda pro zahájení přidávání prvku.
        /// </summary>
        private void StartAddingElement()
        {
            _sharedStateService.IsDrawingElementModeActive = true;
            _sharedStateService.IsDrawingRelationModeActive = false;
            _sharedStateService.IsDraggingElementModeActive = false;
            ReleaseSelection();
            CanvasCursor = null;
        }

        /// <summary>
        /// Metoda pro zahájení vytváření vazby.
        /// </summary>
        private void StartRelationCreation()
        {
            _sharedStateService.IsDrawingRelationModeActive = true;
            _sharedStateService.IsDrawingElementModeActive = false;
            _sharedStateService.IsDraggingElementModeActive = false;
            ReleaseSelection();
            CanvasCursor = null; // Notify that the cursor might need to change
        }

        /// <summary>
        /// Metoda pro odstranění vazby, pokud je kreslena mimo plátno.
        /// </summary>
        private void RemoveRelationWhenDrawnOutsideCanvas()
        {
            RemoveDrawnRelation();
            FinishRelationDrawing();
        }

        /// <summary>
        /// Metoda pro dokončení kreslení vazby.
        /// </summary>
        private void FinishRelationDrawing()
        {
            _fromElement = null;
            _tempRelation = null;
            _sharedStateService.IsDrawingRelationModeActive = false;
            _sharedStateService.IsDraggingElementModeActive = true;
            CanvasCursor = null;
        }

        /// <summary>
        /// Metoda pro odstranění kreslené vazby.
        /// </summary>
        private void RemoveDrawnRelation()
        {
            if (_tempRelation == null) return;
            _fromElement = null;
            _relationManager.Relations.Remove(_tempRelation);
            _tempRelation = null;
        }

        private void DropGridColumnToElementInCanvas(object parameter)
        {
            if (parameter is ElementViewModel elementViewModel && _sharedCanvasPageItems.TableColumn != null)
            {
                //elementViewModel.Label = _sharedCanvasPageItems.TableColumn.Heading;
                //elementViewModel.Title = _sharedCanvasPageItems.TableColumn.Heading;
                elementViewModel.GridTableColumn = _sharedCanvasPageItems.TableColumn;
                _sharedStateService.IsDraggingGridColumnModeActive = false;
                _sharedStateService.IsDraggingElementModeActive = true;
                //ReleaseSelection();
                elementViewModel.IsSelected = true;
                //CanvasCursor = null;
            }
        }

        /// <summary>
        /// Metoda pro kontrolu, zda již existuje vazba mezi dvěma prvky.
        /// </summary>
        public bool RelationAlreadyExists(ElementViewModel fromElement, ElementViewModel toElement)
        {
            return Relations.Any(relation =>
                (relation.ObjectFrom == fromElement && relation.ObjectTo == toElement) ||
                (relation.ObjectFrom == toElement && relation.ObjectTo == fromElement)
            );
        }

        /// <summary>
        /// Metoda pro oznamování změn vlastností - pro binding
        /// </summary>
        /// <param name="propertyName">Jméno vlastnosti, která se změnila.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}