using AnalystDataImporter.Globals;

namespace AnalystDataImporter.Models
{
    /// <summary>
    /// Třída reprezentující model vazba. Definuje vlastnosti jako název, typ, datum, barvu, směr a související prvky.
    /// </summary>
    public class Relation : BaseDiagramItem
    {
        /// <summary>
        /// Barva vazby.
        /// </summary>
        public Constants.Colors Color { get; set; }

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