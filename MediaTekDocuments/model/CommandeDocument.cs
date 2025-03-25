using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier CommandeDocument qui hérite de Commande
    /// </summary>
    public class CommandeDocument : Commande
    {

        public int nbExemplaire { get; set; }
        public string idSuivi { get; set; }
        public string etapeSuivi { get; set; }
        public string idLivreDvd { get; set; }

        public CommandeDocument(string id, DateTime dateCommande, double montant, int nbExemplaire,
            string idSuivi, string idDvdLivre)
            : base(id, dateCommande, montant)
        {
            this.nbExemplaire = nbExemplaire;
            this.idSuivi = idSuivi;
            this.idLivreDvd = idDvdLivre;
        }
    }
}
