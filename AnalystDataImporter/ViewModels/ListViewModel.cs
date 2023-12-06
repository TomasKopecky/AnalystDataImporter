using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.ViewModels
{
    public class ListViewModel : INotifyPropertyChanged
    {
        //public GridViewModel()
        //{
        //    LoadData();
        //}

        private ObservableCollection<KeyValuePair<int, string>> _items;

        public ObservableCollection<KeyValuePair<int, string>> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        //private ObservableCollection<TableColumnViewModel> _items;

        //public ObservableCollection<TableColumnViewModel> Items
        //{
        //    get => _items;
        //    set
        //    {
        //        _items = value;
        //        OnPropertyChanged(nameof(Items));
        //    }
        //}

        public void LoadTestData()
        {
            // Initialize the collection
            Items = new ObservableCollection<KeyValuePair<int, string>>();

            // Generate sample data
            for (int i = 1; i <= 10; i++) // Create 10 sample rows
            {
                Items.Add(new KeyValuePair<int, string>(i, $"Item {i}"));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
