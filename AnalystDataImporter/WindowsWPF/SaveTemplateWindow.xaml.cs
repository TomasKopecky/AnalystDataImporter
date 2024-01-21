using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml;
using AnalystDataImporter.Managers;
using AnalystDataImporter.Services;
using Newtonsoft.Json;
using System.IO.Packaging;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Contexts;
using static AnalystDataImporter.Globals.Constants;
using AnalystDataImporter.Models;
using AnalystDataImporter.Globals;

namespace AnalystDataImporter.WindowsWPF
{
    /// <summary>
    /// Interakční logika pro SaveWindow.xaml
    /// </summary>
    public partial class SaveWindow : Window
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IElementManager _elementManager;
        private readonly CsvParserService _csvParserService;
        public SaveWindow(IMessageBoxService messageBoxService, IElementManager elementManager, CsvParserService csvParserService)
        {
            InitializeComponent();
            _messageBoxService = messageBoxService;
            _elementManager = elementManager;
            _csvParserService = csvParserService;
            this.KeyDown += new KeyEventHandler(SaveWindow_KeyDown);

            txtBxNazev.Focus();
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
            //// pro 
            //SaveOptionsPopup.IsOpen = true;

            // Zobrazte ComboBox
            cmbBxSave.Visibility = Visibility.Visible;
            cmbBxSave.IsDropDownOpen = true;

            if (txtBxNazev.Text == null || txtBxNazev.Text == "")
                _messageBoxService.ShowError("Zadejte název ukládané šablony");

            else
                SaveTemplateFile(txtBxNazev.Text);

            // TODO: Uložení hodnot ze Šablony: 
            //txtBxNazev
            //txtBxPopis
            //chckBxGlobalniSablona
        }

        public void SaveTemplateFile(string fileName)
        {
            string folderPath = Constants.CreateAndGetTemplateSaveFolderPath();
            // Ensure the folder exists

            // Combine the folder path and file name
            string filePath = System.IO.Path.Combine(folderPath, fileName+".template");

            string content = JsonConvert.SerializeObject(_elementManager.Elements, Newtonsoft.Json.Formatting.Indented);

            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Ask the user if they want to overwrite
                MessageBoxResult messageBoxResult = MessageBox.Show(
                    "The file already exists. Do you want to overwrite it?",
                    "Overwrite File",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                // If the user chooses not to overwrite, just return
                if (messageBoxResult != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            string metadata = $"// Description: {txtBxPopis.Text}\n" +
                      $"// Date: {DateTime.Now.Date}\n" +
                      $"// Input File Path: {_csvParserService.inputFilePath}\n" +
                      $"// First Row Heading: {_csvParserService.isFirstRowHeading}\n" +
                      $"// Delimiter: {_csvParserService.delimiter}\n";

            string contentToSave = metadata + txtBxPopis.Text + Environment.NewLine + content;
            // Save the content to the file
            File.WriteAllText(filePath, contentToSave);

            _messageBoxService.ShowInformation("Šablona úspěšně uložena");

            Close();
        }

        // obsulužná událost po zaškrtnutí CheckBoxu pro uložení Globální Šablony:
        private void chckBxGlobalniSablona_Checked(object sender, RoutedEventArgs e)
        {
            // TODO: událost po zaškrtnutí CheckBoxu pro uložení Globální Šablony
            //chckBxGlobalniSablona
        }
        // obsulužná událost po zaškrtnutí CheckBoxu pro Zveřejnění vstupního souboru:
        private void chckBxZverejniVstupniSoubor_Checked(object sender, RoutedEventArgs e)
        {

        }

        // obslužá metoda po změně prvku v ComboBoxu cmbBxSave - Uložit / Uložit jako...
        private void cmbBxSave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO: dodělat metodu pro výběr "Uložit" nebo "Uložit jako..."
        }
    }
}
