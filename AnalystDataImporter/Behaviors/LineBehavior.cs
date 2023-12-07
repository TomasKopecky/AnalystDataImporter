using Microsoft.Xaml.Behaviors;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using AnalystDataImporter.Services;
using AnalystDataImporter.ViewModels;

namespace AnalystDataImporter.Behaviors
{
    /// <summary>
    /// Chování pro správu událostí myši na objektu typu Line.
    /// </summary>
    internal class LineBehavior : Behavior<UIElement>
    {
        // Privátní proměnná pro service IMouseHandlingService
        private IMouseHandlingService _mouseHandlingService;
        private SharedStatesService _sharedStateService;

        //// Vlastnosti DependencyProperty
        //public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
        //    nameof(IsEnabled),
        //    typeof(bool),
        //    typeof(LineBehavior),
        //    new PropertyMetadata(null)); // new PropertyMetadata(true, OnIsEnabledChanged));

        public static readonly DependencyProperty RelationDeleteWhenOutsideCanvasCommandProperty =
            DependencyProperty.Register(
                nameof(RelationDeleteWhenOutsideCanvasCommand), typeof(ICommand),
                typeof(LineBehavior),
                new PropertyMetadata(null));

        /// <summary>
        /// Zda je chování aktivní.
        /// </summary>
        //public bool IsEnabled
        //{
        //    get => (bool)GetValue(IsEnabledProperty);
        //    set => SetValue(IsEnabledProperty, value);
        //}

        /// <summary>
        /// Příkaz pro odstranění vazby, pokud je kreslena mimo plátno.
        /// </summary>
        public ICommand RelationDeleteWhenOutsideCanvasCommand
        {
            get => (ICommand)GetValue(RelationDeleteWhenOutsideCanvasCommandProperty);
            set => SetValue(RelationDeleteWhenOutsideCanvasCommandProperty, value);
        }

        /// <summary>
        /// Plátno, ke kterému je chování připojeno - sdíleno z SharedBehaviorProperties třídy
        /// </summary>
        public Canvas ParentCanvas
        {
            get => SharedBehaviorProperties.GetParentCanvas(this);
            set => SharedBehaviorProperties.SetParentCanvas(this, value);
        }

        /// <summary>
        /// Metoda volaná při připojení chování k objektu - tedy při jeho vytvoření
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            // Zde připojujeme service IMouseHandlingService
            _mouseHandlingService = ServiceLocator.Current.GetService<IMouseHandlingService>();
            _sharedStateService = ServiceLocator.Current.GetService<SharedStatesService>();
            AssociateEventHandlers();
            StartDrawingNewRelation();
        }

        ///// <summary>
        ///// Připojení mouse services při vytvoření objektu
        ///// </summary>
        private void AssociateEventHandlers()
        {
            AssociatedObject.MouseLeftButtonDown += MouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += MouseLeftButtonPreviewUp;
            AssociatedObject.MouseMove += MouseMove;
        }

        ///// <summary>
        ///// Metoda pro zahájení kreslení line
        ///// </summary>
        private void StartDrawingNewRelation()
        {
            FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
            RelationViewModel relationViewModel = (RelationViewModel)associatedElement.DataContext;

            // Zde začíná kreslení vazby, tedy ihned po jejím vytvoření
            _mouseHandlingService.StartOperation(associatedElement, null, relationViewModel, "drawing");
        }

        /// <summary>
        /// Zpracovává událost kliknutí levým tlačítkem myši na objekt. Pokud je chování povoleno, zahajuje operaci přetahování.
        /// </summary>
        /// <param name="sender">Objekt, který vyvolal událost (obvykle UI element).</param>
        /// <param name="e">Data události obsahující informace o kliknutí myši.</param>
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Relation MouseLeftButtonDown");
            if (!_sharedStateService.IsDraggingElementModeActive) return;

            FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
            BaseDiagramItemViewModel relationViewModel = (BaseDiagramItemViewModel)associatedElement.DataContext;
            _mouseHandlingService.StartOperation(associatedElement, null, relationViewModel, "dragging");
            e.Handled = true;
        }

        /// <summary>
        /// Zpracovává událost uvolnění levého tlačítka myši na objektu. Pokud je myš umístěna mimo plátno, provádí se akce smazání vazby.
        /// </summary>
        /// <param name="sender">Objekt, který vyvolal událost (obvykle UI element).</param>
        /// <param name="e">Data události obsahující informace o pozici myši a akci myši.</param>
        private void MouseLeftButtonPreviewUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Relation MouseLeftButtonPreviewUp");
            FrameworkElement associatedElement = (FrameworkElement)this.AssociatedObject;
            RelationViewModel relationViewModel = (RelationViewModel)associatedElement.DataContext;
            Point mousePosition = e.GetPosition(ParentCanvas);

            // Kontroluje, zda je aktuální element stejný jako element pod myší, a jestli vazba není již dokončena
            if (_mouseHandlingService.CurrentViewModelElement != relationViewModel || relationViewModel.IsFinished)
                return;

            // Pokud je myš mimo plátno, spustí se příkaz pro odstranění vazby
            if (!_mouseHandlingService.IsMouseInCanvas(mousePosition, ParentCanvas))
            {
                //Debug.WriteLine("Relation MouseLeftButtonPreviewUp inside canvas");
                RelationDeleteWhenOutsideCanvasCommand.Execute(null);
            }

            // Ukončí operaci s myší
            _mouseHandlingService.EndDragOperation();
        }

        /// <summary>
        /// Reaguje na pohyb myši. Aktualizuje pozici prvku během přetahování nebo kreslení.
        /// </summary>
        /// <param name="sender">Objekt, který událost vyvolal (typicky UI element).</param>
        /// <param name="e">Data události obsahující informace o pozici myši.</param>
        private void MouseMove(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine("LineBehavior MouseMove");
            Point mousePosition = e.GetPosition(ParentCanvas);

            // metoda v service MouseHandlingService pro kreslení vazby - logika i podmínky jsou řešeny v dané metodě UpdateOperation
            _mouseHandlingService.UpdateOperation(mousePosition, ParentCanvas, null);
        }

        /// <summary>
        /// Metoda volaná při odpojení chování od objektu - tedy destrukci
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            DisassociateEventHandlers();
        }

        ///// <summary>
        ///// Odpojení mouse services při destrukci objektu - detail
        ///// </summary>
        private void DisassociateEventHandlers()
        {
            AssociatedObject.MouseLeftButtonDown -= MouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp -= MouseLeftButtonPreviewUp;
            AssociatedObject.MouseMove -= MouseMove;
        }
    }
}