using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AnalystDataImporter.Models;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Services
{
    public class MouseHandlingService : IMouseHandlingService
    {
        private UIElement _currentElement;
        //private Point _startPosition;
        private Point _anchorPoint; // Position where the drag started
        private BaseDiagramItemViewModel _currentViewModelElement;
        private bool _isInUse = false;
        private string _lastItemTypeInUse;

        //public UIElement CurrentElement { get { return _currentElement; } }

        public BaseDiagramItemViewModel CurrentViewModelElement { get { return _currentViewModelElement; } }

        public bool IsInUse { get { return _isInUse; } }

        public string LastItemTypeInUse { get { return _lastItemTypeInUse; } }

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
            _currentElement?.CaptureMouse();
        }

        public void CaptureMouseOnDrawnElement()
        {
            _currentElement?.CaptureMouse();
        }

        public void StartDragWhenDrawingOperation(Point? startPosition, BaseDiagramItemViewModel itemViewModel, bool temporary)
        {
            if (!_isInUse && _currentViewModelElement == null)
            {
                if (startPosition != null) _anchorPoint = (Point)startPosition;

                _currentViewModelElement = itemViewModel;
                _isInUse = true;
            }
        }

        public void StartDragOrSelectOperation(UIElement element, Point? startPosition, BaseDiagramItemViewModel itemViewModel, bool temporary)
        {
            Debug.WriteLine("MouseHandlingService: StartDragOrSelectOperation");

            //if (_currentElement == null)
            //{
                if (startPosition != null) _anchorPoint = (Point)startPosition;

                _currentElement = element;
                _currentViewModelElement = itemViewModel;
                _isInUse = true;
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
            //}
        }

        public void UpdateDragOperationWhenDrawingRelation(Point currentPosition, Canvas parentCanvas)
        {
            if (_currentViewModelElement == null || !_isInUse) return;


            if (IsMouseInCanvas(currentPosition, parentCanvas) && _currentViewModelElement is RelationViewModel relation)
            {
                Debug.WriteLine("MouseHandlingService: UpdateDragOperationWhenDrawingRelation inside canvas");
                relation.X2 = currentPosition.X;
                relation.Y2 = currentPosition.Y;
            }
            else
                Debug.WriteLine("MouseHandlingService: UpdateDragOperationWhenDrawingRelation outside canvas");
        }

        public void StartRelationDragOrSelectOperation(UIElement element, Point? startPosition, BaseDiagramItemViewModel itemViewModel, bool temporary)
        {
            Debug.WriteLine("MouseHandlingService: StartRelationDragOrSelectOperation");

            if (_currentElement == null)
            {
                if (startPosition != null) _anchorPoint = (Point)startPosition;

                _currentElement = element;
                _currentViewModelElement = itemViewModel;
            _lastItemTypeInUse = "relation";
                // Start capturing the mouse on the element
                if (_currentElement != null)
                {
                    if (temporary)
                    {
                        if (_currentViewModelElement is RelationViewModel)
                        {
                            _isInUse = true;
                        
                            _currentElement.CaptureMouse();
                        //_currentElement.IsHitTestVisible = false;
                    }
                    }
                    else
                    {
                        EndDragOperation();
                    }
                }
            }
        }

        

        public void UpdateDragOperationWhenDrawing(Point currentPosition, Canvas parentCanvas)
        {
            if (_currentViewModelElement == null || !_isInUse) return;


            if (IsMouseInCanvas(currentPosition, parentCanvas) && _currentViewModelElement is ElementViewModel element)
            {
                Debug.WriteLine("MouseHandlingService: UpdateDragOperationWhenDrawing inside canvas");
                Debug.WriteLine("MouseHandlingService: UpdateDragOperationWhenDrawing inside canvas");
                element.XCenter = currentPosition.X;
                element.YCenter = currentPosition.Y;
            }
            else
                Debug.WriteLine("MouseHandlingService: UpdateDragOperationWhenDrawing outside canvas");
        }

        public void UpdateDragOperationWhenDragging(Point currentPosition, Canvas parentCanvas, Grid parentGrid)
        {
            if (_currentElement == null | !_isInUse) return;

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

        public void EndDragOperation()
        {
            Debug.WriteLine("MouseHandlingService: EndDragOperation");
            // Release mouse capture
            if (_currentElement != null)
            {
                _currentViewModelElement = null;
                _isInUse = false;
                if (_currentElement.IsMouseCaptured)
                    _currentElement.ReleaseMouseCapture();
                _currentElement = null;
            }
        }


    }

}
