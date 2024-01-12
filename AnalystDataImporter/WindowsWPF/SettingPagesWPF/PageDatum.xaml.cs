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
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.WindowsWPF.SettingPagesWPF
{
    /// <summary>
    /// Interakční logika pro PageDatum.xaml
    /// </summary>
    public partial class PageDatum : Page
    {
        public BaseSettingPage basicSettingPage;
        private BaseDiagramItemViewModel _baseDiagramItemViewModel;

        public PageDatum(CanvasViewModel canvasViewModel)
        {
            InitializeComponent();
            basicSettingPage = new BaseSettingPage(canvasViewModel);
            _baseDiagramItemViewModel = (BaseDiagramItemViewModel)canvasViewModel.SelectedSingleItem;
            if (_baseDiagramItemViewModel.Date != string.Empty)
            {
                rdBtnJedenSloupec_Click(null, null);
            }

            cmbBxSloupecDatum.ItemsSource = null; // TODO: přidat počet sloupců - metodou foreach asi nebo něco vymyslet, stejně jako u Popisek!!
            cmbBxSloupecDatum.Text = canvasViewModel.SelectedSingleItem.Date;
            if (_baseDiagramItemViewModel.Date == string.Empty)
            {
                rdBtnNenastaveno_Click(null, null);
            }
            else
            {
                rdBtnJedenSloupec_Click(null, null);
            }
        }

        private void rdBtnNenastaveno_Click(object sender, RoutedEventArgs e)
        {
            rdBtnNenastaveno.IsChecked = true;
            cmbBxSloupecDatum.IsEnabled = false;
        }

        private void rdBtnJedenSloupec_Click(object sender, RoutedEventArgs e)
        {
            rdBtnJedenSloupec.IsChecked = true;
            cmbBxSloupecDatum.IsEnabled = true;

            cmbBxSloupecDatum.ItemsSource = null; // TODO: naplnit čísly - podle počtu sloupců CSV (abych mohl vybrat číslo sloupce..) - nebo ještě lépe NÁZEV SLOUPCE (???) - dotaz na Toma
            cmbBxSloupecDatum.Text = _baseDiagramItemViewModel.Date; // zobrazím první hodnotu: ""Vložit Sloupec"
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (rdBtnNenastaveno.IsChecked == true)
            {
                _baseDiagramItemViewModel.Date = string.Empty; // pokud zvolím prázdné, uložím do prázdný string
            }
            else
            {
                _baseDiagramItemViewModel.Date = cmbBxSloupecDatum.Text; // pokud nezvolím prázdné, ulož hodnotu Sloupce z ComboBoxu
            }

            basicSettingPage.OnRequestClose();
        }

        private void btnStorno_Click(object sender, RoutedEventArgs e)
        {
            basicSettingPage.OnRequestClose();
        }
    }
}