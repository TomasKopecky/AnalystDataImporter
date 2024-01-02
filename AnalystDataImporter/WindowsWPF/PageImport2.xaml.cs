using System;
using System.ComponentModel;
using System.Windows.Controls;
using AnalystDataImporter.Services;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.WindowsWPF
{
    /// <summary>
    /// Interakční logika pro PageImport2.xaml
    /// </summary>
    public partial class PageImport2 : Page
    {
        public PageImport2(CompositeCanvasGridViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            viewModel.CanvasViewModel.TestingMode = true;
            viewModel.CanvasViewModel.AddTestingElementsAndRelation();
            viewModel.GridViewModel.LoadTestData();

            //btnImportObjektOdstranit.IsEnabled = false;
        }


    }
}
