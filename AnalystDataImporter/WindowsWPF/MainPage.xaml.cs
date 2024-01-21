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
using System.Collections.ObjectModel;
using AnalystDataImporter.ViewModels;
using AnalystDataImporter.WindowsWPF.SettingPagesWPF;
using AnalystDataImporter.Services;
using AnalystDataImporter.Managers;
using AnalystDataImporter.Models;
using Newtonsoft.Json;
using AnalystDataImporter.Globals;
using System.Runtime.ExceptionServices;
using static System.Collections.Specialized.BitVector32;

namespace AnalystDataImporter.WindowsWPF
{
    /// <summary>
    /// Interakční logika pro MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        //private MainWindow mainWindow;
        private readonly PageImport1 _pageImport1;
        private readonly PageImport2 _pageImport2;
        private readonly IMessageBoxService _messageBoxService;
        private readonly IElementManager _elementManager;
        private readonly CsvParserService _csvParserService;
        private readonly SqliteDbService _sqliteDbService;
        private readonly GridViewModel _gridViewModel;
        
        // konstruktor
        public MainPage(PageImport1 pageImport1, PageImport2 pageImport2, IMessageBoxService messageBoxService, IElementManager elementManager, CsvParserService csvParserService, SqliteDbService sqliteDbService, GridViewModel gridViewModel)
        {
            InitializeComponent(); // Inicializace komponent
            _pageImport1 = pageImport1;
            _pageImport2 = pageImport2;
            _messageBoxService = messageBoxService;
            _elementManager = elementManager;
            _csvParserService = csvParserService;
            _sqliteDbService = sqliteDbService;
            _gridViewModel = gridViewModel;

            //this.Loaded += Load;

            #region NAPLNĚNÍ STROMU TREE-VIEW:
            // TODO: dočasné řešení TreeView - TEST:

            LoadTemplates();

            //var rootNode = new Node { Name = "název AKCE" };
            //rootNode.Children.Add(new Node { Name = "Datum: 2024-01-02" });
            //rootNode.Children.Add(new Node { Name = "Popis: Akce Kyštof - pachatel ujel v Labu" });
            //// přidání zdroje dat to TreeView pro Zdroje:
            //trVwZdroje.ItemsSource = new ObservableCollection<Node> { rootNode };
            // přidání zdroje dat to TreeView pro Šablony:
            //trVwSablony.ItemsSource = new ObservableCollection<Node> { rootNode };
            this._pageImport2 = pageImport2;
            #endregion
        }

        private async void Load(object sender, RoutedEventArgs e)
        {
            await LoadIndexActions();
        }
        private async Task LoadIndexActions()
        {
            trVwZdroje.Items.Clear();
            List<IndexAction> actions = await _sqliteDbService.GetAllActionsAsync();
            if (actions != null && actions.Count > 0)
            {
                foreach (IndexAction action in actions)
                {
                    // Create the main node for the action name
                    TextBlock actionTextBlock = new TextBlock
                    {
                        Text = action.Name,
                        FontWeight = FontWeights.Bold
                    };

                    // Create the main node for the action name with the TextBlock as header
                    TreeViewItem actionNode = new TreeViewItem { Header = actionTextBlock };

                    // Create and add child nodes
                    actionNode.Items.Add(new TreeViewItem { Header = $"Popis: {action.Description}" });
                    actionNode.Items.Add(new TreeViewItem { Header = $"Vytvořeno: {action.Created:d}" });

                    // Add the main node to the TreeView
                    trVwZdroje.Items.Add(actionNode);
                }
            }
        }

        private async void CreateIndexAction(object sender, RoutedEventArgs e)
        {
            IndexAction indexAction = new IndexAction();
            indexAction.ActionId = 54;
            indexAction.Name = "flasjflksdf";
            indexAction.Description = "fladsjfalksjfklsd";
            await _sqliteDbService.CreateActionAsync(indexAction);
        }

        private void LoadTemplates()
        {
            trVwSablony.Items.Clear();
            string[] fileEntries = Directory.GetFiles(Constants.CreateAndGetTemplateSaveFolderPath(), "*.template");

            foreach (string filePath in fileEntries)
            {
                string fileName = System.IO.Path.GetFileName(filePath);

                // Load the metadata (assuming the LoadTemplateFile method is available)
                var metadata = LoadTemplateFile(filePath);

                TextBlock actionTextBlock = new TextBlock
                {
                    Text = fileName.Replace(".template", ""),
                    FontWeight = FontWeights.Bold
                };

                // Create the main node for the file
                TreeViewItem fileNode = new TreeViewItem { Header = actionTextBlock };

                // Add description and date as child nodes
                fileNode.Items.Add(new TreeViewItem { Header = "Popis: " + metadata[0] });
                fileNode.Items.Add(new TreeViewItem { Header = "Datum: " + metadata[1] });

                // Add the file node to the TreeView
                trVwSablony.Items.Add(fileNode);
            }
        }

        public string[] LoadTemplateFile(string filePath)
        {
            string description = "";
            string date = "";
            string inputFilePath = "";
            string firstRowHeading = ""; // Use string to store the boolean value temporarily
            string delimiter = ""; // Use string to store the char value temporarily

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                if (line.StartsWith("// Description:"))
                    description = line.Substring("// Description:".Length).Trim();
                else if (line.StartsWith("// Date:"))
                    date = line.Substring("// Date:".Length).Trim();
                else if (line.StartsWith("// Input File Path:"))
                    inputFilePath = line.Substring("// Input File Path:".Length).Trim();
                else if (line.StartsWith("// First Row Heading:"))
                    firstRowHeading = line.Substring("// First Row Heading:".Length).Trim();
                else if (line.StartsWith("// Delimiter:"))
                    delimiter = line.Substring("// Delimiter:".Length).Trim();
                else if (!line.StartsWith("//"))
                    break; // Stop reading lines if it's no longer metadata
            }

            return new string[] { description, date, inputFilePath, firstRowHeading, delimiter };
        }



        public void LoadTemplateFile()
        {

            // načíst obsah souboru dané akce
            string content = " ";
            var elements = JsonConvert.DeserializeObject<IEnumerable<Element>>(content);
        }

        // Po nacteni Stránky:
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Načíst stránku PageImport1 do Frame v záložce 'Analyst Data Importer'
            //Page page1 = new PageImport1();
            frmImporter.Navigate(_pageImport1);
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
        private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                // Check if the newly selected item is the tab you are interested in
                TabControl tabControl = (TabControl)sender;

                if (tabControl.SelectedItem is TabItem selectedTab)
                {
                    // Check if it's the specific tab
                    if (selectedTab.Header.ToString() == "Šablony")
                    {
                        LoadTemplates();
                    }

                    if (selectedTab.Header.ToString() == "Akce")
                    {
                        // Run your method here
                        await LoadIndexActions();
                    }
                }
            }
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
            //LoadingWindow loadingWindow = new LoadingWindow();
            //loadingWindow.ShowDialog();
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
            //Page page2 = new PageImport2();
            frmImporter.Navigate(_pageImport2);

            // Tlačítka která budou v kroku 'Další' povolena nebo zakázána:
            btnImportZpet.IsEnabled = true; // Zpět povoleno - chceme se dostat na krok jedna
            btnImportDalsi.IsEnabled = false; // Dlaší zakázáno - nejde jít už o krok dál!
            btnImportImportovat.IsEnabled = true; // Importovat povoleno - odtud chceme teprve importovat!
            btnImportUlozit.IsEnabled = true; // Uložit povoleno - odtud chceme teprve Ukládat Šablonu
        }

        // Po kliknutí na Tlačítko 'Zpět' v záložce 'ANALYST DATA Import'
        private void btnImportZpet_Click(object sender, RoutedEventArgs e)
        {
            // Tlačítko 'Zpět' načte stránku 'PageImport1.xaml' do Frame 'frmImporter'
            //Page page1 = new PageImport1();
            frmImporter.Navigate(_pageImport1);

            // Tlačítka která budou v kroku 'Zpět' zakázána nebo povolena:
            btnImportZpet.IsEnabled = false; // Zpět zakázáno - nejde jít už o krok zpět!
            btnImportDalsi.IsEnabled = true; // Dlaší povoleno - chceme se dostat na krok dva
            btnImportImportovat.IsEnabled = false; // Importovat zakázáno - to má jít až ze druhé stránky
            btnImportUlozit.IsEnabled = false; // Uložit zakázáno - to má jít až ze druhé stránky
        }

        // Po kliknutí na Tlačítko 'Uložit' v záložce 'ANALYST DATA Import' - pro Uložení Šablony
        private void btnImportUlozit_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Dodělat funkci Tlačítka 'Uložit'
            SaveWindow saveWindow = new SaveWindow(_messageBoxService, _elementManager, _csvParserService);
            saveWindow.ShowDialog();
        }

        // Po kliknutí na Tlačítko 'Importovat' v záložce 'ANALYST DATA Import' - pro import dat do ANALYST DATAu
        private void btnImportImportovat_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Dodělat funkci Tlačítka 'Importovat'

            // Původní metoda z kódu od Toma:
            //string filePath = txtFilePath.Text;
            //// Process the file using the specified mappings and perform the import.
            //MessageBox.Show("Import complete!");

            ImportWindow importWindow = new ImportWindow(_sqliteDbService, _messageBoxService, _csvParserService, _gridViewModel, _elementManager, _pageImport1.loadedFilePath, _pageImport1.selectedDelimiter, _pageImport1.isFirstRowHeading);
            importWindow.ShowDialog();
        }

        #endregion

        #endregion

        // Zakázání navigace "Vpřed" a "Zpět"
        private void frmImporter_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back || e.NavigationMode == NavigationMode.Forward)
            {
                e.Cancel = true; // Zakázání navigace "Vpřed" a "Zpět"
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // Otevření URL v defaultním prohlížeči
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            });
            e.Handled = true;
        }
    }
}
