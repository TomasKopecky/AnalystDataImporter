using System;
using System.Windows;
using AnalystDataImporter.Globals;
using AnalystDataImporter.Models;

namespace AnalystDataImporter.ViewModels
{
    /// <summary>
    /// ViewModel pro jednotlivé prvky v aplikaci. Implementuje rozhraní INotifyPropertyChanged pro oznamování změn vlastností.
    /// </summary>
    public class ElementViewModel : BaseDiagramItemViewModel
    {
        private readonly Element _element;

        //private bool _isSelected;

        public bool temporary = false;

        //// Pozice prvku na plátně
        public new double _xPosition;

        public new double _yPosition;

        public double _xCenter;
        public double _yCenter;

        // Poslední známá pozice myši (může být užitečné pro drag & drop operace)
        public double LastMouseX { get; set; }

        public double LastMouseY { get; set; }

        public int _zIndex;

        public double _width;
        public double _height;

        /// <summary>
        /// Konstruktor třídy ElementViewModel. Přijímá instanci prvku, který má reprezentovat.
        /// </summary>
        public ElementViewModel(Element element)
        {
            _element = element ?? throw new ArgumentNullException(nameof(element));
            _model = _element;
            //_element = element ?? throw new ArgumentNullException(nameof(element));
        }

        public void ConfigureTempElement(double x, double y)
        {
            temporary = true;
            XCenter = x;
            YCenter = y;
            ZIndex = 2;
            //IsSelected = true;
        }

        public void FinishTempElement(Point point)
        {
            XCenter = point.X;
            YCenter = point.Y;
            Label = Constants.defaultElementLabel;
            Title = Constants.defaultElementTitle;
            temporary = false;
            ZIndex = 1;
            IsSelected = true;
        }

        public void FinishTempElement()
        {
            Label = Constants.defaultElementLabel;
            Title = Constants.defaultElementTitle;
            temporary = false;
            ZIndex = 1;
            IsSelected = true;
        }

        // Vlastnosti prvku a jejich gettery a settery s logikou pro oznamování změn vlastností

        public DateTime DateFrom
        {
            get => _element.DateFrom;
            set
            {
                if (_element.DateFrom != value)
                {
                    _element.DateFrom = value;
                    OnPropertyChanged(nameof(DateFrom));
                }
            }
        }

        public DateTime DateTo
        {
            get => _element.DateTo;
            set
            {
                if (_element.DateTo != value)
                {
                    _element.DateTo = value;
                    OnPropertyChanged(nameof(DateTo));
                }
            }
        }

        public Constants.Icons Icon
        {
            get => _element.Icon;
            set
            {
                if (_element.Icon != value)
                {
                    _element.Icon = value;
                    OnPropertyChanged(nameof(Icon));
                }
            }
        }

        public bool HasFrame
        {
            get => _element.HasFrame;
            set
            {
                if (_element.HasFrame != value)
                {
                    _element.HasFrame = value;
                    OnPropertyChanged(nameof(HasFrame));
                }
            }
        }

        public new double XPosition
        {
            get => _xPosition;
            set
            {
                _xPosition = value;
                if (!temporary)
                {
                    if (_width > 0)
                        _xCenter = _xPosition + (_width / 2.0); 
                    
                    else
                        _xCenter = _xPosition + Constants.ellipseWidth / 2;
                }

                OnPropertyChanged(nameof(XPosition));
            }
        }

        public new double YPosition
        {
            get => _yPosition;
            set
            {
                _yPosition = value;
                if (!temporary)
                    _yCenter = _yPosition + (Constants.ellipseWidth / 2) + Constants.selectBorderThickness;

                OnPropertyChanged(nameof(YPosition));
            }
        }

        public double XCenter
        {
            get => _xCenter;
            set
            {
                //if (!temporary) return;
                _xCenter = value;
                if (temporary)
                    XPosition = _xCenter - (_width / 2);
            }
        }

        public double YCenter
        {
            get => _yCenter;
            set
            {
                //if (!temporary) return;
                _yCenter = value;
                if (temporary)
                    YPosition = _yCenter - (Constants.ellipseWidth / 2);
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

        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                //_xCenter = _xPosition + (_width / 2.0);
                XPosition = _xCenter - (_width / 2.0);
                OnPropertyChanged(nameof(Width));
            }
        }

        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        // Událost, která se vyvolá při změně vlastnosti
        //public event PropertyChangedEventHandler PropertyChanged;

        ///// <summary>
        ///// Metoda pro oznamování změn vlastností.
        ///// </summary>
        ///// <param name="propertyName">Jméno vlastnosti, která se změnila.</param>
        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}