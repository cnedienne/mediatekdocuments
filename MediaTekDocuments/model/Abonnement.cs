using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Abonnement : Commande
    {
        public string idRevue { get; set; }
        public DateTime dateFinAbonnement { get; set; }
        public string titre { get; set; }

        public Abonnement(string id, DateTime dateCommande, double montant, string idRevue, DateTime dateFinAbonnement, string titre = null)
            : base(id, dateCommande, montant)
        {
            this.idRevue = idRevue;
            this.dateFinAbonnement = dateFinAbonnement;
            this.titre = titre;
        }
    }
}