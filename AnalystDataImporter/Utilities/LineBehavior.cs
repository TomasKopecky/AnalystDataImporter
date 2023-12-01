using Microsoft.Xaml.Behaviors;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using AnalystDataImporter.Services;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Utilities
{
    internal class LineBehavior : Behavior<UIElement>
    {
        private IMouseHandlingService _mouseHandlingService;

        // Define the DependencyProperty
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            nameof(IsEnabled),
            typeof(bool),
            typeof(LineBehavior),
            new PropertyMetadata(null)); // new PropertyMetadata(true, OnIsEnabledChanged));

        public static readonly DependencyProperty RelationDeleteWhenOutsideCanvasCommandProperty = DependencyProperty.Register(
            nameof(RelationDeleteWhenOutsideCanvasCommand), typeof(ICommand),
            typeof(LineBehavior),
            new PropertyMetadata(null));

        // Property to get and set the DependencyProperty
        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        public ICommand RelationDeleteWhenOutsideCanvasCommand
        {
            get => (ICommand)GetValue(RelationDeleteWhenOutsideCanvasCommandProperty);
            set => SetValue(RelationDeleteWhenOutsideCanvasCommandProperty, value);
        }

        public Canvas ParentCanvas
        {
            get => SharedBehaviorProperties.GetParentCanvas(this);
            set => SharedBehaviorProperties.SetParentCanvas(this, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown += MouseLeftButtonDown;
            this.AssociatedObject.PreviewMouseLeftButtonUp += MouseLeftButtonPreviewUp;
            this.AssociatedObject.MouseMove += MouseMove;
            _mouseHandlingService = ServiceLocator.Current.GetService<IMouseHandlingService>();

            FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
            RelationViewModel relationtViewModel = (RelationViewModel)associatedElement.DataContext;
            //_mouseHandlingService.StartRelationDragOrSelectOperation(associatedElement, null, relationtViewModel, !relationtViewModel.IsFinished);
            _mouseHandlingService.StartOperation(associatedElement, null, relationtViewModel, "drawing");//, !relationtViewModel.IsFinished);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= MouseLeftButtonDown;
            this.AssociatedObject.PreviewMouseLeftButtonUp -= MouseLeftButtonPreviewUp;
            this.AssociatedObject.MouseMove -= MouseMove;
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Element MouseLeftButtonDown");
            if (!IsEnabled) return;

            FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
            BaseDiagramItemViewModel relationViewModel = (BaseDiagramItemViewModel)associatedElement.DataContext;
            //_mouseHandlingService.StartDragOrSelectOperation(associatedElement, null, relationViewModel, false);
            _mouseHandlingService.StartOperation(associatedElement, null, relationViewModel, "dragging");//, false);
            e.Handled = true;
        }

        private void MouseLeftButtonPreviewUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Relation MouseLeftButtonPreviewUp");
            FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
            RelationViewModel relationViewModel = (RelationViewModel)associatedElement.DataContext;
            Point mousePosition = e.GetPosition(ParentCanvas);
            if (_mouseHandlingService.CurrentViewModelElement == relationViewModel && !relationViewModel.IsFinished)
            {
                if (!_mouseHandlingService.IsMouseInCanvas(mousePosition,ParentCanvas))
                {
                    Debug.WriteLine("Relation MouseLeftButtonPreviewUp inside canvas");
                    RelationDeleteWhenOutsideCanvasCommand.Execute(null);
                } 
                _mouseHandlingService.EndDragOperation();
            }
        }


        private void MouseMove(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("LineBehavior MouseMove");
            Point mousePosition = e.GetPosition(ParentCanvas);
            //_mouseHandlingService.UpdateDragOperationWhenDrawingRelation(mousePosition, ParentCanvas);
            _mouseHandlingService.UpdateOperation(mousePosition, ParentCanvas, null);
        }

    }
}