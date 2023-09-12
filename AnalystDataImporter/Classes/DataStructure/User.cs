using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.Classes.DataStructure
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Constants.UserRights Rights { get; set; }  // Typ práv pro tento účet
                                                          // ... ostatní vlastnosti ...
    }
}
