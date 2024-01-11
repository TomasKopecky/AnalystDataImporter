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
        ///// Typ objektu. Specifikuje kategorii nebo typ prvku - avšak pouze v naší aplikaci, tedy buď element nebo relation
        ///// </summary>
        public string Type { get; set; }

        ///// <summary>
        ///// Typ objektu. Specifikuje kategorii nebo typ prvku tedy např. telefon, IMEI, atd...
        ///// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Datum a čas objektu. Reprezentuje datum spojené s prvkem pro pozdější využití hodnot ze sloupvů označených jako Date a Time v CSV !!
        /// </summary>
        public DateTime DateTimes { get; set; }

        /// <summary>
        /// Datum objektu. Reprezentuje datum spojené s prvkem.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Čas objektu. Reprezentuje čas spojený s prvkem.
        /// </summary>
        public string Time { get; set; }

        //public DateTime DateFrom { get; set; }
        //public DateTime DateTo { get; set; }
    }
}