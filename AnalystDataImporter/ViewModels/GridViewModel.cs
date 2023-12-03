using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.ViewModels
{
    public class GridViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TableColumnViewModel> _items;

        public ObservableCollection<TableColumnViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private void LoadData()
        {
            // Load or generate data here
            Items = new ObservableCollection<TableColumnViewModel>
            {
                // Add sample data here
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
