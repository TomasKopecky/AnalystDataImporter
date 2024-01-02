using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.WindowsWPF
{
    internal class Node
    {
        public string Name { get; set; }
        public ObservableCollection<Node> Children { get; set; }
        public bool IsExpanded { get; set; }

        public Node()
        {
            IsExpanded = true;
            Children = new ObservableCollection<Node>();
        }
    }
}
