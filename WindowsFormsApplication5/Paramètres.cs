using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonPaint
{
    public partial class Paramètres : Form
    {

        public Paramètres()
        {
            InitializeComponent();
        }

        public int Epaisseur
        {
            get { return (int)epaisseur.Value; }
            set { epaisseur.Value = value; }
        }

        public Color Couleur
        {
            get { return couleur.BackColor; }
            set { couleur.BackColor = value; }
        }

        public string Texte
        {
            get { return texteNoeud.Text; }
            set { texteNoeud.Text = value; }
        }


        private void Paramètres_Load(object sender, EventArgs e)
        {

        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void couleur_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            c.Color = couleur.BackColor;
            if (c.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                couleur.BackColor = c.Color;
        }

        private void texte_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
