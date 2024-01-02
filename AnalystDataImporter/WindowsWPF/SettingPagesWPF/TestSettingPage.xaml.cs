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

namespace AnalystDataImporter.WindowsWPF.SettingPagesWPF
{
    /// <summary>
    /// Interakční logika pro TestSettingPage.xaml
    /// </summary>
    public partial class TestSettingPage : Page
    {
        public TestSettingPage()
        {
            InitializeComponent();
        }

        //SettingWindow SetWin = new SettingWindow(); // vytvoření instance okna SettingWindow
        PageBarva PgBarva = new PageBarva();
        PageDatumCas PgDatCas = new PageDatumCas();
        PageIdentita PgIdentita = new PageIdentita();
        PageNasobnost PgNasobnost = new PageNasobnost();
        PagePopis PgPopis = new PagePopis();
        PagePopisek PgPopisek = new PagePopisek();
        PageSila PgSila = new PageSila();
        PageSirka PgSirka = new PageSirka();
        PageSmer PgSmer = new PageSmer();
        PageTyp PgTyp = new PageTyp();

        //testc - ELEMENT:
        private void btnETyp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgTyp;
            SetWin.ShowDialog();
        }

        private void btnEIdentita_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgIdentita;
            SetWin.ShowDialog();
        }

        private void btnEDatum_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgDatCas;
            SetWin.ShowDialog();
        }

        private void btnECas_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgDatCas;
            SetWin.ShowDialog();
        }

        private void btnEPopis_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgPopis;
            SetWin.ShowDialog();
        }

        //testc - RELATION:
        private void btnRNasobnost_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgNasobnost;
            SetWin.ShowDialog();
        }

        private void btnRBarva_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgBarva;
            SetWin.ShowDialog();
        }

        private void btnRPopisek_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgPopisek;
            SetWin.ShowDialog();
        }

        private void btnRSmer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgSmer;
            SetWin.ShowDialog();
        }

        private void btnRSila_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgSila;
            SetWin.ShowDialog();
        }

        private void btnRDatum_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgDatCas;
            SetWin.ShowDialog();
        }

        private void btnRCas_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgDatCas;
            SetWin.ShowDialog();
        }

        private void btnRSirka_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgSirka;
            SetWin.ShowDialog();
        }

        private void btnRPopis_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow SetWin = new SettingWindow();
            SetWin.Content = PgPopis;
            SetWin.ShowDialog();
        }
    }
}
