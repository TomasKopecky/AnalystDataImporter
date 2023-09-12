using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.Classes.DataStructure
{
    public class Object
    {
        public int Id { get; set; }
        // Popisek objektu
        public string Label { get; set; }
        // Název objektu
        public string Title { get; set; }
        // Typ objektu
        public string Type { get; set; }
        // Datum objektu
        public DateTime Date { get; set; }
        // Počáteční datum pro objekt
        public DateTime DateFrom { get; set; }
        // Koncové datum pro objekt
        public DateTime DateTo { get; set; }
        // Ikona reprezentující objekt
        public Constants.Icons Icon { get; set; }
        // Má objekt rámeček?
        public bool HasFrame { get; set; }
    }
}
