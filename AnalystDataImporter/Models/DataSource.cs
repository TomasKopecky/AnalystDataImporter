using System.Collections.Generic;

namespace AnalystDataImporter.Models
{
    /// <summary>
    /// Třída reprezentující model zdroje dat. Obsahuje vlastnosti, které definují spojení zdroje dat s uživatelem, zdroji a specifikacemi.
    /// </summary>
    public class DataSource
    {
        /// <summary>
        /// Uživatel, který je spojen s tímto zdrojem dat.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Seznam zdrojů spojených s tímto zdrojem dat.
        /// </summary>
        public List<Source> Sources { get; set; }

        /// <summary>
        /// Seznam specifikací spojených s tímto zdrojem dat.
        /// </summary>
        public List<Specification> Specifications { get; set; }
    }
}