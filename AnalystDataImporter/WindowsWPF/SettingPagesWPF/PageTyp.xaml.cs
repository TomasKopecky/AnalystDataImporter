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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnalystDataImporter.Globals;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.WindowsWPF.SettingPagesWPF
{
    /// <summary>
    /// Interakční logika pro PageTyp.xaml
    /// </summary>
    public partial class PageTyp : Page
    {
        public BaseSettingPage basicSettingPage;
        private ElementViewModel _elementViewModel;

        public PageTyp(CanvasViewModel canvasViewModel)
        {
            InitializeComponent();
            basicSettingPage = new BaseSettingPage(canvasViewModel);
            cmbBxTypElement.ItemsSource = Constants.Classes;
            cmbBxTypElement.Text = canvasViewModel.SelectedSingleItem.Class;
            _elementViewModel = (ElementViewModel) canvasViewModel.SelectedSingleItem;
        }

        private void btnStorno_Click(object sender, RoutedEventArgs e)
        {
            basicSettingPage.OnRequestClose();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            basicSettingPage.canvasViewModel.SelectedSingleItem.Class = cmbBxTypElement.Text;
            basicSettingPage.canvasViewModel.SelectedSingleItem.Label = cmbBxTypElement.Text;
            _elementViewModel.IconSourcePath = Constants.Icons[cmbBxTypElement.Text];
            basicSettingPage.OnRequestClose();
        }
    }
}
