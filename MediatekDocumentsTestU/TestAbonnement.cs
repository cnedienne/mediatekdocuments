using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MediatekDocumentsTests
{
    [TestClass]
    public class AbonnementTest
    {
        private bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFinAbonnement;
        }

        [TestMethod]
        public void ParutionDansAbonnement_DateDansIntervalle_RetourneVrai()
        {
            // Arrange
            DateTime dateCommande = new DateTime(2024, 1, 1);
            DateTime dateFinAbonnement = new DateTime(2024, 12, 31);
            DateTime dateParution = new DateTime(2024, 6, 15);

            // Act
            bool resultat = ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            // Assert
            Assert.IsTrue(resultat);
        }

        [TestMethod]
        public void ParutionDansAbonnement_DateAvantCommande_RetourneFaux()
        {
            // Arrange
            DateTime dateCommande = new DateTime(2024, 1, 1);
            DateTime dateFinAbonnement = new DateTime(2024, 12, 31);
            DateTime dateParution = new DateTime(2023, 12, 31);

            // Act
            bool resultat = ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            // Assert
            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void ParutionDansAbonnement_DateApresFinAbonnement_RetourneFaux()
        {
            // Arrange
            DateTime dateCommande = new DateTime(2024, 1, 1);
            DateTime dateFinAbonnement = new DateTime(2024, 12, 31);
            DateTime dateParution = new DateTime(2025, 1, 1);

            // Act
            bool resultat = ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            // Assert
            Assert.IsFalse(resultat);
        }
    }
}
