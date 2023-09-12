using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalystDataImporter.Classes.DataStructure;

namespace AnalystDataImporter.Classes
{
    public class DataSource
    {
        // Uživatel, který je spojen s tímto zdrojem dat
        public User User { get; set; }
        // Seznam zdrojů spojených s tímto zdrojem dat
        public List<Source> Sources { get; set; }
        // Seznam specifikací spojených s tímto zdrojem dat
        public List<Specification> Specifications { get; set; }
    }
}
