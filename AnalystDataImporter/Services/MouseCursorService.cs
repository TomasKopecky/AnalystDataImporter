using System;
using System.ComponentModel;

namespace AnalystDataImporter.Services
{
    public class MouseCursorService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _currentCursor = "Arrow";

        public string CurrentCursor
        {
            get => _currentCursor;
            private set
            {
                if (_currentCursor != value)
                {
                    _currentCursor = value;
                    CursorChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler CursorChanged;

        public void UpdateCursorForCanvas(bool isDrawingElement, bool isDrawingRelationModeActive, bool isMouseOnElement, bool isAddingElementOutsideCanvas)
        {
            if (isDrawingElement)
                CurrentCursor = isAddingElementOutsideCanvas ? "Arrow" : "None";
            else if (isDrawingRelationModeActive)
                CurrentCursor = "Cross";
            else if (isMouseOnElement)
                CurrentCursor = "SizeAll";
            else
                CurrentCursor = "Arrow";

            OnPropertyChanged(nameof(CurrentCursor));
        }

        public void UpdateCursorForGrid(bool isMouseOnGrid, bool isDraggingColumn)
        {
            // Logic to determine cursor for grid
            if (isMouseOnGrid)
                CurrentCursor = "Hand";
            else if (isDraggingColumn)
                CurrentCursor = "Wait";
            else
                CurrentCursor = "Arrow";

            OnPropertyChanged(nameof(CurrentCursor));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
