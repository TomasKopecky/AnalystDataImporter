using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Services
{
    public class SharedCanvasPageItems : INotifyPropertyChanged
    {
        //ElementViewModel _fromElement;
        //ElementViewModel _toElement;
        TableColumnViewModel _tableColumn;

        public TableColumnViewModel TableColumn
        {
            get => _tableColumn;
            set
            {
                if (_tableColumn == value) return;
                _tableColumn = value;
                OnPropertyChanged(nameof(TableColumnViewModel));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
