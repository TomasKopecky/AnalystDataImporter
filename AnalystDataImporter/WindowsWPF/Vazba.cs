using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AnalystDataImporter.WindowsWPF
{
    public class Vazba
    {
        public int Id { get; set; }
        public Objekt StartObjekt { get; set; }
        public Objekt EndObjekt { get; set; }
        public Line Line { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public Vazba(int id, Objekt startObjekt, Objekt endObjekt, Line line)
        {
            Id = id;
            StartObjekt = startObjekt;
            EndObjekt = endObjekt;
            Line = line;
        }

        public void Highlight()
        {
            Line.Stroke = Brushes.DodgerBlue; // Barva označení
            Line.StrokeThickness = 3; // Tloušťka označení
        }

        public void ResetAppearance()
        {
            Line.Stroke = Brushes.Black; // Resetuj barvu čáry
            Line.StrokeThickness = 1; // Resetuj tloušťku čáry
        }
    }
}
