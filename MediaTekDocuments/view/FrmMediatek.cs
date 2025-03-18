using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private readonly BindingSource bdgSuivis = new BindingSource();


        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Rempli un combo de suivi
        /// </summary>
        /// <param name="lesSuivis">liste des etats de suivis</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboSuivi(List<Suivi> lesSuivis, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesSuivis;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Rempli un combo d'etat
        /// </summary>
        /// <param name="lesEtats"></param>
        /// <param name="bdg"></param>
        /// <param name="cbx"></param>
        public void RemplirComboEtat(List<Etat> lesEtats, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesEtats;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
        }


        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }
        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        #endregion

        #region Onglet Parutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }

        #endregion

        #region Onglet Commande de Livre
        private string Mode = ""; // Mode (Ajout, Supp, Modif)
        private void VisibleGroupBoxCommandeLivre()
        {
            label70.Visible = true;
            txbLivresComNbCommande.Visible = true;
            txbLivresComNbCommande.ReadOnly = true;
            txbLivresComMontant.ReadOnly = true;
            dtpLivresComDateCommande.Enabled = false;
            txbLivresComNbExemplaires.ReadOnly = true;
            txbLivresComNumLivre.Visible = false;
            cbxLivresComEtat.Visible = false;
            labelNumCmdLivre.Visible = false;
            labelEtatCmdLivre.Visible = false;
            btnLivresComValider.Visible = false;
            btnLivresComAnnuler.Visible = false;
        }
        private void VisibleCommandeLivre()
        {
            label70.Visible = false;
            txbLivresComNbCommande.Visible = false;
            txbLivresComMontant.ReadOnly = false;
            dtpLivresComDateCommande.Enabled = true;
            txbLivresComNbExemplaires.ReadOnly = false;
            txbLivresComNumLivre.Visible = true;
            labelEtatCmdLivre.Visible = false;
            cbxLivresComEtat.Visible = false;
            labelNumCmdLivre.Visible = true;
            btnLivresComValider.Visible = true;
            btnLivresComAnnuler.Visible = true;
        }
        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void ViderCmdLivresInfos()
        {
            txbLivresComNbCommande.Text = "";
            dtpLivresComDateCommande.Value = DateTime.Now;
            txbLivresComMontant.Text = "";
            txbLivresComNbExemplaires.Text = "";
            txbLivresComNumLivre.Text = "";
        }
        /// <summary>
        /// Modifie la visibilité de deux boutons pour les rendre invisibles si true, et visibles si falses.
        /// </summary>
        /// <param name="button1">Premier bouton</param>
        /// <param name="button2">Deuxième bouton</param>
        private void RendreBoutonsVisiblesOuInvisibles(Button button1, Button button2, Button button3, bool cacher)
        {
            if (cacher)
            {
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
            }
            else
            {
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
            }
        }
        private void AfficheCommandeLivreInfo(CommandeDocument commande)
        {
            if (dgvLivresComListe.Rows.Count > 0 && commande != null)
            {
                // Remplir les champs avec les info de la commande sélectionnée
                txbLivresComNbCommande.Text = commande.Id.ToString();
                dtpLivresComDateCommande.Value = commande.DateCommande;
                txbLivresComMontant.Text = commande.Montant.ToString("F2");
                txbLivresComNbExemplaires.Text = commande.NbExemplaire.ToString();
                txbLivresComNumLivre.Text = commande.IdLivreDvd;

                // nettoyer les éléments existants dans le ComboBox
                cbxLivresComEtat.Items.Clear();
                // récupérer tous les suivis
                var suivis = controller.GetAllSuivis();
                // Ajouter les suivis dans le ComboBox
                var suivisItems = suivis.Select(s => new Suivi(s.Id, s.Etat)).ToArray();
                cbxLivresComEtat.Items.AddRange(suivisItems);
                // définir pair clé/valeur
                cbxLivresComEtat.DisplayMember = "Etat";
                cbxLivresComEtat.ValueMember = "Id";
                // trouver l'état de la commande + sélectionner dans le ComboBox
                var etatCommande = suivisItems.FirstOrDefault(s => s.Id == commande.IdSuivi);
                if (etatCommande != null)
                {
                    cbxLivresComEtat.SelectedItem = etatCommande;
                }
                // événement pour récupérer l'id suivi lorsque l'état est sélectionné
                cbxLivresComEtat.SelectedIndexChanged += (sender, e) =>
                {
                    var selectedEtat = (Suivi)cbxLivresComEtat.SelectedItem;
                    int idSuiviSelectionne = selectedEtat.Id;
                };
            }

            else
            {
                ViderCmdLivresInfos();
            }
        }
        private void RemplirCmdLivresListeComplete()
        {
            RemplirCmdLivresListe(lesLivres);
            VideCmdLivresZones();
        }
        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideCmdLivresZones()
        {
            cbxLivresComGenres.SelectedIndex = -1;
            cbxLivresComRayons.SelectedIndex = -1;
            cbxLivresComPublics.SelectedIndex = -1;
            txtNumDoc.Text = "";
            txtCommandeLivresRecherche.Text = "";
        }
        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirCmdLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresComListe.DataSource = bdgLivresListe;
            dgvLivresComListe.Columns["isbn"].Visible = false;
            dgvLivresComListe.Columns["idRayon"].Visible = false;
            dgvLivresComListe.Columns["idGenre"].Visible = false;
            dgvLivresComListe.Columns["idPublic"].Visible = false;
            dgvLivresComListe.Columns["image"].Visible = false;
            dgvLivresComListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresComListe.Columns["id"].DisplayIndex = 0;
            dgvLivresComListe.Columns["titre"].DisplayIndex = 1;
        }
        /// <summary>
        /// Remplit le DataGridView avec les commandes associées au livre sélectionné.
        /// </summary>
        /// <param name="commandes">Liste des commandes.</param>
        private void RemplirCmdLivresCommandes(List<CommandeDocument> commandes)
        {
            BindingSource bdgCommandes = new BindingSource();
            bdgCommandes.DataSource = commandes;
            dgvLivresComListeCom.DataSource = bdgCommandes;

            // Configurer l'affichage des colonnes
            dgvLivresComListeCom.Columns["Id"].HeaderText = "ID Commande";
            dgvLivresComListeCom.Columns["DateCommande"].HeaderText = "Date de Commande";
            dgvLivresComListeCom.Columns["Montant"].HeaderText = "Montant (€)";
            dgvLivresComListeCom.Columns["NbExemplaire"].HeaderText = "Exemplaires";
            dgvLivresComListeCom.Columns["Etat"].HeaderText = "État";

            // Réorganiser les colonnes en fonction de leur index d'affichage
            dgvLivresComListeCom.Columns["Id"].DisplayIndex = 0;
            dgvLivresComListeCom.Columns["DateCommande"].DisplayIndex = 1;
            dgvLivresComListeCom.Columns["Montant"].DisplayIndex = 2;
            dgvLivresComListeCom.Columns["NbExemplaire"].DisplayIndex = 3;
            dgvLivresComListeCom.Columns["Etat"].DisplayIndex = 4;

            dgvLivresComListeCom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        /// <summary>
        /// Désactive la DataGridView pour ne pas avoir de problème en cas d'ajout ou de modification
        /// </summary>
        /// <param name="dataGridView">Le DataGridView à configurer</param>
        private void DesactiverDataGridViewLivres(DataGridView dgv, bool bloquer)
        {
            if (bloquer)
            {
                // Désactiver la sélection et interdire les interactions
                dgv.ClearSelection();
                dgv.Enabled = false;
                dgv.ReadOnly = true;
            }
            else
            {
                // Réactiver la sélection et les interactions
                dgv.ClearSelection();
                dgv.Enabled = true;
                dgv.ReadOnly = true;
            }
        }
        private void GererEtatComboBoxLivres(int etatActuel)
        {
            cbxLivresComEtat.Items.Clear();

            switch (etatActuel)
            {
                case 1: // En cours
                    cbxLivresComEtat.Items.Add(new Suivi(2, "Livrée"));
                    cbxLivresComEtat.Items.Add(new Suivi(4, "Relancée"));
                    cbxLivresComEtat.Items.Add(new Suivi(5, "Annulée"));
                    break;

                case 2: // Livrée
                    cbxLivresComEtat.Items.Add(new Suivi(3, "Réglée"));
                    cbxLivresComEtat.Items.Add(new Suivi(5, "Annulée"));
                    break;

                case 3: // Réglée
                    cbxLivresComEtat.Items.Add(new Suivi(5, "Annulée"));
                    break;

                case 4: // Relancée
                    cbxLivresComEtat.Items.Add(new Suivi(2, "Livrée"));
                    cbxLivresComEtat.Items.Add(new Suivi(5, "Annulée"));
                    break;

                case 5: // Annulée
                        // Une commande annulée ne peut pas changer d'état.
                    MessageBox.Show("Une commande annulée ne peut pas changer d'état.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;

                default:
                    MessageBox.Show("État inconnu.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            // Sélectionner automatiquement la première option si disponible
            if (cbxLivresComEtat.Items.Count > 0)
            {
                cbxLivresComEtat.SelectedIndex = 0;
            }
        }
        private void LabelCrudTitre(string mode, bool cacher)
        {
            // Visibilité du texte
            lblCrudTitre.Visible = cacher;

            switch (mode)
            {
                case "Ajout":
                    lblCrudTitre.Text = "Êtes-vous sûr de réaliser cet ajout ?";
                    break;
                case "Suppression":
                    lblCrudTitre.Text = "Êtes-vous sûr de réaliser cette suppression ?";
                    break;
                case "Modification":
                    lblCrudTitre.Text = "Êtes-vous sûr de réaliser cette modification ?";
                    break;
                default:
                    lblCrudTitre.Text = "";
                    break;
            }
        }
        private bool VerifierSelectionLivres(DataGridView dgvLivresComListeCom)
        {
            if (dgvLivresComListeCom.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vous devez sélectionner une ligne pour réaliser cette opération.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // Aucun élément sélectionné
            }
            return true; // Une ligne est sélectionnée
        }
        private void CacherValiderLivres()
        {
            LabelCrudTitre(null, false);
            VisibleGroupBoxCommandeLivre();
            DesactiverDataGridViewLivres(dgvLivresComListe, false);
            DesactiverDataGridViewLivres(dgvLivresComListeCom, false);
            RendreBoutonsVisiblesOuInvisibles(btnLivresComModifier, btnLivresComSupprimer, btnLivresComAjouter, false);
        }



        private void TabCmdLivre_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresComGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresComPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresComRayons);
            RemplirCmdLivresListeComplete();
            VisibleGroupBoxCommandeLivre();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            RemplirCmdLivresListeComplete();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            RemplirCmdLivresListeComplete();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RemplirCmdLivresListeComplete();
        }
        private void txtCommandeLivresRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txtCommandeLivresRecherche.Text.Equals(""))
            {
                cbxLivresComGenres.SelectedIndex = -1;
                cbxLivresComRayons.SelectedIndex = -1;
                cbxLivresComPublics.SelectedIndex = -1;
                txtNumDoc.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txtCommandeLivresRecherche.Text.ToLower()));
                RemplirCmdLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresComGenres.SelectedIndex < 0 && cbxLivresComPublics.SelectedIndex < 0 && cbxLivresComRayons.SelectedIndex < 0
                    && txtNumDoc.Text.Equals(""))
                {
                    RemplirCmdLivresListeComplete();
                }
            }
        }
        private void btnRechercherComLivres_Click(object sender, EventArgs e)
        {
            if (!txtNumDoc.Text.Equals(""))
            {
                txtCommandeLivresRecherche.Text = "";
                cbxLivresComGenres.SelectedIndex = -1;
                cbxLivresComRayons.SelectedIndex = -1;
                cbxLivresComPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txtNumDoc.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirCmdLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirCmdLivresListeComplete();
                }
            }
            else
            {
                RemplirCmdLivresListeComplete();
            }
        }
        private void dgvLivresComListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresComListe.CurrentRow != null)
            {
                // Récupère le livre sélectionné
                Livre livreSelectionne = (Livre)dgvLivresComListe.CurrentRow.DataBoundItem;

                // Utilise la méthode du contrôleur pour récupérer les commandes associées au livre sélectionné
                List<CommandeDocument> commandesAssociees = controller.GetCommandesLivres(livreSelectionne.Id);

                // Remplir la DataGridView avec les commandes récupérées
                RemplirCmdLivresCommandes(commandesAssociees);
            }
        }
        private void dgvLivresComListeCom_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresComListeCom.CurrentRow != null)
            {
                // Récupérer la commande sélectionnée dans la DataGridView
                var commande = (CommandeDocument)dgvLivresComListeCom.CurrentRow.DataBoundItem;
                // Appeler AfficheCommandeLivreInfo avec une seule commande
                AfficheCommandeLivreInfo(commande);
            }
        }
        private void btnLivresComModifier_Click(object sender, EventArgs e)
        {
            ModeDvd = "Modification";

            if (!VerifierSelectionLivres(dgvLivresComListeCom))
            {
                ModeDvd = "";
            }
            else
            {
                LabelCrudTitre(ModeDvd, true);
                VisibleCommandeLivre();
                RendreBoutonsVisiblesOuInvisibles(btnLivresComModifier, btnLivresComSupprimer, btnLivresComAjouter, true);
                cbxLivresComEtat.Visible = true;
                DesactiverDataGridViewLivres(dgvLivresComListe, true);
                DesactiverDataGridViewLivres(dgvLivresComListeCom, true);
                // Vérifier si un élément a été sélectionné dans le ComboBox
                if (cbxLivresComEtat.SelectedItem != null)
                {
                    // Récupérer l'objet Suivi sélectionné
                    Suivi suiviSelectionne = (Suivi)cbxLivresComEtat.SelectedItem;

                    // Récupérer l'ID du suivi sélectionné
                    int idSuivi = suiviSelectionne.Id;
                    GererEtatComboBoxLivres(idSuivi);
                }
            }
        }
        private void btnLivresComSupprimer_Click(object sender, EventArgs e)
        {
            Mode = "Suppression";
            if (!VerifierSelectionLivres(dgvLivresComListeCom))
            {
                Mode = "";
            }
            else
            {
                LabelCrudTitre(Mode, true);
                RendreBoutonsVisiblesOuInvisibles(btnLivresComModifier, btnLivresComSupprimer, btnLivresComAjouter, true);
                VisibleCommandeLivre();
            }

        }
        private void btnLivresComAjouter_Click(object sender, EventArgs e)
        {
            Mode = "Ajout";
            LabelCrudTitre(Mode, true);
            VisibleCommandeLivre();
            RendreBoutonsVisiblesOuInvisibles(btnLivresComModifier, btnLivresComSupprimer, btnLivresComAjouter, true);
            DesactiverDataGridViewLivres(dgvLivresComListe, true);
            DesactiverDataGridViewLivres(dgvLivresComListeCom, true);
            ViderCmdLivresInfos();
        }
        private void btnLivresComValider_Click(object sender, EventArgs e)
        {
            try
            {
                // Récupération des données communes
                DateTime dateCommande = dtpLivresComDateCommande.Value;
                double montant = double.TryParse(txbLivresComMontant.Text, out double tempMontant) ? tempMontant : throw new Exception("Montant invalide.");
                int nbExemplaires = int.TryParse(txbLivresComNbExemplaires.Text, out int tempNbExemplaires) ? tempNbExemplaires : throw new Exception("Nombre d'exemplaires invalide.");
                string numLivre = txbLivresComNumLivre.Text;

                CommandeDocument commandeDocument = null;

                // Vérifie le mode sélectionné et crée l'objet `CommandeDocument`
                switch (Mode)
                {
                    case "Ajout":
                        // Valeurs spécifiques à l'ajout
                        commandeDocument = new CommandeDocument(null, dateCommande, montant, nbExemplaires, numLivre, 1, "En Cours");
                        ExecuterOperationLivres(() => controller.CreerCommandeDocument(commandeDocument), "Commande ajoutée avec succès.", "Erreur lors de l'ajout.");
                        ViderCmdLivresInfos();
                        CacherValiderLivres();
                        break;

                    case "Suppression":
                        // Vérification de la sélection dans le ComboBox
                        if (cbxLivresComEtat.SelectedItem == null)
                            throw new Exception("Veuillez sélectionner un état de suivi pour la suppression.");

                        Suivi suiviSelectionneSupp = (Suivi)cbxLivresComEtat.SelectedItem;
                        int idSuiviSupp = suiviSelectionneSupp.Id;
                        string etatSuiviSupp = suiviSelectionneSupp.Etat;

                        int idCommandeSupp = int.TryParse(txbLivresComNbCommande.Text, out int tempIdCommandeSupp) ? tempIdCommandeSupp : throw new Exception("ID commande invalide.");
                        commandeDocument = new CommandeDocument(idCommandeSupp, dateCommande, montant, nbExemplaires, numLivre, idSuiviSupp, etatSuiviSupp);
                        ExecuterOperationLivres(() => controller.SupprimerCommandeDocument(commandeDocument), "Commande supprimée avec succès.", "Erreur lors de la suppression.");
                        ViderCmdLivresInfos();
                        CacherValiderLivres();
                        break;

                    case "Modification":
                        // Vérification de la sélection dans le ComboBox
                        if (cbxLivresComEtat.SelectedItem == null)
                            throw new Exception("Veuillez sélectionner un état de suivi pour la modification.");

                        Suivi suiviSelectionneModif = (Suivi)cbxLivresComEtat.SelectedItem;
                        int idSuiviModif = suiviSelectionneModif.Id;
                        string etatSuiviModif = suiviSelectionneModif.Etat;

                        int idCommandeModif = int.TryParse(txbLivresComNbCommande.Text, out int tempIdCommandeModif) ? tempIdCommandeModif : throw new Exception("ID commande invalide.");
                        commandeDocument = new CommandeDocument(idCommandeModif, dateCommande, montant, nbExemplaires, numLivre, idSuiviModif, etatSuiviModif);
                        ExecuterOperationLivres(() => controller.ModifierCommandeDocument(commandeDocument), "Commande modifiée avec succès.", "Erreur lors de la modification.");
                        ViderCmdLivresInfos();
                        CacherValiderLivres();
                        break;

                    default:
                        MessageBox.Show("Veuillez sélectionner une opération.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Méthode pour exécuter une opération et afficher un message selon le résultat.
        /// </summary>
        private void ExecuterOperationLivres(Func<bool> operation, string messageSucces, string messageErreur)
        {
            try
            {
                bool result = operation();
                MessageBox.Show(result ? messageSucces : messageErreur, result ? "Succès" : "Erreur", MessageBoxButtons.OK, result ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur inattendue est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLivresComAnnuler_Click(object sender, EventArgs e)
        {
            LabelCrudTitre(null, false);
            VisibleGroupBoxCommandeLivre();
            DesactiverDataGridViewLivres(dgvLivresComListe, false);
            DesactiverDataGridViewLivres(dgvLivresComListeCom, false);
            RendreBoutonsVisiblesOuInvisibles(btnLivresComModifier, btnLivresComSupprimer, btnLivresComAjouter, false);


        }
        #endregion


        #region Onglet Commande de Dvd

        private string ModeDvd = ""; // ModeDvd (Ajout, Supp, Modif)
        private void VisibleGroupBoxCommandeDvd()
        {
            label70.Visible = true;
            txbDvdComNbCommande.Visible = true;
            txbDvdComNbCommande.ReadOnly = true;
            txbDvdComMontant.ReadOnly = true;
            dtpDvdComDateCommande.Enabled = false;
            txbDvdComNbExemplaires.ReadOnly = true;
            txbDvdComNumDvd.Visible = false;
            cbxDvdComEtat.Visible = false;
            labelNumCmdDvd.Visible = false;
            labelEtatCmdDvd.Visible = false;
            btnDvdComValider.Visible = false;
            btnDvdComAnnuler.Visible = false;
        }
        private void VisibleCommandeDvd()
        {
            label70.Visible = false;
            txbDvdComNbCommande.Visible = false;
            txbDvdComMontant.ReadOnly = false;
            dtpDvdComDateCommande.Enabled = true;
            txbDvdComNbExemplaires.ReadOnly = false;
            txbDvdComNumDvd.Visible = true;
            labelEtatCmdDvd.Visible = false;
            cbxDvdComEtat.Visible = false;
            labelNumCmdDvd.Visible = true;
            btnDvdComValider.Visible = true;
            btnDvdComAnnuler.Visible = true;
        }
        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void ViderCmdDvdInfos()
        {
            txbDvdComNbCommande.Text = "";
            dtpDvdComDateCommande.Value = DateTime.Now;
            txbDvdComMontant.Text = "";
            txbDvdComNbExemplaires.Text = "";
            txbDvdComNumDvd.Text = "";
        }
        /// <summary>
        /// Modifie la visibilité de deux boutons pour les rendre invisibles si true, et visibles si falses.
        /// </summary>
        /// <param name="button1">Premier bouton</param>
        /// <param name="button2">Deuxième bouton</param>
        private void RendreBoutonsVisiblesOuInvisiblesDvd(Button button1, Button button2, Button button3, bool cacher)
        {
            if (cacher)
            {
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
            }
            else
            {
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
            }
        }
        private void AfficheCommandeDvdInfo(CommandeDocument commande)
        {
            if (dgvDvdComListe.Rows.Count > 0 && commande != null)
            {
                // Remplir les champs avec les info de la commande sélectionnée
                txbDvdComNbCommande.Text = commande.Id.ToString();
                dtpDvdComDateCommande.Value = commande.DateCommande;
                txbDvdComMontant.Text = commande.Montant.ToString("F2");
                txbDvdComNbExemplaires.Text = commande.NbExemplaire.ToString();
                txbDvdComNumDvd.Text = commande.IdLivreDvd;

                // nettoyer les éléments existants dans le ComboBox
                cbxDvdComEtat.Items.Clear();
                // récupérer tous les suivis
                var suivis = controller.GetAllSuivis();
                // Ajouter les suivis dans le ComboBox
                var suivisItems = suivis.Select(s => new Suivi(s.Id, s.Etat)).ToArray();
                cbxDvdComEtat.Items.AddRange(suivisItems);
                // définir pair clé/valeur
                cbxDvdComEtat.DisplayMember = "Etat";
                cbxDvdComEtat.ValueMember = "Id";
                // trouver l'état de la commande + sélectionner dans le ComboBox
                var etatCommande = suivisItems.FirstOrDefault(s => s.Id == commande.IdSuivi);
                if (etatCommande != null)
                {
                    cbxDvdComEtat.SelectedItem = etatCommande;
                }
                // événement pour récupérer l'id suivi lorsque l'état est sélectionné
                cbxDvdComEtat.SelectedIndexChanged += (sender, e) =>
                {
                    var selectedEtat = (Suivi)cbxDvdComEtat.SelectedItem;
                    int idSuiviSelectionne = selectedEtat.Id;
                };
            }
            else
            {
                ViderCmdDvdInfos();
            }
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideCmdDvdZones()
        {
            cbxDvdComGenres.SelectedIndex = -1;
            cbxDvdComRayons.SelectedIndex = -1;
            cbxDvdComPublics.SelectedIndex = -1;
            txtNumDoc.Text = "";
            txtCommandeDvdRecherche.Text = "";
        }

        private void RemplirCmdDvdListeComplete()
        {
            RemplirCmdDvdListe(lesDvd);
            VideCmdDvdZones();
        }

        /// <summary>
        /// Remplit le DataGridView avec la liste reçue en paramètre
        /// </summary>
        /// <param name="dvd">liste de dvd</param>
       /* private void RemplirCmdDvdListe(List<Dvd> dvd)
        {
            bdgDvdListe.DataSource = dvd;
            dgvDvdComListe.DataSource = bdgDvdListe;
            dgvDvdComListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdComListe.Columns["id"].DisplayIndex = 0;
            dgvDvdComListe.Columns["titre"].DisplayIndex = 1;
        } */

        /// <summary>
        /// Remplit le DataGridView avec les commandes associées au dvd sélectionné.
        /// </summary>
        /// <param name="commandes">Liste des commandes.</param>
        private void RemplirCmdDvdCommandes(List<CommandeDocument> commandes)
        {

            BindingSource bdgCommandes = new BindingSource();
            if (bdgCommandes.Count > 0)
            {
                dgvDvdComListeCom.DataSource = bdgCommandes;
            }
            else
            {
                MessageBox.Show("Aucune commande trouvée.");
            }


            // Configurer l'affichage des colonnes
            dgvDvdComListeCom.Columns["Id"].HeaderText = "ID Commande";
            dgvDvdComListeCom.Columns["DateCommande"].HeaderText = "Date de Commande";
            dgvDvdComListeCom.Columns["Montant"].HeaderText = "Montant (€)";
            dgvDvdComListeCom.Columns["NbExemplaire"].HeaderText = "Exemplaires";
            dgvDvdComListeCom.Columns["Etat"].HeaderText = "État";

            // Réorganiser les colonnes en fonction de leur index d'affichage
            dgvDvdComListeCom.Columns["Id"].DisplayIndex = 0;
            dgvDvdComListeCom.Columns["DateCommande"].DisplayIndex = 1;
            dgvDvdComListeCom.Columns["Montant"].DisplayIndex = 2;
            dgvDvdComListeCom.Columns["NbExemplaire"].DisplayIndex = 3;
            dgvDvdComListeCom.Columns["Etat"].DisplayIndex = 4;

            dgvDvdComListeCom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdComListeCom.Refresh();

        }


        /// <summary>
        /// Remplit le DataGridView avec la liste reçue en paramètre
        /// </summary>
        /// <param name="dvd">liste de dvd</param>
        private void RemplirCmdDvdListe(List<Dvd> dvd)
        {
            bdgDvdListe.DataSource = dvd;
            dgvDvdComListe.DataSource = bdgDvdListe;
            dgvDvdComListe.Columns["idRayon"].Visible = false;
            dgvDvdComListe.Columns["idGenre"].Visible = false;
            dgvDvdComListe.Columns["idPublic"].Visible = false;
            dgvDvdComListe.Columns["image"].Visible = false;
            dgvDvdComListe.Columns["synopsis"].Visible = false;
            dgvDvdComListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdComListe.Columns["id"].DisplayIndex = 0;
            dgvDvdComListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Désactive la DataGridView pour ne pas avoir de problème en cas d'ajout ou de modification
        /// </summary>
        /// <param name="dataGridView">Le DataGridView à configurer</param>
        private void DesactiverDataGridViewDvd(DataGridView dgv, bool bloquer)
        {
            if (bloquer)
            {
                // Désactiver la sélection et interdire les interactions
                dgv.ClearSelection();
                dgv.Enabled = false;
                dgv.ReadOnly = true;
            }
            else
            {
                // Réactiver la sélection et les interactions
                dgv.ClearSelection();
                dgv.Enabled = true;
                dgv.ReadOnly = true;
            }
        }
        private void GererEtatComboBoxDvd(int etatActuel)
        {
            cbxDvdComEtat.Items.Clear();

            switch (etatActuel)
            {
                case 1: // En cours
                    cbxDvdComEtat.Items.Add(new Suivi(2, "Livrée"));
                    cbxDvdComEtat.Items.Add(new Suivi(4, "Relancée"));
                    cbxDvdComEtat.Items.Add(new Suivi(5, "Annulée"));
                    break;

                case 2: // Livrée
                    cbxDvdComEtat.Items.Add(new Suivi(3, "Réglée"));
                    cbxDvdComEtat.Items.Add(new Suivi(5, "Annulée"));
                    break;

                case 3: // Réglée
                    cbxDvdComEtat.Items.Add(new Suivi(5, "Annulée"));
                    break;

                case 4: // Relancée
                    cbxDvdComEtat.Items.Add(new Suivi(2, "Livrée"));
                    cbxDvdComEtat.Items.Add(new Suivi(5, "Annulée"));
                    break;

                case 5: // Annulée
                        // Une commande annulée ne peut pas changer d'état.
                    MessageBox.Show("Une commande annulée ne peut pas changer d'état.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;

                default:
                    MessageBox.Show("État inconnu.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            // Sélectionner automatiquement la première option si disponible
            if (cbxDvdComEtat.Items.Count > 0)
            {
                cbxDvdComEtat.SelectedIndex = 0;
            }
        }

        private void LabelCrudTitreDvd(string mode, bool cacher)
        {
            // Visibilité du texte
            lblCrudDvd.Visible = cacher;

            switch (mode)
            {
                case "Ajout":
                    lblCrudDvd.Text = "Êtes-vous sûr de réaliser cet ajout ?";
                    break;
                case "Suppression":
                    lblCrudDvd.Text = "Êtes-vous sûr de réaliser cette suppression ?";
                    break;
                case "Modification":
                    lblCrudDvd.Text = "Êtes-vous sûr de réaliser cette modification ?";
                    break;
                default:
                    lblCrudDvd.Text = "";
                    break;
            }
        }
        private bool VerifierSelectionDvd(DataGridView dgvDvdComListeCom)
        {
            if (dgvDvdComListeCom.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vous devez sélectionner une ligne pour réaliser cette opération.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // Aucun élément sélectionné
            }
            return true; // Une ligne est sélectionnée
        }
        private void CacherValiderDvd()
        {
            LabelCrudTitreDvd(null, false);
            VisibleGroupBoxCommandeDvd();
            DesactiverDataGridViewDvd(dgvDvdComListe, false);
            DesactiverDataGridViewDvd(dgvDvdComListeCom, false);
            RendreBoutonsVisiblesOuInvisiblesDvd(btnDvdComModifier, btnDvdComSupprimer, btnDvdComAjouter, false);
        }

        private void TabCmdDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdComGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdComPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdComRayons);
            RemplirCmdDvdListeComplete();
            VisibleGroupBoxCommandeDvd();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            RemplirCmdDvdListeComplete();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            RemplirCmdDvdListeComplete();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            RemplirCmdDvdListeComplete();
        }
        private void txtCommandeDvdRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txtCommandeDvdRecherche.Text.Equals(""))
            {
                cbxDvdComGenres.SelectedIndex = -1;
                cbxDvdComRayons.SelectedIndex = -1;
                cbxDvdComPublics.SelectedIndex = -1;
                txtNumDoc.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txtCommandeDvdRecherche.Text.ToLower()));
                RemplirCmdDvdListe(lesDvdParTitre);
            }

            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdComGenres.SelectedIndex < 0 && cbxDvdComPublics.SelectedIndex < 0 && cbxDvdComRayons.SelectedIndex < 0
                    && txtNumDoc.Text.Equals(""))
                {
                    RemplirCmdDvdListeComplete();
                }
            }
        }
        private void btnRechercherComDvd_Click(object sender, EventArgs e)
        {
            if (!txtNumDoc.Text.Equals(""))
            {
                txtCommandeDvdRecherche.Text = "";
                cbxDvdComGenres.SelectedIndex = -1;
                cbxDvdComRayons.SelectedIndex = -1;
                cbxDvdComPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txtNumDoc.Text));
                if (dvd != null)
                {
                    List<Dvd> dvdList = new List<Dvd>() { dvd };
                    RemplirCmdDvdListe(dvdList);
                }

                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirCmdDvdListeComplete();
                }
            }

            else
            {
                RemplirCmdDvdListeComplete();
            }
        }
        private void dgvDvdComListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdComListe.CurrentRow != null)
            {
                // Récupère le dvd sélectionné
                Dvd dvdSelectionne = (Dvd)dgvDvdComListe.CurrentRow.DataBoundItem;

                // Utilise la méthode du contrôleur pour récupérer les commandes associées au dvd sélectionné
                List<CommandeDocument> commandesAssociees = controller.GetCommandesDvd(dvdSelectionne.Id);

                // Remplir la DataGridView avec les commandes récupérées
                RemplirCmdDvdCommandes(commandesAssociees);
            }
        }
        private void dgvDvdComListeCom_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdComListeCom.CurrentRow != null)
            {
                // Récupérer la commande sélectionnée dans la DataGridView
                var commande = (CommandeDocument)dgvDvdComListeCom.CurrentRow.DataBoundItem;
                // Appeler AfficheCommandeDvdInfo avec une seule commande
                AfficheCommandeDvdInfo(commande);
            }
        }
        private void btnDvdComModifier_Click(object sender, EventArgs e)
        {
            ModeDvd = "Modification";

            if (!VerifierSelectionDvd(dgvDvdComListeCom))
            {
                ModeDvd = "";
            }

            else
            {
                LabelCrudTitreDvd(ModeDvd, true);
                VisibleCommandeDvd();
                RendreBoutonsVisiblesOuInvisiblesDvd(btnDvdComModifier, btnDvdComSupprimer, btnDvdComAjouter, true);
                cbxDvdComEtat.Visible = true;
                DesactiverDataGridViewDvd(dgvDvdComListe, true);
                DesactiverDataGridViewDvd(dgvDvdComListeCom, true);
                // Vérifier si un élément a été sélectionné dans le ComboBox
                if (cbxDvdComEtat.SelectedItem != null)
                {
                    // Récupérer l'objet Suivi sélectionné
                    Suivi suiviSelectionne = (Suivi)cbxDvdComEtat.SelectedItem;

                    // Récupérer l'ID du suivi sélectionné
                    int idSuivi = suiviSelectionne.Id;
                    GererEtatComboBoxDvd(idSuivi);
                }
            }
        }
        private void btnDvdComSupprimer_Click(object sender, EventArgs e)
        {
            ModeDvd = "Suppression";
            if (!VerifierSelectionDvd(dgvDvdComListeCom))
            {
                ModeDvd = "";
            }

            else
            {
                LabelCrudTitreDvd(ModeDvd, true);
                RendreBoutonsVisiblesOuInvisiblesDvd(btnDvdComModifier, btnDvdComSupprimer, btnDvdComAjouter, true);
                VisibleCommandeDvd();
            }
        }
        private void btnDvdComAjouter_Click(object sender, EventArgs e)
        {
            ModeDvd = "Ajout";
            LabelCrudTitreDvd(ModeDvd, true);
            VisibleCommandeDvd();
            RendreBoutonsVisiblesOuInvisiblesDvd(btnDvdComModifier, btnDvdComSupprimer, btnDvdComAjouter, true);
            DesactiverDataGridViewDvd(dgvDvdComListe, true);
            DesactiverDataGridViewDvd(dgvDvdComListeCom, true);
            ViderCmdDvdInfos();
        }
        private void btnDvdComValider_Click(object sender, EventArgs e)
        {
            try
            {
                // Récupération des données communes
                DateTime dateCommande = dtpDvdComDateCommande.Value;
                double montant = double.TryParse(txbDvdComMontant.Text, out double tempMontant) ? tempMontant : throw new Exception("Montant invalide.");
                int nbExemplaires = int.TryParse(txbDvdComNbExemplaires.Text, out int tempNbExemplaires) ? tempNbExemplaires : throw new Exception("Nombre d'exemplaires invalide.");
                string numDvd = txbDvdComNumDvd.Text;

                CommandeDocument commandeDocument = null;

                // Vérifie le mode de DVD sélectionné et crée l'objet `CommandeDocument`
                switch (ModeDvd)
                {
                    case "Ajout":
                        // Valeurs spécifiques à l'ajout
                        commandeDocument = new CommandeDocument(null, dateCommande, montant, nbExemplaires, numDvd, 1, "En Cours");
                        ExecuterOperationDvd(() => controller.CreerCommandeDocument(commandeDocument), "Commande ajoutée avec succès.", "Erreur lors de l'ajout.");
                        ViderCmdDvdInfos();
                        CacherValiderDvd();
                        break;

                    case "Suppression":
                        // Vérification de la sélection dans le ComboBox
                        if (cbxDvdComEtat.SelectedItem == null)
                            throw new Exception("Veuillez sélectionner un état de suivi pour la suppression.");

                        Suivi suiviSelectionneSupp = (Suivi)cbxDvdComEtat.SelectedItem;
                        int idSuiviSupp = suiviSelectionneSupp.Id;
                        string etatSuiviSupp = suiviSelectionneSupp.Etat;

                        int idCommandeSupp = int.TryParse(txbDvdComNbCommande.Text, out int tempIdCommandeSupp) ? tempIdCommandeSupp : throw new Exception("ID commande invalide.");
                        commandeDocument = new CommandeDocument(idCommandeSupp, dateCommande, montant, nbExemplaires, numDvd, idSuiviSupp, etatSuiviSupp);
                        ExecuterOperationDvd(() => controller.SupprimerCommandeDocument(commandeDocument), "Commande supprimée avec succès.", "Erreur lors de la suppression.");
                        ViderCmdDvdInfos();
                        CacherValiderDvd();
                        break;

                    case "Modification":
                        // Vérification de la sélection dans le ComboBox
                        if (cbxDvdComEtat.SelectedItem == null)
                            throw new Exception("Veuillez sélectionner un état de suivi pour la modification.");

                        Suivi suiviSelectionneModif = (Suivi)cbxDvdComEtat.SelectedItem;
                        int idSuiviModif = suiviSelectionneModif.Id;
                        string etatSuiviModif = suiviSelectionneModif.Etat;

                        int idCommandeModif = int.TryParse(txbDvdComNbCommande.Text, out int tempIdCommandeModif) ? tempIdCommandeModif : throw new Exception("ID commande invalide.");
                        commandeDocument = new CommandeDocument(idCommandeModif, dateCommande, montant, nbExemplaires, numDvd, idSuiviModif, etatSuiviModif);
                        ExecuterOperationDvd(() => controller.ModifierCommandeDocument(commandeDocument), "Commande modifiée avec succès.", "Erreur lors de la modification.");
                        ViderCmdDvdInfos();
                        CacherValiderDvd();
                        break;

                    default:
                        MessageBox.Show("Veuillez sélectionner une opération.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Méthode pour exécuter une opération et afficher un message selon le résultat.
        /// </summary>

        private void ExecuterOperationDvd(Func<bool> operation, string messageSucces, string messageErreur)
        {
            try
            {
                bool result = operation();
                MessageBox.Show(result ? messageSucces : messageErreur, result ? "Succès" : "Erreur", MessageBoxButtons.OK, result ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur inattendue est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDvdComAnnuler_Click(object sender, EventArgs e)
        {
            LabelCrudTitreDvd(null, false);
            VisibleGroupBoxCommandeDvd();
            DesactiverDataGridViewDvd(dgvDvdComListe, false);
            DesactiverDataGridViewDvd(dgvDvdComListeCom, false);
            RendreBoutonsVisiblesOuInvisibles(btnDvdComModifier, btnDvdComSupprimer, btnDvdComAjouter, false);
        }

        #endregion

    }

}