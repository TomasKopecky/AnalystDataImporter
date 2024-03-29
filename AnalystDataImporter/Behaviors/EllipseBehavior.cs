﻿using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using AnalystDataImporter.Services;
using AnalystDataImporter.ViewModels;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AnalystDataImporter.Behaviors
{
    /// <summary>
    /// Chování pro správu událostí myši na objektu typu Ellipse.
    /// </summary>
    public class EllipseBehavior : Behavior<Ellipse>
    {
        private bool _isDragging; // ndikace zda je aktivní mód tažení elipsy
        private bool _isDrawing; // indikace zda je aktivní mód kreslení elipsy
        private IMouseHandlingService _mouseHandlingService; // proměnná pro přiřazení service pro obsluhu mouse events
        private SharedStatesService _sharedStateService;

        // Commands pro komunikaci s CanvasViewModel třídou_mouseHandlingService
        public ICommand ElementSelectedCommand
        {
            get => (ICommand)GetValue(ElementSelectedCommandProperty);
            set => SetValue(ElementSelectedCommandProperty, value);
        }

        public ICommand FinishDrawingElementCommand
        {
            get => (ICommand)GetValue(ChangeFinishDrawingElementProperty);
            set => SetValue(ChangeFinishDrawingElementProperty, value);
        }

        public ICommand RelationStartOrEndElementSetCommand
        {
            get => (ICommand)GetValue(GetStartingOrEndingElementCommandProperty);
            set => SetValue(GetStartingOrEndingElementCommandProperty, value);
        }

        public ICommand GridViewColumnDraggedSetCommand
        {
            get => (ICommand)GetValue(GridViewColumnDraggedSetCommandProperty);
            set => SetValue(GridViewColumnDraggedSetCommandProperty, value);
        }

        // Vlastnosti DependencyProperty - návaznost na výše uvedené commands - připojení k view
        public static readonly DependencyProperty ChangeFinishDrawingElementProperty = DependencyProperty.Register(
            nameof(FinishDrawingElementCommand), typeof(ICommand),
            typeof(EllipseBehavior),
            new PropertyMetadata(null));

        public static readonly DependencyProperty GetStartingOrEndingElementCommandProperty = DependencyProperty.Register(
            nameof(RelationStartOrEndElementSetCommand), typeof(ICommand),
            typeof(EllipseBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty ElementSelectedCommandProperty = DependencyProperty.Register(
            nameof(ElementSelectedCommand), typeof(ICommand), typeof(EllipseBehavior),
            new PropertyMetadata(null));

        public static readonly DependencyProperty GridViewColumnDraggedSetCommandProperty = DependencyProperty.Register(
            nameof(GridViewColumnDraggedSetCommand), typeof(ICommand),
            typeof(EllipseBehavior),
            new PropertyMetadata(null));

        //public static readonly DependencyProperty _sharedStateService.IsDraggingElementModeActiveProperty = DependencyProperty.Register(
        //    nameof(_sharedStateService.IsDraggingElementModeActive),
        //    typeof(bool),
        //    typeof(EllipseBehavior),
        //    new PropertyMetadata(null)); //new PropertyMetadata(true, OnIsEnabledChanged));

        //public static readonly DependencyProperty IsDrawingElementEnabledProperty = DependencyProperty.Register(
        //    nameof(IsDrawingElementEnabled),
        //    typeof(bool),
        //    typeof(EllipseBehavior),
        //    new PropertyMetadata(null)); // new PropertyMetadata(true, OnIsEnabledChanged));

        public static readonly DependencyProperty GridProperty = DependencyProperty.Register(
            nameof(ParentGrid),
            typeof(Grid),
            typeof(EllipseBehavior),
            new PropertyMetadata(null));

        //public static readonly DependencyProperty IsDrawingRelationEnabledProperty = DependencyProperty.Register(
        //   nameof(IsDrawingRelationEnabled),
        //   typeof(bool),
        //   typeof(EllipseBehavior),
        //   new PropertyMetadata(null));

        /// <summary>
        /// Plátno, ke kterému je chování připojeno - sdíleno z SharedBehaviorProperties třídy
        /// </summary>
        public Canvas ParentCanvas
        {
            get => SharedBehaviorProperties.GetParentCanvas(this);
            set => SharedBehaviorProperties.SetParentCanvas(this, value);
        }

        public ICommand ChangeCursorCommand
        {
            get => SharedBehaviorProperties.GetChangeCursorCommand(this);
            set => SharedBehaviorProperties.SetChangeCursorCommand(this, value);
        }

        /// <summary>
        /// Grid, ve kterému je elipsa vnořena, tedy celý obdelník kolem
        /// </summary>
        public Grid ParentGrid
        {
            get => (Grid)GetValue(GridProperty);
            set => SetValue(GridProperty, value);
        }

        //public bool _sharedStateService.IsDraggingElementModeActive
        //{
        //    get => (bool)GetValue(_sharedStateService.IsDraggingElementModeActiveProperty);
        //    set => SetValue(_sharedStateService.IsDraggingElementModeActiveProperty, value);
        //}

        //public bool IsDrawingElementEnabled
        //{
        //    get => (bool)GetValue(IsDrawingElementEnabledProperty);
        //    set => SetValue(IsDrawingElementEnabledProperty, value);
        //}

        //public bool IsDrawingRelationEnabled
        //{
        //    get => (bool)GetValue(IsDrawingRelationEnabledProperty);
        //    set => SetValue(IsDrawingRelationEnabledProperty, value);
        //}

        ///// <summary>
        ///// Připojení sužeb při vytvoření objektu
        ///// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            // Připojení _mouseHandlingService pomocí ServiceLocator pro operace s myší na objektu elipsy
            _mouseHandlingService = ServiceLocator.Current.GetService<IMouseHandlingService>();
            _sharedStateService = ServiceLocator.Current.GetService<SharedStatesService>();
            AssociateEventHandlers();
            StartDrawingNewElement();
        }

        ///// <summary>
        ///// Připojení mouse services při vytvoření objektu
        ///// </summary>
        private void AssociateEventHandlers()
        {
            AssociatedObject.MouseLeftButtonDown += MouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp += MouseLeftButtonUp;
            AssociatedObject.MouseMove += MouseMove;
        }

        ///// <summary>
        ///// Metoda pro zahájení kreslení elipsy
        ///// </summary>
        private void StartDrawingNewElement()
        {
            Point mousePosition = Mouse.GetPosition(ParentCanvas);

            if (_isDrawing)
                return;

            //Debug.WriteLine("Element MouseMove - _isDrawing = false");
            FrameworkElement associatedElement = (FrameworkElement)AssociatedObject;
            ElementViewModel elementViewModel = (ElementViewModel)associatedElement.DataContext;

            if (!elementViewModel._temporary || !_mouseHandlingService.IsMouseInCanvas(mousePosition, ParentCanvas))
                return;

            _isDrawing = true;
            _mouseHandlingService.StartOperation(associatedElement, mousePosition, elementViewModel, "drawing");

        }

        /// <summary>
        /// Reaguje na stisknutí levého tlačítka myši. Startuje operaci přesouvání, výběru nebo kreslení vazby v závislosti na aktivovaném módu.
        /// </summary>
        /// <param name="sender">Objekt, který událost vyvolal (typicky UI element).</param>
        /// <param name="e">Data události obsahující informace o stisknutí tlačítka myši.</param>
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(ParentCanvas);
            FrameworkElement associatedElement = (FrameworkElement)sender;
            BaseDiagramItemViewModel elementViewModel = (BaseDiagramItemViewModel)associatedElement.DataContext;

            // v případě módu dragging startujeme přesouvací mód v _mouseHandlingService
            if (_sharedStateService.IsDraggingElementModeActive)
            {
                _mouseHandlingService.StartOperation(associatedElement, mousePosition, elementViewModel, "dragging");
                _isDragging = true;
            }

            // v případě módu kreslení vazby posíláme pomocí command elipsu jako startovní do CanvasViewModel
            else if (_sharedStateService.IsDrawingRelationModeActive && RelationStartOrEndElementSetCommand.CanExecute(elementViewModel))
            {
                RelationStartOrEndElementSetCommand.Execute(new List<object> { elementViewModel, mousePosition, "start" });
            }

            else if (_sharedStateService.IsDrawingElementModeActive)
            {
                FinishDrawingIfNeeded();
                SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "EllipseDraggingCursor");
            }

            e.Handled = true;
        }

        /// <summary>
        /// Reaguje na uvolnění levého tlačítka myši. Dokončuje operace přesouvání nebo kreslení vazby.
        /// </summary>
        /// <param name="sender">Objekt, který událost vyvolal (typicky UI element).</param>
        /// <param name="e">Data události obsahující informace o uvolnění tlačítka myši.</param>
        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_sharedStateService.IsDrawingElementModeActive && !_sharedStateService.IsDraggingGridColumnModeActive)
                return;

            Point mousePosition = e.GetPosition(ParentCanvas);
            FrameworkElement associatedElement = (FrameworkElement)sender;
            BaseDiagramItemViewModel elementViewModel = (BaseDiagramItemViewModel)associatedElement.DataContext;

            if (!_sharedStateService.IsDrawingElementModeActive)
            {
                if (_sharedStateService.IsDraggingElementModeActive)
                {
                    _mouseHandlingService.EndDragOperation();
                    _isDragging = false;
                }
                else if (_sharedStateService.IsDrawingRelationModeActive && RelationStartOrEndElementSetCommand.CanExecute(elementViewModel))
                {
                    RelationStartOrEndElementSetCommand.Execute(new List<object> { elementViewModel, mousePosition, "end" });
                    SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "EllipseDraggingCursor");
                }
            }
            if (_sharedStateService.IsDraggingGridColumnModeActive)
            {
                GridViewColumnDraggedSetCommand.Execute(elementViewModel);
                SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "EllipseDraggingCursor");
            }
            e.Handled = true;
        }

        /// <summary>
        /// Reaguje na pohyb myši. Aktualizuje pozici prvku během přetahování nebo kreslení.
        /// </summary>
        /// <param name="sender">Objekt, který událost vyvolal (typicky UI element).</param>
        /// <param name="e">Data události obsahující informace o pozici myši.</param>
        private void MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(ParentCanvas);
            if (_sharedStateService.IsDrawingElementModeActive && _isDrawing)
            {
                SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "EllipseDrawingInsideCanvasCursor");
                _mouseHandlingService.UpdateOperation(mousePosition, ParentCanvas, null);
            }
            else if (_sharedStateService.IsDraggingElementModeActive)
            {
                SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "EllipseDraggingCursor");
                if (_isDragging)
                {
                    _mouseHandlingService.UpdateOperation(mousePosition, ParentCanvas, ParentGrid);
                }
            }
            e.Handled = true;
        }

        ///// <summary>
        ///// Ukončení módu kreslení elipsy
        ///// </summary>
        private void FinishDrawingIfNeeded()
        {
            _isDrawing = false;
            _mouseHandlingService.EndDragOperation();
            if (FinishDrawingElementCommand.CanExecute(null))
            {
                FinishDrawingElementCommand.Execute(null);
            }
        }


        ///// <summary>
        ///// Odpojení služeb při destrukci objektu - obecně
        ///// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            DisassociateEventHandlers();
        }

        ///// <summary>
        ///// Odpojení mouse services při destrukci objektu - detail
        ///// </summary>
        private void DisassociateEventHandlers()
        {
            AssociatedObject.MouseLeftButtonDown -= MouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp -= MouseLeftButtonUp;
            AssociatedObject.MouseMove -= MouseMove;
        }
    }
}