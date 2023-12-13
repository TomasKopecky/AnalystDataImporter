using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AnalystDataImporter.Behaviors
{
    public static class SharedBehaviorProperties
    {


        public static readonly DependencyProperty ParentCanvasProperty = DependencyProperty.RegisterAttached(
            "ParentCanvas",
            typeof(Canvas),
            typeof(SharedBehaviorProperties),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ChangeCursorCommandProperty =
            DependencyProperty.RegisterAttached(
                "ChangeCursorCommand",
                typeof(ICommand),
                typeof(SharedBehaviorProperties),
                new PropertyMetadata(null));

        public static readonly DependencyProperty HeadingDataGridProperty = DependencyProperty.RegisterAttached(
            "HeadingDataGrid",
            typeof(DataGrid),
            typeof(SharedBehaviorProperties),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ContentDataGridProperty = DependencyProperty.RegisterAttached(
            "ContentDataGrid",
            typeof(DataGrid),
            typeof(SharedBehaviorProperties),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ScrollViewerWithDataGridsProperty = DependencyProperty.RegisterAttached(
            "ScrollViewerWithDataGrids",
            typeof(ScrollViewer),
            typeof(SharedBehaviorProperties),
            new PropertyMetadata(null));

        public static readonly DependencyProperty HeadingScrollViewerProperty = DependencyProperty.RegisterAttached(
            "HeadingScrollViewer",
            typeof(ScrollViewer),
            typeof(SharedBehaviorProperties),
            new PropertyMetadata(null));

        public static Canvas GetParentCanvas(DependencyObject obj)
        {
            return (Canvas)obj.GetValue(ParentCanvasProperty);
        }

        public static void SetParentCanvas(DependencyObject obj, Canvas value)
        {
            obj.SetValue(ParentCanvasProperty, value);
        }

        public static DataGrid GetHeadingDataGrid(DependencyObject obj)
        {
            return (DataGrid)obj.GetValue(HeadingDataGridProperty);
        }

        public static void SetHeadingDataGrid(DependencyObject obj, DataGrid value)
        {
            obj.SetValue(HeadingDataGridProperty, value);
        }

        public static DataGrid GetContentDataGrid(DependencyObject obj)
        {
            return (DataGrid)obj.GetValue(ContentDataGridProperty);
        }

        public static void SetContentDataGrid(DependencyObject obj, DataGrid value)
        {
            obj.SetValue(ContentDataGridProperty, value);
        }

        public static ScrollViewer GetScrollViewerWithDataGrids(DependencyObject obj)
        {
            return (ScrollViewer)obj.GetValue(ScrollViewerWithDataGridsProperty);
        }

        public static void SetScrollViewerWithDataGrids(DependencyObject obj, ScrollViewer value)
        {
            obj.SetValue(ScrollViewerWithDataGridsProperty, value);
        }

        public static ScrollViewer GetHeadingScrollViewer(DependencyObject obj)
        {
            return (ScrollViewer)obj.GetValue(HeadingScrollViewerProperty);
        }

        public static void SetHeadingScrollViewer(DependencyObject obj, ScrollViewer value)
        {
            obj.SetValue(HeadingScrollViewerProperty, value);
        }

        public static ICommand GetChangeCursorCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ChangeCursorCommandProperty);
        }

        public static void SetChangeCursorCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ChangeCursorCommandProperty, value);
        }

        ///// <summary>
        ///// Posílání command do CanvasViewModel nebo GridViewModel pro změnu kurzoru myši v canvas
        ///// </summary>
        public static void UpdateCursor(ICommand command, string cursorType)
        {
            if (command != null && command.CanExecute(cursorType))
            {
                command.Execute(cursorType);
            }
        }
    }

}
