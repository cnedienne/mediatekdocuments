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
    public partial class FrmAuthentification : Form
    {
        private readonly FrmMediatekController controller;
        public FrmAuthentification()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();

        }
        private List<Utilisateur> utilisateur = new List<Utilisateur>();

        private void btnConnexion_Click(object sender, EventArgs e)
        {
            if (!tbxUtilisateur.Text.Equals("") && !tbxMdp.Text.Equals(""))
            {
                string login = tbxUtilisateur.Text;
                string password = tbxMdp.Text;
                string idService = controller.GetAllUtilisateur(login, password);
                if (!idService.Equals(""))
                {
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
                    MessageBox.Show("Mot de passe ou utilisateur inconnu", "Erreur");
                    tbxUtilisateur.Text = "";
                    tbxMdp.Text = "";
                    tbxUtilisateur.Focus();
                }
            }
            else
            {
                MessageBox.Show("Merci de remplir la case utilisateur et mot de passe", "Erreur");
                tbxUtilisateur.Text = "";
                tbxMdp.Text = "";
                tbxUtilisateur.Focus();
            }

        }
    }
}
