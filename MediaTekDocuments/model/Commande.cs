using System;

namespace MediaTekDocuments.model
{
    public class Commande
    {
        public int? Id { get; set; } // ID nullable
        public DateTime DateCommande { get; set; }
        public double Montant { get; set; }

        public Commande(int? id, DateTime dateCommande, double montant)
        {
            this.Id = id;
            this.DateCommande = dateCommande;
            this.Montant = montant;
        }
    }
}
