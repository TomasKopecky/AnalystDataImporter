using System;
using System.Linq;
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
        public bool _temporary;
        public new double _xPosition;
        public new double _yPosition;
        public double _xCenter;
        public double _yCenter;
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
            Type = "element";
            Class = Constants.Classes[0];
            IconSourcePath = Constants.Icons.ElementAt(0).Value;
            Id = Label;
            Label = string.Empty; // Popisek
            Date = string.Empty; // Datum - prázdné (protože jde o sloupec!!)
            Time = string.Empty; // Čas - prázdný (protože jde o sloupec!!)
            Title = string.Empty; //Popis
        }

        public void ConfigureTempElement(double x, double y)
        {
            _temporary = true;
            XCenter = x;
            YCenter = y;
            ZIndex = 2;
        }

        public void FinishTempElement()
        {
            Label = Constants.DefaultElementLabel;
            Id = Constants.DefaultElementId;
            _temporary = false;
            ZIndex = 1;
            IsSelected = true;
        }

        public void ConfigureElementFromGridView(double x, double y, TableColumnViewModel tableColumnViewModel)
        {
            _temporary = true;
            XCenter = x;
            YCenter = y;
            ZIndex = 1;
            GridTableColumn = tableColumnViewModel;
        }

        public void FinishElementFromGridView()
        {
            _temporary = false;
            IsSelected = true;
        }

        // Vlastnosti prvku a jejich gettery a settery s logikou pro oznamování změn vlastností

        public DateTime DateFrom
        {
            get => _element.DateFrom;
            set
            {
                if (_element.DateFrom == value) return;

                _element.DateFrom = value;
                OnPropertyChanged(nameof(DateFrom));
            }
        }

        public DateTime DateTo
        {
            get => _element.DateTo;
            set
            {
                if (_element.DateTo == value) return;

                _element.DateTo = value;
                OnPropertyChanged(nameof(DateTo));
            }
        }

        public string IconSourcePath
        {
            get => _element.IconSourcePath;
            set
            {
                if (_element.IconSourcePath == value) return;

                _element.IconSourcePath = value;
                OnPropertyChanged(nameof(IconSourcePath));
            }
        }

        public bool HasFrame
        {
            get => _element.HasFrame;
            set
            {
                if (_element.HasFrame == value) return;

                _element.HasFrame = value;
                OnPropertyChanged(nameof(HasFrame));
            }
        }

        public new double XPosition
        {
            get => _xPosition;
            set
            {
                _xPosition = value;
                if (!_temporary)
                {
                    if (_width > 0)
                        _xCenter = _xPosition + (_width / 2.0);

                    else
                        _xCenter = _xPosition + Constants.EllipseWidth / 2;
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
                if (!_temporary)
                    _yCenter = _yPosition + (Constants.EllipseWidth / 2) + Constants.SelectBorderThicknessInt;

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
                if (_temporary)
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
                if (_temporary)
                    YPosition = _yCenter - (Constants.EllipseWidth / 2);
            }
        }

        public int ZIndex
        {
            get => _zIndex;
            set
            {
                if (_zIndex == value) return;

                _zIndex = value;
                OnPropertyChanged(nameof(ZIndex));
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

        public override bool IsSelected
        {
            get => base.IsSelected;
            set
            {
                base.IsSelected = value;
                if (value == true)
                    ZIndex= 2;
                else
                    ZIndex = 1;
            }
        }
    }
}