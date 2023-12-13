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
        public string Color { get; set; }

        /// <summary>
        /// Směr vazby (např. od-do).
        /// </summary>
        public Constants.RelationDirections Direction { get; set; }
    }
}