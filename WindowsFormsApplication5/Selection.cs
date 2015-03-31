using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonPaint
{
    class Selection
    {
        private Rectangle rectangle;
        private Point origin;
        private Point destination;
        private bool reverseX;
        private bool reverseY;
        public Selection(Point p)
        {
            origin = p;
            destination = new Point(p.X+1, p.Y+1);
            rectangle = new Rectangle(origin, new Size(1,1));
            reverseX = false;
            reverseY = false;

        }

        public void Dessine(Graphics g)
        {
            g.DrawRectangle(new Pen(Color.Red), rectangle);
        }
    

        public void Update(Point p)
        {

            if (reverseX)
            {

                if (p.X > destination.X)
                {
                    reverseX = false;
                    origin.X = destination.X;
                    destination.X = p.X;
                }
                else
                    origin.X = p.X;
            }

            else
            {
                

                if (p.X < origin.X)
                {
                    reverseX = true;
                    destination.X = origin.X;
                    origin.X = p.X;
                }
                else
                    destination.X = p.X;
            }


            if (reverseY)
            {

                if (p.Y > destination.Y)
                {
                    reverseY = false;
                    origin.Y = destination.Y;
                    destination.Y = p.Y;
                }
                else
                    origin.Y = p.Y;
            }

            else
            {


                if (p.Y < origin.Y)
                {
                    reverseY = true;
                    destination.Y = origin.Y;
                    origin.Y = p.Y;
                }
                else
                    destination.Y = p.Y;
            }





            rectangle = new Rectangle(origin, new Size(destination.X-origin.X, destination.Y-origin.Y));
        }


        public void Selectionner(List<Noeud> collectionNoeuds, ref List<Noeud> collectionNoeudsSélectionnés)
        {
            

            foreach (Noeud n in collectionNoeuds)
            {
                if ((n.Rect.X >= origin.X && n.Rect.X <= destination.X && n.Rect.Y >= origin.Y && n.Rect.Y <= destination.Y)
                    || (n.Rect.X + n.Rect.Width >= origin.X && n.Rect.X + n.Rect.Width <= destination.X && n.Rect.Y + n.Rect.Height >= origin.Y && n.Rect.Y + n.Rect.Height <= destination.Y))
                {
                    collectionNoeudsSélectionnés.Add(n);
                    n.Selectionner();
                }
                else
                {
                    n.Déselectionner();
                    collectionNoeudsSélectionnés.Remove(n);
                }
            }
        }
    }
}
