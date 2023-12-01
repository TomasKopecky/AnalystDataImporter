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

        private string _canvasCursor;
        private ElementViewModel _fromElement;
        private bool _isAddingElementOutsideCanvas;
        private bool _isDraggingElementModeActive;
        private bool _isDrawingElement;
        private bool _isDrawingElementModeActive;
        private readonly bool _isMultipleSelectionActivated;
        private bool _isRelationDrawingModeActive;
        private bool _mouseOnElement;
        private object _selectedSingleItem;
        private ElementViewModel _tempElement;
        private RelationViewModel _tempRelation;
        #endregion

        #region Constructor
        // Konstruktor
        public CanvasViewModel(IElementViewModelFactory elementViewModelFactory, IElementManager elementManager,
            IRelationViewModelFactory relationViewModelFactory, IRelationManager relationManager,
            IMouseHandlingService mouseHandlingService)
        {
            // Inicializace proměnných a závislostí
            _elementViewModelFactory = elementViewModelFactory ?? throw new ArgumentNullException(nameof(elementViewModelFactory));
            _elementManager = elementManager ?? throw new ArgumentNullException(nameof(elementManager));
            _relationViewModelFactory = relationViewModelFactory ?? throw new ArgumentNullException(nameof(relationViewModelFactory));
            _relationManager = relationManager ?? throw new ArgumentNullException(nameof(relationManager));
            _mouseHandlingService = mouseHandlingService ?? throw new ArgumentNullException(nameof(mouseHandlingService));

            InitializeCommands();

            CanvasItems = new ObservableCollection<object>();
            SelectedItems = new ObservableCollection<object>();

            Elements.CollectionChanged += (s, e) => OnCollectionChanged(e, CanvasItems);
            Relations.CollectionChanged += (s, e) => OnCollectionChanged(e, CanvasItems);

            _isMultipleSelectionActivated = false;
            _isDraggingElementModeActive = true;
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
        public RelayCommand StartAddingElementCommand { get; private set; }
        public ICommand StartRelationCreatingCommand { get; private set; }
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

        public bool IsDrawingRelationModeActive
        {
            get => _isRelationDrawingModeActive;
            set
            {
                if (_isRelationDrawingModeActive == value) return;
                _isRelationDrawingModeActive = value;
                OnPropertyChanged(nameof(IsDrawingRelationModeActive));
            }
        }

        public bool IsDraggingElementModeActive
        {
            get => _isDraggingElementModeActive;
            set
            {
                if (_isDraggingElementModeActive == value) return;
                _isDraggingElementModeActive = value;
                OnPropertyChanged(nameof(IsDraggingElementModeActive));
            }
        }

        public bool IsDrawingElement
        {
            get => _isDrawingElement;
            set
            {
                if (_isDrawingElement == value) return;
                _isDrawingElement = value;
                OnPropertyChanged(nameof(IsDrawingElement));
            }
        }

        public bool IsDrawingElementModeActive
        {
            get => _isDrawingElementModeActive;
            set
            {
                if (_isDrawingElementModeActive == value) return;
                _isDrawingElementModeActive = value;
                OnPropertyChanged(nameof(IsDrawingElementModeActive));
            }
        }

        public string CanvasCursor
        {
            get
            {
                if (IsDrawingElement)
                    _canvasCursor = _isAddingElementOutsideCanvas ? "Arrow" : "None";
                else if (IsDrawingRelationModeActive)
                    _canvasCursor = "Cross";
                else if (_mouseOnElement)
                    _canvasCursor = "SizeAll";
                else
                    _canvasCursor = "Arrow";

                //Debug.WriteLine("Getting cursor: " + _canvasCursor);
                return _canvasCursor;
            }
            set
            {
                _canvasCursor = value;
                OnPropertyChanged(nameof(CanvasCursor));
            }
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
            IsDraggingElementModeActive = true;
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
            IsDrawingElement = false;
            IsDrawingElementModeActive = false;
            IsDraggingElementModeActive = true;
            OnPropertyChanged(nameof(CanvasCursor));
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
                    _isAddingElementOutsideCanvas = false;
                    _mouseOnElement = true;
                    break;
                case "EllipseDrawingInsideCanvasCursor":
                    _isAddingElementOutsideCanvas = false;
                    break;
                case "EllipseDrawingOutsideCanvasCursor":
                    _isAddingElementOutsideCanvas = true;
                    break;
            }

            OnPropertyChanged(nameof(CanvasCursor));
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
            _elementManager.AddElement(_tempElement); // Assuming an element manager or similar mechanism
        }

        /// <summary>
        /// Metoda pro vytvoření nového prvku.
        /// </summary>
        private ElementViewModel CreateNewElement(Point position)
        {
            var element = _elementViewModelFactory.Create();
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

            if (IsDrawingRelationModeActive)
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

            if (IsDraggingElementModeActive)
            {
                _mouseOnElement = false;
                OnPropertyChanged(nameof(CanvasCursor));
            }

            if (!IsDrawingElementModeActive) return;
            if (_tempElement != null) return;

            IsDrawingElement = true;
            OnPropertyChanged(nameof(CanvasCursor));
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
            if ((IsDrawingElementModeActive == false && IsDrawingRelationModeActive == false) ||
                !(parameter is Canvas canvas)) return;

            if (!IsDrawingRelationModeActive) return;
            var mousePosition = Mouse.GetPosition(canvas);
            if (!_mouseHandlingService.IsMouseInCanvas(mousePosition, canvas)) return;
            RemoveDrawnRelation();
            FinishRelationDrawing();
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
            IsDrawingElementModeActive = true;
            IsDrawingRelationModeActive = false;
            IsDraggingElementModeActive = false;
            ReleaseSelection();
        }

        /// <summary>
        /// Metoda pro zahájení vytváření vazby.
        /// </summary>
        private void StartRelationCreation()
        {
            IsDrawingRelationModeActive = true;
            IsDrawingElementModeActive = false;
            IsDraggingElementModeActive = false;
            ReleaseSelection();
            OnPropertyChanged(nameof(CanvasCursor)); // Notify that the cursor might need to change
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
            IsDrawingRelationModeActive = false;
            IsDraggingElementModeActive = true;
            OnPropertyChanged(nameof(CanvasCursor));
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