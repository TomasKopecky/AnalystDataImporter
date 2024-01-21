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
using AnalystDataImporter.Models;
using AnalystDataImporter.Services;

namespace AnalystDataImporter.WindowsWPF
{
    /// <summary>
    /// Interakční logika pro ImportWindow.xaml
    /// </summary>
    public partial class ImportWindow : Window
    {
        private readonly SqliteDbService _sqliteDbService;
        private readonly IMessageBoxService _messageBoxService;
        public ImportWindow(SqliteDbService sqliteDbService, IMessageBoxService messageBoxService)
        {
            InitializeComponent();
            _sqliteDbService = sqliteDbService;
            _messageBoxService = messageBoxService;
            this.KeyDown += new KeyEventHandler(SaveWindow_KeyDown);
            Loaded += LoadActions;

            rdBtnNovaAkce.IsChecked = true;
        }

        // metoda pro odchytávání stisknutých kláves na klávesnici
        private void SaveWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) // pokud stisknu klávesu Esc(ape)
            {
                this.Close(); // Zavře okno
            }
        }

        private async void LoadActions(object sender, RoutedEventArgs e)
        {
            List<IndexAction> actions = await _sqliteDbService.GetAllActionsAsync();
            cmbBxExistujiciAkce.ItemsSource = actions;
            cmbBxExistujiciAkce.DisplayMemberPath = "Name";
            cmbBxExistujiciAkce.SelectedIndex = 0;
        }

        // pokud zvolím NOVÁ AKCE:
        private void rdBtnNovaAkce_Checked(object sender, RoutedEventArgs e)
        {
            cmbBxExistujiciAkce.IsEnabled = false;
            txtBxNazevAkce.IsEnabled = true;
            txtBxPopis.IsEnabled = true;
            txtBxNazevAkce.Focus();
        }
        // pokud zvolím EXISTUJÍCÍ AKCE:
        private void rdBtnExistujiciAkce_Checked(object sender, RoutedEventArgs e)
        {
            txtBxNazevAkce.IsEnabled = false;
            cmbBxExistujiciAkce.IsEnabled = true;
            txtBxPopis.IsEnabled = false;
        }

        private void btnStorno_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Zavře okno
        }

        // po kliknutí na tlačítko "Spustit Import"...
        private async void btnImport_Click(object sender, RoutedEventArgs e)
        {
            // TODO: co se stane po kliknutí na tlačítko "Spustit Import"...

            IndexAction indexActionToImport;
            if (rdBtnNovaAkce.IsChecked == true)
            {
                if (txtBxNazevAkce.Text == null || txtBxNazevAkce.Text == "")
                {
                    _messageBoxService.ShowError("Nelze založit akci s prázdným jménem");
                    return;
                }
                IndexAction newAction = new IndexAction();
                newAction.Name = txtBxNazevAkce.Text;
                newAction.Description = txtBxPopis.Text;
                newAction.Created = DateTime.Now.Date;
                await CreateNewIndexAction(newAction);
                indexActionToImport = newAction;
            }
            else
                indexActionToImport = cmbBxExistujiciAkce.SelectedItem as IndexAction;


            await ImportDataToAction(indexActionToImport);

            this.Close();
        }

        private async Task CreateNewIndexAction(IndexAction action)
        {
            await _sqliteDbService.CreateActionAsync(action);
        }

        private async Task ImportDataToAction(IndexAction action)
        {

        }

    }
}
