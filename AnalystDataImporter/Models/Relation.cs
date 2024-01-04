﻿using System.Drawing;
using AnalystDataImporter.Globals;

namespace AnalystDataImporter.Models
{
    /// <summary>
    /// Třída reprezentující model vazba. Definuje vlastnosti jako název, typ, datum, barvu, směr a související prvky.
    /// </summary>
    public class Relation : BaseDiagramItem
    {
        /// <summary>
        /// Barva vazby reprezentovaná přímo ze System.Drawings.
        /// </summary>
        public string ColorValue { get; set; }

        /// <summary>
        /// Barva vazby reprezentovaná českým názvem barvy.
        /// </summary>
        public string ColorKey { get; set; }

        /// <summary>
        /// Tloušťka vazby - původně v Analystu jako šířka
        /// </summary>
        public string Thickness { get; set; }

        /// <summary>
        /// Styl vazby - původně v Analystu jako síla
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// Násobnost vazby.
        /// </summary>
        public string Multiplicity { get; set; }

        /// <summary>
        /// Směr vazby (např. od-do).
        /// </summary>
        public Constants.RelationDirections Direction { get; set; }
    }
}