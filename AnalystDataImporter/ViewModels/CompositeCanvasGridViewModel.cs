using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.ViewModels
{
    public class CompositeCanvasGridViewModel// : INotifyPropertyChanged
    {
        public CanvasViewModel CanvasViewModel { get; private set; }
        public GridViewModel GridViewModel { get; private set; }

        // Událost, která se vyvolá při změně vlastnosti - pro binding
        //public event PropertyChangedEventHandler PropertyChanged;

        public CompositeCanvasGridViewModel(CanvasViewModel canvasViewModel, GridViewModel gridViewModel)
        {
            CanvasViewModel = canvasViewModel;
            GridViewModel = gridViewModel;
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
