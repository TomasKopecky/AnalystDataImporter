using System.Windows;
using System.Windows.Controls;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Services
{
    public interface IMouseHandlingService
    {
        //UIElement CurrentElement { get; set; }
        BaseDiagramItemViewModel CurrentViewModelElement { get; }
        bool IsInUse { get; }
        bool IsMouseInCanvas(Point mousePosition, Canvas canvas);

        void StartOperation(UIElement element, Point? startPosition, BaseDiagramItemViewModel itemViewModel,
            string operationType);//, bool temporary);

        void UpdateOperation(Point currentPosition, Canvas parentCanvas, Grid parentGrid);

        //void StartRelationDragOrSelectOperation(UIElement element, Point? startPosition, BaseDiagramItemViewModel itemViewModel, bool temporary);
        //void StartDragOrSelectOperation(UIElement element, Point? startPosition, BaseDiagramItemViewModel itemViewModel, bool temporary);

        //void UpdateDragOperationWhenDrawingRelation(Point currentPosition, Canvas parentCanvas);
        //void UpdateDragOperationWhenDragging(Point currentPosition, Canvas parentCanvas, Grid paretnGrid);
        //void UpdateDragOperationWhenDrawing(Point currentPosition, Canvas parentCanvas);
        void EndDragOperation();
        // Additional methods as needed
    }
}
