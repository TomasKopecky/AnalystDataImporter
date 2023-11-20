﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Services
{
    public interface IMouseHandlingService
    {
        UIElement CurrentElement { get; }
        BaseDiagramItemViewModel CurrentViewModelElement { get; }
        bool IsInUse { get; }
        void CaptureMouseOnDrawnElement();
        void CheckDraggingDrawnElementOutsideCanvas();
        bool IsMouseOverElement(UIElement element, Point mousePosition);
        bool IsMouseInCanvas(Point mousePosition, Canvas canvas);
        void StartDragWhenDrawingOperation(Point? startPosition, BaseDiagramItemViewModel itemViewModel, bool temporary);
        void StartDragOrSelectOperation(UIElement element, Point? startPosition, BaseDiagramItemViewModel itemViewModel, bool temporary);
        void UpdateDragOperationWhenDragging(Point currentPosition, Canvas parentCanvas, Grid paretnGrid);
        void UpdateDragOperationWhenDrawing(Point currentPosition, Canvas parentCanvas);
        void EndDragOperation();
        // Additional methods as needed
    }
}
