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
    /// Interakční logika pro PageDatumCas.xaml
    /// </summary>
    public partial class PageDatumCas : Page
    {
        public PageDatumCas()
        {
            InitializeComponent();
        }

        private void rdBtnNenastaveno_Click(object sender, RoutedEventArgs e)
        {
            cmbBxSloupecDatum.IsEnabled = false;

            // TODO: Metoda - co se stane, když nechci nastavit Datum nebo Čas
        }

        private void rdBtnJedenSloupec_Click(object sender, RoutedEventArgs e)
        {
            cmbBxSloupecDatum.IsEnabled = true;

            // TODO: Metoda - co se stane, když vyberu sloupec pro Datum nebo Čas
            // TODO: Kontorla formátu Datum/Čas (??)
        }
    }
}
