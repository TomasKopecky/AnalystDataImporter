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
    /// Interakční logika pro PageBarva.xaml
    /// </summary>
    public partial class PageBarva : Page
    {
        public BaseSettingPage basicSettingPage;
        private RelationViewModel _relationViewModel;

        public PageBarva(CanvasViewModel canvasViewModel)
        {
            InitializeComponent();
            basicSettingPage = new BaseSettingPage(canvasViewModel);
            cmbBxVazbaBarva.ItemsSource = Constants.Colors.Keys;
            _relationViewModel = (RelationViewModel) canvasViewModel.SelectedSingleItem;
            cmbBxVazbaBarva.Text = Constants.Colors.FirstOrDefault(color => color.Value.ToString() == _relationViewModel.ColorValue.ToString()).Key;

            cmbBxVazbaBarva.Focus();
        }

        private void btnStorno_Click(object sender, RoutedEventArgs e)
        {
            basicSettingPage.OnRequestClose();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            _relationViewModel.ColorValue = Constants.Colors[cmbBxVazbaBarva.Text];
            basicSettingPage.OnRequestClose();
        }
    }
}
