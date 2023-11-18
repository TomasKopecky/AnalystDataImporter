using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Managers
{
    /// <summary>
    /// Správce prvků, který obsahuje logiku pro práci s prvky. Tato třída implementuje rozhraní IElementManager.
    /// </summary>
    public class ElementManager : IElementManager
    {
        /// <summary>
        /// Kolekce všech prvků v aplikaci.
        /// </summary>
        private readonly ObservableCollection<ElementViewModel> _elements;
        public IList<ElementViewModel> Elements => _elements;

        /// <summary>
        /// Konstruktor třídy ElementManager, který inicializuje objekty a nastavuje výchozí hodnoty.
        /// </summary>
        public ElementManager()
            {
                _elements = new ObservableCollection<ElementViewModel>();
            }

        /// <summary>
        /// Metoda pro přidání prvku do kolekce prvků.
        /// </summary>
        /// <param name="elementViewModel">Prvek, který chceme přidat.</param>
        public void AddElement(ElementViewModel elementViewModel)
            {
                _elements.Add(elementViewModel);
            }
    }
}
