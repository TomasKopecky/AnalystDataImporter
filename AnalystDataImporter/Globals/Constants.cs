using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace AnalystDataImporter.Globals
{
    public static class Constants
    {
        // Cesta k aktualizacím
        public static string UpdatePath { get; set; }

        // Indexová cesta
        public static string IndexPath { get; set; }

        public const double EllipseWidth = 40;

        public const int SelectBorderThicknessInt = 4;

        public static Thickness SelectBorderThicknes = new Thickness(SelectBorderThicknessInt);

        public const string DefaultElementLabel = "(identita není nastavena)";

        public const string DefaultElementId = "(identita není nastavena)";

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

        //// implicitní směry vazeb - bez směru, od A k B, od B k A
        //public enum RelationDirections
        //{
        //    noDirection,
        //    fromAtoB,
        //    fromBtoA
        //}
        // // implicitní směry vazeb - bez směru, od A k B, od B k A
        public static Dictionary<string, int> RelationDirections = new Dictionary<string, int>
        {
            {"Žádné",0}, // 0 je pomocná proměnná, podle které se nastaví <Line> bez šipek
            {"Ikona 1 až Ikona 2",2}, // 2 je pomocná proměnná, podle které se přidá <Line> šipka do bodu 2 (koncového)
            {"Ikona 2 až Ikona 1",1} // 1 je pomocná proměnná, podle které se přidá <Line> šipka do bodu 1 (počátečního)
        };

        //// implicitní násobnost vazby - Jednoduchá, Násobná
        //public static Dictionary<string, string> Multiplicity = new Dictionary<string, string>
        //{
        //    {"Jednoduchá","0"}, // Násobnost vazby? - NE = 0
        //    {"Násobná","1" } // Násobnost vazby? - ANO = 1
        //};

        // implicitní Styl (Síla) čáry - 
        public static Dictionary<string, DoubleCollection> Style = new Dictionary<string, DoubleCollection>
        {
            {"Potvrzené",new DoubleCollection(){1,0}}, //{1,0}}, ← správné hodnoty
            {"Nepotvrzené",new DoubleCollection(){6,2}}, //{6,2}}, ← správné hodnoty
            {"Nezávazné",new DoubleCollection(){1,4}} //{1,4}}, ← správné hodnoty
        };
        //// implicitní Styl (Síla) čáry - 
        //public static Dictionary<DoubleCollection, string> Style = new Dictionary<DoubleCollection, string>
        //{
        //    {new DoubleCollection(){1,0},"Potvrzené"}, //{1,0}}, ← správné hodnoty
        //    {new DoubleCollection(){6,2},"Nepotvrzené"}, //{6,2}}, ← správné hodnoty
        //    {new DoubleCollection(){1,4},"Nezávazné"} //{1,4}}, ← správné hodnoty
        //};

        // Implicitní barvy
        public static Dictionary<string, string> Colors = new Dictionary<string, string>
        {
            {"Černá","Black"},
            {"Červená","Red"},
            {"Modrá","Blue"},
            {"Zelená","Green"},
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