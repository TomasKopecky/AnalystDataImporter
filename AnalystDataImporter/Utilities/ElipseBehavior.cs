using Microsoft.Xaml.Behaviors;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using AnalystDataImporter.Services;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Utilities
{
    public class ElipseBehavior : Behavior<UIElement>
    {
        private bool _isDragging = false;
        private bool _isDrawing = false;
        private IMouseHandlingService _mouseHandlingService;

        public ICommand ElementSelectedCommand
        {
            get => (ICommand)GetValue(ElementSelectedCommandProperty);
            set => SetValue(ElementSelectedCommandProperty, value);
        }

        public ICommand ChangeCursorCommand
        {
            get { return (ICommand)GetValue(ChangeCursorCommandProperty); }
            set { SetValue(ChangeCursorCommandProperty, value); }
        }

        public ICommand FinishDrawingElement
        {
            get { return (ICommand)GetValue(ChangeFinishDrawingElementProperty); }
            set { SetValue(ChangeFinishDrawingElementProperty, value); }
        }

        public static readonly DependencyProperty ChangeFinishDrawingElementProperty = DependencyProperty.Register(
            nameof(FinishDrawingElement), typeof(ICommand), typeof(ElipseBehavior),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ChangeCursorCommandProperty = DependencyProperty.Register(
            nameof(ChangeCursorCommand), typeof(ICommand),
            typeof(ElipseBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty ElementSelectedCommandProperty = DependencyProperty.Register(
            nameof(ElementSelectedCommand), typeof(ICommand), typeof(ElipseBehavior),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            nameof(IsDraggingEnabled),
            typeof(bool),
            typeof(ElipseBehavior),
            new PropertyMetadata(null)); //new PropertyMetadata(true, OnIsEnabledChanged));

        public static readonly DependencyProperty ParentCanvasProperty = DependencyProperty.Register(
            nameof(ParentCanvas),
            typeof(Canvas),
            typeof(ElipseBehavior),
            new PropertyMetadata(null));

        public Canvas ParentCanvas
        {
            get => (Canvas)GetValue(ParentCanvasProperty);
            set => SetValue(ParentCanvasProperty, value);
        }

        public static readonly DependencyProperty IsDrawingEnabledProperty = DependencyProperty.Register(
            nameof(IsDrawingEnabled),
            typeof(bool),
            typeof(ElipseBehavior),
            new PropertyMetadata(null)); // new PropertyMetadata(true, OnIsEnabledChanged));

        public static readonly DependencyProperty GridProperty =
            DependencyProperty.Register(
                nameof(ParentGrid),
                typeof(Grid),
                typeof(ElipseBehavior),
                new PropertyMetadata(null));

        public Grid ParentGrid
        {
            get => (Grid)GetValue(GridProperty);
            set => SetValue(GridProperty, value);
        }

        public bool IsDraggingEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        public bool IsDrawingEnabled
        {
            get => (bool)GetValue(IsDrawingEnabledProperty);
            set => SetValue(IsDrawingEnabledProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            _mouseHandlingService = ServiceLocator.Current.GetService<IMouseHandlingService>();
            this.AssociatedObject.MouseLeftButtonDown += MouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonUp += MouseLeftButtonUp;
            this.AssociatedObject.MouseMove += MouseMove;
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Element MouseLeftButtonDown");
            if (!IsDraggingEnabled && !IsDrawingEnabled) return;

            Point mousePosition = e.GetPosition(ParentCanvas);

            if (IsDraggingEnabled)
            {
                FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
                BaseDiagramItemViewModel elementViewModel = (BaseDiagramItemViewModel)associatedElement.DataContext;

                _mouseHandlingService.StartDragOrSelectOperation(associatedElement, mousePosition, elementViewModel, false);
                _isDragging = true;
                e.Handled = true;
            }

            if (IsDrawingEnabled)
            {
                _isDrawing = false;
                _mouseHandlingService.EndDragOperation();
                if (FinishDrawingElement.CanExecute(null))
                {
                    FinishDrawingElement.Execute(null);
                }
                e.Handled = true;
            }
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsDrawingEnabled && _isDrawing)
            {
                _mouseHandlingService.CheckDraggingDrawnElementOutsideCanvas();
            }

            if (IsDraggingEnabled)
            {
                _mouseHandlingService.EndDragOperation();
                _isDragging = false;
            }
            e.Handled = true;
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging && !IsDraggingEnabled && !IsDrawingEnabled) return;

            if (IsDrawingEnabled)
            {
                Debug.WriteLine("Element MouseMove - IsDrawingEnabled = true");
                Point mousePosition = e.GetPosition(ParentCanvas);
                if (!_isDrawing)
                {
                    Debug.WriteLine("Element MouseMove - _isDrawing = false");
                    FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
                    BaseDiagramItemViewModel elementViewModel = (BaseDiagramItemViewModel)associatedElement.DataContext;
                    _isDrawing = true;
                    _mouseHandlingService.StartDragOrSelectOperation(associatedElement, mousePosition, elementViewModel, true);
                }

                if (_isDrawing)
                {
                    Debug.WriteLine("Element MouseMove - _isDrawing = true");
                    //_mouseHandlingService.UpdateDragOperationWhenDragging(mousePosition, ParentCanvas, ParentGrid);
                    _mouseHandlingService.UpdateDragOperationWhenDrawing(mousePosition, ParentCanvas);
                }
            }

            if (IsDraggingEnabled)
            {
                Debug.WriteLine("Element MouseMove - dragging enabled");
                if (ChangeCursorCommand != null)
                {
                    // Determine cursor type based on your logic
                    string cursorType = "dragging"; // Example
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
                e.Handled = true;
                //Mouse.OverrideCursor = Cursors.Hand;
                // Command pro změnu kurzoru myši
            }

            //if (_isDragging && IsDraggingEnabled)
            //{
            //    Debug.WriteLine("Element MouseMove - _isDragging and IsDraggingEnabled true");
            //    var mousePosition = e.GetPosition(ParentCanvas);
            //    _mouseHandlingService.UpdateDragOperationWhenDragging(mousePosition, ParentCanvas, ParentGrid);
            //}
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