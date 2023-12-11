using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
        private IMouseHandlingService _mouseHandlingService; // proměnná pro přiřazení service pro obsluhu mouse events
        private SharedStatesService _sharedStateService;
        private DataGridColumn selectedColumn;
        private bool _isDragging; // ndikace zda je aktivní mód tažení sloupce gridu
        private int draggedColumnIndex;
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
            // Zde připojujeme service IMouseHandlingService
            _mouseHandlingService = ServiceLocator.Current.GetService<IMouseHandlingService>();
            _sharedStateService = ServiceLocator.Current.GetService<SharedStatesService>();
            AssociateEventHandlers();
        }

        //private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (e.NewValue is GridViewModel gridViewModel)
        //    {
        //        // Now you have access to GridViewModel
        //        // You can also move your column collection change event subscription here if needed
        //    }
        //}


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
            //AssociatedObject.AutoGeneratingColumn += OnAutoGeneratingColumn;

            //if (AssociatedObject is FrameworkElement frameworkElement)
            //{
            //    frameworkElement.DataContextChanged += OnDataContextChanged;
            //}

            //var gridViewModel = AssociatedObject;

            FrameworkElement associatedElement = (FrameworkElement)AssociatedObject;
            GridViewModel gridViewModel = (GridViewModel)associatedElement.DataContext;
            if (gridViewModel != null)
            {
                //gridViewModel.Columns.CollectionChanged += OnColumnsCollectionChanged;
                //foreach (var column in gridViewModel.Columns)
                //{
                //    column.PropertyChanged += OnColumnPropertyChanged;
                //}
            }
        }

        //private void OnColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    // Handle adding/removing subscriptions based on changes in the collection
        //    if (e.OldItems != null)
        //    {
        //        foreach (TableColumnViewModel oldItem in e.OldItems)
        //        {
        //            oldItem.PropertyChanged -= OnColumnPropertyChanged;
        //        }
        //    }

        //    if (e.NewItems != null)
        //    {
        //        foreach (TableColumnViewModel newItem in e.NewItems)
        //        {
        //            newItem.PropertyChanged += OnColumnPropertyChanged;
        //        }
        //    }
        //}


        private void OnColumnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(AssociatedObject is DataGrid datagrid)) return;

            if (e.PropertyName != nameof(TableColumnViewModel.Width)) return;

            //GridViewModel gridViewModel = AssociatedObject.DataContext as GridViewModel;

            TableColumnViewModel changedColumnViewModel = sender as TableColumnViewModel;

            // V případě, že instance této třídy patří k data grid elementu který reprezentuje heading data grid, změň
            if (_isHeadingGrid && changedColumnViewModel.Temporary)
            {
                Debug.WriteLine("DataGridDragBehavior: OnColumnPropertyChanged - _isHeadingGrid && changedColumnViewModel.Temporary");
                datagrid.Columns[changedColumnViewModel.Index].Width = changedColumnViewModel.Width;
                changedColumnViewModel.Temporary = false;
                //int i = 0;
                //foreach (var column in gridViewModel.Columns)
                //{
                //    //if (column.Temporary == true)
                //    datagrid.Columns[i].Width = column.Width;
                //    i++;
                //}
            }
            else if (!_isHeadingGrid)
            {
                Debug.WriteLine("DataGridDragBehavior: OnColumnPropertyChanged - !_isHeadingGrid");
                datagrid.Columns[changedColumnViewModel.Index].Width = changedColumnViewModel.Width;
            }
        }

        //private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        //{
        //    if (AssociatedObject.DataContext is GridViewModel gridViewModel)
        //    {
        //        // Logic to set the column header based on GridViewModel
        //        // For example, you might use the property name to determine the header
        //        var propertyName = e.PropertyName; // Or any other logic to determine the header
        //        //var columnHeader = gridViewModel.GetHeaderForProperty(propertyName);
        //        //if (columnHeader != null)
        //        //{
        //        //    e.Column.Header = columnHeader;
        //        //}
        //        }
        //}

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is GridViewModel gridViewModel)
            {
                if (AssociatedObject is DataGrid dataGrid)
                {
                    //if (dataGrid.Columns.Count == gridViewModel.Columns.Count)
                    //{
                    //foreach (var column in gridViewModel.Columns)
                    //    dataGrid.Columns.Add
                    //foreach (DataGridColumn column in dataGrid.Columns)
                    //    column.Header = gridViewModel.Columns[column.DisplayIndex];
                    //}     
                }
            }
        }

        //private void OnColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    DataGrid dataGrid = AssociatedObject as DataGrid;
        //    dataGrid.Columns.Clear();

        //    foreach (var columnInfo in (ObservableCollection<ColumnInfo>)sender)
        //    {
        //        var column = new DataGridTextColumn
        //        {
        //            Header = columnInfo.Header,
        //            Binding = new Binding(columnInfo.BindingPath)
        //        };

        //        dataGrid.Columns.Add(column);
        //    }
        //}

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is DataGrid datagrid)) return;

            if (datagrid.Items.Count == 0)
                _isHeadingGrid = true;
            else
                _isHeadingGrid = false;


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
                //gridViewModel.Columns.CollectionChanged += OnColumnsCollectionChanged;
                //int i = 0;
                //foreach (var column in gridViewModel.Columns)
                //{
                //    if (!_isHeadingGrid)
                //        column.Width = datagrid.Columns[i].ActualWidth;
                //    column.PropertyChanged += OnColumnPropertyChanged;
                //    i++;
                //}
            }

        }

        private void PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("DataGridDragBehavior: PreviewMouseMove");
            if (!(sender is DataGrid datagrid)) return;

            if (_isheadingColumnWidthDragging)
            {
                //GridViewModel gridViewModel = AssociatedObject.DataContext as GridViewModel;
                ContentDataGrid.Columns[_headingColumnWidthDraggedIndex].Width = datagrid.Columns[_headingColumnWidthDraggedIndex].Width;
                //gridViewModel.Columns[_headingColumnWidthDraggedIndex].Width = datagrid.Columns[_headingColumnWidthDraggedIndex].ActualWidth;
                Debug.WriteLine("DataGridDragBehavior: PreviewMouseMove - _isheadingColumnWidthDragging true");
            }
        }

        //Když dvojklik na thumb, tak autofit na velikost toho obsahového column, nikoliv toho heading

        /// <summary>
        /// Reaguje na pohyb myši. Aktualizuje pozici prvku během přetahování nebo kreslení.
        /// </summary>
        /// <param name="sender">Objekt, který událost vyvolal (typicky UI element).</param>
        /// <param name="e">Data události obsahující informace o pozici myši.</param>
        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (!(sender is DataGrid datagrid)) return;

            else if (!_isDragging)
                SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewMouseOverCursor");
            else
            {
                GridViewModel gridViewModel = (GridViewModel)AssociatedObject.DataContext;
                _mouseHandlingService.StartOperation(AssociatedObject, null, gridViewModel, "dragging");
                Point mousePosition = e.GetPosition(ParentCanvas);
                if (_mouseHandlingService.IsMouseInCanvas(mousePosition, ParentCanvas))
                    SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewDraggingAllowedCursor");
                else
                    SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewDraggingDisallowedCursor");
            }
            e.Handled = true;
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while (dep != null)
            {
                if (dep is Thumb)
                {
                    _isheadingColumnWidthDragging = true;
                    // Find the DataGridColumnHeader from the Thumb
                    var columnHeader = FindParent<DataGridColumnHeader>(dep);
                    if (columnHeader != null)
                    {
                        var mousePosition = e.GetPosition(columnHeader);
                        double relativePosition = mousePosition.X / columnHeader.ActualWidth;
                        //_headingColumnWidthDraggedIndex = columnHeader.Column.DisplayIndex;
                        if (relativePosition < 0.5)
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
                    //e.Handled = true;
                    break;
                }
                else if (dep is DataGridColumnHeader columnHeader)
                {
                    selectedColumn = columnHeader.Column;
                    draggedColumnIndex = columnHeader.Column.DisplayIndex;
                    UpdateCellStyle(selectedColumn);
                    _isDragging = true;
                    e.Handled = true;
                    break;
                }
                else if (dep is DataGridCell cell)
                {
                    selectedColumn = cell.Column;
                    draggedColumnIndex = cell.Column.DisplayIndex;
                    UpdateCellStyle(selectedColumn);
                    _isDragging = true;
                    e.Handled = true;
                    break;
                }

                dep = VisualTreeHelper.GetParent(dep);
            }
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


        //private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    //GridViewModel gridViewModel = (GridViewModel)AssociatedObject.DataContext;

        //    DependencyObject dep = (DependencyObject)e.OriginalSource;

        //    while (dep != null)
        //    {
        //        dep = VisualTreeHelper.GetParent(dep);
        //        if (dep is Thumb)
        //        {
        //            // TODO: Tady se začíná rozšiřovat sloupec v heading column - 
        //            _isheadingColumnWidthDragging = true;
        //            var bb = VisualTreeHelper.GetParent(dep);
        //            break;
        //            //return;
        //        }
        //        else if (dep is DataGridColumnHeader columnHeader)
        //        {
        //            selectedColumn = columnHeader.Column;
        //            draggedColumnIndex = columnHeader.Column.DisplayIndex;
        //            UpdateCellStyle(selectedColumn);
        //            _isDragging = true;
        //            e.Handled = true;
        //            break;
        //        }
        //        else if (dep is DataGridCell cell)
        //        {
        //            // A cell was clicked
        //            selectedColumn = cell.Column;
        //            draggedColumnIndex = cell.Column.DisplayIndex;
        //            UpdateCellStyle(selectedColumn);
        //            _isDragging = true;
        //            e.Handled = true;
        //            break;
        //        }
        //    }
        //}

        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("OnPreviewMouseLeftButtonUp");
            if (_isheadingColumnWidthDragging)
            {
                Debug.WriteLine("OnPreviewMouseLeftButtonUp - _isheadingColumnWidthDragging");
                _isheadingColumnWidthDragging = false;
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
                    GetDraggedGridViewColumnCommand.Execute(draggedColumnIndex);
                }
                //SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewLeaveCursor");
                _isDragging = false;
            }
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

            //GridViewModel gridViewModel = AssociatedObject.DataContext as GridViewModel;
            //if (gridViewModel != null)
            //{
            //    gridViewModel.Columns.CollectionChanged -= OnColumnsCollectionChanged;
            //    foreach (var column in gridViewModel.Columns)
            //    {
            //        column.PropertyChanged -= OnColumnPropertyChanged;
            //    }
            //    //AssociatedObject.AutoGeneratingColumn -= OnAutoGeneratingColumn;

            //    //if (AssociatedObject is FrameworkElement frameworkElement)
            //    //{
            //    //    frameworkElement.DataContextChanged -= OnDataContextChanged;
            //    //    if (frameworkElement.DataContext is GridViewModel gridViewModel)
            //    //    {
            //    //        gridViewModel.Columns.CollectionChanged -= OnColumnsCollectionChanged;
            //    //    }
            //    //}


            //}
        }

        private void OnColumnWidthsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Logic to update DataGrid column widths based on ViewModel
            // This could involve iterating over AssociatedObject.Columns
            // and updating their widths to match the ViewModel's collection
        }

        // Vlastnosti DependencyProperty - návaznost na výše uvedené commands - připojení k view
        //public static readonly DependencyProperty ChangeFinishDrawingElementProperty = DependencyProperty.Register(
        //    nameof(FinishDrawingElementCommand), typeof(ICommand),
        //    typeof(DataGridDragBehavior),
        //    new PropertyMetadata(null));

        //public ICommand FinishDrawingElementCommand
        //{
        //    get => (ICommand)GetValue(ChangeFinishDrawingElementProperty);
        //    set => SetValue(ChangeFinishDrawingElementProperty, value);
        //}

    }
}
