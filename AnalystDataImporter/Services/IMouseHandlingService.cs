using System.Windows;
using System.Windows.Controls;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Services
{
    public interface IMouseHandlingService
    {
        object CurrentViewModelElement { get; }
        bool IsInUse { get; }
        bool IsMouseInCanvas(Point mousePosition, Canvas canvas);
        void StartOperation(UIElement element, Point? startPosition, object itemViewModel, string operationType);
        void UpdateOperation(Point currentPosition, Canvas parentCanvas, Grid parentGrid);
        void EndDragOperation();
    }
}
