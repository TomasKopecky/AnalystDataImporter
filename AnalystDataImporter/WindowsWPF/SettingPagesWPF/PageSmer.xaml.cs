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
    /// Interakční logika pro PageSmer.xaml
    /// </summary>
    public partial class PageSmer : Page
    {
        public BaseSettingPage basicSettingPage;
        private RelationViewModel _relationViewModel;

        public PageSmer(CanvasViewModel canvasViewModel)
        {
            InitializeComponent();
            basicSettingPage = new BaseSettingPage(canvasViewModel);
            cmbBxVazbaSmer.ItemsSource = Constants.RelationDirections.Keys;
            _relationViewModel = (RelationViewModel)canvasViewModel.SelectedSingleItem;
            cmbBxVazbaSmer.Text = Constants.RelationDirections.FirstOrDefault(direction => direction.Value.ToString() == _relationViewModel.DirectionValue.ToString()).Key;
        }

        private void btnStorno_Click(object sender, RoutedEventArgs e)
        {
            basicSettingPage.OnRequestClose();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            _relationViewModel.DirectionValue = Constants.RelationDirections[cmbBxVazbaSmer.Text];
            basicSettingPage.OnRequestClose();
        }
    } 
}
