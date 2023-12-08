using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;
using AnalystDataImporter.Utilities;

namespace AnalystDataImporter.Services
{
    public class MouseCursorService : INotifyPropertyChanged
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, UInt16 lpCursorName);

        private readonly SharedStatesService _sharedStateService;
        public event PropertyChangedEventHandler PropertyChanged;
        private string _currentCursorIcon = "Arrow";
        private Cursor _currentCursor = Cursors.Arrow;
        public Cursor CopyCursor { get; private set; }

        public MouseCursorService(SharedStatesService sharedStateService)
        {
            _sharedStateService = sharedStateService;
            IntPtr copyCursorHandle = LoadCursor(LoadLibrary("ole32.dll"), 6);
            CopyCursor = CursorInteropHelper.Create(new SafeCursorHandle(copyCursorHandle));
        }

        public Cursor CurrentCursor
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

        //public string CurrentCursor
        //{
        //    get => _currentCursor;
        //    private set
        //    {
        //        if (_currentCursor != value)
        //        {
        //            _currentCursor = value;
        //            CursorChanged?.Invoke(this, EventArgs.Empty);
        //        }
        //    }
        //}

        public event EventHandler CursorChanged;

        public void UpdateCursor()
        {
            // Define cursor based on the state priorities
            if (_sharedStateService.IsDrawingRelationModeActive)
            {
                //CurrentCursorIcon = "Cross";
                CurrentCursor = Cursors.Cross;
            }
            else if (_sharedStateService.IsDrawingElement)
            {
                //CurrentCursorIcon = _sharedStateService.IsAddingElementOutsideCanvas ? "Arrow" : "None";
                CurrentCursor = _sharedStateService.IsAddingElementOutsideCanvas ? Cursors.Arrow : Cursors.None;
            }
            else if (_sharedStateService.MouseOnGrid)
            {
                //CurrentCursorIcon = "Hand";
                CurrentCursor = Cursors.Hand;
            }
            else if (_sharedStateService.IsDraggingGridColumnModeActive)
            {
                if (_sharedStateService.MouseOnCanvas) // TODO: zde ještě bude třeba dodat podmínku pro přítomnost nad textblock nebo gridview, ve kterém budou properites vybraného prvku v canvas na pravé straně vedle canvas
                    //CurrentCursorIcon = "Copy";
                    CurrentCursor = CopyCursor;
                else
                    //CurrentCursorIcon = "No";
                    CurrentCursor = Cursors.No;
            }
            else if (_sharedStateService.MouseOnElement)
            {
                //CurrentCursorIcon = "SizeAll";
                CurrentCursor = Cursors.SizeAll;
            }
            else
            {
                //CurrentCursorIcon = "Arrow";
                CurrentCursor = Cursors.Arrow;
            }

            Debug.WriteLine(CurrentCursor);
            OnPropertyChanged(nameof(CurrentCursor));
        }


        //public void UpdateCursorForCanvas(bool isDrawingElement, bool isDrawingRelationModeActive, bool isMouseOnElement, bool isAddingElementOutsideCanvas)
        //{
        //    if (isDrawingElement)
        //        CurrentCursor = isAddingElementOutsideCanvas ? "Arrow" : "None";
        //    else if (isDrawingRelationModeActive)
        //        CurrentCursor = "Cross";
        //    else if (isMouseOnElement)
        //        CurrentCursor = "SizeAll";
        //    else
        //        CurrentCursor = "Arrow";

        //    OnPropertyChanged(nameof(CurrentCursor));
        //}

        //public void UpdateCursorForGrid(bool isMouseOnGrid, bool isDraggingColumn)
        //{
        //    // Logic to determine cursor for grid
        //    if (isMouseOnGrid)
        //        CurrentCursor = "Hand";
        //    else if (isDraggingColumn)
        //        CurrentCursor = "Wait";
        //    else
        //        CurrentCursor = "Arrow";

        //    OnPropertyChanged(nameof(CurrentCursor));
        //}

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
