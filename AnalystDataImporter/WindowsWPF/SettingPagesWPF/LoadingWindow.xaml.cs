using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnalystDataImporter.WindowsWPF.SettingPagesWPF
{
    /// <summary>
    /// Interakční logika pro LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        private DispatcherTimer timer;
        private int imageIndex = 0;

        public LoadingWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1); // nastavit interval pro změnu obrázků
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var imageUri = new Uri($"pack://application:,,,/AnalystDataImporter;component/WindowsWPF/SettingPagesWPF/Images/LOADING_GIFs/Load{imageIndex}.png", UriKind.Absolute);
            AnimatedImage.Source = new BitmapImage(imageUri);

            imageIndex = (imageIndex + 1) % 5; // Přepne na další obrázek, vrátí se na začátek po dosažení posledního
        }

    }
}
