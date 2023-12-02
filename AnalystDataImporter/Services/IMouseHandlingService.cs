using System.Windows;
using System.Windows.Controls;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Services
{
    public interface IMouseHandlingService
    {
        BaseDiagramItemViewModel CurrentViewModelElement { get; }
        bool IsInUse { get; }
        bool IsMouseInCanvas(Point mousePosition, Canvas canvas);
        void StartOperation(UIElement element, Point? startPosition, BaseDiagramItemViewModel itemViewModel, string operationType);
        void UpdateOperation(Point currentPosition, Canvas parentCanvas, Grid parentGrid);
        void EndDragOperation();
    }
}
