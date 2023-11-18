using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.Services
{
    public interface IDrawnItemsModeService
    {
        bool IsAddingElementModeActive { get; set; }
        bool IsAddingRelationModeActive { get; set; }
        event EventHandler ElementOrRelationModeChanged;
    }
}
