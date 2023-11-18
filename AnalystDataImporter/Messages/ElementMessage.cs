using System.Windows;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Messages
{
    public class ElementMessage
    {
        public ElementViewModel Element { get; set; }
        public RelationViewModel Relation { get; set; }
        public string Context { get; set; }
        public Point MousePointInElement { get; set; }
    }
}