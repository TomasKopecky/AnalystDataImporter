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
        private readonly IRelationManager _relationManager;
        private readonly string _csvFilePath;
        private readonly char _delimiter;
        private readonly bool _isFirstRowHeading;
        public ImportWindow(SqliteDbService sqliteDbService, IMessageBoxService messageBoxService, CsvParserService csvParserService, GridViewModel gridViewModel, IElementManager elementManager, IRelationManager relationManager, string csvFilePath, char delimiter, bool isFirstRowHeading)
        {
            InitializeComponent();
            _sqliteDbService = sqliteDbService;
            _messageBoxService = messageBoxService;
            _csvParserService = csvParserService;
            _gridViewModel = gridViewModel;
            _elementManager = elementManager;
            _relationManager = relationManager;
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
            {
                indexActionToImport = cmbBxExistujiciAkce.SelectedItem as IndexAction;

                if (indexActionToImport == null)
                {
                    _messageBoxService.ShowError("Je třeba vybrat existující akci či založit novou");
                    return;
                }
            }

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

                //Dictionary<List<string>, ElementViewModel> stringAndElements = new Dictionary<List<string>, ElementViewModel>();

                List<string[]> transposedData = TransposeCsvData(result);

                int i = 0;
                List<int> newInsertedObjectIds = new List<int>();
                foreach (ElementViewModel elementViewModel in _elementManager.Elements)
                {
                    List<string> stringList = transposedData[i].ToList();

                    newInsertedObjectIds.AddRange(await _sqliteDbService.InsertElementsGlobalAndUserAsync(stringList, elementViewModel, action));

                    i++;
                }

                List<Tuple<int, int>> relationColumnIndexes = new List<Tuple<int, int>>();
                foreach (RelationViewModel relationViewModel in _relationManager.Relations)
                {
                    try
                    {
                        int key = _csvParserService.recalculatedColumnIndexes[relationViewModel.ObjectFrom.GridTableColumn.Index];
                        int value = _csvParserService.recalculatedColumnIndexes[relationViewModel.ObjectTo.GridTableColumn.Index];
                        relationColumnIndexes.Add(new Tuple<int,int>(key, value));
                    }
                    catch (Exception)
                    {

                    }
                }

                List<int> newInsertedRelations = await _sqliteDbService.InsertRelationsAsync(relationColumnIndexes, result, action);

                if (newInsertedObjectIds.Count > 0 || newInsertedRelations.Count > 0)
                {
                    await CreateObjectInsertAnalystDataFile(newInsertedObjectIds, newInsertedRelations, action);
                    _messageBoxService.ShowInformation("Import úspěšně proveden, data vložena do dané akce");
                }

                else
                    _messageBoxService.ShowInformation("Import úspěšně proveden, avšak nebyla nalezena žádná nová data, která by již v akci nebyla obsažena");

                Close();
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

        private async Task CreateObjectInsertAnalystDataFile(List<int> objectIds, List<int> relationIds, IndexAction action)
        {
            string filePath = @"\\IP_ADDRESS\analyst_data_importer\input_files\inputFile.txt";
            using (var writer = new StreamWriter(filePath, append: true))
            {
                foreach (int id in objectIds)
                {
                    var data = await _sqliteDbService.FetchObjectDataAsync(id);
                    if (!string.IsNullOrEmpty(data.Key))
                    {
                        await writer.WriteLineAsync($"Key: {data.Key}");
                        await writer.WriteLineAsync($"Action: {action.ActionId}");
                        await writer.WriteLineAsync($"itemType: 1");
                        await writer.WriteLineAsync($"Class: {data.Class}");
                        await writer.WriteLineAsync($"Title: {data.Title}");
                        await writer.WriteLineAsync($"Style: {data.Style}");
                        await writer.WriteLineAsync("<<EOD>>");
                    }
                }

                foreach (int id in relationIds)
                {
                    var data = await _sqliteDbService.FetchRelationDataAsync(id);
                    if (!string.IsNullOrEmpty(data.ObjectFromKey))
                    {
                        await writer.WriteLineAsync($"Key: {data.ObjectFromKey}_{data.ObjectToKey}");
                        await writer.WriteLineAsync($"Action: {action.ActionId}");
                        await writer.WriteLineAsync($"itemType: 2");
                        await writer.WriteLineAsync($"Class: vazba");
                        await writer.WriteLineAsync($"Node1: {data.ObjectFromKey}");
                        await writer.WriteLineAsync($"Node2: {data.ObjectToKey}");
                        await writer.WriteLineAsync("<<EOD>>");
                    }
                }
            }
        }
    }
}
