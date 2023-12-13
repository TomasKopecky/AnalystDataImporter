using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AnalystDataImporter.WindowsWPF
{
    public class Objekt
    {
        public int ID { get; set; }
        public Ellipse Shape { get; set; }
        public TextBlock Popisek { get; set; }

        public Objekt() { }

        public Objekt(int id)
        {
            ID = id;
        }

        public Objekt(int id, Ellipse shape)
        {
            ID = id;
            Shape = shape;
        }

        public void Highlight()
        {
            Shape.Fill = Brushes.LightCyan; // Barva výplně
            Shape.Stroke = Brushes.DodgerBlue; // Barva označení
            Shape.StrokeThickness = 3; // Tloušťka označení
        }

        public void ResetAppearance()
        {
            Shape.Fill = Brushes.White; // Barva výplně
            Shape.Stroke = Brushes.Black; // Barva označení
            Shape.StrokeThickness = 1; // Tloušťka označení
        }
    }
}
