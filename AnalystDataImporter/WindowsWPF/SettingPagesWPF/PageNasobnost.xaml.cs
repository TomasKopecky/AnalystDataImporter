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
    /// Interakční logika pro PageNasobnost.xaml
    /// </summary>
    public partial class PageNasobnost : Page
    {
        public BaseSettingPage basicSettingPage;
        private RelationViewModel _relationViewModel;

        public PageNasobnost(CanvasViewModel canvasViewModel)
        {
            InitializeComponent();
            basicSettingPage = new BaseSettingPage(canvasViewModel);
            _relationViewModel = (RelationViewModel)canvasViewModel.SelectedSingleItem;

            if (_relationViewModel.Multiplicity == "Jednoduchá")
            {
                rdBtnVazbaJednoducha.IsChecked = true;
            }
            else if (_relationViewModel.Multiplicity == "Násobná")
            {
                rdBtnVazbaNasobna.IsChecked = true;
            }
        }

        private void btnStorno_Click(object sender, RoutedEventArgs e)
        {
            basicSettingPage.OnRequestClose();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (rdBtnVazbaJednoducha.IsChecked == true)
            {
                _relationViewModel.Multiplicity = "Jednoduchá";
            }
            else if (rdBtnVazbaNasobna.IsChecked == true)
            {
                _relationViewModel.Multiplicity = "Násobná";
            }
            basicSettingPage.OnRequestClose();
        }
    }
}
