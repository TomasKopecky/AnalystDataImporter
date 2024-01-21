using System;
using System.Collections.Generic;
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
using AnalystDataImporter.Managers;
using AnalystDataImporter.Models;
using AnalystDataImporter.Services;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.WindowsWPF
{
    /// <summary>
    /// Interakční logika pro ImportWindow.xaml
    /// </summary>
    public partial class ImportWindow : Window
    {
        private readonly SqliteDbService _sqliteDbService;
        private readonly IMessageBoxService _messageBoxService;
        private readonly CsvParserService _csvParserService;
        private readonly GridViewModel _gridViewModel;
        private readonly IElementManager _elementManager;
        private readonly string _csvFilePath;
        private readonly char _delimiter;
        private readonly bool _isFirstRowHeading;
        public ImportWindow(SqliteDbService sqliteDbService, IMessageBoxService messageBoxService, CsvParserService csvParserService, GridViewModel gridViewModel, IElementManager elementManager, string csvFilePath, char delimiter, bool isFirstRowHeading)
        {
            InitializeComponent();
            _sqliteDbService = sqliteDbService;
            _messageBoxService = messageBoxService;
            _csvParserService = csvParserService;
            _gridViewModel = gridViewModel;
            _elementManager = elementManager;
            _csvFilePath = csvFilePath;
            _isFirstRowHeading = isFirstRowHeading;
            _delimiter = delimiter;
            this.KeyDown += new KeyEventHandler(SaveWindow_KeyDown);
            Loaded += LoadActions;

            rdBtnNovaAkce.IsChecked = true;
            _csvParserService = csvParserService;
        }

        // metoda pro odchytávání stisknutých kláves na klávesnici
        private void SaveWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) // pokud stisknu klávesu Esc(ape)
            {
                this.Close(); // Zavře okno
            }
        }

        private async void LoadActions(object sender, RoutedEventArgs e)
        {
            List<IndexAction> actions = await _sqliteDbService.GetAllActionsAsync();
            cmbBxExistujiciAkce.ItemsSource = actions;
            cmbBxExistujiciAkce.DisplayMemberPath = "Name";
            cmbBxExistujiciAkce.SelectedIndex = 0;
        }

        // pokud zvolím NOVÁ AKCE:
        private void rdBtnNovaAkce_Checked(object sender, RoutedEventArgs e)
        {
            cmbBxExistujiciAkce.IsEnabled = false;
            txtBxNazevAkce.IsEnabled = true;
            txtBxPopis.IsEnabled = true;
            txtBxNazevAkce.Focus();
        }
        // pokud zvolím EXISTUJÍCÍ AKCE:
        private void rdBtnExistujiciAkce_Checked(object sender, RoutedEventArgs e)
        {
            txtBxNazevAkce.IsEnabled = false;
            cmbBxExistujiciAkce.IsEnabled = true;
            txtBxPopis.IsEnabled = false;
        }

        private void btnStorno_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Zavře okno
        }

        // po kliknutí na tlačítko "Spustit Import"...
        private async void btnImport_Click(object sender, RoutedEventArgs e)
        {
            // TODO: co se stane po kliknutí na tlačítko "Spustit Import"...

            IndexAction indexActionToImport;
            if (rdBtnNovaAkce.IsChecked == true)
            {
                if (txtBxNazevAkce.Text == null || txtBxNazevAkce.Text == "")
                {
                    _messageBoxService.ShowError("Nelze založit akci s prázdným jménem");
                    return;
                }
                IndexAction newAction = new IndexAction();
                newAction.Name = txtBxNazevAkce.Text;
                newAction.Description = txtBxPopis.Text;
                newAction.Created = DateTime.Now.Date;
                await CreateNewIndexAction(newAction);
                indexActionToImport = newAction;
            }
            else
                indexActionToImport = cmbBxExistujiciAkce.SelectedItem as IndexAction;


            await ImportDataToAction(indexActionToImport);

            this.Close();
        }

        private async Task CreateNewIndexAction(IndexAction action)
        {
            await _sqliteDbService.CreateActionAsync(action);
        }

        private async Task ImportDataToAction(IndexAction action)
        {
            List<int> columnIndexes = new List<int>();

            foreach (BaseDiagramItemViewModel element in _elementManager.Elements)
                columnIndexes.Add(element.GridTableColumn.Index);

            if (columnIndexes.Count > 0)
            {

                List<string[]> result = _csvParserService.ParseCsv(_csvFilePath, null, _delimiter, columnIndexes);
                if (_isFirstRowHeading)
                    result.RemoveAt(0);

                Dictionary<List<string>, ElementViewModel> stringAndElements = new Dictionary<List<string>, ElementViewModel>();

                List<string[]> transposedData = TransposeCsvData(result);

                int i = 0;
                foreach (ElementViewModel elementViewModel in _elementManager.Elements)
                {
                    List<string> stringList = transposedData[i].ToList();
                    List<int> newInsertedObjectIds = await _sqliteDbService.InsertElementsGlobalAndUserAsync(stringList, elementViewModel, action);
                    await CreateObjectInsertAnalystDataFile();
                    i++;
                }

                //List<string> stringList = result.SelectMany(array => array).ToList();

                //await _sqliteDbService.InsertElementsIntoGlobalAsync(stringList);

                //await _sqliteDbService.InsertIntoUserObjectsAsync();

                //await _sqliteDbService.InsertElementsGlobalAndUserAsync(stringList, elementViewModel, action);
            }
        }

        public List<string[]> TransposeCsvData(List<string[]> csvData)
        {
            if (csvData == null || csvData.Count == 0)
                return new List<string[]>();

            int rowCount = csvData.Count;
            int colCount = csvData[0].Length;

            var transposedData = new List<string[]>(colCount);

            for (int col = 0; col < colCount; col++)
            {
                var columnData = new string[rowCount];
                for (int row = 0; row < rowCount; row++)
                {
                    columnData[row] = csvData[row].Length > col ? csvData[row][col] : "";
                }
                transposedData.Add(columnData);
            }

            return transposedData;
        }

        private async void CreateObjectInsertAnalystDataFile(List<int> ids, IndexAction action)
        {
            string filePath = @"\\ServerName\SharedFolder\outputFile.txt";
            using (var writer = new StreamWriter(filePath, append: true))
            {
                foreach (int id in ids)
                {
                    var data = await _sqliteDbService.FetchObjectDataAsync(id);
                    if (!string.IsNullOrEmpty(data.Key))
                    {
                        await writer.WriteLineAsync($"Key: {data.Key}");
                        await writer.WriteLineAsync($"Class: {data.Class}");
                        await writer.WriteLineAsync($"Title: {data.Title}");
                        await writer.WriteLineAsync($"style: {data.Style}");
                        // TODO: akce
                        await writer.WriteLineAsync("<<EOD>>");
                    }
                }
            }





        }
    }
