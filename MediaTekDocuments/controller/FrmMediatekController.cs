using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using System;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des Commandes
        /// </summary>
        /// <returns>Liste d'objets Commandes</returns>
        public List<CommandeDocument> GetAllCommandes(string id)
        {
            return access.GetAllCommandes(id);
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur la liste des Abonnements Revues
        /// </summary>
        /// <returns>Liste d'objets Abonnement</returns>
        public List<Abonnement> GetAllAbonnementsRevues(string idRevue)
        {
            return access.GetAllAbonnementsRevues(idRevue);
        }

        /// <summary>
        /// getter sur la Création des Abonnements Revues
        /// </summary>
        /// <returns>Liste création Abonnement</returns>
        public bool CreerAbonnement(string id, DateTime dateFinAbonnement, string idRevue)
        {
            return access.CreerAbonnement(id, dateFinAbonnement, idRevue);
        }


        /// <summary>
        /// Supprime un abonnement dans la bdd
        /// </summary>
        /// <param name="id">Id de l'abonnement de document à supprimer</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool SupprimerAbonnement(string id)
        {
            return access.SupprimerAbonnement(id);
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// getter sur les rappels
        /// </summary>
        /// <returns>Liste d'objets Abonnement</returns>
        public List<Abonnement> GetAllRappelRevue()
        {
            return access.GetAllRappelRevue();
        }

        /// <summary>
        /// getter sur les utilisateurs
        /// </summary>
        /// <returns>Liste d'objets Utilisateur</returns>
        public string GetAllUtilisateur(string login, string password)
        {
            List<Utilisateur> Utilisateur = access.GetAllUtilisateur(login);
            foreach (var user in Utilisateur)
            {
                if (user.password == password)
                {
                    return user.idService;
                }
            }
            return "";
        }

        /// <summary>
        /// Créer une commande d'un livre dans la BDD
        /// </summary>
        /// <param name="commande">L'objet Commande concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommande(Commande commande)
        {
            return access.CreerCommande(commande);
        }

        /// <summary>
        /// Crée une commande de livre dans la bdd
        /// </summary>
        /// <param name="commandedocument">L'objet commande concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommandeDocument(string id, int nbExemplaire, string idLivreDvd, string idSuivi)
        {
            return access.CreerCommandeDocument(id, nbExemplaire, idLivreDvd, idSuivi);
        }

        /// <summary>
        /// Supprimer une commande de livre dans la bdd
        /// </summary>
        /// <param name="commandedocument">L'objet commande concerné</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerCommande(string id)
        {
            return access.SupprimerCommande(id);
        }

        /// <summary>
        /// Modifie l'étape de suivi d'une commande
        /// </summary>
        /// <param name="id">Id de la commande de document à modifier</param>
        /// <param name="idSuivi">Id de l'étape de suivi</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool ModifierSuiviCommandeDocument(string id, string idSuivi)
        {
            return access.ModifierSuiviCommandeDocument(id, idSuivi);
        }
    }
}
