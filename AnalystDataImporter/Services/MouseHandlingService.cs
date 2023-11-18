using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Services
{
    public class MouseHandlingService : IMouseHandlingService
    {
        public UIElement _currentElement { get; set; }
        //private Point _startPosition;
        private Point _anchorPoint; // Position where the drag started
        private BaseDiagramItemViewModel _currentViewModelElement;
        bool _isBeingUsed;

        public UIElement CurrentElement()
        {
            return _currentElement;
        }


    public bool IsMouseOverElement(UIElement element, Point mousePosition)
    {
        return true;
        // Implement hit testing logic
        // Return true if the mouse is over the element
    }

    public bool IsMouseInCanvas(Point mousePosition, Canvas canvas)
    {
            if (mousePosition.X >= 0 && mousePosition.X <= canvas.ActualWidth &&
                mousePosition.Y >= 0 && mousePosition.Y <= canvas.ActualHeight)
                return true;

            return false;
    }


    public void CheckDraggingDrawnElementOutsideCanvas()
        {
                if (_currentElement != null)
                {
                    _currentElement.CaptureMouse();
                }
        }

    public void StartDragOrSelectOperation(UIElement element, Point? startPosition, BaseDiagramItemViewModel itemViewModel, bool temporary)
    {
            Debug.WriteLine("MouseHandlingService: StartDragOrSelectOperation");

            if (_currentElement == null)
            { 

            _currentElement = element;
        _currentViewModelElement = itemViewModel;
        if (startPosition != null) _anchorPoint = (Point)startPosition;

                // Start capturing the mouse on the element
                if (_currentElement != null)
                {
                    if (!temporary)
                        SelectElement();

                    if (_currentViewModelElement is ElementViewModel)
                    {
                        _currentElement.CaptureMouse();
                    }
                }
        }
    }

    public void EndDragOperation()
    {
            Debug.WriteLine("MouseHandlingService: EndDragOperation");
            // Release mouse capture
            if (_currentElement != null)
        {
            _currentElement.ReleaseMouseCapture();
            _currentElement = null;
        }
    }

        public void UpdateDragOperationWhenDrawing(Point currentPosition, Canvas parentCanvas)
        {
            if (_currentElement == null) return;


            if (_currentElement is FrameworkElement associatedElement && associatedElement.DataContext is ElementViewModel viewModel)
            {
                if (IsMouseInCanvas(currentPosition,parentCanvas))
                {
                    Debug.WriteLine("MouseHandlingService: UpdateDragOperationWhenDrawing inside canvas");
                    //tempElement.XPosition = mousePosition.X;
                    //tempElement.YPosition = mousePosition.Y;
                    viewModel.XCenter = currentPosition.X;
                    viewModel.YCenter = currentPosition.Y;
                }
                else
                    Debug.WriteLine("MouseHandlingService: UpdateDragOperationWhenDrawing outside canvas");
            }
        }

        public void UpdateDragOperationWhenDragging(Point currentPosition, Canvas parentCanvas, Grid parentGrid)
    {
        if (_currentElement == null) return;

        if (_currentElement is FrameworkElement associatedElement && associatedElement.DataContext is ElementViewModel viewModel)
        {

            bool isInsideCanvas = currentPosition.X >= 0 && currentPosition.Y >= 0 &&
                                  currentPosition.X <= parentCanvas.ActualWidth &&
                                  currentPosition.Y <= parentCanvas.ActualHeight;



                if (isInsideCanvas)
                {
                    Debug.WriteLine("MouseHandlingService: UpdateDragOperationWhenDragging: inside canvas");
                    double newX = currentPosition.X - _anchorPoint.X + viewModel.XPosition;
                    double newY = currentPosition.Y - _anchorPoint.Y + viewModel.YPosition;

                    newX = Math.Max(0, Math.Min(newX, parentCanvas.ActualWidth - parentGrid.ActualWidth));
                    newY = Math.Max(0, Math.Min(newY, parentCanvas.ActualHeight - parentGrid.ActualHeight));

                    viewModel.XPosition = newX;
                    viewModel.YPosition = newY;

                    _anchorPoint = currentPosition;
                }
                else
                {
                    Debug.WriteLine("Element: MouseMove: outside canvas");

                    // Make sure the ellipse is still within the canvas bounds
                    viewModel.XPosition = Math.Max(0,
                        Math.Min(viewModel.XPosition, parentCanvas.ActualWidth - parentGrid.ActualWidth));
                    viewModel.YPosition = Math.Max(0,
                        Math.Min(viewModel.YPosition, parentCanvas.ActualHeight - parentGrid.ActualHeight));

                    // Update the anchor point to the center of the ellipse
                    _anchorPoint = new Point(viewModel.XCenter, viewModel.YCenter);
                }
        }
        }

    private void SelectElement()
    {
        _currentViewModelElement.IsSelected = true;
    }


    }

}
