﻿using System;
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
    /// ViewModel pro plátno, které může obsahovat více prvků a poskytuje funkcionalitu pro přidání nových prvků.
    /// </summary>
    //TODO: If ElementViewModel has logic for moving the element on the canvas, consider adding bounds checking to ensure elements don't go outside the canvas or overlap with other elements.
    public class CanvasViewModel : INotifyPropertyChanged
    {
        private readonly IMouseHandlingService _mouseHandlingService;

        public ICommand ChangeCursorWhenOperatingElementCommand { get; private set; }

        public ICommand FinishDrawingElementCommand { get; private set; }

        public ICommand GetRelationStartOrEndElementCommand { get; private set; }

        private object _selectedSingleItem;

        private RelationViewModel _selectedSingleRelation;

        public ICommand DeleteSelectionCommand { get; private set; }

        public ObservableCollection<object> CanvasItems { get;}

        public ObservableCollection<object> SelectedItems { get; }

        private bool _isAddingElementOutsideCanvas;

        private bool _isMultipleSelectionActivated;

        private string _canvasCursor;

        private bool _mouseOnElement;

        private ElementViewModel _fromElement;
        //private ElementViewModel _toElement;

        private RelationViewModel _tempRelation;

        private ElementViewModel _tempElement;

        // aktivace enabled vlastnosti obou tlačítek na začátku
        private bool _isRelationDrawingModeActive;

        private bool _isDrawingElementModeActive;

        private bool _isDraggingElementModeActive;

        public ICommand CanvasMouseLeftButtonDownCommand { get; private set; }
        public ICommand CanvasMouseMoveCommand { get; private set; }

        public ICommand RelationDeleteWhenOutsideCanvasCommand { get; private set; }
        public ICommand CanvasMouseLeftButtonUpCommand { get; private set; }

        private bool _isDrawingElement;

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

        /// <summary>
        /// Příkaz pro zahájení kreslení vazby.
        /// </summary>
        public ICommand StartRelationCreatingCommand { get; private set; }

        public bool TestingMode { get; set; }

        /// <summary>
        /// Konstruktor třídy CanvasViewModel. Přijímá závislosti potřebné pro tuto třídu.
        /// </summary>
        /// <param name="elementViewModelFactory">Factory pro vytváření ViewModelů prvků.</param>
        /// <param name="elementManager">Správce prvků pro manipulaci a získání informací o prvcích.</param>
        /// <param name="relationViewModelFactory"></param>
        /// <param name="relationManager"></param>
        /// <param name="mouseHandlingService"></param>
        public CanvasViewModel(IElementViewModelFactory elementViewModelFactory, IElementManager elementManager, IRelationViewModelFactory relationViewModelFactory, IRelationManager relationManager, IMouseHandlingService mouseHandlingService)
        {

            _elementViewModelFactory = elementViewModelFactory ?? throw new ArgumentNullException(nameof(elementViewModelFactory));
            _elementManager = elementManager ?? throw new ArgumentNullException(nameof(elementManager));
            _relationViewModelFactory = relationViewModelFactory ?? throw new ArgumentNullException(nameof(relationViewModelFactory));
            _relationManager = relationManager ?? throw new ArgumentNullException(nameof(relationManager));
            _mouseHandlingService = mouseHandlingService ?? throw new ArgumentNullException(nameof(mouseHandlingService));

            StartAddingElementCommand = new RelayCommand(StartAddingElement);
            StartRelationCreatingCommand = new RelayCommand(StartRelationCreation);
            CanvasMouseLeftButtonDownCommand = new RelayCommand<object>(CanvasMouseLeftButtonDownExecute);
            CanvasMouseMoveCommand = new RelayCommand<object>(CanvasMouseMoveExecute);
            CanvasMouseLeftButtonUpCommand = new RelayCommand<object>(CanvasMouseLeftButtonUpExecute);

            GetRelationStartOrEndElementCommand = new RelayCommand<object>(StartOrFinishDrawingRelation);

            DeleteSelectionCommand = new RelayCommand(DeleteSelectionExecute);

            CanvasItems = new ObservableCollection<object>();
            SelectedItems = new ObservableCollection<object>();

            Elements.CollectionChanged += (s, e) => OnCollectionChanged(e, CanvasItems);
            Relations.CollectionChanged += (s, e) => OnCollectionChanged(e, CanvasItems);
            ChangeCursorWhenOperatingElementCommand = new RelayCommand<string>(ChangeCursorByElement);
            FinishDrawingElementCommand = new RelayCommand(FinishElementDrawing);
            RelationDeleteWhenOutsideCanvasCommand = new RelayCommand(RemoveRelationWhenDrawnOutsideCanvas);

            _isMultipleSelectionActivated = false;
            _isDraggingElementModeActive = true;
            TestingMode = false;
        }

        public void AddTestingElementsAndRelation()
        {
            if (TestingMode)
            {
                ElementViewModel fromElement = AddTestingElementToCanvas(new Point(200, 200), "First element", "identita není nastavena");
                ElementViewModel toElement = AddTestingElementToCanvas(new Point(400, 100), "Second element", "identita není");
                AddTestingElementToCanvas(new Point(2, 2), "Third element", "identita");
                AddTestingRelationToCanvas(fromElement, toElement);
                IsDraggingElementModeActive = true;
            }
        }

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

        public void AddTestingRelationToCanvas(ElementViewModel fromElement, ElementViewModel toElement)
        {
            RelationViewModel relationViewModel = _relationViewModelFactory.Create(new Point(fromElement.XCenter, fromElement.YCenter), new Point(toElement.XCenter, toElement.YCenter));
            relationViewModel.ZIndex = 0;
            relationViewModel.Title = "vazba";
            relationViewModel.ObjectFrom = fromElement;
            relationViewModel.ObjectTo = toElement;
            relationViewModel.IsFinished = true;
            _relationManager.AddRelation(relationViewModel);
        }

        private void FinishElementDrawing()
        {
            Debug.WriteLine("CanvasViewModel: MouseButtoDown - finishing new element in canvas");
            _tempElement.FinishTempElement();
            _tempElement = null;
            IsDrawingElement = false;
            IsDrawingElementModeActive = false;
            IsDraggingElementModeActive = true;
            OnPropertyChanged(nameof(CanvasCursor));
        }

        private void ChangeCursorByElement(string operation)
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
            if (SelectedSingleItem != null)
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
                        Debug.WriteLine("Adding object to the canvasCollection: " + item);
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

        private void AddNewElementToCanvas(Point mousePosition)
        {
            Debug.WriteLine("CanvasViewModel: Adding new element to canvas");

            _tempElement = CreateNewElement(mousePosition); // A method that creates and configures the new element
            _elementManager.AddElement(_tempElement); // Assuming an element manager or similar mechanism
        }

        private ElementViewModel CreateNewElement(Point position)
        {
            ElementViewModel element = _elementViewModelFactory.Create();
            element.ConfigureTempElement(position.X, position.Y);
            return element;
        }


        public bool IsDrawingRelationModeActive
        {
            get => _isRelationDrawingModeActive;
            set
            {
                if (_isRelationDrawingModeActive != value)
                {
                    _isRelationDrawingModeActive = value;
                    OnPropertyChanged(nameof(IsDrawingRelationModeActive));
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
            Debug.WriteLine("CanvasViewModel: MouseButtonDown");
            if (!(parameter is Canvas canvas)) return;

            if (IsDrawingRelationModeActive)
            {
                Point mousePosition = Mouse.GetPosition(canvas);
                if (_mouseHandlingService.IsMouseInCanvas(mousePosition, canvas))
                {
                    RemoveDrawnRelation();
                    FinishRelationDrawing();
                }
            }
            else
            {
                ReleaseSelection();
                //_mouseHandlingService.EndDragOperation();
            }
        }

        private void CanvasMouseMoveExecute(object parameter)
        {
            Debug.WriteLine("CanvasViewModel: Mouse move");

            if (!(parameter is Canvas canvas)) return;

            //Canvas canvas = (Canvas)parameter;

            Point mousePosition = Mouse.GetPosition(canvas);

            if (IsDraggingElementModeActive)
            {
                _mouseOnElement = false;
                OnPropertyChanged(nameof(CanvasCursor));
            }

            if (IsDrawingElementModeActive)
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
        }

        private void RemoveRelationWhenDrawnOutsideCanvas()
        {
            RemoveDrawnRelation();
            FinishRelationDrawing();
        }

        private void FinishRelationDrawing()
        {
            _fromElement = null;
            _tempRelation = null;
            IsDrawingRelationModeActive = false;
            IsDraggingElementModeActive = true;
            OnPropertyChanged(nameof(CanvasCursor));
        }

        private void RemoveDrawnRelation()
        {
            if (_tempRelation != null)
            {
                _fromElement = null;
                _relationManager.Relations.Remove(_tempRelation);
                _tempRelation = null;
            }
        }

        public void CanvasMouseLeftButtonUpExecute(object parameter)
        {
            Debug.WriteLine("CanvasViewModel: MouseButtoUp");
            if ((IsDrawingElementModeActive == false && IsDrawingRelationModeActive == false) || !(parameter is Canvas canvas)) return;

            if (IsDrawingRelationModeActive)
            {
                Point mousePosition = Mouse.GetPosition(canvas);
                if (_mouseHandlingService.IsMouseInCanvas(mousePosition, canvas))
                {
                    RemoveDrawnRelation();
                    FinishRelationDrawing();
                }
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
                    _canvasCursor = _isAddingElementOutsideCanvas ? "Arrow" : "None";
                }
                else if (IsDrawingRelationModeActive)
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
            }
            set
            {
                _canvasCursor = value;
                OnPropertyChanged(nameof(CanvasCursor));
            }
        }

        private void StartRelationCreation()
        {
            IsDrawingRelationModeActive = true; // Set to true when this method is called // Trigger OnPropertyChanged for all properties that depend on this mode
            IsDrawingElementModeActive = false;
            IsDraggingElementModeActive = false;
            OnPropertyChanged(nameof(CanvasCursor)); // Notify that the cursor might need to change
        }

        private void StartOrFinishDrawingRelation(object parameters)
        {
            if (parameters is List<object> parameter)
            {

                if (parameter[1] is Point mousePoint && parameter[0] is ElementViewModel elementViewModel)
                {
                    string command = parameter[2] as string;
                    if (command == "start")
                    {
                        _fromElement = elementViewModel;
                        _tempRelation = _relationViewModelFactory.Create(mousePoint, mousePoint);
                        _tempRelation.IsFinished = false;
                        _tempRelation.ZIndex = 2;
                        _relationManager.AddRelation(_tempRelation);
                    }
                    else if (command == "end")
                    {
                        if (_fromElement == elementViewModel || RelationAlreadyExists(_fromElement, elementViewModel))
                            RemoveDrawnRelation();
                        else
                        {
                            _tempRelation.ZIndex = 0;
                            _tempRelation.ObjectFrom = _fromElement;
                            _tempRelation.ObjectTo = elementViewModel;
                            _tempRelation.IsFinished = true;
                        }
                        FinishRelationDrawing();
                    }
                }
            }
        }

        private void StartAddingElement()
        {
            ReleaseSelection();
            IsDrawingElementModeActive = true;
            IsDrawingRelationModeActive = false;
            IsDraggingElementModeActive = false;
        }

    }
}