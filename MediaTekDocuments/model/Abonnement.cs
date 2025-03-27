using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Représente un abonnement qui hérite de la classe Commande.
    /// </summary>
    public class Abonnement : Commande
    {
        /// <summary>
        /// Identifiant de la revue associée à l'abonnement.
        /// </summary>
        public string idRevue { get; set; }

        /// <summary>
        /// Date de fin de l'abonnement.
        /// </summary>
        public DateTime dateFinAbonnement { get; set; }

        /// <summary>
        /// Titre de la revue (facultatif).
        /// </summary>
        public string titre { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe Abonnement.
        /// </summary>
        /// <param name="id">Identifiant de la commande.</param>
        /// <param name="dateCommande">Date de la commande.</param>
        /// <param name="montant">Montant de la commande.</param>
        /// <param name="idRevue">Identifiant de la revue associée.</param>
        /// <param name="dateFinAbonnement">Date de fin de l'abonnement.</param>
        /// <param name="titre">Titre de la revue (facultatif).</param>
        public Abonnement(string id, DateTime dateCommande, double montant, string idRevue, DateTime dateFinAbonnement, string titre = null)
            : base(id, dateCommande, montant)
        {
            this.idRevue = idRevue;
            this.dateFinAbonnement = dateFinAbonnement;
            this.titre = titre;
        }
    }
}