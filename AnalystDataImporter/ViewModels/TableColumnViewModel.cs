using System;
using AnalystDataImporter.Models;
using System.ComponentModel;

namespace AnalystDataImporter.ViewModels
{
    /// <summary>
    /// ViewModel třída pro model TableColumn. Definuje vlastnosti sloupce tabulky a poskytuje mechanismus k upozornění uživatelského rozhraní na jakékoli změny ve vlastnostech ViewModelu.
    /// </summary>
    public class TableColumnViewModel : INotifyPropertyChanged
    {
        private readonly TableColumn _tableColumn;

        public TableColumnViewModel(TableColumn tableColumn)
        {
            // Ověřte, zda poskytnutý tableColumn není null a inicializujte interní _tableColumn
            _tableColumn = tableColumn ?? throw new ArgumentNullException(nameof(tableColumn));
        }

        /// <summary>
        /// Vlastnost ID sloupce tabulky.
        /// </summary>
        public int Id
        {
            get => _tableColumn.Id;
            set
            {
                if (_tableColumn.Id != value)
                {
                    _tableColumn.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        /// <summary>
        /// Hlavička sloupce tabulky.
        /// </summary>
        public string Heading
        {
            get => _tableColumn.Heading;
            set
            {
                if (_tableColumn.Heading != value)
                {
                    _tableColumn.Heading = value;
                    OnPropertyChanged(nameof(Heading));
                }
            }
        }

        /// <summary>
        /// Určuje, zda se mají nahradit prázdné hodnoty.
        /// </summary>
        public bool ReplaceEmptyValues
        {
            get => _tableColumn.ReplaceEmptyValues;
            set
            {
                if (_tableColumn.ReplaceEmptyValues != value)
                {
                    _tableColumn.ReplaceEmptyValues = value;
                    OnPropertyChanged(nameof(ReplaceEmptyValues));
                }
            }
        }

        /// <summary>
        /// Hodnota, kterou se mají nahradit prázdné hodnoty.
        /// </summary>
        public string ReplaceEmptyValuesWith
        {
            get => _tableColumn.ReplaceEmptyValuesWith;
            set
            {
                if (_tableColumn.ReplaceEmptyValuesWith != value)
                {
                    _tableColumn.ReplaceEmptyValuesWith = value;
                    OnPropertyChanged(nameof(ReplaceEmptyValuesWith));
                }
            }
        }

        /// <summary>
        /// Událost, která se vyvolá, pokud se změní některá z vlastností ViewModelu.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Metoda vyvolává událost PropertyChanged.
        /// </summary>
        /// <param name="propertyName">Název změněné vlastnosti.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}