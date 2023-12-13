using AnalystDataImporter.Globals;

namespace AnalystDataImporter.Models
{
    /// <summary>
    /// Třída reprezentující model uživatele. Definuje vlastnosti jako ID, uživatelské jméno a uživatelská práva.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Jedinečné ID uživatele.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Uživatelské jméno.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Uživatelská práva, která definují, co může uživatel v aplikaci dělat.
        /// </summary>
        public Constants.UserRights Rights { get; set; }
    }
}