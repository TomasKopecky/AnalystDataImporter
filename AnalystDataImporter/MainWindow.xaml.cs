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

namespace AnalystDataImporter
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Po nacteni Okna:
        private void Window_Loaded(object sender, RoutedEventArgs e)
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
        }

        // Inicializace komponent
        public MainWindow()
        {
            InitializeComponent();
        }

        // 
        private void Grid_Drop(object sender, DragEventArgs e)
        {

        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {            
                // Získání vybrané záložky
                TabItem selectedTab = (TabItem)e.AddedItems[0];

                // Podle vybrané záložky načíst příslušnou stránku do Frame
                if (selectedTab.Header.ToString() == "ANALYST DATA Importer")
                {
                    Page page1 = new PageImport1();
                    frmImporter.Navigate(page1);
                }
            }
            catch (Exception ex)
            { }
        }

        // Po kliknutí na Tlačítko 'Ďalší'
        private void btnDalsi_Click(object sender, RoutedEventArgs e)
        {
            // Tlačítko 'Ďalší' načte stránku 'PageImport2.xaml' do Frame 'frmImporter'
            Page page2 = new PageImport2();
            frmImporter.Navigate(page2);

            // Tlačítka která budou v kroku 'Další' povolena:
            btnZpet.IsEnabled = true;
            btnDalsi.IsEnabled = false;
            btnImport.IsEnabled = true;
        }

        // Po kliknutí na Tlačítko 'Zpět'
        private void btnZpet_Click(object sender, RoutedEventArgs e)
        {
            // Tlačítko 'Zpět' načte stránku 'PageImport1.xaml' do Frame 'frmImporter'
            Page page1 = new PageImport1();
            frmImporter.Navigate(page1);

            // Tlačítka která budou v kroku 'Zpět' zakázána:
            btnZpet.IsEnabled = false;
            btnDalsi.IsEnabled = true;
            btnImport.IsEnabled = false;
        }

        // Po kliknutí na Tlačítko 'Importovat'
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Dodělat funkci Tlačítka 'Importovat'

            // Původní metoda z kódu od Toma:
            //string filePath = txtFilePath.Text;
            //// Process the file using the specified mappings and perform the import.
            //MessageBox.Show("Import complete!");
        }
    }
}
