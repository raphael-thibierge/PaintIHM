using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MonPaint
{
    [Serializable] class Noeud : ISupprimable
    {
        private Color couleur;
        private Color couleurSelection;
        private int epaisseur;
        private Rectangle rect;
        private Rectangle rectDessin;
        private int id;
        private string texte;
        private bool supprimé;
        private bool selection;

        #region Accesseurs
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
        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Texte
        {
            get { return texte; }
            set { texte = value; }
        }
        #endregion

        #region constructeur
        public Noeud(Rectangle r, Color c, int e)
        { // constructeur paramétré de la classe Noeud
            rect = r;
            couleur = c;
            couleurSelection = Color.Red;
            epaisseur = e;
            id = -1;
            texte = "";
            rectDessin = rect;

            supprimé = false;
            selection = false;
        }
        public Noeud(Rectangle r, Color c, int e, int num)
        { // constructeur paramétré de la classe Noeud
            rect = r;
            couleur = c;
            couleurSelection = Color.Red;
            epaisseur = e;
            id = num;
            texte = "";
            rectDessin = rect;

            supprimé = false;
            selection = false;

        }
        public Noeud(XmlNode xNN)
        { // constructeur paramétré de la classe Noeud depuis un fichier XML
            epaisseur = 1;
            couleur = Color.Black;
            id = 0;
            texte = "";
            rect = new Rectangle();
            selection = false;
            supprimé = false;
            
            

            foreach (XmlNode xNNN in xNN.ChildNodes)
            {
                switch (xNNN.Name)
                {
                    case "EPAISSEUR":
                        epaisseur = int.Parse(xNNN.InnerText);
                        break;
                    case "COULEUR":
                        int c = int.Parse(xNNN.InnerText);
                        couleur = Color.FromArgb(c);
                        break;
                    case "RECTANGLE":
                        string[] data = xNNN.InnerText.Split(',');
                        int x = int.Parse(data[0].Split('=')[1]);
                        int y = int.Parse(data[1].Split('=')[1]);
                        int w = int.Parse(data[2].Split('=')[1]);
                        int h = int.Parse(data[3].Replace("}", "").Split('=')[1]);
                        rect = new Rectangle(x, y, w, h);
                        break;
                    case "ID":
                        id = int.Parse(xNNN.InnerText);
                        break;
                    case "TEXTE":
                        texte = xNNN.InnerText;
                        break;

                }
            }
            rectDessin = rect;
        }

        #endregion

        #region fonctions
        public Point Centre()
        { // fonction qui renvoi le point correspondant au centre du noeud
            Point p = new Point((rectDessin.X+(rectDessin.Width/2)), (rectDessin.Y+(rectDessin.Height/2)));
            return p;
        }

        public void Dessine(Graphics g, Point origin, int zoom)
        { // fonction qui dessine le noeud
            if (!supprimé)
            {
                 Pen p;
                if (selection)
                    p = new Pen(couleurSelection, epaisseur * zoom / 100);
                else
                    p = new Pen(couleur, epaisseur * zoom / 100);

                rectDessin = new Rectangle((rect.X + origin.X) * zoom / 100, (rect.Y + origin.Y) * zoom / 100, rect.Width * zoom / 100, rect.Height * zoom / 100);
                g.DrawEllipse(p, rectDessin);

                if (texte != "")
                    g.DrawString(texte, new Font("Arial", 12), Brushes.Red, Centre());
            }
        }

        public bool Contains(Point p)
        { // renvoi crai si le point p est contenue dans le noeud
            if (p.X>rect.X-10 && p.X<rect.X+rect.Width+10 && p.Y>rect.Y-10 && p.Y<rect.Y+rect.Height+10)
            {
                return true;
            }
            return false;
        }

        public void Move(Point p)
        { // fonction qui déplace le noeud au point p 
            rect.X = p.X;
            rect.Y = p.Y;
        }

        public string ToXML()
        { // fonction qui renvoit une chaine de caractère au format XML
            string text = "<NOEUD>";
            text += " <EPAISSEUR>";
            text += " " + epaisseur.ToString();
            text += " </EPAISSEUR>";
            text += " <COULEUR>";
            text += " " + couleur.ToArgb().ToString();
            text += " </COULEUR>";
            text += " <RECTANGLE>";
            text += " " + rect.ToString();
            text += " </RECTANGLE>";
            text += " <ID>";
            text += " " + id.ToString();
            text += " </ID>";
            text += " <TEXTE>";
            text += texte;
            text += " </TEXTE>";
            text += " </NOEUD>";
            return text;
        }

        public void Selectionner()
        {
            selection = true;
        }

        public void Déselectionner()
        {
            selection = false;
        }

        #endregion

         #region Implémentation de l'interface
         
         bool ISupprimable.Supprimé { get { return supprimé; }}
         void ISupprimable.Supprime() { supprimé = true; }
         void ISupprimable.Restaure() { supprimé = false; }
 #endregion
    }
}
