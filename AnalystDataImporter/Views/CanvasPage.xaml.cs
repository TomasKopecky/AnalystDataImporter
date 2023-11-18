﻿using System.Windows.Controls;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Views
{
    /// <summary>
    /// Tato třída reprezentuje code-behind pro stránku WPF, na které je zobrazeno plátno, kde lze přidávat a pomocí myši přesouvat elipsy.
    /// </summary>
    public partial class CanvasPage : Page
    {
        /// <summary>
        /// Konstruktor třídy CanvasPage.
        /// </summary>
        /// <param name="viewModel">ViewModel asociovaný s tímto pohledem.</param>
        public CanvasPage(CanvasViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}