﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AnalystDataImporter.Factories;
using AnalystDataImporter.Managers;
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
        private List<List<String>> Rows;
        public DataTable Table { get; set; }
        private readonly ITableColumnViewModelFactory _tableColumnViewModelFactory;
        private ObservableCollection<TableColumnViewModel> _columns;
        //public ObservableCollection<ColumnInfo> Columns { get; private set; }
        private string _gridCursor;
        private bool _mouseOnGrid;
        private bool _isDraggingColumnModeActive;
        public ICommand ChangeCursorWhenOperatingGridCommand { get; private set; }

        public GridViewModel(ITableColumnViewModelFactory tableColumnViewModelFactory)
        {
            ChangeCursorWhenOperatingGridCommand = new RelayCommand<string>(ChangeCursorByGridView);
            _tableColumnViewModelFactory = tableColumnViewModelFactory ?? throw new ArgumentNullException(nameof(tableColumnViewModelFactory));
        }



        public ObservableCollection<TableColumnViewModel> Columns
        {
            get => _columns;
            set
            {
                _columns = value;
                OnPropertyChanged(nameof(Columns));
            }
        }

        public void LoadTestData()
        {
            Table = new DataTable();

            Rows = new List<List<string>>();

            Columns = new ObservableCollection<TableColumnViewModel>();

            TableColumnViewModel tableColumn = _tableColumnViewModelFactory.Create();
            tableColumn.Heading = "SIM";

            Columns.Add(tableColumn);

            tableColumn = _tableColumnViewModelFactory.Create();
            tableColumn.Heading = "IMEI";

            Columns.Add(tableColumn);

            Rows.Add(new List<string> { "721546132", "3512135435453453" });
            Rows.Add(new List<string> { "777853453", "6534534534534355" });
            Rows.Add(new List<string> { "605325456", "2455778454452412" });
            Rows.Add(new List<string> { "606454354", "2124524564545335" });

            foreach (var item in Columns)
            {
                Table.Columns.Add(item.Heading, typeof(string));
            }

            foreach (var tableRow in Rows)
            {
                DataRow row = Table.NewRow();
                int i = 0;
                foreach (var rowValue in tableRow)
                {
                    try
                    {
                        row[i] = rowValue;
                    }
                    catch (Exception ex)
                    {
                        throw new NotImplementedException();
                    }
                    i++;
                }
                Table.Rows.Add(row);
            }





            //row["Column1"] = "Value1";
            //row["Column2"] = "Value2";
            //Table.Rows.Add(row);

            //Items = new ObservableCollection<TableColumnViewModel>();

            //var tableColumnViewModel = _tableColumnViewModelFactory.Create();
            //tableColumnViewModel.Heading = "fsalfjasl";
            //tableColumnViewModel.Content.Add("fasldfjslkdjf");
            //tableColumnViewModel.Content.Add("fasldfjslkdjfdsfg");

            //Items.Add(tableColumnViewModel);

            // Example data loading
            //for (int i = 0; i < 10; i++)
            //{
            //    Items.Add(new TableColumnViewModel
            //    {
            //        Heading = "Item " + i + " Col1",
            //        //Content = "Item " + i + " Col2"
            //    });
            //}

            //Columns = new ObservableCollection<ColumnInfo>
            //{
            //    new ColumnInfo { Header = "falsdfjs", BindingPath = "asdljfal" }
            //};
        }

        public bool IsDraggingColumnModeActive
        {
            get => _isDraggingColumnModeActive;
            set
            {
                if (_isDraggingColumnModeActive == value) return;
                _isDraggingColumnModeActive = value;
                OnPropertyChanged(nameof(IsDraggingColumnModeActive));
            }
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
                    _isDraggingColumnModeActive = false;
                    _mouseOnGrid = true;
                    break;
                case "GridViewDraggingCursor":
                    _isDraggingColumnModeActive = true;
                    _mouseOnGrid = false;
                    break;
            }

            OnPropertyChanged(nameof(GridCursor));
        }


        public string GridCursor
        {
            get
            {
                if (_mouseOnGrid)
                    _gridCursor = "Hand";
                else if (IsDraggingColumnModeActive)
                    _gridCursor = "Wait";
                else
                    _gridCursor = "Arrow";

                //Debug.WriteLine("Getting cursor: " + _canvasCursor);
                return _gridCursor;
            }
            set
            {
                _gridCursor = value;
                OnPropertyChanged(nameof(GridCursor));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AnalystDataImporter.ViewModels
//{
//    public class GridViewModel : INotifyPropertyChanged
//    {
//        //public GridViewModel()
//        //{
//        //    LoadData();
//        //}

//        private ObservableCollection<KeyValuePair<int, string>> _items;

//        public ObservableCollection<KeyValuePair<int, string>> Items
//        {
//            get => _items;
//            set
//            {
//                _items = value;
//                OnPropertyChanged(nameof(Items));
//            }
//        }
//        //private ObservableCollection<TableColumnViewModel> _items;

//        //public ObservableCollection<TableColumnViewModel> Items
//        //{
//        //    get => _items;
//        //    set
//        //    {
//        //        _items = value;
//        //        OnPropertyChanged(nameof(Items));
//        //    }
//        //}

//        public void LoadTestData()
//        {
//            // Initialize the collection
//            Items = new ObservableCollection<KeyValuePair<int, string>>();

//            // Generate sample data
//            for (int i = 1; i <= 10; i++) // Create 10 sample rows
//            {
//                Items.Add(new KeyValuePair<int, string>(i, $"Item {i}"));
//            }
//        }


//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void OnPropertyChanged(string propertyName)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }

//}
