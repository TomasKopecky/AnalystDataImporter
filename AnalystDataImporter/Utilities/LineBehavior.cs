using Microsoft.Xaml.Behaviors;
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
    internal class LineBehavior : Behavior<UIElement>
    {
        private IMouseHandlingService _mouseHandlingService;

        // Define the DependencyProperty
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            nameof(IsEnabled),
            typeof(bool),
            typeof(LineBehavior),
            new PropertyMetadata(null)); // new PropertyMetadata(true, OnIsEnabledChanged));

        // Property to get and set the DependencyProperty
        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown += MouseLeftButtonDown;
            _mouseHandlingService = ServiceLocator.Current.GetService<IMouseHandlingService>();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= MouseLeftButtonDown;
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Element MouseLeftButtonDown");
            if (IsEnabled)
            {
                FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
                BaseDiagramItemViewModel elementViewModel = (BaseDiagramItemViewModel)associatedElement.DataContext;
                _mouseHandlingService.StartDragOrSelectOperation(associatedElement, null, elementViewModel,false);
                e.Handled = true;
            }
        }

    }
}