using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.Classes.DataStructure
{
    public class Source
    {
        public int Id { get; set; }
        // Hlavička zdroje
        public string Heading { get; set; }
        // Podnázev zdroje 1
        public string Suname1 { get; set; }
        // Podnázev zdroje 2
        public string Subname2 { get; set; }
        // Celkové jméno zdroje
        public string Name { get; set; }
    }
}
