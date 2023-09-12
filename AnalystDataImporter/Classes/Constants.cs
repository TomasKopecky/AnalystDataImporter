using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.Classes
{
    public class Constants
    {
        // Cesta k aktualizacím
        public string UpdatePath { get; set; }
        // Indexová cesta
        public string IndexPath { get; set; }
        // Slovník ikon a tříd
        //public Dictionary<Icon, Class> IconClass { get; set; }
        // Seznam dostupných oddělovačů
        public List<string> Delimiters { get; set; }
        // Seznam dostupných barev relací
        public List<string> RelationColors { get; set; }
        // Dostupné typy práv uživatelů
        public enum UserRights
        {
            BasicUser,
            AdvancedUser,
            SuperUser
        }
    }
}
