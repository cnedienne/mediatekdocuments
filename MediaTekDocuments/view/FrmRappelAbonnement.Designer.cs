
namespace MediaTekDocuments.view
{
    partial class FrmRappelAbonnement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvRappelRevue = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFermerRappel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRappelRevue)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRappelRevue
            // 
            this.dgvRappelRevue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRappelRevue.Location = new System.Drawing.Point(15, 51);
            this.dgvRappelRevue.Name = "dgvRappelRevue";
            this.dgvRappelRevue.RowHeadersWidth = 51;
            this.dgvRappelRevue.Size = new System.Drawing.Size(442, 345);
            this.dgvRappelRevue.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(394, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ces abonnemets arrivent à expriation dans moins de 30 jours";
            // 
            // btnFermerRappel
            // 
            this.btnFermerRappel.Location = new System.Drawing.Point(382, 415);
            this.btnFermerRappel.Name = "btnFermerRappel";
            this.btnFermerRappel.Size = new System.Drawing.Size(75, 23);
            this.btnFermerRappel.TabIndex = 2;
            this.btnFermerRappel.Text = "Fermer";
            this.btnFermerRappel.UseVisualStyleBackColor = true;
            this.btnFermerRappel.Click += new System.EventHandler(this.btnFermerRappel_Click);
            // 
            // FrmRappelAbonnement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 450);
            this.Controls.Add(this.btnFermerRappel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvRappelRevue);
            this.Name = "FrmRappelAbonnement";
            this.Text = "Rappel Abonnement";
            ((System.ComponentModel.ISupportInitialize)(this.dgvRappelRevue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            this.Load += new System.EventHandler(this.FrmRappelAbonnement_Load);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRappelRevue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFermerRappel;
    }
}