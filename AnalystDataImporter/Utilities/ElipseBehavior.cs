using Microsoft.Xaml.Behaviors;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using AnalystDataImporter.Services;
using AnalystDataImporter.ViewModels;
using System.Collections.Generic;

namespace AnalystDataImporter.Utilities
{
    public class ElipseBehavior : Behavior<UIElement>
    {
        private bool _isDragging;
        private bool _isDrawing;
        private IMouseHandlingService _mouseHandlingService;

        public ICommand ElementSelectedCommand
        {
            get => (ICommand)GetValue(ElementSelectedCommandProperty);
            set => SetValue(ElementSelectedCommandProperty, value);
        }

        public ICommand ChangeCursorCommand
        {
            get => (ICommand)GetValue(ChangeCursorCommandProperty);
            set => SetValue(ChangeCursorCommandProperty, value);
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

        public static readonly DependencyProperty ChangeFinishDrawingElementProperty = DependencyProperty.Register(
            nameof(FinishDrawingElementCommand), typeof(ICommand),
            typeof(ElipseBehavior),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ChangeCursorCommandProperty = DependencyProperty.Register(
            nameof(ChangeCursorCommand), typeof(ICommand),
            typeof(ElipseBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty GetStartingOrEndingElementCommandProperty = DependencyProperty.Register(
            nameof(RelationStartOrEndElementSetCommand), typeof(ICommand),
            typeof(ElipseBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty ElementSelectedCommandProperty = DependencyProperty.Register(
            nameof(ElementSelectedCommand), typeof(ICommand), typeof(ElipseBehavior),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IsDraggingElementEnabledProperty = DependencyProperty.Register(
            nameof(IsDraggingElementEnabled),
            typeof(bool),
            typeof(ElipseBehavior),
            new PropertyMetadata(null)); //new PropertyMetadata(true, OnIsEnabledChanged));

        public static readonly DependencyProperty IsDrawingElementEnabledProperty = DependencyProperty.Register(
            nameof(IsDrawingElementEnabled),
            typeof(bool),
            typeof(ElipseBehavior),
            new PropertyMetadata(null)); // new PropertyMetadata(true, OnIsEnabledChanged));

        public static readonly DependencyProperty GridProperty = DependencyProperty.Register(
            nameof(ParentGrid),
            typeof(Grid),
            typeof(ElipseBehavior),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IsDrawingRelationEnabledProperty = DependencyProperty.Register(
           nameof(IsDrawingRelationEnabled),
           typeof(bool),
           typeof(ElipseBehavior),
           new PropertyMetadata(null));

        public Canvas ParentCanvas
        {
            get => SharedBehaviorProperties.GetParentCanvas(this);
            set => SharedBehaviorProperties.SetParentCanvas(this, value);
        }

        public Grid ParentGrid
        {
            get => (Grid)GetValue(GridProperty);
            set => SetValue(GridProperty, value);
        }

        public bool IsDraggingElementEnabled
        {
            get => (bool)GetValue(IsDraggingElementEnabledProperty);
            set => SetValue(IsDraggingElementEnabledProperty, value);
        }

        public bool IsDrawingElementEnabled
        {
            get => (bool)GetValue(IsDrawingElementEnabledProperty);
            set => SetValue(IsDrawingElementEnabledProperty, value);
        }

        public bool IsDrawingRelationEnabled
        {
            get => (bool)GetValue(IsDrawingRelationEnabledProperty);
            set => SetValue(IsDrawingRelationEnabledProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            _mouseHandlingService = ServiceLocator.Current.GetService<IMouseHandlingService>();
            this.AssociatedObject.MouseLeftButtonDown += MouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonUp += MouseLeftButtonUp;
            this.AssociatedObject.MouseMove += MouseMove;

            Point mousePosition = Mouse.GetPosition(ParentCanvas);
            if (!_isDrawing && !_mouseHandlingService.IsInUse)
            {
                Debug.WriteLine("Element MouseMove - _isDrawing = false");
                FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
                ElementViewModel elementViewModel = (ElementViewModel)associatedElement.DataContext;
                if (elementViewModel._temporary && _mouseHandlingService.IsMouseInCanvas(mousePosition, ParentCanvas))
                {
                    _isDrawing = true;
                    _mouseHandlingService.StartDragOrSelectOperation(associatedElement, mousePosition, elementViewModel, true);
                }

            }
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Element MouseLeftButtonDown");
            //if (!IsDraggingElementEnabled && !IsDrawingElementEnabled) return;

            Point mousePosition = e.GetPosition(ParentCanvas);

            if (IsDraggingElementEnabled || IsDrawingRelationEnabled)
            {
                FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
                BaseDiagramItemViewModel elementViewModel = (BaseDiagramItemViewModel)associatedElement.DataContext;

                if (IsDraggingElementEnabled)
                {
                    _mouseHandlingService.StartDragOrSelectOperation(associatedElement, mousePosition, elementViewModel, false);
                    _isDragging = true;
                }

                else if (IsDrawingRelationEnabled)
                {
                    if (RelationStartOrEndElementSetCommand.CanExecute(elementViewModel))
                    {
                        RelationStartOrEndElementSetCommand.Execute(new List<object> { elementViewModel, mousePosition, "start" });
                    }
                }
                e.Handled = true;
            }

            else if (IsDrawingElementEnabled)
            {
                _isDrawing = false;
                _mouseHandlingService.EndDragOperation();
                if (FinishDrawingElementCommand.CanExecute(null))
                {
                    FinishDrawingElementCommand.Execute(null);
                }
                e.Handled = true;
            }
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Element MouseButtonUp");
            if (IsDrawingElementEnabled) return;

            if (IsDraggingElementEnabled)
            {
                _mouseHandlingService.EndDragOperation();
                _isDragging = false;
            }

            else if (IsDrawingRelationEnabled)
            {
                Point mousePosition = e.GetPosition(ParentCanvas);
                FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
                BaseDiagramItemViewModel elementViewModel = (BaseDiagramItemViewModel)associatedElement.DataContext;
                if (RelationStartOrEndElementSetCommand.CanExecute(elementViewModel))
                {
                    RelationStartOrEndElementSetCommand.Execute(new List<object> { elementViewModel, mousePosition, "end" });
                }
            }

            e.Handled = true;
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("Element MouseMove");
            //if (!IsDraggingElementEnabled && !IsDrawingElementEnabled) return;

            if (IsDrawingElementEnabled)
            {
                Debug.WriteLine("Element MouseMove - IsDrawingEnabled = true");
                Point mousePosition = e.GetPosition(ParentCanvas);

                if (_isDrawing)
                {
                    string cursorType;
                    Debug.WriteLine("Element MouseMove - _isDrawing = true");
                    if (_mouseHandlingService.IsMouseInCanvas(mousePosition, ParentCanvas))
                    {
                        cursorType = "EllipseDrawingInsideCanvasCursor";
                        if (ChangeCursorCommand.CanExecute(cursorType))
                        {
                            ChangeCursorCommand.Execute(cursorType);
                        }
                        _mouseHandlingService.UpdateDragOperationWhenDrawing(mousePosition, ParentCanvas);
                    }
                    else
                    {
                        cursorType = "EllipseDrawingOutsideCanvasCursor";
                        if (ChangeCursorCommand.CanExecute(cursorType))
                        {
                            ChangeCursorCommand.Execute(cursorType);
                        }
                    }
                }
            }

            else if (IsDraggingElementEnabled)
            {
                Debug.WriteLine("Element MouseMove - dragging enabled");
                if (ChangeCursorCommand != null)
                {
                    // Determine cursor type based on your logic
                    string cursorType = "EllipseDraggingCursor"; // Example
                    if (ChangeCursorCommand.CanExecute(cursorType))
                    {
                        ChangeCursorCommand.Execute(cursorType);
                    }
                }

                if (_isDragging)
                {
                    Debug.WriteLine("Element MouseMove - _isDragging and IsDraggingEnabled true");
                    var mousePosition = e.GetPosition(ParentCanvas);
                    _mouseHandlingService.UpdateDragOperationWhenDragging(mousePosition, ParentCanvas, ParentGrid);
                }
                
            }
            e.Handled = true;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= MouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonUp -= MouseLeftButtonUp;
            this.AssociatedObject.MouseMove -= MouseMove;
        }
    }
}