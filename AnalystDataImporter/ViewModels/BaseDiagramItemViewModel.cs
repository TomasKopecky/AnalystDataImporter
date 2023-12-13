﻿using System;
using System.ComponentModel;
using AnalystDataImporter.Models;

namespace AnalystDataImporter.ViewModels
{
    public abstract class BaseDiagramItemViewModel : INotifyPropertyChanged
    {
        protected bool _isSelected;
        // Pozice prvku na plátně
        protected double _xPosition;

        protected double _yPosition;

        protected BaseDiagramItem _model;

        protected bool _isTemporary;

        protected TableColumnViewModel _gridTableColumn;

        // Common properties and methods for view models, such as IsSelected, PropertyChanged, etc.
        public virtual bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public int Id
        {
            get => _model.Id;
            set
            {
                if (_model.Id != value)
                {
                    _model.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Label
        {
            get => _model.Label;
            set
            {
                if (_model.Label != value)
                {
                    _model.Label = value;
                    OnPropertyChanged(nameof(Label));
                }
            }
        }

        public string Title
        {
            get => _model.Title;
            set
            {
                if (_model.Title != value)
                {
                    _model.Title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        public string Type
        {
            get => _model.Type;
            set
            {
                if (_model.Type != value)
                {
                    _model.Type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        public DateTime Date
        {
            get => _model.Date;
            set
            {
                if (_model.Date != value)
                {
                    _model.Date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        public TableColumnViewModel GridTableColumn
        {
            get => _gridTableColumn;
            set
            {
                if (_gridTableColumn != value)
                {
                    _gridTableColumn = value;
                    Label = _gridTableColumn.Heading;
                    OnPropertyChanged(nameof(GridTableColumn));
                }
            }
        }

        public double XPosition => _xPosition;

        public double YPosition => _yPosition;

        // ... other common properties and methods

        public event PropertyChangedEventHandler PropertyChanged;

        // ... implementation of INotifyPropertyChanged

        /// <summary>
        /// Metoda pro oznamování změn vlastností.
        /// </summary>
        /// <param name="propertyName">Jméno vlastnosti, která se změnila.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}