using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MonPaint
{
    [Serializable] class Trait : ISupprimable
    {
        private Noeud source;
        private Noeud destination;
        private Color couleur;
        private int epaisseur;
        private string texte;
        private int idNoeudSource ;
        private int idNoeudDestination;
        private bool supprimé = false;

        #region constructeurs
        public Trait (Noeud s, Noeud d, Color c, int e)
        { // constructeur paramétré de la classe Trait
            source = s;
            destination = d;
            couleur = c;
            epaisseur = e;
            texte = "";            
        }
        public Trait(XmlNode xNN, List<Noeud> collectionNoeud)
        { // constructeur depuis un fichier XML
            epaisseur = 1;
            couleur = Color.Black;

            foreach(XmlNode xNNN in xNN.ChildNodes)
            {
                switch (xNNN.Name)
                {
                    case "EPAISSEUR" :
                        epaisseur = int.Parse(xNNN.InnerText);
                        break;
                    case "COULEUR" :
                        int c = int.Parse(xNNN.InnerText);
                        couleur = Color.FromArgb(c);
                        break;
                    case "SOURCE" :
                        foreach (Noeud n in collectionNoeud)
                        {
                            if (n.Id == int.Parse(xNNN.InnerText))
                                source = n;
                        }
                        break;
                    case "DESTINATION":
                        foreach (Noeud n in collectionNoeud)
                        {
                            if (n.Id == int.Parse(xNNN.InnerText))
                                destination = n;
                        }
                        break;

                    case "TEXTE" :
                        texte = xNNN.InnerText;
                        break;

                }
            }
        }
        #endregion

        #region Accesseurs
        internal Noeud Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public Color Couleur
        {
            get { return couleur; }
            set { couleur = value; }
        }

        public int Epaisseur
        {
            get { return epaisseur; }
            set { epaisseur = value; }
        }

        internal Noeud Source
        {
            get { return source; }
            set { source = value; }
        }
        internal Noeud Destination1
        {
            get { return destination; }
            set { destination = value; }
        }

        public string Texte
        {
            get { return texte; }
            set { texte = value; }
        }
        #endregion

        #region fonctions
        public void Dessine(Graphics g)
        { // fonction qui dessine un trait
            if (!supprimé)
            {


                Pen p = new Pen(couleur, epaisseur);
                if (source != null && destination != null)
                    g.DrawLine(p, source.Centre(), destination.Centre());
                if (texte != "")
                    g.DrawString(texte, new Font("Arial", 12), Brushes.Red, new Point((destination.Centre().X + source.Centre().X) / 2, (destination.Centre().Y + source.Centre().Y) / 2));
            }
        }

        public bool ancre(Noeud n)
        { // renvoi vrai si le noeud passé en paramètre est une des ancres du trait
            if (n == source || n == destination)
                return true;
            return false;
        }

        public bool Contains(Point p)
        { // renvoi vrai si le point p se trouve sur le trait
            float coefDirecteur = (destination.Centre().Y - source.Centre().Y) / (destination.Centre().X - source.Centre().X);
            float ordonneeOrigine = source.Centre().Y - (source.Centre().Y * coefDirecteur);
            
            float pTheorique = (coefDirecteur * p.X) + ordonneeOrigine ;
            if (p.Y + 1 >= (int) pTheorique && p.Y -1 <= (int) pTheorique)
            {
                  return true;
            }

            if (p.X == 0)
                return true;
            return false;
        }

        public string ToXML()
        { // renvoi une chaine de caractère du trait au format XML
            string text = "<TRAIT>";
            text += " <SOURCE>";
            text += " " + source.Id.ToString();
            text += " </SOURCE>";
            text += " <DESTINATION>";
            text += " " + destination.Id.ToString();
            text += " </DESTINATION>";
            text += " <EPAISSEUR>";
            text += " " + epaisseur.ToString();
            text += " </EPAISSEUR>";
            text += " <COULEUR>";
            text += " " + couleur.ToArgb().ToString();
            text += " </COULEUR>";
            text += " <TEXTE>";
            text += texte;
            text += " </TEXTE>";
            text += " </TRAIT>";
            return text;
        }

        public void lierNoeud()
        { // enregistre les id des noeuds source et destination pour la sérialisation
            idNoeudSource = source.Id;
            idNoeudDestination = destination.Id;
        }


        public void relierNoeud(List<Noeud> collectionNoeud)
        { // relie source et destination aux noeuds correspondant suite à une sérialisation
            foreach (Noeud n in collectionNoeud)
            {
                if (n.Id == idNoeudSource)
                    source = n;
                if (n.Id == idNoeudDestination)
                    destination = n;
            }

        }

        #endregion

        #region Implémentation de l'interface

        bool ISupprimable.Supprimé { get { return supprimé; } }
        void ISupprimable.Supprime() { supprimé = true; }
        void ISupprimable.Restaure() { supprimé = false; }
        #endregion
    }
}
