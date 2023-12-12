using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using AnalystDataImporter.Services;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Behaviors
{
    public class DataGridDragBehavior : Behavior<DataGrid>
    {
        private Point mouseDownPosition;
        private IMouseHandlingService _mouseHandlingService; // proměnná pro přiřazení service pro obsluhu mouse events
        private SharedStatesService _sharedStateService;
        private DataGridColumn selectedColumn;
        private bool _isDragging; // ndikace zda je aktivní mód tažení sloupce gridu
        //private int _headingColumnWidthDraggedIndex;
        private bool _isHeadingGrid;
        private bool _isheadingColumnWidthDragging;
        private int _headingColumnWidthDraggedIndex;

        public static readonly DependencyProperty GetDraggedGridViewColumnCommandProperty =
            DependencyProperty.Register(
                nameof(GetDraggedGridViewColumnCommand), typeof(ICommand),
                typeof(DataGridDragBehavior),
                new PropertyMetadata(null));

        public ICommand GetDraggedGridViewColumnCommand
        {
            get => (ICommand)GetValue(GetDraggedGridViewColumnCommandProperty);
            set => SetValue(GetDraggedGridViewColumnCommandProperty, value);
        }

        /// <summary>
        /// Plátno, ke kterému je chování připojeno - sdíleno z SharedBehaviorProperties třídy
        /// </summary>
        public Canvas ParentCanvas
        {
            get => SharedBehaviorProperties.GetParentCanvas(this);
            set => SharedBehaviorProperties.SetParentCanvas(this, value);
        }

        public DataGrid HeadingDataGrid
        {
            get => SharedBehaviorProperties.GetHeadingDataGrid(this);
            set => SharedBehaviorProperties.SetHeadingDataGrid(this, value);
        }

        public DataGrid ContentDataGrid
        {
            get => SharedBehaviorProperties.GetContentDataGrid(this);
            set => SharedBehaviorProperties.SetContentDataGrid(this, value);
        }

        public ScrollViewer ScrollViewerWithDataGrids
        {
            get => SharedBehaviorProperties.GetScrollViewerWithDataGrids(this);
            set => SharedBehaviorProperties.SetScrollViewerWithDataGrids(this, value);
        }

        public ScrollViewer HeadingScrollViewer
        {
            get => SharedBehaviorProperties.GetHeadingScrollViewer(this);
            set => SharedBehaviorProperties.SetHeadingScrollViewer(this, value);
        }
        public ICommand ChangeCursorCommand
        {
            get => SharedBehaviorProperties.GetChangeCursorCommand(this);
            set => SharedBehaviorProperties.SetChangeCursorCommand(this, value);
        }

        /// <summary>
        /// Metoda volaná při připojení chování k objektu - tedy při jeho vytvoření
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            InitializeServices();
            AssociateEventHandlers();
        }

        private void InitializeServices()
        {
            _mouseHandlingService = ServiceLocator.Current.GetService<IMouseHandlingService>();
            _sharedStateService = ServiceLocator.Current.GetService<SharedStatesService>();
        }

        ///// <summary>
        ///// Připojení mouse services při vytvoření objektu
        ///// </summary>
        private void AssociateEventHandlers()
        {
            AssociatedObject.MouseMove += MouseMove;
            AssociatedObject.PreviewMouseMove += PreviewMouseMove;
            AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
            AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
            AssociatedObject.MouseLeave += OnMouseLeave;
            AssociatedObject.PreviewMouseWheel += OnPreviewMouseWheel;
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.PreviewMouseDoubleClick += OnPreviewMouseDoubleClick;
        }


        private void OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (!(sender is DataGrid datagrid)) return;
            Debug.WriteLine("DataGridDragBehavior: OnPreviewMouseDoubleClick");
            if (TryGetColumnHeaderFromEvent(e, out var columnHeader))
            {
                AdjustColumnWidth(columnHeader, e.GetPosition(columnHeader));
                e.Handled = true;
            }
        }

        // Method to try getting DataGridColumnHeader from event
        private bool TryGetColumnHeaderFromEvent(MouseEventArgs e, out DataGridColumnHeader columnHeader)
        {
            columnHeader = null;
            var depObj = e.OriginalSource as DependencyObject;

            columnHeader = depObj.FindVisualParent<DataGridColumnHeader>();
            return columnHeader != null;
        }

        // Method to adjust column width
        private void AdjustColumnWidth(DataGridColumnHeader columnHeader, Point mousePosition)
        {
            double relativePosition = mousePosition.X / columnHeader.ActualWidth;
            int columnIndex = columnHeader.Column.DisplayIndex;

            if (relativePosition < 0.5 && columnIndex > 0)
            {
                columnIndex--; // Left edge - adjust the preceding column
            }

            ContentDataGrid.Columns[columnIndex].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            AssociatedObject.Columns[columnIndex].Width = ContentDataGrid.Columns[columnIndex].ActualWidth;
        }

        private void HandleColumnWidthDraggingStart(Thumb thumb)
        {
            var columnHeader = thumb.FindVisualParent<DataGridColumnHeader>();
            if (columnHeader != null)
            {
                // Calculate the relative position of the click within the column header
                var mousePosition = Mouse.GetPosition(columnHeader);
                double relativePosition = mousePosition.X / columnHeader.ActualWidth;

                // Determine whether to adjust the current column or the preceding column
                if (relativePosition < 0.5 && columnHeader.Column.DisplayIndex > 0)
                {
                    // Left edge - adjust the preceding column
                    _headingColumnWidthDraggedIndex = columnHeader.Column.DisplayIndex - 1;
                }
                else
                {
                    // Right edge - adjust the current column
                    _headingColumnWidthDraggedIndex = columnHeader.Column.DisplayIndex;
                }
            }
        }



        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is DataGrid datagrid)) return;

            if (datagrid.Items.Count == 0)
                _isHeadingGrid = true;
            else
                _isHeadingGrid = false;


            if (_isHeadingGrid)
            {
                // nastavení výšky heading sloupce přes celou výšku data gridu (dle rodičovského scrollviewer prvku)
                datagrid.ColumnHeaderHeight = datagrid.ActualHeight;
            }

            GridViewModel gridViewModel = AssociatedObject.DataContext as GridViewModel;
            // když je prvek content data grid - nastav width sloupců heading data grid podle prvotní šířky content data grid sloupců
            if (gridViewModel != null && gridViewModel.Columns.Count == datagrid.Columns.Count && !_isHeadingGrid)
            {
                int i = 0;
                foreach (var column in datagrid.Columns)
                {
                    HeadingDataGrid.Columns[i].Width = column.Width;
                    i++;
                }
            }

        }

        //private void PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    Debug.WriteLine("DataGridDragBehavior: PreviewMouseMove");
        //    if (!(sender is DataGrid datagrid)) return;
        //    Point currentMousePosition = e.GetPosition(this.AssociatedObject);
        //    if (_isheadingColumnWidthDragging && mouseDownPosition != currentMousePosition)
        //    {
        //        Debug.WriteLine("DataGridDragBehavior: PreviewMouseMove - _isheadingColumnWidthDragging true && mouseDownPosition != currentMousePosition");
        //        //GridViewModel gridViewModel = AssociatedObject.DataContext as GridViewModel;
        //        ContentDataGrid.Columns[_headingColumnWidthDraggedIndex].Width = datagrid.Columns[_headingColumnWidthDraggedIndex].ActualWidth;
        //        //gridViewModel.Columns[_headingColumnWidthDraggedIndex].Width = datagrid.Columns[_headingColumnWidthDraggedIndex].ActualWidth;

        //    }
        //    else if (!_isDragging)
        //    {
        //        Debug.WriteLine("DataGridDragBehavior: PreviewMouseMove - !dragging");
        //        SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewMouseOverCursor");
        //    }

        //    else
        //    {
        //        GridViewModel gridViewModel = (GridViewModel)AssociatedObject.DataContext;
        //        _mouseHandlingService.StartOperation(AssociatedObject, null, gridViewModel, "dragging");
        //        Point mousePosition = e.GetPosition(ParentCanvas);
        //        if (_mouseHandlingService.IsMouseInCanvas(mousePosition, ParentCanvas))
        //            SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewDraggingAllowedCursor");
        //        else
        //            SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewDraggingDisallowedCursor");
        //    }
        //    //e.Handled = true;
        //}

        /// <summary>
        /// Reaguje na pohyb myši. Aktualizuje pozici prvku během přetahování nebo kreslení.
        /// </summary>
        /// <param name="sender">Objekt, který událost vyvolal (typicky UI element).</param>
        /// <param name="e">Data události obsahující informace o pozici myši.</param>
        //private void MouseMove(object sender, MouseEventArgs e)
        //{
        //    Debug.WriteLine("DataGridDragBehavior: MouseMove");
        //    if (!(sender is DataGrid datagrid)) return;

        //    else if (!_isDragging)
        //    {
        //        Debug.WriteLine("DataGridDragBehavior: MouseMove - !dragging");
        //        SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewMouseOverCursor");
        //    }
        //    else
        //    {
        //        GridViewModel gridViewModel = (GridViewModel)AssociatedObject.DataContext;
        //        _mouseHandlingService.StartOperation(AssociatedObject, null, gridViewModel, "dragging");
        //        Point mousePosition = e.GetPosition(ParentCanvas);
        //        if (_mouseHandlingService.IsMouseInCanvas(mousePosition, ParentCanvas))
        //            SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewDraggingAllowedCursor");
        //        else
        //            SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewDraggingDisallowedCursor");
        //    }
        //    e.Handled = true;
        //}

        private void PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("DataGridDragBehavior: PreviewMouseMove");
            if (!(sender is DataGrid datagrid)) return;

            Point currentMousePosition = e.GetPosition(ParentCanvas);
            if (_isheadingColumnWidthDragging)
            {
                HandleColumnWidthAdjustment(datagrid, currentMousePosition);
            }
            else
            {
                if (_isHeadingGrid)
                    HandleMouseMoveOperation(currentMousePosition);
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("DataGridDragBehavior: MouseMove");
            if (!(sender is DataGrid)) return;
            Point currentMousePosition = e.GetPosition(ParentCanvas);
            if (!_isHeadingGrid)
            {
                Debug.WriteLine("DataGridDragBehavior: MouseMove - !_isHeadingGrid");
                HandleMouseMoveOperation(currentMousePosition);
            }
            e.Handled = true;
        }

        private void HandleColumnWidthAdjustment(DataGrid datagrid, Point currentMousePosition)
        {
            if (mouseDownPosition != currentMousePosition)
            {
                Debug.WriteLine("DataGridDragBehavior: Adjusting Column Width");
                ContentDataGrid.Columns[_headingColumnWidthDraggedIndex].Width = datagrid.Columns[_headingColumnWidthDraggedIndex].ActualWidth;
            }
        }

        private void HandleMouseMoveOperation(Point currentMousePosition)
        {
            if (!_isDragging)
            {
                Debug.WriteLine("DataGridDragBehavior: Not Dragging");
                SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewMouseOverCursor");
            }
            else
            {
                Debug.WriteLine("DataGridDragBehavior: Dragging");
                UpdateCursorBasedOnPosition(currentMousePosition);
            }
        }

        private void UpdateCursorBasedOnPosition(Point mousePosition)
        {
            GridViewModel gridViewModel = (GridViewModel)AssociatedObject.DataContext;
            _mouseHandlingService.StartOperation(AssociatedObject, null, gridViewModel, "dragging");

            Debug.WriteLine("UpdateCursorBasedOnPosition: mouseposition: " + mousePosition);

            if (_mouseHandlingService.IsMouseInCanvas(mousePosition, ParentCanvas))
            {
                Debug.WriteLine("UpdateCursorBasedOnPosition: _mouseHandlingService.IsMouseInCanvas(mousePosition, ParentCanvas)");
                SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewDraggingAllowedCursor");
            }
            else
            {
                Debug.WriteLine("UpdateCursorBasedOnPosition: !_mouseHandlingService.IsMouseInCanvas(mousePosition, ParentCanvas)");
                SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewDraggingDisallowedCursor");
            }
        }


        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("DataGridDragBehavior: OnPreviewMouseLeftButtonDown");
            mouseDownPosition = e.GetPosition(AssociatedObject);
            if (TryGetThumbFromEvent(e, out Thumb thumb))
            {
                _isheadingColumnWidthDragging = true;
                HandleColumnWidthDraggingStart(thumb);
            }
            else if (TryGetHeaderOrCellFromEvent(e, out DataGridColumn column))
            {
                selectedColumn = column;
                _headingColumnWidthDraggedIndex = column.DisplayIndex;
                UpdateCellStyle(selectedColumn);
                _isDragging = true;
                e.Handled = true;
            }
        }

        private bool TryGetThumbFromEvent(MouseButtonEventArgs e, out Thumb thumb)
        {
            thumb = null;
            var depObj = e.OriginalSource as DependencyObject;
            thumb = depObj.FindVisualParent<Thumb>();
            return thumb != null;
        }

        private bool TryGetHeaderOrCellFromEvent(MouseButtonEventArgs e, out DataGridColumn column)
        {
            column = null;
            var depObj = e.OriginalSource as DependencyObject;

            var header = depObj.FindVisualParent<DataGridColumnHeader>();
            if (header != null)
            {
                column = header.Column;
                return true;
            }

            var cell = depObj.FindVisualParent<DataGridCell>();
            if (cell != null)
            {
                column = cell.Column;
                return true;
            }

            return false;
        }


        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentDepObj = VisualTreeHelper.GetParent(child);
            if (parentDepObj == null) return null;

            T parent = parentDepObj as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentDepObj);
            }
        }

        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("OnPreviewMouseLeftButtonUp");
            if (_isheadingColumnWidthDragging)
            {
                Debug.WriteLine("OnPreviewMouseLeftButtonUp - _isheadingColumnWidthDragging");
                _isheadingColumnWidthDragging = false;

                // V případech, kdy se rychle táhne myší během změny šířky sloupce v heading data gridu a mousePreviewUp je proveden mimo data grid, nemusí se operace změny přílušného sloupce v content data gridu provést - proto tato pojistná podmínka
                if (sender is DataGrid datagrid && datagrid.Columns[_headingColumnWidthDraggedIndex].ActualWidth != ContentDataGrid.Columns[_headingColumnWidthDraggedIndex].Width)
                    ContentDataGrid.Columns[_headingColumnWidthDraggedIndex].Width = datagrid.Columns[_headingColumnWidthDraggedIndex].ActualWidth;

            }
            else if (_isDragging)
            {
                _mouseHandlingService.EndDragOperation();
                Point mousePosition = e.GetPosition(ParentCanvas);
                if (!_mouseHandlingService.IsMouseInCanvas(mousePosition, ParentCanvas))
                {
                    SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewLeaveCursor");
                }
                else
                {
                    GridViewModel gridViewModel = (GridViewModel)AssociatedObject.DataContext;
                    GetDraggedGridViewColumnCommand.Execute(_headingColumnWidthDraggedIndex);
                }
                //SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewLeaveCursor");
                _isDragging = false;
            }
            //_isheadingColumnWidthDragging = false;
            ResetCellStyle();
            //SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewMouseOverCursor");
            //e.Handled = true;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewMouseOverCursor");
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //if (sender is DataGrid dataGrid && VisualTreeHelper.GetParent(dataGrid) is Grid grid && grid.Parent is ScrollViewer scrollViewer)
            if (sender is DataGrid dataGrid && ScrollViewerWithDataGrids != null)
            {
                // Calculate the new scroll offset.
                double newOffset = ScrollViewerWithDataGrids.VerticalOffset - (e.Delta/* / 120.0 * 3*/); // Adjust '3' to change scroll speed

                // Clamp the new offset to the valid range.
                newOffset = Math.Max(0, Math.Min(newOffset, ScrollViewerWithDataGrids.ScrollableHeight));

                // Scroll to the new offset.
                ScrollViewerWithDataGrids.ScrollToVerticalOffset(newOffset);

                e.Handled = true;
            }
        }


        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("OnMouseLeave");
            //_isDragging = false;
            if (!_isDragging)
            {
                SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewLeaveCursor");
                //e.Handled = true;
            }
            //_isDragging = false;
        }

        private void UpdateCellStyle(DataGridColumn selectedColumn)
        {
            if (selectedColumn == null) return;

            var cellStyle = new Style(typeof(DataGridCell));
            var headerStyle = new Style(typeof(DataGridColumnHeader));
            if (selectedColumn != null)
            {
                cellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(Colors.RoyalBlue)));
                cellStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, new SolidColorBrush(Colors.White)));
                headerStyle.Setters.Add(new Setter(DataGridColumnHeader.BackgroundProperty, new SolidColorBrush(Colors.LightBlue/*SystemColors.MenuHighlightColor*/)));
                headerStyle.Setters.Add(new Setter(DataGridColumnHeader.ForegroundProperty, new SolidColorBrush(Colors.White)));
            }

            selectedColumn.CellStyle = cellStyle;
            selectedColumn.HeaderStyle = headerStyle;

            //foreach (var column in AssociatedObject.Columns)
            //{
            //    column.CellStyle = cellStyle;
            //}
        }

        private void ResetCellStyle()
        {
            if (selectedColumn == null) return;

            var defaultCellStyle = new Style(typeof(DataGridCell));
            var defaultHeaderStyle = new Style(typeof(DataGridColumnHeader));

            selectedColumn.CellStyle = defaultCellStyle; // Set to null to revert to default style
            selectedColumn.HeaderStyle = defaultHeaderStyle;

        }

        /// <summary>
        /// Metoda volaná při odpojení chování od objektu - tedy destrukci
        /// </summary>
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
            AssociatedObject.MouseMove -= MouseMove;
            AssociatedObject.PreviewMouseMove += PreviewMouseMove;
            AssociatedObject.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp -= OnPreviewMouseLeftButtonUp;
            AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
            AssociatedObject.MouseLeave -= OnMouseLeave;
            AssociatedObject.PreviewMouseWheel -= OnPreviewMouseWheel;
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.PreviewMouseDoubleClick -= OnPreviewMouseDoubleClick;
        }
    }
}

public static class DependencyObjectExtensions
{
    public static T FindVisualParent<T>(this DependencyObject child) where T : DependencyObject
    {
        while (child != null)
        {
            if (child is T correctlyTyped)
            {
                return correctlyTyped;
            }

            child = VisualTreeHelper.GetParent(child);
        }

        return null;
    }
}