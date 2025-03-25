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
    public partial class FrmRappelAbonnement : Form
    {
        private readonly FrmMediatekController controller;
        public FrmRappelAbonnement()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }
        private readonly BindingSource bdgRappelRevue = new BindingSource();
        private List<Abonnement> lesRappels = new List<Abonnement>();

        private void btnFermerRappel_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }
        private void FrmRappelAbonnement_Load(object sender, EventArgs e)
        {
            lesRappels = controller.GetAllRappelRevue();
            RemplirRappelRevueListe(lesRappels);
        }
        private void RemplirRappelRevueListe(List<Abonnement> lesRappels)
        {
            if (lesRappels != null)
            {
                bdgRappelRevue.DataSource = lesRappels;
                dgvRappelRevue.DataSource = bdgRappelRevue;
                dgvRappelRevue.Columns["id"].Visible = false;
                dgvRappelRevue.Columns["dateCommande"].Visible = false;
                dgvRappelRevue.Columns["montant"].Visible = false;
                dgvRappelRevue.Columns["idRevue"].Visible = false;
                dgvRappelRevue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            else
            {
                bdgRappelRevue.DataSource = null;
            }
        }
    }
}
