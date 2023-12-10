using System;
using AnalystDataImporter.Models;
using System.ComponentModel;
using System.Collections.Generic;

namespace AnalystDataImporter.ViewModels
{
    /// <summary>
    /// ViewModel třída pro model TableColumn. Definuje vlastnosti sloupce tabulky a poskytuje mechanismus k upozornění uživatelského rozhraní na jakékoli změny ve vlastnostech ViewModelu.
    /// </summary>
    public class TableColumnViewModel : INotifyPropertyChanged
    {
        private readonly TableColumn _tableColumn;
        private double _width;


        public TableColumnViewModel(TableColumn tableColumn)
        {
            // Ověřte, zda poskytnutý tableColumn není null a inicializujte interní _tableColumn
            _tableColumn = tableColumn ?? throw new ArgumentNullException(nameof(tableColumn));
            _tableColumn.Content = new List<string>();
            _tableColumn.Temporary = true;
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

        public int Index
        {
            get => _tableColumn.Index;
            set
            {
                if (_tableColumn.Index != value)
                {
                    _tableColumn.Index = value;
                    OnPropertyChanged(nameof(Index));
                }
            }
        }

        public bool Temporary
        {
            get => _tableColumn.Temporary;
            set
            {
                if (_tableColumn.Temporary != value)
                {
                    _tableColumn.Temporary = value;
                    OnPropertyChanged(nameof(Temporary));
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

        public List<string> Content
        {
            get => _tableColumn.Content;
            set
            {
                OnPropertyChanged(nameof(Content));
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

        public double Width
        {
            get => _width;
            set
            {
                if (_width != value)
                {
                    _width = value;
                    OnPropertyChanged(nameof(Width));
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