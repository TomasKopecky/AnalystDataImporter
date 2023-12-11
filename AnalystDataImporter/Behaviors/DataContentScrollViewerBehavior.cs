using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AnalystDataImporter.Services;

namespace AnalystDataImporter.Behaviors
{
    public class DataContentScrollViewerBehavior : Behavior<ScrollViewer>
    {
        public ScrollViewer HeadingScrollViewer
        {
            get => SharedBehaviorProperties.GetHeadingScrollViewer(this);
            set => SharedBehaviorProperties.SetHeadingScrollViewer(this, value);
        }

        public DataGrid HeadingDataGrid
        {
            get => SharedBehaviorProperties.GetHeadingDataGrid(this);
            set => SharedBehaviorProperties.SetHeadingDataGrid(this, value);
        }

        /// <summary>
        /// Metoda volaná při připojení chování k objektu - tedy při jeho vytvoření
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociateEventHandlers();
        }

        private void AssociateEventHandlers()
        {
            AssociatedObject.ScrollChanged += OnScrollChanged;
            AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //if (HeadingDataGrid != null)
                //AssociatedObject.Height = HeadingDataGrid.ActualHeight;
            //AssociatedObject.Height = HeadingDataGrid.Height;

        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            DisassociateEventHandlers();
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e) 
        {
            if (e.HorizontalChange != 0)
            {
                //double rozdil = HeadingScrollViewer.ScrollableWidth - AssociatedObject.ScrollableWidth;
                //HeadingScrollViewer.Offs

                //HeadingScrollViewer.Width = AssociatedObject.ActualWidth;
                //if (HeadingScrollViewer.HorizontalOffset < HeadingScrollViewer.ScrollableWidth)
                    HeadingScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset);
                // The change was initiated by the horizontal scrollbar
            }
        }

        ///// <summary>
        ///// Odpojení mouse services při destrukci objektu - detail
        ///// </summary>
        private void DisassociateEventHandlers()
        {
            AssociatedObject.ScrollChanged += OnScrollChanged;
            AssociatedObject.Loaded += OnLoaded;
        }
    }
}
