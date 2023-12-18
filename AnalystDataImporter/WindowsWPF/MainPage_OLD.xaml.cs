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
using Microsoft.Win32;
using System.Data;
using System.IO;
using AnalystDataImporter.WindowsWPF;

namespace AnalystDataImporter.WindowsWPF
{
    /// <summary>
    /// Interakční logika pro MainPage.xaml
    /// </summary>
    public partial class MainPage_OLD : Page
    {
        // Po nacteni Okna:
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Získání výšky monitoru
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            // Výpočet požadované výšky pro okno (2/3 výšky monitoru)
            double desiredHeight = screenHeight * 2 / 3;

            // Nastavení výšky hlavního okna
            this.Height = desiredHeight;


            // Získání šířky monitoru
            double screenWidth = SystemParameters.PrimaryScreenWidth;

            // Výpočet požadované šířky pro okno (3/4 šířky monitoru)
            double desiredWidth = screenWidth * 3 / 4;

            // Nastavení šířky hlavního okna
            this.Width = desiredWidth;

            // Načíst stránku PageImport1 do Frame v záložce 'Analyst Data Importer'
            Page page1 = new PageImport1();
            frmImporter.Navigate(page1);
        }

        // konstruktor
        public MainPage_OLD()
        {
            InitializeComponent(); // Inicializace komponent
        }

        // DROP do GRIDu
        private void Grid_Drop(object sender, DragEventArgs e)
        {
            // TODO: Grid_Drop
        }

        // DRAG_OVER -- ??
        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            // TODO: Grid_DragOver
        }

        // Metoda - co se stane po přepnutí záložky
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Pokud bych tu chtěl něco, co se stane po přepnutí záložky...

            //try
            //{
            //    // Získání vybrané záložky
            //    TabItem selectedTab = (TabItem)e.AddedItems[0];

            //    // Podle vybrané záložky načíst příslušnou stránku do Frame
            //    if (selectedTab.Header.ToString() == "ANALYST DATA Importer")
            //    {
            //        Page page1 = new PageImport1();
            //        frmImporter.Navigate(page1);
            //    }
            //}
            //catch (Exception ex)
            //{ }
        }


        // TLAČÍTKA:
        #region TLAČÍTKA

        // Záložka ZDROJE:
        #region ZDROJE
        // Po kliknutí na Tlačítko 'Aktualizovat' v záložce 'Zdroje'
        private void btnZdrojeAktualizovat_Click(object sender, RoutedEventArgs e)
        {
            // TODO: funkce Tlačítka Aktualizovat
        }
        #endregion

        // Záložka ŠABLONY:
        #region 
        // Po kliknutí na Tlačítko 'Přejmenovat' v záložce 'Šablony'
        private void btnSablonaPrejmenovat_Click(object sender, RoutedEventArgs e)
        {
            // TODO: funkce Tlačítka Přejmenovat
        }
        // Po kliknutí na Tlačítko 'Upravit' v záložce 'Šablony'
        private void btnSablonaUpravit_Click(object sender, RoutedEventArgs e)
        {
            // TODO: funkce Tlačítka Upravit
        }
        // Po kliknutí na Tlačítko 'Použít' v záložce 'Šablony'
        private void btnSablonaPoužít_Click(object sender, RoutedEventArgs e)
        {
            // TODO: funkce Tlačítka Použít
        }
        // Po kliknutí na Tlačítko 'Smazat' v záložce 'Šablony'
        private void btnSablonaSmazat_Click(object sender, RoutedEventArgs e)
        {
            // TODO: funkce Tlačítka Smazat
        }
        #endregion

        // Záložka IMPORT:
        #region IMPORT
        // Po kliknutí na Tlačítko 'Ďalší' v záložce 'ANALYST DATA Import'
        private void btnImportDalsi_Click(object sender, RoutedEventArgs e)
        {
            // Tlačítko 'Ďalší' načte stránku 'PageImport2.xaml' do Frame 'frmImporter'
            Page page2 = new PageImport2();
            frmImporter.Navigate(page2);

            // Tlačítka která budou v kroku 'Další' povolena nebo zakázána:
            btnImportZpet.IsEnabled = true; // Zpět zakázáno - nejde jít už o krok zpět!
            btnImportDalsi.IsEnabled = false; // Dlaší povoleno - chceme se dostat na krok dva
            btnImportImportovat.IsEnabled = true; // Importovat zakázáno - to má jít až ze druhé stránky
        }

        // Po kliknutí na Tlačítko 'Zpět' v záložce 'ANALYST DATA Import'
        private void btnImportZpet_Click(object sender, RoutedEventArgs e)
        {
            // Tlačítko 'Zpět' načte stránku 'PageImport1.xaml' do Frame 'frmImporter'
            Page page1 = new PageImport1();
            frmImporter.Navigate(page1);

            // Tlačítka která budou v kroku 'Zpět' zakázána nebo povolena:
            btnImportZpet.IsEnabled = false; // Zpět povoleno - chceme se dostat na krok jedna
            btnImportDalsi.IsEnabled = true; // Dlaší zakázáno - nejde jít už o krok dál!
            btnImportImportovat.IsEnabled = false; // Importovat povoleno - odtud chceme teprve importovat!
        }

        // Po kliknutí na Tlačítko 'Importovat' v záložce 'ANALYST DATA Import'
        private void btnImportImportovat_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Dodělat funkci Tlačítka 'Importovat'

            // Původní metoda z kódu od Toma:
            //string filePath = txtFilePath.Text;
            //// Process the file using the specified mappings and perform the import.
            //MessageBox.Show("Import complete!");
        }
        #endregion

        #endregion
    }
}
