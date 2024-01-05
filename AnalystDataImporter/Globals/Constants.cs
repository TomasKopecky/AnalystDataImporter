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

        // implicitní Styl (Síla) čáry - StrokeDashArray
        //public static List<Tuple<string, double, double>> Style = new List<Tuple<string, double, double>>
        //{
        //    new Tuple<string, double, double>("Potvrzené",0,0),
        //    new Tuple<string, double, double>("Nepotvrzené",2,1),
        //    new Tuple<string, double, double>("Nezávazné",0.5,2)
        //};


        // implicitní Styl (Síla) čáry - 
        public static Dictionary<string,DoubleCollection> Style = new Dictionary<string, DoubleCollection>
        {
            {"Potvrzené",new DoubleCollection(){0,0}},
            {"Nepotvrzené",new DoubleCollection(){2,1}},
            {"Nezávazné",new DoubleCollection(){0.5,2}},
        };

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