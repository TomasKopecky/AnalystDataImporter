using System.Windows;

namespace AnalystDataImporter.Services
{
    public static class BehaviorServices
    {
        public static readonly DependencyProperty MessageServiceProperty =
            DependencyProperty.RegisterAttached(
                "MessageService",
                typeof(IMessageService),
                typeof(BehaviorServices),
                new PropertyMetadata(null));

        public static void SetMessageService(DependencyObject element, IMessageService value)
        {
            element.SetValue(MessageServiceProperty, value);
        }

        public static IMessageService GetMessageService(DependencyObject element)
        {
            return (IMessageService)element.GetValue(MessageServiceProperty);
        }
    }
}