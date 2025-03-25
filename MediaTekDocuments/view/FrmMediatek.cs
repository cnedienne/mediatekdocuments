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
        /// Ouvre la fenêtre qui affiche la liste des abonnemnts qui expirent dans moins de 30 jours
        /// </summary>
        private void tabOngletsApplication_Enter(object sender, EventArgs e)
        {
            FrmRappelAbonnement frmAlerte = new FrmRappelAbonnement();
            frmAlerte.ShowDialog();
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

        #region Onglet Paarutions
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

        #region Onglet Commande de Livres

        private readonly BindingSource bdgLivresComListe = new BindingSource();

        private List<CommandeDocument> lesCommandesLivres = new List<CommandeDocument>();

        /// <summary>
        /// Ouverture de l'onglet Commande de Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresComNumRecherche_Click(Object sender, EventArgs e)
        {
            if (!txbLivresComNumRecherche.Text.Equals(""))
            {
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresComNumRecherche.Text));
                lesCommandesLivres = controller.GetAllCommandes(txbLivresComNumRecherche.Text);
                if (livre != null)
                {
                    AfficherInfosLivresCommande(livre);
                    RemplirLivresListeCom(lesCommandesLivres);
                    MessageBox.Show("Nombre de commandes chargées : " + lesCommandesLivres.Count);
                }
                else
                {
                    MessageBox.Show("Le numéro est introuvable");
                }
            }
        }


        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficherInfosLivresCommande(Livre livre)
        {
            txbLivresComISBN.Text = livre.Isbn;
            txbLivresComTitre.Text = livre.Titre;
            txbLivresComAuteur.Text = livre.Auteur;
            txbLivresComCollection.Text = livre.Collection;
            txbLivresComGenre.Text = livre.Genre;
            txbLivresComPublic.Text = livre.Public;
            txbLivresComRayon.Text = livre.Rayon;
        }

        private void ViderInfosLivresCom()
        {
            txbLivresComISBN.Text = "";
            txbLivresComTitre.Text = "";
            txbLivresComAuteur.Text = "";
            txbLivresComCollection.Text = "";
            txbLivresComGenre.Text = "";
            txbLivresComPublic.Text = "";
            txbLivresComRayon.Text = "";
        }

        /// <summary>
        /// Ouverture de l'onglet Commande de Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemplirLivresListeCom(List<CommandeDocument> lesCommandesDocument)
        {
            if (lesCommandesDocument != null)
            {
                bdgLivresComListe.DataSource = lesCommandesDocument;
                dgvLivresComListe.DataSource = bdgLivresComListe;
                dgvLivresComListe.Columns["id"].Visible = false;
                dgvLivresComListe.Columns["idLivreDvd"].Visible = false;
                dgvLivresComListe.Columns["idSuivi"].Visible = false;
                dgvLivresComListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                bdgLivresComListe.ResetBindings(false);
                dgvLivresComListe.Refresh();
            }
            else
            {
                bdgLivresComListe.DataSource = null;
                bdgLivresComListe.ResetBindings(false);
                dgvLivresComListe.Refresh();
            }
        }
        /*
        private void txbLivresComNumRecherche_TextChanged(object sender, EventArgs e)
        {
            RemplirLivresListeCom(null);
            ViderInfosLivresCom();
        }*/


        /// <summary>
        /// Ouverture de l'onglet Commande de Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivresCom_Enter(object sender, EventArgs e)
        {
            RemplirLivresListeCom(null);
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresComListeCom_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresComListe.CurrentCell != null)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgLivresComListe.List[bdgLivresComListe.Position];
                AfficherCommandeLivresInfos(commandeDocument);
                ViderInfosSuivi();
            }
        }

        private void AfficherCommandeLivresInfos(CommandeDocument commande)
        {
            txbNumCommande.Text = commande.id;
            txbNbExemplaire.Text = commande.nbExemplaire.ToString();
            txbMontant.Text = commande.montant.ToString();
            dtpDateCommande.Value = commande.dateCommande;
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresComListeCom_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvLivresComListe.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "dateCommande":
                    sortedList = lesCommandesLivres.OrderBy(o => o.dateCommande).Reverse().ToList();
                    break;
                case "montant":
                    sortedList = lesCommandesLivres.OrderBy(o => o.montant).Reverse().ToList();
                    break;
                case "nbExemplaire":
                    sortedList = lesCommandesLivres.OrderBy(o => o.nbExemplaire).ToList();
                    break;
                case "etapeSuivi":
                    sortedList = lesCommandesLivres.OrderBy(o => o.etapeSuivi).ToList();
                    break;
            }
            RemplirLivresListeCom(sortedList);

        }

        private void ViderInfosCommandes()
        {
            txbNumCommande.Text = "";
            txbNbExemplaire.Text = "";
            txbMontant.Text = "";
            dtpDateCommande.Value = DateTime.Now;
        }

        private void btnAjouterCommande_Click(object sender, EventArgs e)
        {
            ViderInfosCommandes();
            btnValiderCommande.Visible = true;
            btnAnnulerCommande.Visible = true;
            ViderInfosSuivi();
        }


        private void btnSupprimerCommande_Click(object sender, EventArgs e)
        {
            CommandeDocument commande = (CommandeDocument)bdgLivresComListe.List[bdgLivresComListe.Position];
            if ((commande.etapeSuivi == "en cours" || commande.etapeSuivi == "relancée")
                && MessageBox.Show("Voulez-vous vraiment supprimer la commande numéro " + commande.id + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                try
                {
                    controller.SupprimerCommande(commande.id);
                    lesCommandesLivres = controller.GetAllCommandes(txbLivresComNumRecherche.Text);
                    RemplirLivresListeCom(lesCommandesLivres);
                    dgvLivresComListe.Refresh();

                }
                catch
                {
                    MessageBox.Show("Erreur", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Cette commande ne peut pas être supprimée", "Erreur");
            }
        }

        private void btnValiderCommande_Click(object sender, EventArgs e)
        {
            if (!txbNumCommande.Text.Equals("") && !txbNbExemplaire.Text.Equals("") && !txbMontant.Text.Equals(""))
            {
                try
                {
                    string id = txbNumCommande.Text;
                    int nbExemplaire = int.Parse(txbNbExemplaire.Text);
                    double montant = double.Parse(txbMontant.Text);
                    DateTime dateCommande = dtpDateCommande.Value;
                    string idLivreDvd = txbLivresComNumRecherche.Text;
                    string idSuivi = "01";
                    Commande commande = new Commande(id, dateCommande, montant);

                    if (controller.CreerCommande(commande))
                    {
                        controller.CreerCommandeDocument(id, nbExemplaire, idLivreDvd, idSuivi);
                        lesCommandesLivres = controller.GetAllCommandes(txbLivresComNumRecherche.Text);
                        RemplirLivresListeCom(lesCommandesLivres);
                        MessageBox.Show("La commande " + id + " a bien été enregistrée pour le livre : " + txbLivresComTitre.Text, "Information");
                        dgvLivresComListe.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("le numéro de commande existe déjà", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("les informations saisies ne sont pas corrects", "Information");
                    ViderInfosLivresCom();
                    txbNumCommande.Focus();
                }
            }
            else
            {
                MessageBox.Show("tous les champs sont obligatoires", "Information");
            }
        }

        private void btnAnnulerCommande_Click(object sender, EventArgs e)
        {
            ViderInfosCommandes();
            btnAnnulerCommande.Visible = false;
            btnValiderCommande.Visible = false;
        }

        /*                  SUIVI COMMANDE              */

        private void btnSuiviCommande_Click(object sender, EventArgs e)
        {
            groupBox3.Enabled = true;
            CommandeDocument commande = (CommandeDocument)bdgLivresComListe.List[bdgLivresComListe.Position];
            txbEtapeSuivi.Text = commande.etapeSuivi;

            cbxEtapeSuivi.Items.Clear();
            switch (commande.etapeSuivi)
            {
                case "en cours":
                    cbxEtapeSuivi.Text = "";
                    cbxEtapeSuivi.Items.Add("relancée");
                    cbxEtapeSuivi.Items.Add("livrée");
                    break;
                case "relancée":
                    cbxEtapeSuivi.Text = "";
                    cbxEtapeSuivi.Items.Add("en cours");
                    cbxEtapeSuivi.Items.Add("livrée");
                    break;
                case "livrée":
                    cbxEtapeSuivi.Text = "";
                    cbxEtapeSuivi.Items.Add("réglée");
                    break;

            }
        }

        private void ViderInfosSuivi()
        {
            txbEtapeSuivi.Text = "";
            btnValiderSuivi.Visible = false;
            cbxEtapeSuivi.Visible = false;
            groupBox3.Enabled = false;
        }

        private void btnAnnulerSuivi_Click(object sender, EventArgs e)
        {
            ViderInfosSuivi();
        }

        private void btnModifierSuiviCommande_Click(object sender, EventArgs e)
        {
            cbxEtapeSuivi.Visible = true;
            btnValiderSuivi.Visible = true;
        }

        // Convertir idSuivi avec l'ID
        private string ConvertitIdSuivi(string etapeSuivi)
        {
            string idSuivi;
            switch (etapeSuivi)
            {
                case "en cours":
                    idSuivi = "01";
                    break;
                case "relancée":
                    idSuivi = "02";
                    break;
                case "livrée":
                    idSuivi = "03";
                    break;
                case "réglée":
                    idSuivi = "04";
                    break;
                default:
                    idSuivi = "";
                    break;
            }
            return idSuivi;
        }

        private void btnValiderSuivi_Click(object sender, EventArgs e)
        {
            if (!cbxEtapeSuivi.Text.Equals(""))
            {
                try
                {
                    string idSuivi = ConvertitIdSuivi(cbxEtapeSuivi.Text);
                    CommandeDocument commande = (CommandeDocument)bdgLivresComListe.List[bdgLivresComListe.Position];
                    controller.ModifierSuiviCommandeDocument(commande.id, idSuivi);
                    lesCommandesLivres = controller.GetAllCommandes(txbNumCommande.Text);
                    RemplirLivresListeCom(lesCommandesLivres);
                    dgvLivresComListe.Refresh();
                    btnValiderCommande.Visible = false;
                    btnAnnulerSuivi.Visible = false;
                    cbxEtapeSuivi.Visible = false;
                }
                catch
                {
                    MessageBox.Show("Erreur", "Erreur");
                }

            }
            else
            {
                MessageBox.Show("Veuillez saisir une nouvelle étape de suivi", "Erreur");
            }

        }
        #endregion Onglet Commande de Livres


        #region Onglet Commande de DVD

        private readonly BindingSource bdgDvdsComListe = new BindingSource();

        private List<CommandeDocument> lesCommandesDvds = new List<CommandeDocument>();

        /// <summary>
        /// Ouverture de l'onglet Commande de DVDs : 
        /// appel des méthodes pour remplir le datagrid des DVDs et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDvdsComNumRecherche_Click(Object sender, EventArgs e)
        {
            if (!txbDvdsComNumRecherche.Text.Equals(""))
            {
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdsComNumRecherche.Text));
                lesCommandesDvds = controller.GetAllCommandes(txbDvdsComNumRecherche.Text);
                if (dvd != null)
                {
                    AfficherInfosDvdsCommande(dvd);
                    RemplirDvdsListeCom(lesCommandesDvds);
                    MessageBox.Show("Nombre de commandes chargées : " + lesCommandesDvds.Count);
                }
                else
                {
                    MessageBox.Show("Le numéro est introuvable");
                }
            }
        }

        /// <summary>
        /// Affichage des informations du DVD sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficherInfosDvdsCommande(Dvd dvd)
        {
            txbDvdsComRealisateur.Text = dvd.Realisateur;
            txbDvdsComTitre.Text = dvd.Titre;
            txbDvdsComNumero.Text = dvd.Id;
            txbDvdsComDuree.Text = dvd.Duree.ToString();
            txbDvdsComGenre.Text = dvd.Genre;
            txbDvdsComPublic.Text = dvd.Public;
            txbDvdsComRayon.Text = dvd.Rayon;
        }

        private void ViderInfosDvdsCom()
        {
            txbDvdsComRealisateur.Text = "";
            txbDvdsComTitre.Text = "";
            txbDvdsComNumero.Text = "";
            txbDvdsComDuree.Text = "";
            txbDvdsComGenre.Text = "";
            txbDvdsComPublic.Text = "";
            txbDvdsComRayon.Text = "";
        }

        /// <summary>
        /// Ouverture de l'onglet Commande de DVDs : 
        /// appel des méthodes pour remplir le datagrid des DVDs et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemplirDvdsListeCom(List<CommandeDocument> lesCommandesDocument)
        {
            if (lesCommandesDocument != null)
            {
                bdgDvdsComListe.DataSource = lesCommandesDocument;
                dgvDvdsComListe.DataSource = bdgDvdsComListe;
                dgvDvdsComListe.Columns["id"].Visible = false;
                dgvDvdsComListe.Columns["idLivreDvd"].Visible = false;
                dgvDvdsComListe.Columns["idSuivi"].Visible = false;
                dgvDvdsComListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                bdgDvdsComListe.ResetBindings(false);
                dgvDvdsComListe.Refresh();
            }
            else
            {
                bdgDvdsComListe.DataSource = null;
                bdgDvdsComListe.ResetBindings(false);
                dgvDvdsComListe.Refresh();
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvDvdsComListeCom_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdsComListe.CurrentCell != null)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgDvdsComListe.List[bdgDvdsComListe.Position];
                AfficherCommandeDvdsInfos(commandeDocument);
                ViderInfosSuiviDvds();
            }
        }

        private void AfficherCommandeDvdsInfos(CommandeDocument commande)
        {
            txbNumCommandeDvds.Text = commande.id;
            txbNbExemplaireDvds.Text = commande.nbExemplaire.ToString();
            txbMontantDvds.Text = commande.montant.ToString();
            dtpDateCommandeDvds.Value = commande.dateCommande;
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvDvdsComListeCom_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvDvdsComListe.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "dateCommande":
                    sortedList = lesCommandesDvds.OrderBy(o => o.dateCommande).Reverse().ToList();
                    break;
                case "montant":
                    sortedList = lesCommandesDvds.OrderBy(o => o.montant).Reverse().ToList();
                    break;
                case "nbExemplaire":
                    sortedList = lesCommandesDvds.OrderBy(o => o.nbExemplaire).ToList();
                    break;
                case "etapeSuivi":
                    sortedList = lesCommandesDvds.OrderBy(o => o.etapeSuivi).ToList();
                    break;
            }
            RemplirDvdsListeCom(sortedList);
        }


        private void btnValiderCommandeDvds_Click(object sender, EventArgs e)
        {
            if (!txbNumCommandeDvds.Text.Equals("") && !txbNbExemplaireDvds.Text.Equals("") && !txbMontantDvds.Text.Equals(""))
            {
                try
                {
                    string id = txbNumCommandeDvds.Text;
                    int nbExemplaire = int.Parse(txbNbExemplaireDvds.Text);
                    double montant = double.Parse(txbMontantDvds.Text);
                    DateTime dateCommande = dtpDateCommandeDvds.Value;
                    string idLivreDvd = txbDvdsComNumRecherche.Text;
                    string idSuivi = "01";
                    Commande commande = new Commande(id, dateCommande, montant);

                    if (controller.CreerCommande(commande))
                    {
                        controller.CreerCommandeDocument(id, nbExemplaire, idLivreDvd, idSuivi);
                        lesCommandesDvds = controller.GetAllCommandes(txbDvdsComNumRecherche.Text);
                        RemplirDvdsListeCom(lesCommandesDvds);
                        MessageBox.Show("La commande " + id + " a bien été enregistrée pour le DVD : " + txbDvdsComTitre.Text, "Information");
                        dgvDvdsComListe.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("le numéro de commande existe déjà", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("les informations saisies ne sont pas corrects", "Information");
                    ViderInfosDvdsCom();
                    txbNumCommande.Focus();
                }
            }
            else
            {
                MessageBox.Show("tous les champs sont obligatoires", "Information");
            }
        }

        private void TabDvdsCom_Enter(object sender, EventArgs e)
        {
            RemplirDvdsListeCom(null);
        }


        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void ViderInfosCommandesDvds()
        {
            txbNumCommandeDvds.Text = "";
            txbNbExemplaireDvds.Text = "";
            txbMontantDvds.Text = "";
            dtpDateCommandeDvds.Value = DateTime.Now;
        }

        private void btnAjouterCommandeDvds_Click(object sender, EventArgs e)
        {
            ViderInfosCommandesDvds();
            btnValiderCommandeDvds.Visible = true;
            btnAnnulerCommandeDvds.Visible = true;
            ViderInfosSuiviDvds();
        }

        private void btnSupprimerCommandeDvds_Click(object sender, EventArgs e)
        {
            CommandeDocument commande = (CommandeDocument)bdgDvdsComListe.List[bdgDvdsComListe.Position];
            if ((commande.etapeSuivi == "en cours" || commande.etapeSuivi == "relancée")
                && MessageBox.Show("Voulez-vous vraiment supprimer la commande numéro " + commande.id + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    controller.SupprimerCommande(commande.id);
                    lesCommandesDvds = controller.GetAllCommandes(txbDvdsComNumRecherche.Text);
                    RemplirDvdsListeCom(lesCommandesDvds);
                    dgvDvdsComListe.Refresh();
                }
                catch
                {
                    MessageBox.Show("Erreur", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Cette commande ne peut pas être supprimée", "Erreur");
            }
        }



        private void btnAnnulerCommandeDvds_Click(object sender, EventArgs e)
        {
            ViderInfosCommandesDvds();
            btnAnnulerCommandeDvds.Visible = false;
            btnValiderCommandeDvds.Visible = false;
        }

        /*                  SUIVI COMMANDE              */

        private void btnSuiviCommandeDvds_Click(object sender, EventArgs e)
        {
            groupBox3.Enabled = true;
            CommandeDocument commande = (CommandeDocument)bdgDvdsComListe.List[bdgDvdsComListe.Position];
            txbEtapeSuiviDvds.Text = commande.etapeSuivi;

            cbxEtapeSuiviDvds.Items.Clear();
            switch (commande.etapeSuivi)
            {
                case "en cours":
                    cbxEtapeSuiviDvds.Text = "";
                    cbxEtapeSuiviDvds.Items.Add("relancée");
                    cbxEtapeSuiviDvds.Items.Add("livrée");
                    break;
                case "relancée":
                    cbxEtapeSuiviDvds.Text = "";
                    cbxEtapeSuiviDvds.Items.Add("en cours");
                    cbxEtapeSuiviDvds.Items.Add("livrée");
                    break;
                case "livrée":
                    cbxEtapeSuiviDvds.Text = "";
                    cbxEtapeSuiviDvds.Items.Add("réglée");
                    break;
            }
        }

        private void ViderInfosSuiviDvds()
        {
            txbEtapeSuiviDvds.Text = "";
            btnValiderSuiviDvds.Visible = false;
            cbxEtapeSuiviDvds.Visible = false;
            groupBox3.Enabled = false;
        }

        private void btnAnnulerSuiviDvds_Click(object sender, EventArgs e)
        {
            ViderInfosSuiviDvds();
        }

        private void btnModifierSuiviCommandeDvds_Click(object sender, EventArgs e)
        {
            cbxEtapeSuiviDvds.Visible = true;
            btnValiderSuiviDvds.Visible = true;
        }

        private string ConvertitIdSuiviDvds(string etapeSuivi)
        {
            string idSuivi;
            switch (etapeSuivi)
            {
                case "en cours":
                    idSuivi = "01";
                    break;
                case "relancée":
                    idSuivi = "02";
                    break;
                case "livrée":
                    idSuivi = "03";
                    break;
                case "réglée":
                    idSuivi = "04";
                    break;
                default:
                    idSuivi = "";
                    break;
            }
            return idSuivi;
        }

        private void btnValiderSuiviDvds_Click(object sender, EventArgs e)
        {
            if (!cbxEtapeSuiviDvds.Text.Equals(""))
            {
                try
                {
                    string idSuivi = ConvertitIdSuiviDvds(cbxEtapeSuiviDvds.Text);
                    CommandeDocument commande = (CommandeDocument)bdgDvdsComListe.List[bdgDvdsComListe.Position];
                    controller.ModifierSuiviCommandeDocument(commande.id, idSuivi);
                    lesCommandesDvds = controller.GetAllCommandes(txbNumCommande.Text);
                    RemplirDvdsListeCom(lesCommandesDvds);
                    dgvDvdsComListe.Refresh();
                    btnValiderCommandeDvds.Visible = false;
                    btnAnnulerSuiviDvds.Visible = false;
                    cbxEtapeSuiviDvds.Visible = false;
                }
                catch
                {
                    MessageBox.Show("Erreur", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Veuillez saisir une nouvelle étape de suivi", "Erreur");
            }
        }



        #endregion Onglet Commande de DVD



        #region Onglet Commande de Revue

        private readonly BindingSource bdgRevuesComListe = new BindingSource();

        private List<Abonnement> lesAbonnementsRevues = new List<Abonnement>();

        /// <summary>
        /// Ouverture de l'onglet Commande de Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        private void BtnRevuesComNumRecherche_Click(Object sender, EventArgs e)
        {
            if (!txbRevuesComNumRecherche.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesComNumRecherche.Text));
                lesAbonnementsRevues = controller.GetAllAbonnementsRevues(txbRevuesComNumRecherche.Text);
                Console.WriteLine(lesAbonnementsRevues);
                if (revue != null)
                {
                    AfficherInfosRevuesCommande(revue);
                    RemplirRevuesListeCom(lesAbonnementsRevues);
                    MessageBox.Show("Nombre de commandes chargées : " + lesAbonnementsRevues.Count);
                }
                else
                {
                    MessageBox.Show("Le numéro est introuvable");
                }
            }
        }

        private void AfficherInfosRevuesCommande(Revue revue)
        {
            txbRevuesComDelai.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesComTitre.Text = revue.Titre;
            txbRevuesComNumero.Text = revue.Id;
            txbRevuesComPeriodicite.Text = revue.Periodicite;
            txbRevuesComGenre.Text = revue.Genre;
            txbRevuesComPublic.Text = revue.Public;
            txbRevuesComRayon.Text = revue.Rayon;
        }

        private void ViderInfosRevuesCom()
        {
            txbRevuesComDelai.Text = "";
            txbRevuesComTitre.Text = "";
            txbRevuesComNumero.Text = "";
            txbRevuesComPeriodicite.Text = "";
            txbRevuesComGenre.Text = "";
            txbRevuesComPublic.Text = "";
            txbRevuesComRayon.Text = "";
        }

        private void RemplirRevuesListeCom(List<Abonnement> lesAbonnementsRevues)
        {
            if (lesAbonnementsRevues != null)
            {
                bdgRevuesComListe.DataSource = lesAbonnementsRevues;
                dgvRevuesComListe.DataSource = bdgRevuesComListe;
                dgvRevuesComListe.Columns["id"].Visible = false;
                dgvRevuesComListe.Columns["titre"].Visible = false;
                dgvRevuesComListe.Columns["idRevue"].Visible = false;
                dgvRevuesComListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                bdgRevuesComListe.ResetBindings(false);
                dgvRevuesComListe.Refresh();
            }
            else
            {
                bdgRevuesComListe.DataSource = null;
                bdgRevuesComListe.ResetBindings(false);
                dgvRevuesComListe.Refresh();
            }
        }

        private void DgvRevuesComListeCom_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesComListe.CurrentCell != null)
            {
                Abonnement Revues = (Abonnement)bdgRevuesComListe.List[bdgRevuesComListe.Position];
                AfficherAbonnementsRevuesInfos(Revues);
                ViderInfosAboRevues();
            }
        }

        private void AfficherAbonnementsRevuesInfos(Abonnement Revues)
        {
            txbNumCommandeRevues.Text = Revues.id;
            dtpFinRevues.Value= Revues.dateFinAbonnement;
            txbMontantRevues.Text = Revues.montant.ToString();
            dtpDateCommandeRevues.Value = Revues.dateCommande;
        }

        private void DgvRevuesComListeCom_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvRevuesComListe.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "dateCommande":
                    sortedList = lesAbonnementsRevues.OrderBy(o => o.dateCommande).Reverse().ToList();
                    break;
                case "montant":
                    sortedList = lesAbonnementsRevues.OrderBy(o => o.montant).Reverse().ToList();
                    break;
                case "dateFinAbonnement":
                    sortedList = lesAbonnementsRevues.OrderBy(o => o.dateFinAbonnement).Reverse().ToList();
                    break;
            }
            RemplirRevuesListeCom(sortedList);
        }

        private void btnValiderAboRevues_Click(object sender, EventArgs e)
        {
            if (!txbNumCommandeRevues.Text.Equals("") && !txbMontantRevues.Text.Equals(""))
            {
                try
                {
                    string id = txbNumCommandeRevues.Text;
                    double montant = double.Parse(txbMontantRevues.Text);
                    DateTime dateCommande = dtpDateCommandeRevues.Value;
                    Commande commande = new Commande(id, dateCommande, montant);
                    string idRevue = txbRevuesComNumRecherche.Text;
                    DateTime dateFinAbonnement = dtpFinRevues.Value;

                    if (controller.CreerCommande(commande) && controller.CreerAbonnement(id, dateFinAbonnement, idRevue))
                    {
                        lesAbonnementsRevues = controller.GetAllAbonnementsRevues(txbRevuesComNumRecherche.Text);
                        RemplirRevuesListeCom(lesAbonnementsRevues);
                        MessageBox.Show("L'abonnement " + id + " a bien été enregistrée pour la revue : " + txbRevuesComTitre.Text, "Information");
                        dgvRevuesComListe.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("le numéro d'abonnement existe déjà", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("Les informations saisies ne sont pas correctes", "Information");
                    ViderInfosRevuesCom();
                    txbNumCommande.Focus();
                }
            }
            else
            {
                MessageBox.Show("Tous les champs sont obligatoires", "Information");
            }
        }

        private void TabRevuesCom_Enter(object sender, EventArgs e)
        {
            RemplirRevuesListeCom(null);
        }

        private void btnAnnulerAboRevues_Click(object sender, EventArgs e)
        {
            ViderInfosAboRevues();
            btnAnnulerAboRevues.Visible = false;
            btnValiderAboRevues.Visible = false;
        }

        /// <summary>
        /// Annulation des informations
        /// </summary>
        private void ViderInfosAboRevues()
        {
            txbNumCommandeRevues.Text = "";
            txbMontantRevues.Text = "";
            dtpDateCommandeRevues.Value = DateTime.Now;
            dtpFinRevues.Value = DateTime.Now;
        }


        private void btnAjoutAboRevues_Click(object sender, EventArgs e)
        {
            ViderInfosAboRevues();
            btnValiderAboRevues.Visible = true;
        }

        private void btnSupprimerAboRevues_Click(object sender, EventArgs e)
        {
            Abonnement abonnement = (Abonnement)bdgRevuesComListe.List[bdgRevuesComListe.Position];
            DateTime dateFinAbonnement = abonnement.dateFinAbonnement;
            DateTime dateCommande = abonnement.dateCommande;
            lesExemplaires = controller.GetExemplairesRevue(txbRevuesComNumRecherche.Text);

            if (MessageBox.Show("Voulez-vous vraiment supprimer la commande numéro " + abonnement.id + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes
                && Exemplaire(lesExemplaires, dateFinAbonnement, dateCommande))
            {
                try
                {
                    controller.SupprimerAbonnement(abonnement.id);
                    lesAbonnementsRevues = controller.GetAllAbonnementsRevues(txbRevuesComNumRecherche.Text);
                    RemplirRevuesListeCom(lesAbonnementsRevues);

                }
                catch
                {
                    MessageBox.Show("Erreur", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Cet abonnement ne peut pas être supprimé car il contient des exemplaires", "Erreur");
            }
        }

        private bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFinAbonnement;
        }

        private bool Exemplaire(List<Exemplaire> lesExemplaires, DateTime dateFinAbonnement, DateTime dateCommande)
        {
            foreach (var exemplaire in lesExemplaires)
            {
                if (ParutionDansAbonnement(dateCommande, dateFinAbonnement, exemplaire.DateAchat))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion Onglet Commande de Revue

        #region Fenetre Authentification

        /// <summary>
        /// Empêche l'accès aux onglets en fonction de l'utilisateur
        /// </summary>
        public void EmpecherAcces()
        {
            tabOngletsApplication.TabPages.Remove(tabPage1);
            tabOngletsApplication.TabPages.Remove(tabPage2);
            tabOngletsApplication.TabPages.Remove(tabPage3);
            tabOngletsApplication.TabPages.Remove(tabReceptionRevue);
            tabOngletsApplication.Enter -= tabOngletsApplication_Enter;
        }

        #endregion Fenêtre Authentifiation
    }

}

