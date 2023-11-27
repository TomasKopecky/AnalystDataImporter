using System.Collections.Generic;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Managers
{
    /// <summary>
    /// Rozhraní definující metody pro správu prvků v aplikaci. Rozhraní specifikuje, jaké metody a vlastnosti třída, která toto rozhraní implementuje, musí mít.
    /// </summary>
    public interface IElementManager
    {
        /// <summary>
        /// Kolekce prvků ElementViewModel.
        /// </summary>
        IList<ElementViewModel> Elements { get; }

        /// <summary>
        /// Metoda pro přidání prvku do kolekce prvků.
        /// </summary>
        /// <param name="elementViewModel">Prvek, který chceme přidat.</param>
        void AddElement(ElementViewModel elementViewModel);
    }
}
