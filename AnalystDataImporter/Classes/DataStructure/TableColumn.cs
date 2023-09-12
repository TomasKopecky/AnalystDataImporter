using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.Classes.DataStructure
{
    public class TableColumn
    {
        public int Id { get; set; }
        // Hlavička sloupce tabulky
        public string Heading { get; set; }
        // Nahradit prázdné hodnoty?
        public bool ReplaceEmptyValues { get; set; }
        // Hodnota, kterou se mají nahradit prázdné hodnoty
        public string ReplaceEmptyValuesWith { get; set; }
    }
}
