using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AnalystDataImporter.Factories;
using AnalystDataImporter.Managers;
using AnalystDataImporter.Services;
using AnalystDataImporter.Utilities;

namespace AnalystDataImporter.ViewModels
{
    public class MyDataModel
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }
    public class ColumnInfo
    {
        public string Header { get; set; }
        public string BindingPath { get; set; }
    }
    public class GridViewModel : INotifyPropertyChanged
    {
        private readonly MouseCursorService _mouseCursorService;
        private readonly SharedStatesService _sharedStateService;
        private readonly SharedCanvasPageItems _sharedCanvasPageItems;
        private readonly CsvParserService _csvParserService;
        private List<List<String>> Rows;
        private string _selectedColumnName;
        private DataTable _contentTable;
        private DataTable _headingTable;
        private readonly ITableColumnViewModelFactory _tableColumnViewModelFactory;
        private ObservableCollection<TableColumnViewModel> _columns;
        public ICommand ChangeCursorWhenOperatingGridCommand { get; private set; }

        public ICommand GetDraggedGridViewColumnCommand { get; private set; }

        public ICommand ProcesCsvCommand { get; private set; }

        public GridViewModel(ITableColumnViewModelFactory tableColumnViewModelFactory, MouseCursorService mouseCursorService, SharedStatesService sharedStateService, SharedCanvasPageItems sharedCanvasPageItems, CsvParserService csvParserService)
        {
            ChangeCursorWhenOperatingGridCommand = new RelayCommand<string>(ChangeCursorByGridView);
            GetDraggedGridViewColumnCommand = new RelayCommand<object>(GetDraggedGridViewColumnIndex);
            ProcesCsvCommand = new RelayCommand<object>(ProcesCsv);
            _tableColumnViewModelFactory = tableColumnViewModelFactory ?? throw new ArgumentNullException(nameof(tableColumnViewModelFactory));
            _mouseCursorService = mouseCursorService;
            _sharedStateService = sharedStateService;
            _sharedCanvasPageItems = sharedCanvasPageItems;
            _csvParserService = csvParserService;
        }

        private void ProcesCsv(object parameters)
        {
            if (parameters is Tuple<string, char> tuple) 
            {
                if (_csvParserService.IsValidCsvStructure(tuple.Item1, tuple.Item2))
                {
                    List<string[]> parsedCsvString = _csvParserService.ParseCsv(tuple.Item1, Encoding.UTF8, tuple.Item2);
                    LoadData(parsedCsvString);
                }
            }
        }

        public void LoadTestingDataNew()
        {
            Rows = new List<List<string>>();

            TableColumnsViewModel = new ObservableCollection<TableColumnViewModel>();

            HeadingTable = new DataTable();
            GenerateTestDataGridContentNew();
        }

        private void LoadData(List<string[]> inputString)
        {
            DataTable Content = new DataTable();

            Rows = new List<List<string>>();

            TableColumnsViewModel = new ObservableCollection<TableColumnViewModel>();

            DataTable Heading = new DataTable();

            int columns = inputString[0].Length;

            for (int column = 0; column < columns; column++)
            {
                Heading.Columns.Add(column.ToString());
                Content.Columns.Add(column.ToString());
                foreach (string[] values in inputString) 
                {
                    DataRow newRow = Content.NewRow();
                    foreach (string value in values)
                    {
                        newRow[column.ToString()] = value;
                    }
                    Content.Rows.Add(newRow);
                }
            }

            //HeadingTable = new DataTable();

            //GenerateTestDataGridContent();

            //TableColumnViewModel tableColumn = _tableColumnViewModelFactory.Create();
            //tableColumn.Heading = "pokus";
            //tableColumn.Index = 1;
            //Columns.Add(tableColumn);

            //TableColumnViewModel tableColumn2 = _tableColumnViewModelFactory.Create();
            //tableColumn2.Heading = "pokus";
            //tableColumn2.Index = 1;
            //Columns.Add(tableColumn2);

            //// Add corresponding column in DataTable
            //NewTable.Columns.Add(tableColumn.Heading, typeof(string));
            ////NewTable.Columns.Add(tableColumn.Heading, typeof(string));

            //List<string> rowValues = new List<string>();
            //DataRow tableRow = NewTable.NewRow();
            //DataRow tableRow2 = NewTable.NewRow();
            //DataRow tableRow3 = NewTable.NewRow();

            //tableRow[0] = "sadf";
            //tableRow2[0] = "sadf";
            //tableRow3[0] = "sadf";

            //NewTable.Rows.Add(tableRow);
            //NewTable.Rows.Add(tableRow2);
            //NewTable.Rows.Add(tableRow3);

            HeadingTable = Heading;
            ContentTable = Content;



        }

        //public ObservableCollection<TableColumnViewModel> ColumnTable
        //{
        //    get => _columns;
        //    set
        //    {
        //        _columns = value;
        //        OnPropertyChanged(nameof(Columns));
        //    }
        //}

        public DataTable HeadingTable
        {
            get => _headingTable;
            set
            {
                if (_headingTable != value)
                {
                    _headingTable = value;
                }
                OnPropertyChanged(nameof(HeadingTable));
            }
        }

        public DataTable ContentTable
        {
            get => _contentTable;
            set
            {
                if (_contentTable != value)
                {
                    _contentTable = value;
                }
                OnPropertyChanged(nameof(ContentTable));
            }
        }

        public ObservableCollection<TableColumnViewModel> TableColumnsViewModel
        {
            get => _columns;
            set
            {
                _columns = value;
                OnPropertyChanged(nameof(TableColumnsViewModel));
            }
        }

        public string SelectedColumnName
        {
            get => _selectedColumnName;
            set
            {
                _selectedColumnName = value;
                OnPropertyChanged(nameof(SelectedColumnName));
            }
        }

        private void GenerateTestDataGridContentNew()
        {
            // Generate new columns and rows
            int totalColumns = 2;
            Random random = new Random();
            DataTable NewTable = new DataTable();

            for (int columnIndex = 0; columnIndex < totalColumns; columnIndex++)
            {
                // Create and add new TableColumnViewModel
                TableColumnViewModel tableColumn = _tableColumnViewModelFactory.Create();
                tableColumn.Heading = columnIndex % 2 == 0 ? $"SIM{columnIndex / 2 + 1}" : $"IMEI{columnIndex / 2 + 1}";
                tableColumn.Index = columnIndex;
                TableColumnsViewModel.Add(tableColumn);

                // Add corresponding column in DataTable
                NewTable.Columns.Add(tableColumn.Heading, typeof(string));
                HeadingTable.Columns.Add(tableColumn.Heading, typeof(string));
            }

            // Generate rows with random data
            int totalRows = 20;

            for (int rowIndex = 0; rowIndex < totalRows; rowIndex++)
            {
                List<string> rowValues = new List<string>();
                DataRow tableRow = NewTable.NewRow();
                //DataRow columnTableRow = ColumnTable.NewRow();

                for (int columnIndex = 0; columnIndex < totalColumns; columnIndex++)
                {
                    string value = columnIndex % 2 == 0
                        ? random.Next(100000000, 999999999).ToString() // SIM-like number
                        : $"{random.Next(1000000, 9999999)}{random.Next(1000000, 9999999)}"; // IMEI-like number

                    rowValues.Add(value);
                    tableRow[columnIndex] = value;
                }
                NewTable.Rows.Add(tableRow); // Add the DataRow to the DataTable's Rows collection
                Rows.Add(rowValues); // This line maintains your list of rows, assuming it's needed elsewhere
            }
            ContentTable = NewTable;

        }

        private void GenerateTestDataGridContent()
        {
            // Generate new columns and rows
            int totalColumns = 2;
            Random random = new Random();

            for (int columnIndex = 0; columnIndex < totalColumns; columnIndex++)
            {
                // Create and add new TableColumnViewModel
                TableColumnViewModel tableColumn = _tableColumnViewModelFactory.Create();
                tableColumn.Heading = columnIndex % 2 == 0 ? $"SIM{columnIndex / 2 + 1}" : $"IMEI{columnIndex / 2 + 1}";
                tableColumn.Index = columnIndex;
                TableColumnsViewModel.Add(tableColumn);

                // Add corresponding column in DataTable
                ContentTable.Columns.Add(tableColumn.Heading, typeof(string));
                HeadingTable.Columns.Add(tableColumn.Heading, typeof(string));
            }

            // Generate rows with random data
            int totalRows = 20;

            for (int rowIndex = 0; rowIndex < totalRows; rowIndex++)
            {
                List<string> rowValues = new List<string>();
                DataRow tableRow = ContentTable.NewRow();
                //DataRow columnTableRow = ColumnTable.NewRow();

                for (int columnIndex = 0; columnIndex < totalColumns; columnIndex++)
                {
                    string value = columnIndex % 2 == 0
                        ? random.Next(100000000, 999999999).ToString() // SIM-like number
                        : $"{random.Next(1000000, 9999999)}{random.Next(1000000, 9999999)}"; // IMEI-like number

                    rowValues.Add(value);
                    tableRow[columnIndex] = value;
                }
                ContentTable.Rows.Add(tableRow); // Add the DataRow to the DataTable's Rows collection
                Rows.Add(rowValues); // This line maintains your list of rows, assuming it's needed elsewhere
            }
 
        }

        public void LoadTestData()
        {
            ContentTable = new DataTable();

            Rows = new List<List<string>>();

            TableColumnsViewModel = new ObservableCollection<TableColumnViewModel>();

            HeadingTable = new DataTable();

            GenerateTestDataGridContent();

        }

        /// <summary>
        /// Metoda pro změnu kurzoru při operaci s prvkem.
        /// </summary>
        /// <param name="operation">Typ operace, která určuje, jaký kurzor zobrazit.</param>

        private void ChangeCursorByGridView(string operation)
        {
            switch (operation)
            {
                case "GridViewMouseOverCursor":
                    _sharedStateService.IsDraggingGridColumnModeActive = false;
                    _sharedStateService.MouseOnGrid = true;
                    _sharedStateService.MouseOnCanvas = false;
                    break;
                case "GridViewDraggingAllowedCursor":
                    _sharedStateService.IsDraggingElementModeActive = false;
                    _sharedStateService.IsDraggingGridColumnModeActive = true;
                    _sharedStateService.MouseOnCanvas = true;
                    _sharedStateService.MouseOnGrid = false;
                    break;
                case "GridViewDraggingDisallowedCursor":
                    _sharedStateService.IsDraggingElementModeActive = false;
                    _sharedStateService.IsDraggingGridColumnModeActive = true;
                    _sharedStateService.MouseOnGrid = false;
                    _sharedStateService.MouseOnCanvas = false;
                    break;
                case "GridViewLeaveCursor":
                    _sharedStateService.IsDraggingElementModeActive = true;
                    _sharedStateService.IsDraggingGridColumnModeActive = false;
                    _sharedStateService.MouseOnGrid = false;
                    _sharedStateService.MouseOnCanvas = false;
                    break;
            }

            GridCursor = null;
            //OnPropertyChanged(nameof(GridCursor));
        }

        private void GetDraggedGridViewColumnIndex(object parameter)
        {

            if (parameter is int columnIndex)
            {
                _sharedCanvasPageItems.TableColumn = TableColumnsViewModel[columnIndex];
            }
        }

        public Cursor GridCursor
        {
            get => _mouseCursorService.CurrentCursor;
            private set
            {
                if (_mouseCursorService.CurrentCursor != value)
                {
                    _mouseCursorService.UpdateCursor();
                }
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
