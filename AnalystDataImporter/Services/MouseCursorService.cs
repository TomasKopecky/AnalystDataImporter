using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.Services
{
    public class MouseCursorService
    {
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

        public void UpdateCursorForCanvas(bool isDrawingElement, bool isDrawingRelation, bool isMouseOnElement, bool isMouseOnGridView)
        {
            // Logic to determine cursor for canvas
            if (isDrawingElement)
                CurrentCursor = isMouseOnGridView ? "Hand" : "None";
            else if (isDrawingRelation)
                CurrentCursor = "Cross";
            else if (isMouseOnElement)
                CurrentCursor = "SizeAll";
            else
                CurrentCursor = isMouseOnGridView ? "Hand" : "Arrow";
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
        }
    }

}
