using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.Services
{
    public class SharedStatesService : INotifyPropertyChanged
    {
        private bool _isAddingElementOutsideCanvas;
        private bool _isDraggingElementModeActive;
        private bool _isDrawingElement;
        private bool _isDrawingElementModeActive;
        private bool _isRelationDrawingModeActive;
        private bool _isDraggingGridColumnModeActive;
        private bool _mouseOnElement;
        private bool _mouseOnGrid;
        private bool _mouseOnCanvas;

        // Properties with notification

        public bool IsAddingElementOutsideCanvas
        {
            get => _isAddingElementOutsideCanvas;
            set
            {
                if (_isAddingElementOutsideCanvas == value) return;
                _isAddingElementOutsideCanvas = value;
                OnPropertyChanged(nameof(IsAddingElementOutsideCanvas));
            }
        }
        public bool IsDraggingElementModeActive
        {
            get => _isDraggingElementModeActive;
            set
            {
                if (_isDraggingElementModeActive == value) return;
                _isDraggingElementModeActive = value;
                OnPropertyChanged(nameof(IsDraggingElementModeActive));
            }
        }

        public bool IsDrawingElement
        {
            get => _isDrawingElement;
            set
            {
                if (_isDrawingElement == value) return;
                _isDrawingElement = value;
                OnPropertyChanged(nameof(IsDrawingElement));
            }
        }

        public bool IsDrawingElementModeActive
        {
            get => _isDrawingElementModeActive;
            set
            {
                if (_isDrawingElementModeActive == value) return;
                _isDrawingElementModeActive = value;
                OnPropertyChanged(nameof(IsDrawingElementModeActive));
            }
        }

        public bool IsDrawingRelationModeActive
        {
            get => _isRelationDrawingModeActive;
            set
            {
                if (_isRelationDrawingModeActive == value) return;
                _isRelationDrawingModeActive = value;
                OnPropertyChanged(nameof(IsDrawingRelationModeActive));
            }
        }

        public bool IsDraggingGridColumnModeActive
        {
            get => _isDraggingGridColumnModeActive;
            set
            {
                if (_isDraggingGridColumnModeActive == value) return;
                _isDraggingGridColumnModeActive = value;
                OnPropertyChanged(nameof(IsDraggingGridColumnModeActive));
            }
        }

        public bool MouseOnElement
        {
            get => _mouseOnElement;
            set
            {
                if (_mouseOnElement == value) return;
                _mouseOnElement = value;
                OnPropertyChanged(nameof(MouseOnElement));
            }
        }

        public bool MouseOnGrid
        {
            get => _mouseOnGrid;
            set
            {
                if (_mouseOnGrid == value) return;
                _mouseOnGrid = value;
                OnPropertyChanged(nameof(MouseOnGrid));
            }
        }

        public bool MouseOnCanvas
        {
            get => _mouseOnCanvas;
            set
            {
                if (_mouseOnCanvas == value) return;
                _mouseOnCanvas = value;
                OnPropertyChanged(nameof(MouseOnCanvas));
            }
        }

        // ... other properties

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
