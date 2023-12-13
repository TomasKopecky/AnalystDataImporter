using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Views
{
    /// <summary>
    /// Tato třída reprezentuje code-behind pro stránku WPF, která je navržena pro zacházení s jednotlivými prvky.
    /// </summary>
    public partial class ElementPage
    {
        /// <summary>
        /// ViewModel této stránky typu ElementViewModel.
        /// </summary>
        public ElementViewModel ViewModel => (ElementViewModel)DataContext;

        /// <summary>
        /// Konstruktor třídy ElementPage.
        /// </summary>
        /// <param name="viewModel">ViewModel asociovaný s tímto pohledem.</param>
        public ElementPage(ElementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // Nastavení ViewModelu jako DataContextu pro tuto stránku
        }
    }
}
