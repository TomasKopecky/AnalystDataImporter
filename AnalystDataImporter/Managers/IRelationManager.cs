using System.Collections.Generic;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Managers
{
    /// <summary>
    /// Rozhraní definující metody pro správu prvků v aplikaci. Rozhraní specifikuje, jaké metody a vlastnosti třída, která toto rozhraní implementuje, musí mít.
    /// </summary>
    public interface IRelationManager
    {
        /// <summary>
        /// Kolekce prvků ElementViewModel.
        /// </summary>
        IList<RelationViewModel> Relations { get; }

        /// <summary>
        /// Metoda pro přidání prvku do kolekce prvků.
        /// </summary>
        /// <param name="relationViewModel"></param>
        void AddRelation(RelationViewModel relationViewModel);

        IList<RelationViewModel> GetRelationsConnectedToElement(ElementViewModel elementViewModel);
    }
}
