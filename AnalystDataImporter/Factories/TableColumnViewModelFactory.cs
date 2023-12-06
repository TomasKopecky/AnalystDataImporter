using AnalystDataImporter.Models;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Factories
{
    /// <summary>
    /// Factory třída pro vytváření ViewModelů prvků. Factory vzor je návrhový vzor, který umožňuje vytváření objektů bez specifikace konkrétní třídy objektu, který má být vytvořen.
    /// </summary>
    public class TableColumnViewModelFactory : ITableColumnViewModelFactory
    {
        /// <summary>
        /// Vytvoří a vrátí novou instanci ElementViewModel s novým prvkem (Element).
        /// </summary>
        /// <returns>Nová instance třídy ElementViewModel s novým prvkem.</returns>
        public TableColumnViewModel Create()
        {
            return new TableColumnViewModel(new TableColumn());
        }
    }
}