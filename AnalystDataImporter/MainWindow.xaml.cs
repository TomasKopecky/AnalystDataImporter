using AnalystDataImporter.Views;
using AnalystDataImporter.WindowsWPF.SettingPagesWPF;

namespace AnalystDataImporter
{
    public partial class MainWindow
    {

        public MainWindow()
        {
            InitializeComponent();


            //////TOM:
            //// Získání instance stránky ElementPage pomocí závislostní injekce.
            ////var elementPage = App.GetService<ElementPage>();
            //// Možnost nastavit obsah hlavního okna na stránku ElementPage.
            ////this.Content = elementPage;

            //// Získání instance stránky CanvasPage pomocí závislostní injekce.
            //var canvasPage = App.GetService<CanvasPage>();
            //// Nastavení obsahu hlavního okna na stránku CanvasPage.
            //Content = canvasPage;

            ////Consider adding a navigation mechanism if you plan to switch between different pages(e.g., ElementPage and CanvasPage)
            ////in the main window.The Frame control is useful for this purpose.Instead of setting the content of the window directly,
            ////you can navigate to different pages using the MainFrame.Navigate() method.


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
