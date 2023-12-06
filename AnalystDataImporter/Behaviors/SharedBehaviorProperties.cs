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

        public static Canvas GetParentCanvas(DependencyObject obj)
        {
            return (Canvas)obj.GetValue(ParentCanvasProperty);
        }
        
        public static void SetParentCanvas(DependencyObject obj, Canvas value)
        {
            obj.SetValue(ParentCanvasProperty, value);
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
        ///// Posílání command do CanvasViewModel pro změnu kurzoru myši v canvas
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
