using System;

namespace AnalystDataImporter.Globals
{
    public class Constants
    {
        // Cesta k aktualizacím
        public string UpdatePath { get; set; }

        // Indexová cesta
        public string IndexPath { get; set; }

        public const double ellipseWidth = 50;

        public const int selectBorderThickness = 2;

        public const string defaultElementLabel = "(identita není nastavena)";

        public const string defaultElementTitle = "(identita není nastavena)";

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
    internal sealed class EnumStringAttribute : Attribute
    {
        public string Value { get; }

        public EnumStringAttribute(string value)
        {
            Value = value;
        }
    }
}