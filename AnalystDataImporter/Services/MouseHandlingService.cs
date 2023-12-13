using System.Windows;
using System.Windows.Controls;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Services
{
    /// <summary>
    /// Služba pro manipulaci s myší a interakci s UI prvky na plátně.
    /// </summary>
    public class MouseHandlingService : IMouseHandlingService
    {
        private UIElement _currentElement;
        private Point _anchorPoint;
        private string _operationType; // "dragging" nebo "drawing"

        /// <summary>
        /// Aktuální ViewModel prvku, s kterým je prováděna operace.
        /// </summary>
        public object CurrentViewModelElement { get; private set; }

        /// <summary>
        /// Indikátor, zda je služba v procesu použití.
        /// </summary>
        public bool IsInUse { get; private set; }

        /// <summary>
        /// Inicializuje operaci s myší v canvas s prvky elementViewModel (elipsa) a relationViewModel (line), jako je tažení nebo kreslení.
        /// </summary>
        /// <param name="element">Element, který bude ovládán.</param>
        /// <param name="startPosition">Počáteční pozice operace.</param>
        /// <param name="itemViewModel">ViewModel prvku, který je ovládán.</param>
        /// <param name="operationType">Typ operace ("dragging" nebo "drawing").</param>

        /// <summary>
        /// Inicializuje operaci s myší, jako je tažení nebo kreslení.
        /// </summary>
        /// <param name="element">Element, který bude ovládán.</param>
        /// <param name="startPosition">Počáteční pozice operace.</param>
        /// <param name="itemViewModel">ViewModel prvku, který je ovládán.</param>
        /// <param name="operationType">Typ operace ("dragging" nebo "drawing").</param>
        public void StartOperation(UIElement element, Point? startPosition, object itemViewModel, string operationType)//, bool temporary)
        {
            //Debug.WriteLine("MouseHandlingService: StartOperation - " + operationType);
            _operationType = operationType;
            _anchorPoint = startPosition ?? default;
            _currentElement = element;
            CurrentViewModelElement = itemViewModel;
            IsInUse = true;

            if (_currentElement == null) return;

            if (_operationType == "dragging")
            {
                SelectElement();
            }

            if (_operationType == "drawing" || (_operationType == "dragging" && CurrentViewModelElement is ElementViewModel) || (_operationType == "dragging" && CurrentViewModelElement is GridViewModel))
            {
                _currentElement.CaptureMouse();
            }
        }

        /// <summary>
        /// Aktualizuje operaci během tažení nebo kreslení.
        /// </summary>
        /// <param name="currentPosition">Aktuální pozice kurzoru myši.</param>
        /// <param name="parentCanvas">Plátno, na kterém se nachází prvek.</param>
        /// <param name="parentGrid">Grid, ve kterém se nachází prvek.</param>
        public void UpdateOperation(Point currentPosition, Canvas parentCanvas, Grid parentGrid)
        {
            if (CurrentViewModelElement == null || !IsInUse) return;

            if (IsMouseInCanvas(currentPosition, parentCanvas))
            {
                //Debug.WriteLine("MouseHandlingService: UpdateOperation - inside canvas");
                switch (_operationType)
                {
                    case "drawing":
                        UpdatePositionForDrawing(currentPosition);
                        break;
                    case "dragging":
                        UpdatePositionForDragging(currentPosition, parentCanvas, parentGrid);
                        break;
                }
            }
            else
            {
                //Debug.WriteLine("MouseHandlingService: UpdateOperation - outside canvas");
            }
        }

        /// <summary>
        /// Aktualizuje pozici prvku při táhnutí myší. Omezuje novou pozici tak, aby prvek zůstal v rámci rodičovského plátna.
        /// </summary>
        /// <param name="currentPosition">Aktuální pozice kurzoru myši.</param>
        /// <param name="parentCanvas">Plátno, na kterém se nachází prvek.</param>
        /// <param name="parentGrid">Grid, ve kterém se nachází prvek.</param>
        private void UpdatePositionForDragging(Point currentPosition, Canvas parentCanvas, Grid parentGrid)
        {
            if (!(CurrentViewModelElement is ElementViewModel viewModel)) return;

            double newX = currentPosition.X - _anchorPoint.X + viewModel.XPosition;
            double newY = currentPosition.Y - _anchorPoint.Y + viewModel.YPosition;

            viewModel.XPosition = Clamp(newX, 0, parentCanvas.ActualWidth - parentGrid.ActualWidth);
            viewModel.YPosition = Clamp(newY, 0, parentCanvas.ActualHeight - parentGrid.ActualHeight);
            _anchorPoint = currentPosition;
        }

        /// <summary>
        /// Aktualizuje pozici prvku nebo vazby při kreslení.
        /// </summary>
        /// <param name="currentPosition">Aktuální pozice kurzoru myši.</param>
        private void UpdatePositionForDrawing(Point currentPosition)
        {
            switch (CurrentViewModelElement)
            {
                case ElementViewModel element:
                    element.XCenter = currentPosition.X;
                    element.YCenter = currentPosition.Y;
                    break;
                case RelationViewModel relation:
                    relation.X2 = currentPosition.X;
                    relation.Y2 = currentPosition.Y;
                    break;
            }
        }

        /// <summary>
        /// Ukončí operaci tažení nebo kreslení.
        /// </summary>
        public void EndDragOperation()
        {
            //Debug.WriteLine("MouseHandlingService: EndDragOperation");
            if (_currentElement == null) return;

            CurrentViewModelElement = null;
            IsInUse = false;

            if (_currentElement.IsMouseCaptured)
                _currentElement.ReleaseMouseCapture();

            _currentElement = null;
        }

        /// <summary>
        /// Kontroluje, zda se kurzor myši nachází v rámci plátna.
        /// </summary>
        /// <param name="mousePosition">Pozice kurzoru myši.</param>
        /// <param name="canvas">Plátno pro kontrolu pozice.</param>
        /// <returns>True, pokud se kurzor nachází v rámci plátna.</returns>
        public bool IsMouseInCanvas(Point mousePosition, Canvas canvas)
        {
            return mousePosition.X >= 0 && mousePosition.X <= canvas.ActualWidth &&
                   mousePosition.Y >= 0 && mousePosition.Y <= canvas.ActualHeight;
        }

        /// <summary>
        /// Zajišťuje, že hodnota je v určeném rozmezí.
        /// </summary>
        /// <param name="value">Hodnota k omezení.</param>
        /// <param name="min">Minimální možná hodnota.</param>
        /// <param name="max">Maximální možná hodnota.</param>
        /// <returns>Omezená hodnota.</returns>
        private static double Clamp(double value, double min, double max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        /// <summary>
        /// Označí prvek jako vybraný.
        /// </summary>
        private void SelectElement()
        {
            if (CurrentViewModelElement is BaseDiagramItemViewModel viewModel)
                viewModel.IsSelected = true;
        }
    }
}