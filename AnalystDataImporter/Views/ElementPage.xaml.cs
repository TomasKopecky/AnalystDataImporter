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
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Views
{
    /// <summary>
    /// Tato třída reprezentuje code-behind pro stránku WPF, která je navržena pro zacházení s jednotlivými prvky.
    /// </summary>
    public partial class ElementPage : Page
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
