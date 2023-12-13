using System;
using System.ComponentModel;
using System.Windows.Controls;
using AnalystDataImporter.Services;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Views
{
    /// <summary>
    /// Tato třída reprezentuje code-behind pro stránku WPF, na které je zobrazeno plátno, kde lze přidávat a pomocí myši přesouvat elipsy.
    /// </summary>
    public partial class CanvasPage : Page
    {
        /// <summary>
        /// Konstruktor třídy CanvasPage.
        /// </summary>
        /// <param name="viewModel">ViewModel asociovaný s tímto pohledem.</param>
        public CanvasPage(CompositeCanvasGridViewModel viewModel) //CanvasViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.CanvasViewModel.TestingMode = false;
            viewModel.CanvasViewModel.AddTestingElementsAndRelation();
            viewModel.GridViewModel.LoadTestData();
            //viewModel.TestingMode = false;
            //viewModel.AddTestingElementsAndRelation();
        }

        private void HeadingDataGrid_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}