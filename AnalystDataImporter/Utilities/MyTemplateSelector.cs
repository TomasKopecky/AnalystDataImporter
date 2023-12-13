using System.Windows;
using System.Windows.Controls;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Utilities
{
    public class MyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ElementTemplate { get; set; }
        public DataTemplate RelationTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ElementViewModel)
                return ElementTemplate;
            else if (item is RelationViewModel)
                return RelationTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
