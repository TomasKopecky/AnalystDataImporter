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
        

        private readonly SharedStatesService _sharedStateService;
        public event PropertyChangedEventHandler PropertyChanged;
        private Cursor _currentCursor = Cursors.Arrow;
        public Cursor CopyCursor { get; private set; }

        public MouseCursorService(SharedStatesService sharedStateService)
        {
            _sharedStateService = sharedStateService;
            IntPtr copyCursorHandle = NativeMethods.LoadCursor(NativeMethods.LoadLibrary("ole32.dll"), new IntPtr(6)); // Replace '6' with the correct constant for your cursor
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
