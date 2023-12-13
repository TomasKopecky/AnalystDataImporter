using AnalystDataImporter.Views;


namespace AnalystDataImporter
{
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        

            // Získání instance stránky ElementPage pomocí závislostní injekce.
            //var elementPage = App.GetService<ElementPage>();
            // Možnost nastavit obsah hlavního okna na stránku ElementPage.
            //this.Content = elementPage;

            // Získání instance stránky CanvasPage pomocí závislostní injekce.
            var canvasPage = App.GetService<CanvasPage>();
            // Nastavení obsahu hlavního okna na stránku CanvasPage.
            Content = canvasPage;

            //Consider adding a navigation mechanism if you plan to switch between different pages(e.g., ElementPage and CanvasPage)
            //in the main window.The Frame control is useful for this purpose.Instead of setting the content of the window directly,
            //you can navigate to different pages using the MainFrame.Navigate() method.
        }
    }

}
}
