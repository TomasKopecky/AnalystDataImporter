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
using System.Windows.Shapes;

namespace AnalystDataImporter.WindowsWPF
{
    /// <summary>
    /// Interakční logika pro ImportWindow.xaml
    /// </summary>
    public partial class ImportWindow : Window
    {
        public ImportWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(SaveWindow_KeyDown);

            rdBtnNovaAkce.IsChecked = true;
        }

        // metoda pro odchytávání stisknutých kláves na klávesnici
        private void SaveWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) // pokud stisknu klávesu Esc(ape)
            {
                this.Close(); // Zavře okno
            }
        }

        // pokud zvolím NOVÁ AKCE:
        private void rdBtnNovaAkce_Checked(object sender, RoutedEventArgs e)
        {
            cmbBxExistujiciAkce.IsEnabled = false;
            txtBxNazevAkce.IsEnabled = true;
            txtBxNazevAkce.Focus();
        }
        // pokud zvolím EXISTUJÍCÍ AKCE:
        private void rdBtnExistujiciAkce_Checked(object sender, RoutedEventArgs e)
        {
            txtBxNazevAkce.IsEnabled = false;
            cmbBxExistujiciAkce.IsEnabled = true;
        }

        private void btnStorno_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Zavře okno
        }

        // po kliknutí na tlačítko "Spustit Import"...
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            // TODO: co se stane po kliknutí na tlačítko "Spustit Import"...


            this.Close();
        }

    }
}
