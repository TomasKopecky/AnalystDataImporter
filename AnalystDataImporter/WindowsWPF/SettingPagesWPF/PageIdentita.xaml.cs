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
    /// Interakční logika pro PageIdentita.xaml
    /// </summary>
    public partial class PageIdentita : Page
    {
        public BaseSettingPage basicSettingPage;
        private BaseDiagramItemViewModel _baseDiagramItemViewModel;
        private int _addItem = 0;

        public PageIdentita(CanvasViewModel canvasViewModel)
        {
            InitializeComponent();
            basicSettingPage = new BaseSettingPage(canvasViewModel);
            _baseDiagramItemViewModel = (BaseDiagramItemViewModel)canvasViewModel.SelectedSingleItem;
            if (_baseDiagramItemViewModel.Id != string.Empty)
            {
                fillData();
            }

            //TEST:
            List<string> items = new List<string>() { "1", "2", "3" };
            cmbBxSloupec.ItemsSource = items;
        }

        private void fillData()
        {
            cmbBxSloupec.ItemsSource = null; // TODO: naplnit čísly - podle počtu sloupců CSV (abych mohl vybrat číslo sloupce..) - nebo ještě lépe NÁZEV SLOUPCE (???) - dotaz na Toma
            cmbBxSloupec.Text = "Vložit Sloupec"; // TODO: na prvním místě musí být Vložit SLoupec!! - vytvořit List s "Vložit sloupec" a pak přidat FOREACHeme čísla podle počtu sloupců... 1 až n...
            cmbBxSloupec.SelectedIndex = 0; // zobrazím první hodnotu: ""Vložit Sloupec"
            txtBxIdentita.Text = _baseDiagramItemViewModel.Title; // TODO: - (do Label se mají ukládat čísla sloupců, ale jak to naBindovat, to nevim..) viz Anaylst: string:"[1] [2] [3]"
        }

        private void cmbBxSloupec_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (_addItem == 0)
            {
                _addItem++; // pomocná proměnná, aby se mi nepřidal Content při načtení okna hned na první SelectionChanged
            }
            else
            {
                txtBxIdentita.Text = txtBxIdentita.Text + " [" + (cmbBxSloupec.SelectedValue) + "]";
            }
        }

        private void btnStorno_Click(object sender, RoutedEventArgs e)
        {
            basicSettingPage.OnRequestClose();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            _baseDiagramItemViewModel.Title = txtBxIdentita.Text;
            basicSettingPage.OnRequestClose();
        }

       
    }
}
