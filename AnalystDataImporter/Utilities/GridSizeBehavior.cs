using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Utilities
{
    public class GridSizeBehavior : Behavior<Grid>
    {
        public static readonly DependencyProperty ElementViewModelProperty =
            DependencyProperty.Register(
                "ElementViewModel",
                typeof(ElementViewModel),
                typeof(GridSizeBehavior),
                new PropertyMetadata(null));

        public ElementViewModel ElementViewModel
        {
            get { return (ElementViewModel)GetValue(ElementViewModelProperty); }
            set { SetValue(ElementViewModelProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SizeChanged += OnSizeChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.SizeChanged -= OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is Grid grid && ElementViewModel != null)
            {
                ElementViewModel.Width = grid.ActualWidth;
                ElementViewModel.Height = grid.ActualHeight;
            }
        }
    }
}