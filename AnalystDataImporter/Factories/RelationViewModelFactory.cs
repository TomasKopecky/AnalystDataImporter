using System.Windows;
using AnalystDataImporter.Models;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Factories
{
    /// <summary>
    /// Factory třída pro vytváření ViewModelů prvků. Factory vzor je návrhový vzor, který umožňuje vytváření objektů bez specifikace konkrétní třídy objektu, který má být vytvořen.
    /// </summary>
    public class RelationViewModelFactory : IRelationViewModelFactory
    {
        /// <summary>
        /// Vytvoří a vrátí novou instanci ElementViewModel s novým prvkem (Element).
        /// </summary>
        /// <returns>Nová instance třídy ElementViewModel s novým prvkem.</returns>
        //public RelationViewModel Create(ElementViewModel fromElement, ElementViewModel toElement)
        //{
        //    return new RelationViewModel(new Relation(), fromElement, toElement);
        //}

        public RelationViewModel Create(Point fromPoint, Point toPoint)
        {
            return new RelationViewModel(new Relation(), fromPoint, toPoint);
        }
    }
}