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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnalystDataImporter.WindowsWPF.SettingPagesWPF
{
    /// <summary>
    /// Interakční logika pro LoadingPage.xaml
    /// </summary>
    public partial class LoadingPage : Page
    {
        private DispatcherTimer timer;
        private int imageIndex = 0;

        public LoadingPage()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.02); // nastavit interval pro změnu obrázků
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var imageUri = new Uri($"pack://application:,,,/AnalystDataImporter;component/WindowsWPF/SettingPagesWPF/Images/LOADING_GIFs/Load{imageIndex}.png", UriKind.Absolute);
            AnimatedImage.Source = new BitmapImage(imageUri);

            imageIndex = (imageIndex + 1) % 23; // Přepne na další obrázek, vrátí se na začátek po dosažení posledního
        }
    }
}
