using System;
using AnalystDataImporter.Globals;
using AnalystDataImporter.Models;
using System.ComponentModel;

namespace AnalystDataImporter.ViewModels
{
    /// <summary>
    /// ViewModel třída pro model Specification. Definuje vlastnosti specifikace a poskytuje mechanismus k upozornění uživatelského rozhraní na jakékoli změny ve vlastnostech ViewModelu.
    /// </summary>
    public class SpecificationViewModel : INotifyPropertyChanged
    {
        private readonly Specification _specification;

        public SpecificationViewModel(Specification specification)
        {
            // Ověřte, zda poskytnutý specification není null a inicializujte interní _specification
            _specification = specification ?? throw new ArgumentNullException(nameof(specification));
        }

        /// <summary>
        /// Vlastnost ID specifikace.
        /// </summary>
        public int Id
        {
            get => _specification.Id;
            set
            {
                if (_specification.Id != value)
                {
                    _specification.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        /// <summary>
        /// Název specifikace.
        /// </summary>
        public string Name
        {
            get => _specification.Name;
            set
            {
                if (_specification.Name != value)
                {
                    _specification.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        /// <summary>
        /// Cesta k XML souboru specifikace.
        /// </summary>
        public string XmlFilePath
        {
            get => _specification.XmlFilePath;
            set
            {
                if (_specification.XmlFilePath != value)
                {
                    _specification.XmlFilePath = value;
                    OnPropertyChanged(nameof(XmlFilePath));
                }
            }
        }

        /// <summary>
        /// Určuje, zda je specifikace veřejná.
        /// </summary>
        public bool IsPublic
        {
            get => _specification.IsPublic;
            set
            {
                if (_specification.IsPublic != value)
                {
                    _specification.IsPublic = value;
                    OnPropertyChanged(nameof(IsPublic));
                }
            }
        }

        /// <summary>
        /// Oddělovač pro importovaná data.
        /// </summary>
        public Constants.Delimiters Delimiter
        {
            get => _specification.Delimiter;
            set
            {
                if (_specification.Delimiter != value)
                {
                    _specification.Delimiter = value;
                    OnPropertyChanged(nameof(Delimiter));
                }
            }
        }

        /// <summary>
        /// Určuje, zda je první řádek hlavička.
        /// </summary>
        public bool IsFirstRowHeading
        {
            get => _specification.IsFirstRowHeading;
            set
            {
                if (_specification.IsFirstRowHeading != value)
                {
                    _specification.IsFirstRowHeading = value;
                    OnPropertyChanged(nameof(IsFirstRowHeading));
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
