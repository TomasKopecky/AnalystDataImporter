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
    /// Interakční logika pro PageSirka.xaml
    /// </summary>
    public partial class PageSirka : Page
    {
        public BaseSettingPage basicSettingPage;
        private RelationViewModel _relationViewModel;

        public PageSirka(CanvasViewModel canvasViewModel)
        {
            InitializeComponent();
            basicSettingPage = new BaseSettingPage(canvasViewModel);
            cmbBxVazbaSirka.ItemsSource = Constants.Thickness;
            _relationViewModel = (RelationViewModel)canvasViewModel.SelectedSingleItem;
            cmbBxVazbaSirka.Text = _relationViewModel.Thickness.ToString();
            //cmbBxVazbaSirka.Text = Constants.Thickness.FirstOrDefault(thickness => thickness.Value.ToString() == _relationViewModel.ColorValue.ToString()).Key;

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            _relationViewModel.Thickness = Double.Parse(cmbBxVazbaSirka.Text);
            //_relationViewModel.Thickness = Constants.Thickness[cmbBxVazbaSirka.Text];
            basicSettingPage.OnRequestClose();
        }

        private void btnStorno_Click(object sender, RoutedEventArgs e)
        {
            basicSettingPage.OnRequestClose();
        }
    }
}
