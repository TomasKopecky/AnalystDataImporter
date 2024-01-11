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
using System.Windows.Shapes;
using AnalystDataImporter.ViewModels;
using AnalystDataImporter.Views;

namespace AnalystDataImporter.WindowsWPF.SettingPagesWPF
{
    /// <summary>
    /// Interakční logika pro SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        private CanvasViewModel _canvasViewModel;
        public SettingWindow(CanvasViewModel viewModel)
        {
            InitializeComponent();
            _canvasViewModel = viewModel;
            //DataContext = viewModel;
        }

        public void ShowPageTyp()
        {
            PageTyp pageTyp = new PageTyp(_canvasViewModel);
            pageTyp.basicSettingPage.RequestClose += CloseWindow;
            Content = pageTyp;
            ShowDialog();
        }

        public void ShowPagePopisek()
        {
            PagePopisek pagePopisek = new PagePopisek(_canvasViewModel);
            pagePopisek.basicSettingPage.RequestClose += CloseWindow;
            Content = pagePopisek;
            ShowDialog();
        }

        public void ShowPagePopis()
        {
            PagePopis pagePopis = new PagePopis(_canvasViewModel);
            pagePopis.basicSettingPage.RequestClose += CloseWindow;
            Content = pagePopis;
            ShowDialog();
        }

        public void ShowPageDatum()
        {
            PageDatum pageDatum = new PageDatum(_canvasViewModel);
            pageDatum.basicSettingPage.RequestClose += CloseWindow;
            Content = pageDatum;
            ShowDialog();
        }
        public void ShowPageCas()
        {
            PageCas pageCas = new PageCas(_canvasViewModel);
            pageCas.basicSettingPage.RequestClose += CloseWindow;
            Content = pageCas;
            ShowDialog();
        }

        public void ShowPageNasobnost()
        {
            PageNasobnost pageNasobnost = new PageNasobnost(_canvasViewModel);
            pageNasobnost.basicSettingPage.RequestClose += CloseWindow;
            Content = pageNasobnost;
            ShowDialog();
        }

        public void ShowPageBarva()
        {
            PageBarva pageBarva = new PageBarva(_canvasViewModel);
            pageBarva.basicSettingPage.RequestClose += CloseWindow;
            Content = pageBarva;
            ShowDialog();
        }

        public void ShowPageSmer()
        {
            PageSmer pageSmer = new PageSmer(_canvasViewModel);
            pageSmer.basicSettingPage.RequestClose += CloseWindow;
            Content = pageSmer;
            ShowDialog();
        }

        public void ShowPageSila()
        {
            PageSila pageSila = new PageSila(_canvasViewModel);
            pageSila.basicSettingPage.RequestClose += CloseWindow;
            Content = pageSila;
            ShowDialog();
        }

        public void ShowPageSirka()
        {
            PageSirka pageSirka = new PageSirka(_canvasViewModel);
            pageSirka.basicSettingPage.RequestClose += CloseWindow;
            Content = pageSirka;
            ShowDialog();
        }

        private void CloseWindow()
        {
            // TODO: možná bude třeba zde ještě při destroy objektu i odebrat právě event CloseWindow u dané page, tedy něco jako pageTyp.basicSettingPage.RequestClose -= CloseWindow;
            Close();
        }

    }
}
