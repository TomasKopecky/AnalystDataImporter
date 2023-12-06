using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalystDataImporter.Services;

namespace AnalystDataImporter.ViewModels
{
    public class CompositeCanvasGridViewModel : INotifyPropertyChanged
    {

        private readonly MouseCursorService _mouseCursorService;
        public CanvasViewModel CanvasViewModel { get; private set; }
        public GridViewModel GridViewModel { get; private set; }

        public string CurrentCursor => _mouseCursorService.CurrentCursor;

        public event PropertyChangedEventHandler PropertyChanged;
        // Událost, která se vyvolá při změně vlastnosti - pro binding
        //public event PropertyChangedEventHandler PropertyChanged;

        public CompositeCanvasGridViewModel(CanvasViewModel canvasViewModel, GridViewModel gridViewModel, MouseCursorService mouseCursorService)
        {
            CanvasViewModel = canvasViewModel;
            GridViewModel = gridViewModel;
            _mouseCursorService = mouseCursorService ?? throw new ArgumentNullException(nameof(mouseCursorService));
            _mouseCursorService.PropertyChanged += MouseCursorService_PropertyChanged;
        }

        private void MouseCursorService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MouseCursorService.CurrentCursor))
            {
                OnPropertyChanged(nameof(CurrentCursor));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        // /// Metoda pro oznamování změn vlastností - pro binding
        // /// </summary>
        // /// <param name="propertyName">Jméno vlastnosti, která se změnila.</param>
        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
