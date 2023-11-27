using System;

namespace AnalystDataImporter.Services
{
    public interface IDrawnItemsModeService
    {
        bool IsAddingElementModeActive { get; set; }
        bool IsAddingRelationModeActive { get; set; }
        event EventHandler ElementOrRelationModeChanged;
    }
}
