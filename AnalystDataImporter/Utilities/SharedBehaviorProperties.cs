using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace AnalystDataImporter.Utilities
{
    public static class SharedBehaviorProperties
    {
        public static readonly DependencyProperty ParentCanvasProperty = DependencyProperty.RegisterAttached(
            "ParentCanvas",
            typeof(Canvas),
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
    }

}
