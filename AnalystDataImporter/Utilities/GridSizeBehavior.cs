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

        public Canvas ParentCanvas
        {
            get { return SharedBehaviorProperties.GetParentCanvas(this); }
            set { SharedBehaviorProperties.SetParentCanvas(this, value); }
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

                if (!ElementViewModel.temporary)
                {
                    // úprava pozice při změně velikosti, pokud je grid mimo canvas - typicky při vytvoření nové elementu s částí elipsy mimo canvas
                    if (ElementViewModel.XPosition < 0)
                    {
                        ElementViewModel.XPosition = 0;
                    }

                    if (ElementViewModel.XPosition + ElementViewModel.Width > ParentCanvas.ActualWidth)
                    {
                        ElementViewModel.XPosition = ParentCanvas.ActualWidth - ElementViewModel.Width;
                    }

                    if (ElementViewModel.YPosition < 0)
                    {
                        ElementViewModel.YPosition = 0;
                    }

                    if (ElementViewModel.YPosition + ElementViewModel.Height > ParentCanvas.ActualHeight)
                    {
                        ElementViewModel.YPosition = ParentCanvas.ActualHeight - ElementViewModel.Height;
                    }
                }
            }

            var i = ParentCanvas;
        }
    }
}