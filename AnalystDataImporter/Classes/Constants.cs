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
        public enum Delimiters
        {
            [EnumString(",")]
            Comma,

            [EnumString(";")]
            Semicolon
        }

        // Dostupné typy práv uživatelů
        public enum UserRights
        {
            BasicUser,
            AdvancedUser,
            SuperUser
        }

        // implicitní ikony pro objekty
        public enum Icons
        {
            SimCard,
            Imei
        }

        // implicitní směry vazeb - bez směru, od A k B, od B k A
        public enum RelationDirections
        {
            noDirection,
            fromAtoB,
            fromBtoA
        }

        // Implicitní barvy
        public enum Colors
        {
            black,
            blue,
            green,
            yellow,
            orange
        }
    }

    // pomocná třída pro možnost uchování oddělovačů v datové struktuře Enum
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class EnumStringAttribute : Attribute
    {
        public string Value { get; }

        public EnumStringAttribute(string value)
        {
            Value = value;
        }
    }
}
