using System;
using AnalystDataImporter.Models;
using System.ComponentModel;
using AnalystDataImporter.Globals;

namespace AnalystDataImporter.ViewModels
{
    /// <summary>
    /// ViewModel třída pro model User. Definuje vlastnosti uživatele a poskytuje mechanismus k upozornění uživatelského rozhraní na jakékoli změny ve vlastnostech ViewModelu.
    /// </summary>
    public class UserViewModel : INotifyPropertyChanged
    {
        private readonly User _user;

        public UserViewModel(User user)
        {
            // Ověřte, zda poskytnutý user není null a inicializujte interní _user
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }

        /// <summary>
        /// Vlastnost ID uživatele.
        /// </summary>
        public int Id
        {
            get => _user.Id;
            set
            {
                if (_user.Id != value)
                {
                    _user.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        /// <summary>
        /// Uživatelské jméno.
        /// </summary>
        public string Username
        {
            get => _user.Username;
            set
            {
                if (_user.Username != value)
                {
                    _user.Username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        /// <summary>
        /// Oprávnění uživatele.
        /// </summary>
        public Constants.UserRights Rights
        {
            get => _user.Rights;
            set
            {
                if (_user.Rights != value)
                {
                    _user.Rights = value;
                    OnPropertyChanged(nameof(Rights));
                }
            }
        }

        // Zde můžete přidat další vlastnosti podle potřeby pro ViewModel...

        /// <summary>
        /// Událost, která se vyvolá, pokud se změní některá z vlastností ViewModelu.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Metoda vyvolává událost PropertyChanged.
        /// </summary>
        /// <param name="propertyName">Název změněné vlastnosti.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}