using System.Windows.Controls;
using AnalystDataImporter.ViewModels;
using AnalystDataImporter.WindowsWPF.SettingPagesWPF;

namespace AnalystDataImporter.WindowsWPF
{
    /// <summary>
    /// Interakční logika pro PageImport2.xaml
    /// </summary>
    public partial class PageImport2 : Page
    {
        private CanvasViewModel _canvasViewModel;

        public PageImport2(CompositeCanvasGridViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            _canvasViewModel = viewModel.CanvasViewModel;
            viewModel.CanvasViewModel.TestingMode = false;
            viewModel.CanvasViewModel.AddTestingElementsAndRelation();
            viewModel.GridViewModel.LoadTestData();
        }

        private void btnEIkonaTyp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageTyp();
        }

        private void btnEIdentita_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageIdentita();
        }

        private void btnPopis_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPagePopis();
        }

        private void btnRNasobnost_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageNasobnost();
        }

        private void btnRBarva_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageBarva();
        }

        private void btnPopisek_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPagePopisek();
        }

        private void btnRSmer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageSmer();
        }

        private void btnRSila_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageSila();
        }

        private void btnRSirka_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageSirka();
        }

        private void btnDatum_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageDatum();
        }

        private void btnCas_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageCas();
        }
    }
}
