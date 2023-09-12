using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter
{
    public class Specification
    {
        public int Id { get; set; }
        // Název specifikace
        public string Name { get; set; }
        // Cesta k XML souboru specifikace
        public string XmlFilePath { get; set; }
        // Je specifikace veřejná?
        public bool IsPublic { get; set; }
        // Oddělovač pro importovaná data
        public string Delimiter { get; set; }
        // Je první řádek hlavička?
        public bool IsFirstRowHeading { get; set; }
    }
}
