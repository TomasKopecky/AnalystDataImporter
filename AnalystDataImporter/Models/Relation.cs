using System.Drawing;
using System.Windows.Media;
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
        public double Thickness { get; set; }

        ///// <summary>
        ///// Styl vazby - původně v Analystu jako síla
        ///// </summary>
        //public DoubleCollection Style { get; set; }

        /// <summary>
        /// Hodnota stylu vazby - čára, tečky, ... - hodnota v podobě DoubleCollection
        /// </summary>
        public DoubleCollection StyleValue { get; set; }

        /// <summary>
        /// Hodnota stylu vazby - čára, tečky, ... - hodnota v podobě string - tedy textový popis daného stylu vazby
        /// </summary>
        public string StyleKey { get; set; }

        /// <summary>
        /// Násobnost vazby.
        /// </summary>
        public string Multiplicity { get; set; }

        /// <summary>
        /// Směr relace (např. od-do). - Hodnota (string)
        /// </summary>
        public int DirectionValue { get; set; }

        /// <summary>
        /// Směr vazby (např. od-do). - Klíč (string)
        /// </summary>
        public string DirectionKey { get; set; }

    }
}