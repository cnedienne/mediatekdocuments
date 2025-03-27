using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Formulaire d'authentification pour les utilisateurs.
    /// </summary>
    public partial class FrmAuthentification : Form
    {
        /// <summary>
        /// Contrôleur associé au formulaire d'authentification.
        /// </summary>
        private readonly FrmMediatekController controller;

        /// <summary>
        /// Liste des utilisateurs (non utilisée dans le code actuel).
        /// </summary>
        private List<Utilisateur> utilisateur = new List<Utilisateur>();

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="FrmAuthentification"/>.
        /// </summary>
        public FrmAuthentification()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        /// Gère l'événement de clic sur le bouton de connexion.
        /// Vérifie les informations d'identification de l'utilisateur et redirige en fonction de son rôle.
        /// </summary>
        /// <param name="sender">Objet source de l'événement.</param>
        /// <param name="e">Données de l'événement.</param>
        private void btnConnexion_Click(object sender, EventArgs e)
        {
            // Vérifie si les champs utilisateur et mot de passe ne sont pas vides.
            if (!tbxUtilisateur.Text.Equals("") && !tbxMdp.Text.Equals(""))
            {
                string login = tbxUtilisateur.Text;
                string password = tbxMdp.Text;

                // Récupère l'identifiant du service associé à l'utilisateur.
                string idService = controller.GetAllUtilisateur(login, password);

                if (!idService.Equals(""))
                {
                    // Redirige en fonction de l'identifiant du service.
                    switch (idService)
                    {
                        case "1":
                        case "0":
                            FrmMediatek frmMediatek = new FrmMediatek();
                            frmMediatek.ShowDialog();
                            this.Close();
                            break;
                        case "2":
                            FrmMediatek FrmMediatek = new FrmMediatek();
                            FrmMediatek.EmpecherAcces();
                            FrmMediatek.ShowDialog();
                            this.Close();
                            break;
                        case "3":
                            MessageBox.Show("Accès refusé!", "Alerte");
                            Application.Exit();
                            break;
                    }
                }
                else
                {
                    // Affiche un message d'erreur si les informations d'identification sont incorrectes.
                    MessageBox.Show("Mot de passe ou utilisateur inconnu", "Erreur");
                    tbxUtilisateur.Text = "";
                    tbxMdp.Text = "";
                    tbxUtilisateur.Focus();
                }
            }
            else
            {
                // Affiche un message d'erreur si les champs sont vides.
                MessageBox.Show("Merci de remplir la case utilisateur et mot de passe", "Erreur");
                tbxUtilisateur.Text = "";
                tbxMdp.Text = "";
                tbxUtilisateur.Focus();
            }
        }
    }
}
