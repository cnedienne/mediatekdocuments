using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Représente un utilisateur du système.
    /// </summary>
    public class Utilisateur
    {
        /// <summary>
        /// Identifiant unique de l'utilisateur.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Login de l'utilisateur.
        /// </summary>
        public string login { get; set; }

        /// <summary>
        /// Mot de passe de l'utilisateur.
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// Identifiant du service auquel l'utilisateur est associé.
        /// </summary>
        public string idService { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe Utilisateur.
        /// </summary>
        /// <param name="id">Identifiant unique de l'utilisateur.</param>
        /// <param name="login">Login de l'utilisateur.</param>
        /// <param name="password">Mot de passe de l'utilisateur.</param>
        /// <param name="idService">Identifiant du service associé.</param>
        public Utilisateur(string id, string login, string password, string idService)
        {
            this.id = id;
            this.login = login;
            this.password = password;
            this.idService = idService;
        }
    }
}