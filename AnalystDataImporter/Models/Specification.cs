using AnalystDataImporter.Globals;

namespace AnalystDataImporter.Models
{
    /// <summary>
    /// Třída reprezentující model specifikace. Definuje vlastnosti jako ID, název, cestu k XML souboru, viditelnost, oddělovač a příznak, zda je první řádek hlavičkou.
    /// </summary>
    public class Specification
    {
        /// <summary>
        /// Jedinečné ID specifikace.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Název specifikace.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Cesta k XML souboru specifikace.
        /// </summary>
        public string XmlFilePath { get; set; }

        /// <summary>
        /// Určuje, zda je specifikace veřejná.
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Oddělovač pro importovaná data.
        /// </summary>
        public Constants.Delimiters Delimiter { get; set; }

        /// <summary>
        /// Určuje, zda je první řádek hlavička.
        /// </summary>
        public bool IsFirstRowHeading { get; set; }
    }
}