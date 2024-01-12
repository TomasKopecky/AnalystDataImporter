using System;
using System.Windows.Controls;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.WindowsWPF.SettingPagesWPF
{
    public class BaseSettingPage : Page
    {
        public event Action RequestClose;
        public CanvasViewModel canvasViewModel;

        public BaseSettingPage(CanvasViewModel canvasViewModel)
        {
            this.canvasViewModel = canvasViewModel;
        }

        public void OnRequestClose()
        {
            RequestClose?.Invoke();
        }

    }
}
