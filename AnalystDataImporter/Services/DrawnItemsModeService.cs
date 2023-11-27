using System;

namespace AnalystDataImporter.Services
{
    public class DrawnItemsModeService : IDrawnItemsModeService
    {
        private bool _isAddingElementModeActive;
        private bool _isAddingRelationModeActive;

        public bool IsAddingElementModeActive
        {
            get => _isAddingElementModeActive;
            set
            {
                if (_isAddingElementModeActive != value)
                {
                    _isAddingElementModeActive = value;
                    _isAddingRelationModeActive = !value;
                    OnElementModeChanged();
                }
            }
        }

        public bool IsAddingRelationModeActive
        {
            get => _isAddingRelationModeActive;
            set
            {
                if (_isAddingRelationModeActive != value)
                {
                    _isAddingRelationModeActive = value;
                    _isAddingElementModeActive = !value;
                    OnElementModeChanged();
                }
            }
        }

        public event EventHandler ElementOrRelationModeChanged;

        protected virtual void OnElementModeChanged()
        {
            ElementOrRelationModeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
