using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnalystDataImporter.Services;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.WindowsWPF
{
    /// <summary>
    /// Interakční logika pro PageImport1.xaml
    /// </summary>
    public partial class PageImport1 : Page
    {
        private readonly GridViewModel _viewModel;
        private readonly IMessageBoxService _messageBoxService;
        private readonly CsvParserService _csvParserService;
        private char selectedDelimiter = ';';
        private bool isFirstRowHeading;
        private bool isFileLoaded;
        private string loadedFilePath;
        public PageImport1(GridViewModel gridViewModel, IMessageBoxService messageBoxService, CsvParserService csvParserService)
        {
            InitializeComponent();
            _viewModel = gridViewModel;
            DataContext = _viewModel;
            _messageBoxService = messageBoxService;
            _csvParserService = csvParserService;
            //viewModel.LoadTestingDataNew();
            //viewModel.LoadTestData();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                txtCestaKSouboru.Text = openFileDialog.FileName;
                LoadFile(openFileDialog.FileName, isFirstRowHeading);
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length == 1 && (files[0].EndsWith(".txt") || files[0].EndsWith(".csv")))
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }

            e.Handled = true;
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length == 1 && (files[0].EndsWith(".txt") || files[0].EndsWith(".csv")))
                {
                    txtCestaKSouboru.Text = files[0];
                    LoadFile(files[0], isFirstRowHeading);
                }
                else
                {
                    _messageBoxService.ShowError("Nepodporovaný formát souboru nebo načítáte více souborů najednou, což nelze");
                }
            }
        }

        private void LoadFile(string filePath, bool isFirstRowHeading)
        {
            if (File.Exists(filePath))
            {
                //if (viewModel?.ProcesCsvCommand.CanExecute(bool) == true)
                //{
                Tuple<string, char, bool> parameters = new Tuple<string, char, bool>(filePath, selectedDelimiter, isFirstRowHeading);
                _viewModel.ProcesCsvCommand.Execute(parameters);
                if (HeadingDataGrid.ItemsSource != null)
                {
                    isFileLoaded = true;
                    loadedFilePath = filePath;
                }
                else
                {
                    _messageBoxService.ShowError($"Nepodařilo se načíst a zobrazit soubor {filePath}");
                }
                //}

                //if (csvParserService.IsValidCsvStructure(filePath,selectedDelimiter))
                //{
                //    var parsedCsv = csvParserService.ParseCsv(filePath, Encoding.UTF8, selectedDelimiter);
                //}
            }
        }

        // ZAŠKRTÁVACÍ POLÍČKA:
        #region
        // Zaškrtávací pole pro Oddělovač: Čárka ,
        private void chckBxCarka_Checked(object sender, RoutedEventArgs e)
        {
            // TODO: po zaškrtnutí políčka pro Oddělovač: Čárka
            selectedDelimiter = ',';
        }
        private void chckBxCarka_Unchecked(object sender, RoutedEventArgs e)
        {
            // TODO: po odškrtnutí políčka pro Oddělovač: Čárka
        }

        // Zaškrtávací pole pro Oddělovač: Středník ;
        private void chckBxStrednik_Checked(object sender, RoutedEventArgs e)
        {
            // TODO: po zaškrtnutí políčka pro Oddělovač: Středník
            selectedDelimiter = ';';
        }
        private void chckBxStrednik_Unchecked(object sender, RoutedEventArgs e)
        {
            // TODO: po odškrtnutí políčka pro Oddělovač: Středník
        }

        // Zaškrtávací pole pro Oddělovač: Svislítko
        private void chckBxSvitlitko_Checked(object sender, RoutedEventArgs e)
        {
            // TODO: po zaškrtnutí políčka pro Oddělovač: Svislítko
            selectedDelimiter = '|';
        }
        private void chckBxSvitlitko_Unchecked(object sender, RoutedEventArgs e)
        {
            // TODO: po odškrtnutí políčka pro Oddělovač: Svislítko
        }

        // Zaškrtávací pole pro Oddělovač: Tabulátor
        private void chckBxTabulator_Checked(object sender, RoutedEventArgs e)
        {
            // TODO: po zaškrtnutí políčka pro Oddělovač: Tabulátor
            selectedDelimiter = 't';
        }
        private void chckBxTabulator_Unchecked(object sender, RoutedEventArgs e)
        {
            // TODO: po odškrtnutí políčka pro Oddělovač: Tabulátor
        }

        // Zaškrtávací pole pro Oddělovač: Mezera
        private void chckBxMezera_Checked(object sender, RoutedEventArgs e)
        {
            // TODO: po zaškrtnutí políčka pro Oddělovač: Mezera
            selectedDelimiter = ' ';
        }
        private void chckBxMezera_Unchecked(object sender, RoutedEventArgs e)
        {
            // TODO: po odškrtnutí políčka pro Oddělovač: Mezera
        }

        // Zaškrtávací pole pro Záhlaví: 
        private void chckBxZahlavi_Checked(object sender, RoutedEventArgs e)
        {
            // TODO: po zaškrtnutí políčka pro Záhlaví:
            isFirstRowHeading = true;

            if (isFileLoaded && loadedFilePath != null && loadedFilePath != "")
                LoadFile(loadedFilePath, isFirstRowHeading);
        }
        private void chckBxZahlavi_Unchecked(object sender, RoutedEventArgs e)
        {
            // TODO: po odškrtnutí políčka pro Záhlaví: 
            isFirstRowHeading = false;

            if (isFileLoaded && loadedFilePath != null && loadedFilePath != "")
                LoadFile(loadedFilePath, isFirstRowHeading);
        }

        // Zaškrtávací pole pro Oddělovač: Jiný
        private void chckBxJiny_Checked(object sender, RoutedEventArgs e)
        {
            // TODO: po zaškrtnutí políčka pro Oddělovač: Jiný
        }
        private void chckBxJiny_Unchecked(object sender, RoutedEventArgs e)
        {
            // TODO: po odškrtnutí políčka pro Oddělovač: Jiný
        }

        #endregion
    }
}
