using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AnalystDataImporter.ViewModels;
using AnalystDataImporter.Views;

namespace AnalystDataImporter.WindowsWPF.SettingPagesWPF
{
    /// <summary>
    /// Interakční logika pro SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        private CanvasViewModel _canvasViewModel;
        public SettingWindow(CanvasViewModel viewModel)
        {
            InitializeComponent();
            _canvasViewModel = viewModel;
            //DataContext = viewModel;
        }

        public void ShowPageTyp()
        {
            PageTyp pageTyp = new PageTyp(_canvasViewModel);
            pageTyp.basicSettingPage.RequestClose += CloseWindow;
            Content = pageTyp;
            ShowDialog();
        }

        public void ShowPageBarva()
        {
            PageBarva pageBarva = new PageBarva(_canvasViewModel);
            pageBarva.basicSettingPage.RequestClose += CloseWindow;
            Content = pageBarva;
            ShowDialog();
        }

        private void CloseWindow()
        {
            // TODO: možná bude třeba zde ještě při destroy objektu i odebrat právě event CloseWindow u dané page, tedy něco jako pageTyp.basicSettingPage.RequestClose -= CloseWindow;
            Close();
        }

    }
}
