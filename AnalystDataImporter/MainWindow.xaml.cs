using System.Windows.Controls;
using AnalystDataImporter.Views;
using AnalystDataImporter.WindowsWPF;
using AnalystDataImporter.WindowsWPF.SettingPagesWPF;
using System.Windows;
using System.Windows.Input;

namespace AnalystDataImporter
{
    public partial class MainWindow
    {
        private MainPage mainPage;

        public MainWindow(MainPage mainPage)
        {
            InitializeComponent();

            //// VÝŠKA A ŠÍŘKA OKNA DLE MONITORU
            // Získání výšky monitoru
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            // Výpočet požadované výšky pro okno (2/3 výšky monitoru)
            double desiredHeight = screenHeight * 2 / 3;
            // Nastavení výšky hlavního okna
            this.Height = desiredHeight;
            // Získání šířky monitoru
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            // Výpočet požadované šířky pro okno (3/4 šířky monitoru)
            double desiredWidth = screenWidth * 3 / 4;
            // Nastavení šířky hlavního okna
            this.Width = desiredWidth;

            #region TOM
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
            #endregion

            #region PAVEL
            ////// PAVEL: TESTOVACÍ Tlačítka nastavení:
            //MainFrame.Content = new TestSettingPage();

            ////// vlož okno se záložkami:
            //Page mainPage = new MainPage();
            this.mainPage = mainPage;
            MainFrame.Navigate(mainPage);
            #endregion


        }
    }
}
