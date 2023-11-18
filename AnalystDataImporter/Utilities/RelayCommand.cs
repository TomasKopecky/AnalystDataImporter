using System;
using System.Windows.Input;

namespace AnalystDataImporter.Utilities
{
    /// <summary>
    /// Třída RelayCommand implementuje rozhraní ICommand. Je často používána v MVVM vzoru pro propojení příkazů z uživatelského rozhraní s metodami v ViewModelu.
    /// </summary>
    public class RelayCommand<T> : ICommand
    {

        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;
        /// <summary>
        /// Akce, která se má provést při spuštění příkazu.
        /// </summary>
        //private readonly Action _execute;

        ///// <summary>
        ///// Funkce, která určuje, zda lze příkaz spustit.
        ///// </summary>
        //private readonly Func<bool> _canExecute;

        /// <summary>
        /// Událost, která se vyvolá, když se změní možnost spuštění příkazu.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Konstruktor třídy RelayCommand. Přijímá akci, která se má provést, a volitelnou funkci pro určení, zda lze příkaz spustit.
        /// </summary>
        /// <param name="execute">Akce k provedení.</param>
        /// <param name="canExecute">Funkce určující, zda lze akci provést.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            if (parameter == null && typeof(T).IsValueType)
                return _canExecute(default);

            if (parameter == null || parameter is T)
                return _canExecute((T)parameter);

            return false;
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute, Func<bool> canExecute = null)
            : base(param => execute(), param => canExecute?.Invoke() ?? true)
        {
        }

        public RelayCommand(Action execute)
            : base(param => execute(), param => true)
        {
        }
    }
}