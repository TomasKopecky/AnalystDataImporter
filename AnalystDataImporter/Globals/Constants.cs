using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Drawing;


namespace AnalystDataImporter.Globals
{
    public static class Constants
    {
        // Cesta k aktualizacím
        public static string UpdatePath { get; set; }

        // Indexová cesta
        public static string IndexPath { get; set; }

        public const double EllipseWidth = 50;

        public const int SelectBorderThickness = 2;

        public const string DefaultElementLabel = "(identita není nastavena)";

        public const string DefaultElementTitle = "(identita není nastavena)";

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
        public static List<string> Classes = new List<string>{
            "Obecný objekt",
            "SIM karta",
            "IMEI",
            "IMSI",
            "Osoba",
            "Číslo jednací",
            "Doména",
            "Email"
        };
        

        public enum Icons
        {

        }

        // implicitní směry vazeb - bez směru, od A k B, od B k A
        public enum RelationDirections
        {
            noDirection,
            fromAtoB,
            fromBtoA
        }

        // Implicitní barvy
        public static Dictionary<string, string> Colors = new Dictionary<string, string>
        {
            {"Černá","Black"},
            {"Modrá","Blue"},
            {"Zelená","Green"},
            {"Žlutá","Yellow"},
            {"Oranžová","Orange"}
        };

        // implicitní Tloušťka (Šířka) vazby
        public static List<double> Thickness = new List<double>
        {
           1,2,3,4,5,6,7,8,9,10
        };
    }

    // pomocná třída pro možnost uchování oddělovačů v datové struktuře Enum
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class EnumStringAttribute : Attribute
    {
        public string Value { get; }

        public EnumStringAttribute(string value)
        {
            Value = value;
        }
    }
}