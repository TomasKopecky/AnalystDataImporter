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
    /// Interakční logika pro PageSila.xaml
    /// </summary>
    public partial class PageSila : Page
    {
        public BaseSettingPage basicSettingPage;
        private RelationViewModel _relationViewModel;

        public PageSila(CanvasViewModel canvasViewModel)
        {
            InitializeComponent();
            basicSettingPage = new BaseSettingPage(canvasViewModel);
            cmbBxVazbaSila.ItemsSource = Constants.Style.Keys;
            _relationViewModel = (RelationViewModel)canvasViewModel.SelectedSingleItem;
            cmbBxVazbaSila.Text = Constants.Style.FirstOrDefault(styl => styl.Value.ToString() == _relationViewModel.StyleValue.ToString()).Key;

            cmbBxVazbaSila.Focus();
        }

        private void btnStorno_Click(object sender, RoutedEventArgs e)
        {
            basicSettingPage.OnRequestClose();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            _relationViewModel.StyleValue = Constants.Style[cmbBxVazbaSila.Text];
            basicSettingPage.OnRequestClose();
        }
    }
}
