using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
            AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
            AssociatedObject.MouseLeave += OnMouseLeave;
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
                // Handle collection changes to dynamically update columns
                //gridViewModel.Columns.CollectionChanged += OnColumnsCollectionChanged;
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

        private void OnColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DataGrid dataGrid = AssociatedObject as DataGrid;
            dataGrid.Columns.Clear();

            foreach (var columnInfo in (ObservableCollection<ColumnInfo>)sender)
            {
                var column = new DataGridTextColumn
                {
                    Header = columnInfo.Header,
                    Binding = new Binding(columnInfo.BindingPath)
                };

                dataGrid.Columns.Add(column);
            }
        }

        /// <summary>
        /// Reaguje na pohyb myši. Aktualizuje pozici prvku během přetahování nebo kreslení.
        /// </summary>
        /// <param name="sender">Objekt, který událost vyvolal (typicky UI element).</param>
        /// <param name="e">Data události obsahující informace o pozici myši.</param>
        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging)
                SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewMouseOverCursor");
            else
            {
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
            GridViewModel gridViewModel = (GridViewModel)AssociatedObject.DataContext;

            DependencyObject dep = (DependencyObject)e.OriginalSource;

            // Traverse the visual tree upwards to find either a DataGridColumnHeader or a DataGridCell
            while (dep != null && !(dep is DataGridColumnHeader) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep is DataGridColumnHeader columnHeader)
            {
                // Column header was clicked
                selectedColumn = columnHeader.Column;
                draggedColumnIndex = columnHeader.Column.DisplayIndex;
            }
            else if (dep is DataGridCell cell)
            {
                // A cell was clicked
                selectedColumn = cell.Column;
                draggedColumnIndex = cell.Column.DisplayIndex;
            }

            UpdateCellStyle(selectedColumn);
            _isDragging = true;
            _mouseHandlingService.StartOperation(AssociatedObject, null, gridViewModel, "dragging");
            //SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewDraggingDisallowedCursor");
            //var listView = sender as ListView;
            //var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ////foreach (var l in listView.Items)
            ////{
            ////}
            e.Handled = true;
        }

        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
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

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            //_isDragging = false;
            if (!_isDragging)
            {
                SharedBehaviorProperties.UpdateCursor(ChangeCursorCommand, "GridViewLeaveCursor");
                //e.Handled = true;
            }
            _isDragging = false;
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
            AssociatedObject.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp -= OnPreviewMouseLeftButtonUp;
            AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
            AssociatedObject.MouseLeave -= OnMouseLeave;
            //AssociatedObject.AutoGeneratingColumn -= OnAutoGeneratingColumn;

            //if (AssociatedObject is FrameworkElement frameworkElement)
            //{
            //    frameworkElement.DataContextChanged -= OnDataContextChanged;
            //    if (frameworkElement.DataContext is GridViewModel gridViewModel)
            //    {
            //        gridViewModel.Columns.CollectionChanged -= OnColumnsCollectionChanged;
            //    }
            //}


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
