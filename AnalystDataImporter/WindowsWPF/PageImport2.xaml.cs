using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using AnalystDataImporter.Services;
using AnalystDataImporter.ViewModels;
using AnalystDataImporter.WindowsWPF.SettingPagesWPF;

namespace AnalystDataImporter.WindowsWPF
{
    /// <summary>
    /// Interakční logika pro PageImport2.xaml
    /// </summary>
    public partial class PageImport2 : Page
    {
        private CompositeCanvasGridViewModel _compositeCanvasGridViewModel;
        private SettingWindow _settingWindow;

        public PageImport2(CompositeCanvasGridViewModel viewModel, SettingWindow settingWindow)
        {
            InitializeComponent();

            DataContext = viewModel;
            _compositeCanvasGridViewModel = viewModel;
            _settingWindow = settingWindow;
            viewModel.CanvasViewModel.TestingMode = true;
            viewModel.CanvasViewModel.AddTestingElementsAndRelation();
            viewModel.GridViewModel.LoadTestData();

            #region dtGrdTabulkaVlastnostiObejktuAVazeb - OLD:
            //// naplnění dtGrdTabulkaVlastnostiObejktuAVazeb - NEVYBRÁN (objekt ani vazba)
            //List<RowModel> rowsDefault = new List<RowModel>
            //{
            //    new RowModel { Label = "Chcete-li zobrazit vlastnosti", Value = "" },
            //    new RowModel { Label = "objektu nebo vazby,", Value = "" },
            //    new RowModel { Label = "vyberte je.", Value = "" }
            //};

            //// naplnění dtGrdTabulkaVlastnostiObejktuAVazeb - ELEMENT
            //List<RowModel> rowsElement = new List<RowModel>
            //{
            //    new RowModel { Label = "Typ", Value = "Default_1" },
            //    new RowModel { Label = "Identita", Value = "Default_2" },
            //    new RowModel { Label = "Popisek", Value = "Default_3" },
            //    new RowModel { Label = "Datum", Value = "Default_4" },
            //    new RowModel { Label = "Čas", Value = "Default_5" },
            //    new RowModel { Label = "Popis", Value = "Default_6" }
            //};

            //// naplnění dtGrdTabulkaVlastnostiObejktuAVazeb - RELATION
            //List<RowModel> rowsRelation = new List<RowModel>
            //{
            //    new RowModel { Label = "Násobnost", Value = "Jednoduché" },
            //    new RowModel { Label = "Barva", Value = "černá" },
            //    new RowModel { Label = "Popisek", Value = "Default_3" },
            //    new RowModel { Label = "Směr", Value = "Žádné" },
            //    new RowModel { Label = "Síla", Value = "Potvrzené" },
            //    new RowModel { Label = "Datum", Value = "Default_6" },
            //    new RowModel { Label = "Čas", Value = "Default_7" },
            //    new RowModel { Label = "Šířka", Value = "1" },
            //    new RowModel { Label = "Popis", Value = "Default_9" }
            //};
            //dtGrdTabulkaVlastnostiObejktuAVazeb.ItemsSource = rowsDefault;
            //dtGrdTabulkaVlastnostiObejktuAVazeb.ItemsSource = rowsElement;
            //dtGrdTabulkaVlastnostiObejktuAVazeb.ItemsSource = rowsRelation;
            #endregion

            //btnImportObjektOdstranit.IsEnabled = false;
        }

        private void btnEIkonaTyp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Page pageTyp = new PageTyp();
            _settingWindow.Content = pageTyp;
            // TODO:
            //pageTyp
            _settingWindow.ShowDialog();
        }

        private void btnEIdentita_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _settingWindow.Content = new PageIdentita();
            _settingWindow.ShowDialog();
        }

        private void btnDatum_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _settingWindow.Content = new PageDatumCas();
            _settingWindow.ShowDialog();
        }

        private void btnPopis_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _settingWindow.Content = new PagePopis();
            _settingWindow.ShowDialog();
        }

        private void btnRNasobnost_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _settingWindow.Content = new PageNasobnost();
            _settingWindow.ShowDialog();
        }

        private void btnRBarva_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _settingWindow.Content = new PageBarva();
            _settingWindow.ShowDialog();
        }

        private void btnPopisek_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _settingWindow.Content = new PagePopisek();
            _settingWindow.ShowDialog();
        }

        private void btnRSmer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _settingWindow.Content = new PageSmer();
            _settingWindow.ShowDialog();
        }

        private void btnRSila_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _settingWindow.Content = new PageSila();
            _settingWindow.ShowDialog();
        }

        private void btnRSirka_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _settingWindow.Content = new PageSirka();
            _settingWindow.ShowDialog();
        }
    }
}
