using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AnalystDataImporter.Models;

namespace AnalystDataImporter.ViewModels
{
    /// <summary>
    /// ViewModel třída pro model DataSource. Definuje vlastnosti pro uživatele, zdroje a specifikace. Poskytuje mechanismus k upozornění uživatelského rozhraní na jakékoli změny ve vlastnostech ViewModelu.
    /// </summary>
    public class DataSourceViewModel : INotifyPropertyChanged
    {
        private readonly DataSource _dataSource;

        public DataSourceViewModel(DataSource dataSource)
        {
            // Ověřte, zda poskytnutý dataSource není null a inicializujte interní _dataSource
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

            // Převeďte seznamy na ObservableCollection pro lepší vazbu UI
            this.Sources = new ObservableCollection<SourceViewModel>();
            foreach (var source in _dataSource.Sources)
            {
                this.Sources.Add(new SourceViewModel(source));
            }

            this.Specifications = new ObservableCollection<SpecificationViewModel>();
            foreach (var spec in _dataSource.Specifications)
            {
                this.Specifications.Add(new SpecificationViewModel(spec));
            }
        }

        /// <summary>
        /// Vlastnost Uživatel, která vrátí nebo nastaví uživatele pro tento DataSource.
        /// </summary>
        public User User
        {
            get => _dataSource.User;
            set
            {
                if (_dataSource.User != value)
                {
                    _dataSource.User = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }

        /// <summary>
        /// Kolekce ViewModelů zdrojů pro lepší vazbu UI.
        /// </summary>
        public ObservableCollection<SourceViewModel> Sources { get; set; }

        /// <summary>
        /// Kolekce ViewModelů specifikací pro lepší vazbu UI.
        /// </summary>
        public ObservableCollection<SpecificationViewModel> Specifications { get; set; }

        // Událost, která se vyvolá, pokud se změní některá z vlastností ViewModelu
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