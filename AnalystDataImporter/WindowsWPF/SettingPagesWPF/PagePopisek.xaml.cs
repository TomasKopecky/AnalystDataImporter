using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnalystDataImporter.WindowsWPF.SettingPagesWPF
{
    /// <summary>
    /// Interakční logika pro PagePopisek.xaml
    /// </summary>
    public partial class PagePopisek : Page
    {
        public PagePopisek()
        {
            InitializeComponent();
        }

        private void rdBtnPopisekPrazdne_Click(object sender, RoutedEventArgs e)
        {
            txtBxPopisek.IsEnabled = false;
            cmbBxSloupec.IsEnabled = false;
        }

        private void rdBtnPopisekSloupec_Click(object sender, RoutedEventArgs e)
        {
            txtBxPopisek.IsEnabled = true;
            cmbBxSloupec.IsEnabled = true;

        }
    }
}
