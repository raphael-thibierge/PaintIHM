namespace MonPaint
{
    partial class Paramètres
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
            this.texteNoeud = new System.Windows.Forms.TextBox();
            this.labelTexte = new System.Windows.Forms.Label();
            this.labelEpaisseur = new System.Windows.Forms.Label();
            this.labelCouleur = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.epaisseur = new System.Windows.Forms.NumericUpDown();
            this.couleur = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.epaisseur)).BeginInit();
            this.SuspendLayout();
            // 
            // texteNoeud
            // 
            this.texteNoeud.Location = new System.Drawing.Point(307, 52);
            this.texteNoeud.Margin = new System.Windows.Forms.Padding(4);
            this.texteNoeud.Name = "texteNoeud";
            this.texteNoeud.Size = new System.Drawing.Size(132, 31);
            this.texteNoeud.TabIndex = 0;
            this.texteNoeud.TextChanged += new System.EventHandler(this.texte_TextChanged);
            // 
            // labelTexte
            // 
            this.labelTexte.AutoSize = true;
            this.labelTexte.Location = new System.Drawing.Point(104, 60);
            this.labelTexte.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTexte.Name = "labelTexte";
            this.labelTexte.Size = new System.Drawing.Size(66, 25);
            this.labelTexte.TabIndex = 1;
            this.labelTexte.Text = "Texte";
            // 
            // labelEpaisseur
            // 
            this.labelEpaisseur.AutoSize = true;
            this.labelEpaisseur.Location = new System.Drawing.Point(104, 138);
            this.labelEpaisseur.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEpaisseur.Name = "labelEpaisseur";
            this.labelEpaisseur.Size = new System.Drawing.Size(108, 25);
            this.labelEpaisseur.TabIndex = 2;
            this.labelEpaisseur.Text = "Epaisseur";
            this.labelEpaisseur.Click += new System.EventHandler(this.label2_Click);
            // 
            // labelCouleur
            // 
            this.labelCouleur.AutoSize = true;
            this.labelCouleur.Location = new System.Drawing.Point(104, 210);
            this.labelCouleur.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCouleur.Name = "labelCouleur";
            this.labelCouleur.Size = new System.Drawing.Size(87, 25);
            this.labelCouleur.TabIndex = 3;
            this.labelCouleur.Text = "Couleur";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(533, 60);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(147, 48);
            this.button1.TabIndex = 4;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(533, 162);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(147, 48);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // epaisseur
            // 
            this.epaisseur.Location = new System.Drawing.Point(307, 138);
            this.epaisseur.Margin = new System.Windows.Forms.Padding(4);
            this.epaisseur.Name = "epaisseur";
            this.epaisseur.Size = new System.Drawing.Size(160, 31);
            this.epaisseur.TabIndex = 6;
            this.epaisseur.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // couleur
            // 
            this.couleur.AutoSize = true;
            this.couleur.BackColor = System.Drawing.Color.Red;
            this.couleur.Location = new System.Drawing.Point(307, 210);
            this.couleur.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.couleur.Name = "couleur";
            this.couleur.Size = new System.Drawing.Size(36, 25);
            this.couleur.TabIndex = 7;
            this.couleur.Text = "    ";
            this.couleur.Click += new System.EventHandler(this.couleur_Click);
            // 
            // Paramètres
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 299);
            this.Controls.Add(this.couleur);
            this.Controls.Add(this.epaisseur);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelCouleur);
            this.Controls.Add(this.labelEpaisseur);
            this.Controls.Add(this.labelTexte);
            this.Controls.Add(this.texteNoeud);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Paramètres";
            this.Text = "Paramètres";
            this.Load += new System.EventHandler(this.Paramètres_Load);
            ((System.ComponentModel.ISupportInitialize)(this.epaisseur)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox texteNoeud;
        private System.Windows.Forms.Label labelTexte;
        private System.Windows.Forms.Label labelEpaisseur;
        private System.Windows.Forms.Label labelCouleur;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown epaisseur;
        private System.Windows.Forms.Label couleur;
    }
}