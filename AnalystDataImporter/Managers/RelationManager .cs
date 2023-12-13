using System.Collections.Generic;
using System.Collections.ObjectModel;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Managers
{
    /// <summary>
    /// Správce prvků, který obsahuje logiku pro práci s prvky. Tato třída implementuje rozhraní IElementManager.
    /// </summary>
    public class RelationManager : IRelationManager
    {
        /// <summary>
        /// Kolekce všech prvků v aplikaci.
        /// </summary>
        private readonly ObservableCollection<RelationViewModel> _relations;
        public IList<RelationViewModel> Relations => _relations;

        /// <summary>
        /// Konstruktor třídy ElementManager, který inicializuje objekty a nastavuje výchozí hodnoty.
        /// </summary>
        public RelationManager()
            {
                _relations = new ObservableCollection<RelationViewModel>();
            }

        /// <summary>
        /// Metoda pro přidání prvku do kolekce prvků.
        /// </summary>
        /// <param name="relationViewModel">Prvek, který chceme přidat.</param>
        public void AddRelation(RelationViewModel relationViewModel)
            {
                _relations.Add(relationViewModel);
            }

        public IList<RelationViewModel> GetRelationsConnectedToElement(ElementViewModel elementViewModel)
        {
            List<RelationViewModel> connectedElements = new List<RelationViewModel>();
            foreach (RelationViewModel relation in _relations)
            {
                if (relation.ObjectFrom == elementViewModel || relation.ObjectTo == elementViewModel)
                {
                    connectedElements.Add(relation);
                }
            }

            return connectedElements;
        }
    }
}
