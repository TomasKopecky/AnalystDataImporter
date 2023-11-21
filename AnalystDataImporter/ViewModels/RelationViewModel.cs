using System;
using System.ComponentModel;
using System.Windows;
using AnalystDataImporter.Globals;
using AnalystDataImporter.Models;

namespace AnalystDataImporter.ViewModels
{
    /// <summary>
    /// ViewModel třída pro model Relation. Definuje vlastnosti relace a poskytuje mechanismus k upozornění uživatelského rozhraní na jakékoli změny ve vlastnostech ViewModelu.
    /// </summary>
    public class RelationViewModel : BaseDiagramItemViewModel
    {
        private readonly Relation _relation;

        //private bool _isSelected;

        private bool _isFinished;

        /// <summary>
        /// Element, od kterého vazba začíná.
        /// </summary>
        private ElementViewModel _objectFrom { get; set; }

        /// <summary>
        /// Element, ke kterému vazba směřuje.
        /// </summary>
        private ElementViewModel _objectTo { get; set; }

        /// <summary>
        /// X-ová souřadnice počátečního bodu vazby.
        /// </summary>
        public double _x1 { get; set; }

        /// <summary>
        /// Y-ová souřadnice počátečního bodu vazby.
        /// </summary>
        public double _y1 { get; set; }

        /// <summary>
        /// X-ová souřadnice koncového bodu vazby.
        /// </summary>
        public double _x2 { get; set; }

        /// <summary>
        /// Y-ová souřadnice koncového bodu vazby.
        /// </summary>
        public double _y2 { get; set; }

        public int _zIndex { get; set; }

        public RelationViewModel(Relation relation, Point fromPoint, Point toPoint)
        {
            // Ověřte, zda poskytnutý relation není null a inicializujte interní _relation
            _relation = relation ?? throw new ArgumentNullException(nameof(relation));
            //_objectFrom = objectFrom;
            //_objectTo = objectTo;

            this._x1 = fromPoint.X;
            this._y1 = fromPoint.Y;

            this._x2 = toPoint.X;
            this._y2 = toPoint.Y;
            _model = _relation;
        }

        // If either XPosition or YPosition of start element changes, update x1 and y1
        private void ObjectFrom_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "XPosition" || e.PropertyName == "YPosition")
            {
                OnPropertyChanged("X1");
                OnPropertyChanged("Y1");
            }
        }

        // If either XPosition or YPosition of end element changes, update x2 and y2
        private void ObjectTo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "XPosition" || e.PropertyName == "YPosition")
            {
                OnPropertyChanged("X2");
                OnPropertyChanged("Y2");
            }
        }

        /// <summary>
        /// Barva relace.
        /// </summary>
        public string Color
        {
            get => _relation.Color;
            set
            {
                if (_relation.Color != value)
                {
                    _relation.Color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }

        /// <summary>
        /// Směr relace (např. od-do).
        /// </summary>
        public Constants.RelationDirections Direction
        {
            get => _relation.Direction;
            set
            {
                if (_relation.Direction != value)
                {
                    _relation.Direction = value;
                    OnPropertyChanged(nameof(Direction));
                }
            }
        }

        /// <summary>
        /// Objekt, od kterého relace začíná.
        /// </summary>
        public ElementViewModel ObjectFrom
        {
            get => _objectFrom;
            set
            {
                if (_objectFrom != value)
                {
                    _objectFrom = value;
                    _objectFrom.PropertyChanged += ObjectFrom_PropertyChanged;
                    OnPropertyChanged(nameof(ObjectFrom));
                    OnPropertyChanged(nameof(X1));
                    OnPropertyChanged(nameof(Y1));
                }
            }
        }

        /// <summary>
        /// Objekt, ke kterému relace směřuje.
        /// </summary>
        public ElementViewModel ObjectTo
        {
            get => _objectTo;
            set
            {
                if (_objectTo != value)
                {
                    _objectTo = value;
                    _objectTo.PropertyChanged += ObjectTo_PropertyChanged;
                    OnPropertyChanged(nameof(ObjectTo));
                    OnPropertyChanged(nameof(X2));
                    OnPropertyChanged(nameof(Y2));
                }
            }
        }

        public double X1
        {
            get
            {
                if (_objectFrom != null)
                {
                    return _objectFrom._xCenter;
                }
                else
                {
                    return _x1;
                }
            }
        }

        public double Y1
        {
            get
            {
                if (_objectFrom != null)
                {
                    return _objectFrom._yCenter;
                }
                else
                {
                    return _y1;
                }
            }
        }

        public double X2
        {
            get
            {
                if (_objectTo != null)
                {
                    return _objectTo._xCenter;
                }
                else
                {
                    return _x2;
                }
            }
            set
            {
                if (_objectTo == null)
                {
                    _x2 = value;
                }
                else
                {
                    _x2 = _objectTo._xCenter;
                }
                OnPropertyChanged(nameof(X2));
            }
        }

        public double Y2
        {
            get
            {
                if (_objectTo != null)
                {
                    return _objectTo._yCenter;
                }
                else
                {
                    return _y2;
                }
            }
            set
            {
                if (_objectTo == null)
                {
                    _y2 = value;
                }
                else
                {
                    _y2 = _objectTo._yCenter;
                }
                OnPropertyChanged(nameof(Y2));
            }
        }

        public int ZIndex
        {
            get => _zIndex;
            set
            {
                if (_zIndex != value)
                {
                    _zIndex = value;
                    OnPropertyChanged(nameof(ZIndex));
                }
            }
        }

        public bool IsFinished
        {
            get => _isFinished;
            set
            {
                if (_isFinished != value)
                {
                    _isFinished = value;
                    OnPropertyChanged(nameof(IsFinished));
                }
            }
        }

        //public bool IsSelected
        //{
        //    get { return _isSelected; }
        //    set
        //    {
        //        _isSelected = value;
        //        OnPropertyChanged(nameof(IsSelected));
        //    }
        //}

        ///// <summary>
        ///// Událost, která se vyvolá, pokud se změní některá z vlastností ViewModelu.
        ///// </summary>
        //public event PropertyChangedEventHandler PropertyChanged;

        ///// <summary>
        ///// Metoda vyvolává událost PropertyChanged.
        ///// </summary>
        ///// <param name="propertyName">Název změněné vlastnosti.</param>
        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}