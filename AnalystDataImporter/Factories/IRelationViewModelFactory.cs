using System.Windows;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Factories
{
    /// <summary>
    /// Rozhraní definující metodu pro vytváření ViewModelů prvků. Rozhraní specifikuje, jaké metody a vlastnosti třída, která toto rozhraní implementuje, musí mít.
    /// </summary>
    public interface IRelationViewModelFactory
    {
        /// <summary>
        /// Metoda pro vytváření nové instance ViewModel pro prvek.
        /// </summary>
        /// <returns>Nová instance třídy ElementViewModel.</returns>
        //RelationViewModel Create(ElementViewModel fromElement, ElementViewModel toElement);
        RelationViewModel Create(Point fromPoint, Point toPoint);
    }
}