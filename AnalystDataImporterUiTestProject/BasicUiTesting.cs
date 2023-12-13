using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Drawing;
using System.Windows;
using AnalystDataImporter.Factories;
using AnalystDataImporter.Managers;
using AnalystDataImporter.Models;
using AnalystDataImporter.Services;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporterTestProject
{
    //[TestClass]
    //public class BasicUiTesting
    //{
    //    private CanvasViewModel _canvasViewModel;

    //    // Mock objects
    //    private Mock<IElementViewModelFactory> _mockElementViewModelFactory;
    //    private Mock<IRelationViewModelFactory> _mockRelationViewModelFactory;

    //    // Mock objects
    //    private Mock<IElementManager> _mockElementManager;
    //    private Mock<IRelationManager> _mockRelationManager;
    //    private Mock<IMouseHandlingService> _mockMouseHandlingService;

    //    // ... Other mock objects

    //    [TestInitialize]
    //    public void Setup()
    //    {
    //        // Initialize the mocks
    //        _mockRelationViewModelFactory = new Mock<IRelationViewModelFactory>();

    //        // Initialize the mocks
    //        _mockElementViewModelFactory = new Mock<IElementViewModelFactory>();

    //        // Mock the Element object if needed, or create a simple instance
    //        Element mockElement = new Element(); // Adjust as necessary

    //        // Setup mock factory behavior
    //        _mockElementViewModelFactory.Setup(f => f.Create()).Returns(() => new ElementViewModel(mockElement));

    //        // Setup mock factory behavior
    //        Relation mockRelation = new Relation(); // Adjust as necessary

    //        // Setup mock factory behavior
    //        _mockRelationViewModelFactory.Setup(f => f.Create(It.IsAny<Point>(), It.IsAny<Point>()))
    //            .Returns((Point start, Point end) => new RelationViewModel(mockRelation, start, end));


    //        _mockElementManager = new Mock<IElementManager>();
    //        _mockRelationManager = new Mock<IRelationManager>();
    //        _mockMouseHandlingService = new Mock<IMouseHandlingService>();

    //        // Setup the behavior of the mocks, if necessary
    //        // Example: _mockElementManager.Setup(...).Returns(...);

    //        // Create an instance of CanvasViewModel with mock objects
    //        _canvasViewModel = new CanvasViewModel(
    //            _mockElementViewModelFactory.Object,
    //            _mockElementManager.Object,
    //            _mockRelationViewModelFactory.Object,
    //            _mockRelationManager.Object,
    //            _mockMouseHandlingService.Object);
    //        // ... Initialize other mocks and CanvasViewModel
    //    }


    //    private ElementViewModel AddTestingElementToCanvas(Point position, string title, string label)
    //    {
    //        var newElement = _mockElementViewModelFactory.Object.Create();
    //        newElement.XPosition = position.X;
    //        newElement.YPosition = position.Y;
    //        newElement.Title = title;
    //        newElement.ZIndex = 1;
    //        newElement.Label = label;
    //        _canvasViewModel.Elements.Add(newElement);
    //        return newElement;
    //    }

    //    private void AddTestingRelationToCanvas(ElementViewModel fromElement, ElementViewModel toElement)
    //    {
    //        var relationViewModel = _mockRelationViewModelFactory.Object.Create(new Point((int)fromElement.XPosition, (int)fromElement.YPosition),
    //                                                                               new Point((int)toElement.XPosition, (int)toElement.YPosition));
    //        relationViewModel.ZIndex = 0;
    //        relationViewModel.Title = "vazba";
    //        relationViewModel.ObjectFrom = fromElement;
    //        relationViewModel.ObjectTo = toElement;
    //        relationViewModel.IsFinished = true;
    //        _canvasViewModel.Relations.Add(relationViewModel);
    //    }


    //}
}