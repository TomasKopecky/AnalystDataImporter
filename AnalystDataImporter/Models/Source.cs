using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.Models
{
    /// <summary>
    /// Třída reprezentující model zdroje. Definuje vlastnosti jako ID, hlavičku, podnázvy a název.
    /// </summary>
    public class Source
    {
        /// <summary>
        /// Jedinečné ID zdroje.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Hlavička zdroje.
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Podnázev zdroje 1.
        /// </summary>
        public string Suname1 { get; set; }

        /// <summary>
        /// Podnázev zdroje 2.
        /// </summary>
        public string Subname2 { get; set; }

        /// <summary>
        /// Celkové jméno zdroje.
        /// </summary>
        public string Name { get; set; }
    }
}
