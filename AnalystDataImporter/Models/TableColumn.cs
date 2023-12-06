using System.Collections.Generic;

namespace AnalystDataImporter.Models
{
    /// <summary>
    /// Třída reprezentující model sloupce tabulky. Definuje vlastnosti jako ID, hlavičku a konfigurace týkající se prázdných hodnot.
    /// </summary>
    public class TableColumn
    {
        /// <summary>
        /// Jedinečné ID sloupce tabulky.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Hlavička sloupce tabulky.
        /// </summary>
        public string Heading { get; set; }

        public List<string> Content { get; set; }

        /// <summary>
        /// Určuje, zda je třeba nahradit prázdné hodnoty.
        /// </summary>
        public bool ReplaceEmptyValues { get; set; }

        /// <summary>
        /// Hodnota, kterou se mají nahradit prázdné hodnoty.
        /// </summary>
        public string ReplaceEmptyValuesWith { get; set; }
    }
}