﻿using System;
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
using AnalystDataImporter.Views;

namespace AnalystDataImporter.WindowsWPF.SettingPagesWPF
{
    /// <summary>
    /// Interakční logika pro SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
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
    }
}
