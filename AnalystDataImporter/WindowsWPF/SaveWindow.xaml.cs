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
    /// Interakční logika pro SaveWindow.xaml
    /// </summary>
    public partial class SaveWindow : Window
    {
        public SaveWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(SaveWindow_KeyDown);
        }


        // metoda pro odchytávání stisknutých kláves na klávesnici
        private void SaveWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) // pokud stisknu klávesu Esc(ape)
            {
                this.Close(); // Zavře okno
            }
        }


        private void btnStorno_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Zavře okno
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Uložení hodnot ze Šablony: 
            //txtBxNazev
            //txtBxPopis
            //chckBxGlobalniSablona
        }
        // obsulužná událost po zaškrtnutí CheckBoxu pro uložení Globální Šablony:
        private void chckBxGlobalniSablona_Checked(object sender, RoutedEventArgs e)
        {
            // TODO: událost po zaškrtnutí CheckBoxu pro uložení Globální Šablony
            //chckBxGlobalniSablona
        }
    }
}
