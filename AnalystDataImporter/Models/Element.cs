using System;
using AnalystDataImporter.Globals;

namespace AnalystDataImporter.Models
{
    /// <summary>
    /// Třída reprezentující základní prvek v aplikaci. Obsahuje vlastnosti jako ID, popisek, název, typ a datum.
    /// </summary>
    //TODO: For the Element model, consider adding validation or constraints for certain properties (e.g., ensuring that DateFrom is always before DateTo).
    public class Element : BaseDiagramItem
    {
        /// <summary>
        /// Počáteční datum spojené s prvkem.
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Koncové datum spojené s prvkem.
        /// </summary>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Ikona reprezentující objekt. Může být jedna z definovaných ikon v aplikaci.
        /// </summary>
        public Constants.Icons Icon { get; set; }

        /// <summary>
        /// Určuje, zda objekt má vizuální rámeček.
        /// </summary>
        public bool HasFrame { get; set; }
    }
}