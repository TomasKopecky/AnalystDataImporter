using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
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
        private CanvasViewModel _canvasViewModel;

        public PageImport2(CompositeCanvasGridViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            _canvasViewModel = viewModel.CanvasViewModel;
            viewModel.CanvasViewModel.TestingMode = false;
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
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageTyp();
        }

        private void btnEIdentita_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageIdentita();
        }

        private void btnPopis_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPagePopis();
        }

        private void btnRNasobnost_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageNasobnost();
        }

        private void btnRBarva_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageBarva();
        }

        private void btnPopisek_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPagePopisek();
        }

        private void btnRSmer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageSmer();
        }

        private void btnRSila_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageSila();
        }

        private void btnRSirka_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageSirka();
        }

        private void btnDatum_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageDatum();
        }

        private void btnCas_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(_canvasViewModel);
            settingWindow.ShowPageCas();
        }
    }
}
