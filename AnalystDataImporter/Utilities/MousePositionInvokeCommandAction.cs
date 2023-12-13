using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AnalystDataImporter.Utilities
{
    public class MousePositionInvokeCommandAction : TriggerAction<Canvas>
    {
        public static readonly DependencyProperty CommandParameterProperty =
    DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(MousePositionInvokeCommandAction), new PropertyMetadata(null));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(MousePositionInvokeCommandAction), new PropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (Command != null && parameter is MouseEventArgs e)
            {
                if (CommandParameter is Canvas canvas)
                {
                    ExecuteCommand(canvas);
                }
                else
                {
                    Point point = e.GetPosition(AssociatedObject);
                    ExecuteCommand(point);
                }
            }
        }

        private void ExecuteCommand(object parameter)
        {
            if (Command.CanExecute(parameter))
            {
                Command.Execute(parameter);
            }
        }
    }
}