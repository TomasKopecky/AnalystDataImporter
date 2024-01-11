using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media;
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
        private ElementViewModel _objectFrom;

        /// <summary>
        /// Element, ke kterému vazba směřuje.
        /// </summary>
        private ElementViewModel _objectTo;

        /// <summary>
        /// X-ová souřadnice počátečního bodu vazby.
        /// </summary>
        public double _x1;

        /// <summary>
        /// Y-ová souřadnice počátečního bodu vazby.
        /// </summary>
        public double _y1;

        /// <summary>
        /// X-ová souřadnice koncového bodu vazby.
        /// </summary>
        public double _x2;

        /// <summary>
        /// Y-ová souřadnice koncového bodu vazby.
        /// </summary>
        public double _y2;

        public int _zIndex;

        public RelationViewModel(Relation relation, System.Windows.Point fromPoint, System.Windows.Point toPoint)
        {
            // Ověřte, zda poskytnutý relation není null a inicializujte interní _relation
            _relation = relation ?? throw new ArgumentNullException(nameof(relation));

            this._x1 = fromPoint.X;
            this._y1 = fromPoint.Y;

            this._x2 = toPoint.X;
            this._y2 = toPoint.Y;
            _model = _relation;
            Type = "relation";
            Multiplicity = "Jednoduchá"; // Násobnost propojení
            ColorValue = Constants.Colors.ElementAt(0).Value; // Barva
            Label = string.Empty; // Popisek
            DirectionValue = Constants.RelationDirections.ElementAt(0).Value; // Směr = "0"
            Style = Constants.Style.ElementAt(0).Value; // Síla (Styl) ((Čendy - Ha. ha.))
            Date = string.Empty; // Datum - prázdné (protože jde o sloupec!!)
            Time = string.Empty; // Čas - prázdný (protože jde o sloupec!!)
            Thickness = 1.0; // Šířka (Tloušťka)
            Description = string.Empty; //Popis
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
        /// Barva relace přímo v hodnotě System.Drawing.Color.
        /// </summary>
        public string ColorValue
        {
            get => _relation.ColorValue;
            set
            {
                if (_relation.ColorValue != value)
                {
                    _relation.ColorValue = value;
                    ColorKey = Constants.Colors.FirstOrDefault(color => color.Value.ToString() == _relation.ColorValue.ToString()).Key;

                    OnPropertyChanged(nameof(ColorValue));
                }
            }
        }
        /// <summary>
        /// Barva relace přímo v hodnotě System.Drawing.Color.
        /// </summary>
        public string ColorKey
        {
            get => _relation.ColorKey;
            set
            {
                if (_relation.ColorKey != value)
                {
                    _relation.ColorKey = value;
                    OnPropertyChanged(nameof(ColorKey));
                }
            }
        }

        /// <summary>
        /// Style relace ve smyslu stylu čáry
        /// </summary>
        public DoubleCollection Style
        {
            get => _relation.Style;
            set
            {
                if (_relation.Style != value)
                {
                    _relation.Style = value;
                    OnPropertyChanged(nameof(Style));
                }
            }
        }
        ///// <summary>
        ///// Style relace ve smyslu stylu čáry - 
        ///// </summary>
        //public DoubleCollection StyleValue
        //{
        //    get => _relation.StyleValue;
        //    set
        //    {
        //        if (_relation.StyleValue != value)
        //        {
        //            _relation.StyleValue = value;
        //            ColorKey = Constants.Colors.FirstOrDefault(styl => styl.Value.ToString() == _relation.StyleValue.ToString()).Key;

        //            OnPropertyChanged(nameof(StyleValue));
        //        }
        //    }
        //}
        ///// <summary>
        ///// Style relace ve smyslu stylu čáry
        ///// </summary>
        //public string StyleKey
        //{
        //    get => _relation.StyleKey;
        //    set
        //    {
        //        if (_relation.StyleKey != value)
        //        {
        //            _relation.StyleKey = value;
        //            OnPropertyChanged(nameof(StyleKey));
        //        }
        //    }
        //}

        ///// <summary>
        ///// Směr relace (např. od-do).
        ///// </summary>
        //public Constants.RelationDirections Direction
        //{
        //    get => _relation.Direction;
        //    set
        //    {
        //        if (_relation.Direction != value)
        //        {
        //            _relation.Direction = value;
        //            OnPropertyChanged(nameof(Direction));
        //        }
        //    }
        //}
        /// <summary>
        /// Směr relace (např. od-do). - Hodnota (int)
        /// </summary>
        public string DirectionValue
        {
            get => _relation.DirectionValue;
            set
            {
                if (_relation.DirectionValue != value)
                {
                    _relation.DirectionValue = value;
                    DirectionKey = Constants.RelationDirections.FirstOrDefault(direction => direction.ToString() == _relation.DirectionValue.ToString()).Key;

                    OnPropertyChanged(nameof(DirectionValue));
                }
            }
        }
        /// <summary>
        /// Směr relace (např. od-do). - Klíč (string)
        /// </summary>
        public string DirectionKey
        {
            get => _relation.DirectionKey;
            set
            {
                if (_relation.DirectionKey != value)
                {
                    _relation.DirectionKey = value;
                    OnPropertyChanged(nameof(DirectionKey));
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

        public string Multiplicity
        {
            get => _relation.Multiplicity;
            set
            {
                if (_relation.Multiplicity != value)
                {
                    _relation.Multiplicity = value;
                    OnPropertyChanged(nameof(Multiplicity));
                }
            }
        }

        /// <summary>
        /// popis objektu
        /// </summary>
        public string Description
        {
            get => _relation.Description;
            set
            {
                if (_relation.Description != value)
                {
                    _relation.Description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public double Thickness
        {
            get => _relation.Thickness;
            set
            {
                if (_relation.Thickness != value)
                {
                    _relation.Thickness = value;
                    OnPropertyChanged(nameof(Thickness));
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
                _x2 = _objectTo?._xCenter ?? value;
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
                _y2 = _objectTo?._yCenter ?? value;
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
    }
}