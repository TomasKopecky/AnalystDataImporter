using System;

namespace AnalystDataImporter.Models
{
    public abstract class BaseDiagramItem
    {
        /// <summary>
        /// Jedinečné ID vazby.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Popis objektu - dodatečný popis nebo info o objektu.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Popisek objektu
        /// </summary>
        public string Label { get; set; }

        ///// <summary>
        ///// Typ objektu. Specifikuje kategorii nebo typ prvku.
        ///// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Datum objektu. Reprezentuje datum spojené s prvkem.
        /// </summary>
        public DateTime Date { get; set; }

        //public DateTime DateFrom { get; set; }
        //public DateTime DateTo { get; set; }
    }
}