using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Windows;
using AnalystDataImporter.Factories;
using AnalystDataImporter.Managers;
using AnalystDataImporter.Models;
using AnalystDataImporter.Services;
using AnalystDataImporter.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace AnalystDataImporterTestsProject
{
    [TestClass]
    public class BasicUiTesting
    {
        private CanvasViewModel _canvasViewModel;

        // Mock objects
        private Mock<IElementViewModelFactory> _mockElementViewModelFactory;
        private Mock<IRelationViewModelFactory> _mockRelationViewModelFactory;
        private Mock<IElementManager> _mockElementManager;
        private Mock<IRelationManager> _mockRelationManager;
        private Mock<IMouseHandlingService> _mockMouseHandlingService;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the mocks
            _mockElementViewModelFactory = new Mock<IElementViewModelFactory>();
            _mockRelationViewModelFactory = new Mock<IRelationViewModelFactory>();
            _mockElementManager = new Mock<IElementManager>();
            _mockRelationManager = new Mock<IRelationManager>();
            _mockMouseHandlingService = new Mock<IMouseHandlingService>();

            // Create a collection of ElementViewModels for testing
            var testElements = new ObservableCollection<ElementViewModel>
            {
                CreateTestElementViewModel(100, 100, "Element 1", "Label 1"),
                CreateTestElementViewModel(200, 200, "Element 2", "Label 2")
            };

            // Create a collection of RelationViewModels for testing
            var testRelations = new ObservableCollection<RelationViewModel>
            {
                // Create and add RelationViewModel instances as needed
            };

            // Setup the mock ElementManager to return the test collection
            _mockElementManager.SetupGet(m => m.Elements).Returns(testElements);

            // Setup the mock RelationManager to return the test collection
            _mockRelationManager.SetupGet(m => m.Relations).Returns(testRelations);

            // Setup mock factory behavior for ElementViewModelFactory
            _mockElementViewModelFactory.Setup(f => f.Create())
                .Returns(() => CreateTestElementViewModel(0, 0, "New Element", "New Label"));

            // Setup mock factory behavior for RelationViewModelFactory
            _mockRelationViewModelFactory.Setup(f => f.Create(It.IsAny<Point>(), It.IsAny<Point>()))
                .Returns((Point start, Point end) => new RelationViewModel(new Relation(), start, end));

            // Create CanvasViewModel instance
            _canvasViewModel = new CanvasViewModel(
                _mockElementViewModelFactory.Object,
                _mockElementManager.Object,
                _mockRelationViewModelFactory.Object,
                _mockRelationManager.Object,
                _mockMouseHandlingService.Object);
        }

        [TestMethod]
        public void AddTestingElementsAndRelation_AddsElementsToCanvas()
        {
            // Act
            AddTestingElementsAndRelation();

            // Assert
            Assert.IsTrue(_canvasViewModel.CanvasItems.Count == 4);
        }

        private void AddTestingElementsAndRelation()
        {
            ElementViewModel fromElement = AddTestingElementToCanvas(new Point(200, 200), "First element", "Label 1");
            ElementViewModel toElement = AddTestingElementToCanvas(new Point(400, 100), "Second element", "Label 2");
            AddTestingElementToCanvas(new Point(2, 2), "Third element", "Label 3");
            AddTestingRelationToCanvas(fromElement, toElement);
            _canvasViewModel.IsDraggingElementModeActive = true;
        }

        private ElementViewModel CreateTestElementViewModel(double xPosition, double yPosition, string title, string label)
        {
            var element = new Element(); // Create a new Element instance
            var elementViewModel = new ElementViewModel(element);
            elementViewModel.XPosition = xPosition;
            elementViewModel.YPosition = yPosition;
            elementViewModel.Title = title;
            elementViewModel.Label = label;
            return elementViewModel;
        }

        private ElementViewModel AddTestingElementToCanvas(Point position, string title, string label)
        {
            var newElement = _mockElementViewModelFactory.Object.Create();
            newElement.XPosition = position.X;
            newElement.YPosition = position.Y;
            newElement.Title = title;
            newElement.Label = label;
            _canvasViewModel.Elements.Add(newElement);
            return newElement;
        }

        private void AddTestingRelationToCanvas(ElementViewModel fromElement, ElementViewModel toElement)
        {
            var relationViewModel = _mockRelationViewModelFactory.Object.Create(new Point(fromElement.XPosition, fromElement.YPosition),
                                                                               new Point(toElement.XPosition, toElement.YPosition));
            relationViewModel.ObjectFrom = fromElement;
            relationViewModel.ObjectTo = toElement;
            relationViewModel.IsFinished = true;
            _canvasViewModel.Relations.Add(relationViewModel);
        }
    }
}
