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
    /// <summary>
    /// Chování pro správu událostí myši na objektu typu Line.
    /// </summary>
    internal class LineBehavior : Behavior<UIElement>
    {
        // Privátní proměnná pro service IMouseHandlingService
        private IMouseHandlingService _mouseHandlingService;

        // Vlastnosti DependencyProperty
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            nameof(IsEnabled),
            typeof(bool),
            typeof(LineBehavior),
            new PropertyMetadata(null)); // new PropertyMetadata(true, OnIsEnabledChanged));

        public static readonly DependencyProperty RelationDeleteWhenOutsideCanvasCommandProperty = DependencyProperty.Register(
            nameof(RelationDeleteWhenOutsideCanvasCommand), typeof(ICommand),
            typeof(LineBehavior),
            new PropertyMetadata(null));

        /// <summary>
        /// Zda je chování aktivní.
        /// </summary>
        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Příkaz pro odstranění vazby, pokud je kreslena mimo plátno.
        /// </summary>
        public ICommand RelationDeleteWhenOutsideCanvasCommand
        {
            get => (ICommand)GetValue(RelationDeleteWhenOutsideCanvasCommandProperty);
            set => SetValue(RelationDeleteWhenOutsideCanvasCommandProperty, value);
        }

        /// <summary>
        /// Plátno, ke kterému je chování připojeno.
        /// </summary>
        public Canvas ParentCanvas
        {
            get => SharedBehaviorProperties.GetParentCanvas(this);
            set => SharedBehaviorProperties.SetParentCanvas(this, value);
        }

        /// <summary>
        /// Metoda volaná při připojení chování k objektu - tedy při jeho vytvoření
        /// </summary>

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += MouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += MouseLeftButtonPreviewUp;
            AssociatedObject.MouseMove += MouseMove;

            // Zde připojujeme service IMouseHandlingService
            _mouseHandlingService = ServiceLocator.Current.GetService<IMouseHandlingService>();

            FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
            RelationViewModel relationViewModel = (RelationViewModel)associatedElement.DataContext;
            
            // Zde začíná kreslení vazby, tedy ihned po jejím vytvoření
            _mouseHandlingService.StartOperation(associatedElement, null, relationViewModel, "drawing");
        }

        /// <summary>
        /// Metoda volaná při odpojení chování od objektu.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= MouseLeftButtonDown;
            this.AssociatedObject.PreviewMouseLeftButtonUp -= MouseLeftButtonPreviewUp;
            this.AssociatedObject.MouseMove -= MouseMove;
        }

        /// <summary>
        /// Obsluha události MouseLeftButtonDown.
        /// </summary>
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Element MouseLeftButtonDown");
            if (!IsEnabled) return;

            FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
            BaseDiagramItemViewModel relationViewModel = (BaseDiagramItemViewModel)associatedElement.DataContext;
            _mouseHandlingService.StartOperation(associatedElement, null, relationViewModel, "dragging");
            e.Handled = true;
        }

        /// <summary>
        /// Obsluha události PreviewMouseLeftButtonUp.
        /// </summary>
        private void MouseLeftButtonPreviewUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Relation MouseLeftButtonPreviewUp");
            FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
            RelationViewModel relationViewModel = (RelationViewModel)associatedElement.DataContext;
            Point mousePosition = e.GetPosition(ParentCanvas);
            
            if (_mouseHandlingService.CurrentViewModelElement != relationViewModel || relationViewModel.IsFinished)
                return;
            
            if (!_mouseHandlingService.IsMouseInCanvas(mousePosition,ParentCanvas))
            {
                //Debug.WriteLine("Relation MouseLeftButtonPreviewUp inside canvas");
                RelationDeleteWhenOutsideCanvasCommand.Execute(null);
            } 
            _mouseHandlingService.EndDragOperation();
        }

        /// <summary>
        /// Obsluha události MouseMove.
        /// </summary>
        private void MouseMove(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine("LineBehavior MouseMove");
            Point mousePosition = e.GetPosition(ParentCanvas);

            // metoda v service MouseHandlingService pro kreslení vazby - logika i podmínky jsou řešeny v dané metodě UpdateOperation
            _mouseHandlingService.UpdateOperation(mousePosition, ParentCanvas, null);
        }

    }
}