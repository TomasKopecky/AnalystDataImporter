using System;
using AnalystDataImporter.Models;
using System.ComponentModel;

namespace AnalystDataImporter.ViewModels
{
    /// <summary>
    /// ViewModel třída pro model Source. Definuje vlastnosti zdroje a poskytuje mechanismus k upozornění uživatelského rozhraní na jakékoli změny ve vlastnostech ViewModelu.
    /// </summary>
    public class SourceViewModel : INotifyPropertyChanged
    {
        private readonly Source _source;

        public SourceViewModel(Source source)
        {
            // Ověřte, zda poskytnutý source není null a inicializujte interní _source
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// Vlastnost ID zdroje.
        /// </summary>
        public int Id
        {
            get => _source.Id;
            set
            {
                if (_source.Id != value)
                {
                    _source.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        /// <summary>
        /// Hlavička zdroje.
        /// </summary>
        public string Heading
        {
            get => _source.Heading;
            set
            {
                if (_source.Heading != value)
                {
                    _source.Heading = value;
                    OnPropertyChanged(nameof(Heading));
                }
            }
        }

        /// <summary>
        /// Podnázev zdroje 1.
        /// </summary>
        public string Subname1
        {
            get => _source.Suname1;
            set
            {
                if (_source.Suname1 != value)
                {
                    _source.Suname1 = value;
                    OnPropertyChanged(nameof(Subname1));
                }
            }
        }

        /// <summary>
        /// Podnázev zdroje 2.
        /// </summary>
        public string Subname2
        {
            get => _source.Subname2;
            set
            {
                if (_source.Subname2 != value)
                {
                    _source.Subname2 = value;
                    OnPropertyChanged(nameof(Subname2));
                }
            }
        }

        /// <summary>
        /// Celkové jméno zdroje.
        /// </summary>
        public string Name
        {
            get => _source.Name;
            set
            {
                if (_source.Name != value)
                {
                    _source.Name = value;
                    OnPropertyChanged(nameof(Name));
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
