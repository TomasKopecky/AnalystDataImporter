using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.Classes.DataStructure
{
    public class Relation
    {
        public int Id { get; set; }
        // Název relace
        public string Title { get; set; }
        // Popisek relace
        public string Label { get; set; }
        // Typ relace
        public string Type { get; set; }
        // Datum relace
        public DateTime Date { get; set; }
        // Barva relace
        public string Color { get; set; }
        // Směr relace (např. od-do)
        public string Direction { get; set; }
        // Objekt, od kterého relace začíná
        public Object ObjectFrom { get; set; }
        // Objekt, ke kterému relace směřuje
        public Object ObjectTo { get; set; }
    }
}
