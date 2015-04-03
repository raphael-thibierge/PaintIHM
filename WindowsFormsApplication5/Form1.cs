using MonPaint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Printing;

namespace MonPaint
{
    public partial class PaintWindow : Form
    {
        List<Noeud> collectionNoeud;
        List<Trait> collectionTrait;
        List<Noeud> collectionSelection;

        Color couleurDefault;
        Color couleurProvisoire;
        int epaisseurDefault;
        int epaisseurProvisoire;
        Noeud noeudCourant;
        Trait traitCourant;
        int cptSommet; // compteur de sommet pour que chaque sommet ai une id différente
        Point origin; // définit l'origine du dessin, il est utilisé pour le scroll
        int sizeDefault;

        Selection rectSelection;

        Stack<Action> actions;
        Stack<Action> actionsAnnulées;

      
        public PaintWindow()
        { // constructeur du paint
            InitializeComponent();
            couleurDefault = Color.Orange;
            couleurProvisoire = Color.Red;
            epaisseurDefault = 4;
            epaisseurProvisoire = epaisseurDefault;
            sizeDefault = 20;
            collectionNoeud = new List<Noeud>();
            collectionTrait = new List<Trait>();
            collectionSelection = new List<Noeud>();
            actions = new Stack<Action>();
            actionsAnnulées = new Stack<Action>();
            cptSommet = 0;
            origin = new Point(0, 0);
     
        }
        private void Dessin(object sender, PaintEventArgs e)
        { // fonction qui affiche à l'écran tous les noeuds et tous les traits
            foreach (Noeud n in collectionNoeud)
                n.Dessine(e.Graphics, origin);
            foreach (Trait t in collectionTrait)
                t.Dessine(e.Graphics);            
            
            if (noeudCourant != null)            
                noeudCourant.Dessine(e.Graphics, origin);

            if (traitCourant != null)
                traitCourant.Dessine(e.Graphics);

            if (rectSelection != null)
                rectSelection.Dessine(e.Graphics);
            
        }
        private Noeud trouveNoeud(Point p)
        {// fonction qui renvoit le noeud trouvé à un point p ou null s'il n'y en a pas
            foreach (Noeud n in collectionNoeud)            
                if (n.Contains(p))                
                    return n;                   
            return null;
        }
        private Trait trouveTrait(Point p)
        { // fonction qui renvoit le trait trouvé à un point p ou null s'il n'y en a pas
            foreach (Trait t in collectionTrait)
                if (t.Contains(p))
                    return t;
            return null;
        }


        #region Souris

        private Point getLocation(Point p)
        {          
            Point souris = new Point(p.X-origin.X, p.Y-origin.Y);
            return souris;
        }



        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
        // Bouton Gauche
            

            if (e.Button == MouseButtons.Left)
            {
                if (trouveNoeud(getLocation(e.Location)) == null && BtSelection.Checked == false)
                {   // création d'un nouveau noeud
                    Noeud nouveauNoeud = new Noeud(new Rectangle(getLocation(e.Location), new Size(sizeDefault, sizeDefault)), couleurDefault, epaisseurDefault, cptSommet);

                    Action action = new Action(Type_Action.Créer, new List<object>() { nouveauNoeud });
                    actions.Push(action);

                    collectionNoeud.Add(nouveauNoeud);
                    cptSommet++;
                }

                if (BtSelection.Checked)
                {
                    collectionSelection.Clear();
                    rectSelection = new Selection(e.Location);
                    if (trouveNoeud(e.Location) != null)
                    {
                        Noeud n = trouveNoeud(e.Location);
                        collectionSelection.Add(n);
                        n.Selectionner();
                        
                    }
                }   
                

                else if (BtTraits.Checked)
                {
                    noeudCourant = new Noeud(new Rectangle(getLocation(e.Location), new Size(sizeDefault, sizeDefault)), couleurProvisoire, epaisseurDefault);
                    traitCourant = new Trait(trouveNoeud(getLocation(e.Location)), noeudCourant, couleurProvisoire, epaisseurProvisoire);                                       
                }

                else if (BtDéplacement.Checked)
                {
                    noeudCourant = trouveNoeud(getLocation(e.Location));
                }

                
            }

            // Bouton droit
            else if (e.Button == MouseButtons.Right)
            {
                if (trouveNoeud(getLocation(e.Location)) != null || trouveTrait(getLocation(e.Location)) != null)
                {
                    if (trouveNoeud(getLocation(e.Location)) != null)
                        noeudCourant = trouveNoeud(getLocation(e.Location));
                    if (trouveTrait(getLocation(e.Location)) != null)
                        traitCourant = trouveTrait(getLocation(e.Location));

                    string[] libellés = new string[] { "Modifier", "Supprimer" };
                    ContextMenuStrip cm = new ContextMenuStrip();

                    foreach (string libel in libellés)
                    {
                        ToolStripMenuItem menuItem = new ToolStripMenuItem(libel);
                        menuItem.Click += menuItem_Click;
                        cm.Items.Add(menuItem);
                    }
                    cm.Show(this, e.Location);
                }
            }
            Refresh();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (BtSelection.Checked)
            {
                if (rectSelection != null)
                {
                    rectSelection.Update(e.Location);
                    rectSelection.Selectionner(collectionNoeud, ref collectionSelection);
                }
                
              
            }

            if ((BtDéplacement.Checked || BtTraits.Checked) && noeudCourant!=null )
                noeudCourant.Move(getLocation(e.Location));           
            
            if (traitCourant != null)
                traitCourant.Destination = noeudCourant;
            

            Refresh();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (BtSelection.Checked)
            {
                rectSelection = null;
            }
            
            else if (e.Button == MouseButtons.Left)
            {
                if (BtTraits.Checked && traitCourant != null)
                {
                    if (trouveNoeud(getLocation(e.Location)) == null)
                    {
                        Noeud nouveauNoeud = new Noeud(new Rectangle(getLocation(e.Location), new Size(sizeDefault, sizeDefault)), couleurDefault, epaisseurDefault, cptSommet);
                        
                        Action action = new Action(Type_Action.Créer, new List<object>() { nouveauNoeud });
                        actions.Push(action);

                        collectionNoeud.Add(nouveauNoeud);
                        cptSommet++;
                        traitCourant.Destination = nouveauNoeud;


                    }
                    else traitCourant.Destination = trouveNoeud(getLocation(e.Location));

                    traitCourant.Couleur = couleurDefault;
                    traitCourant.Epaisseur = epaisseurDefault;
                    
                    Action actionTrait = new Action(Type_Action.Créer, new List<object>() { traitCourant });
                    actions.Push(actionTrait);
                    collectionTrait.Add(traitCourant);

                    traitCourant = null;
                    noeudCourant = null;
                }

                if (BtDéplacement.Checked)
                {
                    if (noeudCourant != null)
                    {
                        Action action = new Action(Type_Action.Créer, new List<object>() { noeudCourant });
                        actions.Push(action);
                        collectionNoeud.Add(noeudCourant);
                        noeudCourant = null;
                    }
                    
                    if (traitCourant != null)
                    {
                        Action action = new Action(Type_Action.Créer, new List<object>() { traitCourant });
                        actions.Push(action);
                        collectionTrait.Add(traitCourant);
                        traitCourant = null;
                    }
                }
            }
            
            Refresh();
        }
        
        // menu souris
        void menuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tm = (ToolStripMenuItem)sender;
            switch (tm.Text)
            {
                case "Modifier":
                    if (collectionSelection.Count > 0 && noeudCourant!=null)
                    {
                        Paramètres param = new Paramètres();
                        param.Epaisseur = noeudCourant.Epaisseur;
                        param.Couleur = noeudCourant.Couleur;
                        param.Texte = noeudCourant.Texte;

                        if (param.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            foreach (Noeud n in collectionSelection)
                            {
                                n.Epaisseur = param.Epaisseur;
                                n.Couleur = param.Couleur;
                                n.Texte = param.Texte;

                            }

                        }
                    }

                    else if (noeudCourant != null)
                    {
                        Paramètres param = new Paramètres();
                        param.Epaisseur = noeudCourant.Epaisseur;
                        param.Couleur = noeudCourant.Couleur;
                        param.Texte = noeudCourant.Texte;

                        if (param.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            noeudCourant.Epaisseur = param.Epaisseur;
                            noeudCourant.Couleur = param.Couleur;
                            noeudCourant.Texte = param.Texte;

                        }
                    }

                    if (traitCourant != null)
                    {
                        Paramètres param = new Paramètres();
                        
                        param.Epaisseur = traitCourant.Epaisseur;
                        param.Couleur = traitCourant.Couleur;
                        param.Texte = traitCourant.Texte;

                        if (param.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            traitCourant.Epaisseur = param.Epaisseur;
                            traitCourant.Couleur = param.Couleur;
                            traitCourant.Texte = param.Texte;
                        }

                        traitCourant = null;

                    }
                    
                    break;
            

                case "Supprimer":
                    List<object> liste = new List<object>();


                    if (collectionSelection.Count() > 0)
                    {
                        foreach (Noeud n in collectionSelection)
                        {
                            ((ISupprimable)n).Supprime();
                            liste.Add(n);


                            foreach (Trait t in collectionTrait)
                                if (t.ancre(n))
                                {
                                    liste.Add(t);
                                    ((ISupprimable)t).Supprime();

                                }
                        }
                        Action action = new Action(Type_Action.Détruire, liste);
                        actions.Push(action);
                    }

                    else if (noeudCourant != null)
                    {
                        

                        ((ISupprimable)noeudCourant).Supprime();
                        liste.Add(noeudCourant);
                        

                        foreach (Trait t in collectionTrait)
                            if (t.ancre(noeudCourant))
                            {
                                liste.Add(t);
                                ((ISupprimable)t).Supprime();
                          
                            }                         


                        Action action = new Action(Type_Action.Détruire, liste);
                        actions.Push(action);
                        noeudCourant = null;

                    }

                    else if (traitCourant != null)
                    {
                        collectionTrait.Remove(traitCourant);
                    }

                    break;
                    
            }

            Refresh();
        }
#endregion


        #region Boutons
        private void reset_Click(object sender, EventArgs e)
        { // fonction qui efface tous les noeud et tous les traits
            collectionNoeud.Clear();
            collectionTrait.Clear();
            noeudCourant = null;
            traitCourant = null;
            // remise à zéro du compteur de noeud
            cptSommet = 0;
            Refresh();
        }
        private void Plus_Click(object sender, EventArgs e)
        { // augmentation de l'épaisseu par défault des traits et des noeuds
            epaisseurDefault++;
        }
        private void moins_Click(object sender, EventArgs e)
        { // diminution de l'épaisseur par défault des traits et des noeuds
            epaisseurDefault--;
        }
        private void Color_Click(object sender, EventArgs e)
        {// Affichage d'une boite dialogue qui permet de choisir une couleur lorsqu'on clique sur l'icone couleur
            ColorDialog c = new ColorDialog();
            c.Color = couleurDefault;
            if (c.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                couleurDefault = c.Color;
            Couleur.BackColor = couleurDefault;

        }
        private void Traits_Click(object sender, EventArgs e)
        { // fonction qui décoche les autres boutons lorsqu'on clique sur le bouton trait
            BtDéplacement.Checked = false;
            BtSelection.Checked = false;
        }
        private void déplacement_Click(object sender, EventArgs e)
        { // fonction qui décoche les autres boutons lorsqu'on clique sur le bouton déplacement
            BtTraits.Checked = false;
            BtSelection.Checked = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            BtDéplacement.Checked = false;
            BtTraits.Checked = false;
        }

        private void BtMatrice_Click(object sender, EventArgs e)
        { // bouton qui crée une matrice et qui applique l'algorithme first fit au graphe affiché à l'écran.
            int[,] matrice = new int[cptSommet, cptSommet];
            for (int i = 0; i < cptSommet; i++)
                for (int j = 0; j < cptSommet; j++)
                    matrice[i, j] = 0;

            foreach (Trait r in collectionTrait)
            {
                matrice[r.Source.Id, r.Destination.Id] = 1;
                matrice[r.Destination.Id, r.Source.Id] = 1;

            }
            coloration(matrice);
            matrice = null;
            Refresh();
        }
        #endregion

        #region sauvegarde
        private void enregistrerToolStripButton_Click(object sender, EventArgs e)
        { // bouton de sauvegarde par sérialisation
            sauvSerialization();           
        }
        private void enregistrerToolStripMenuItem_Click(object sender, EventArgs e)
        { // bouton de sauyvegardesous forme de fichier xml
            sauvXML();
        }
        private void ouvrirToolStripButton_Click(object sender, EventArgs e)
        { // bouton pour ouvrir un fichier de sérialisation
            openSerialization();
            Refresh();
        }
        private void ouvrirToolStripMenuItem_Click(object sender, EventArgs e)
        { // ouvrir un fichier xml
            openXML();
            Refresh();
        }

        private void sauvXML()
        { // sauvegarde XML
            SaveFileDialog svfd = new SaveFileDialog();
            svfd.Filter = "Fichier xml|*.xml";
            svfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (svfd.ShowDialog() == DialogResult.OK)
            {
                string fichier = svfd.FileName;

                StreamWriter sw = new StreamWriter(svfd.FileName);

                sw.WriteLine("<!--?xml version=\"1.0\" encoding=\"UTF-8\" ?--> ");
                sw.WriteLine("<DESSIN>");
                sw.WriteLine("<CPT>");
                sw.WriteLine(cptSommet.ToString());
                sw.WriteLine("</CPT>");

                foreach (Noeud n in collectionNoeud)
                    sw.WriteLine(n.ToXML());

                foreach (Trait t in collectionTrait)
                    sw.WriteLine(t.ToXML());

                sw.WriteLine("</DESSIN>");
                sw.Close();
            }
        }
        private void openXML()
        { // ouvrir XML
            OpenFileDialog opfd = new OpenFileDialog();
            opfd.Filter = "Fichier xml|*.xml";
            opfd.Title = "Choisir le fichier";
            opfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (opfd.ShowDialog() == DialogResult.OK)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(opfd.FileName);
                foreach (XmlNode xN in doc.ChildNodes)
                {
                    if (xN.Name == "DESSIN")
                    {
                        foreach (XmlNode xNN in xN.ChildNodes)
                        {
                            switch (xNN.Name)
                            {
                                case "CPT":
                                    cptSommet = int.Parse(xNN.InnerText);
                                    break;

                                case "NOEUD":
                                    collectionNoeud.Add(new Noeud(xNN));
                                    break;
                                case "TRAIT":
                                    collectionTrait.Add(new Trait(xNN, collectionNoeud));
                                    break;
                            }
                        }
                    }
                }
            }
        }
        private void sauvSerialization()
        { // sauvegarde sérialisation
            foreach (Trait t in collectionTrait)
                t.lierNoeud();
      
            SaveFileDialog svfd = new SaveFileDialog();
            svfd.Filter = "Fichier des|*.des";
            svfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (svfd.ShowDialog() == DialogResult.OK)
            {
                Stream stream = File.Open(svfd.FileName, FileMode.Create);
                BinaryFormatter btformatter = new BinaryFormatter();
                btformatter.Serialize(stream, collectionNoeud);
                btformatter.Serialize(stream, collectionTrait);
                stream.Close();
            }
        }
        private void openSerialization()
        { // ouvrir fichier sérialisation
            Stream stream = null;
            OpenFileDialog opfd = new OpenFileDialog();
            opfd.Filter = "Fichier des|*.des";
            opfd.Title = "Choisir le fichier";
            opfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (opfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    stream = File.Open(opfd.FileName, FileMode.Open);
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    collectionNoeud = (List<Noeud>)bFormatter.Deserialize(stream);
                    collectionTrait = (List<Trait>)bFormatter.Deserialize(stream);
                    foreach (Trait t in collectionTrait)
                        t.relierNoeud(collectionNoeud);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message);
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
        }
        #endregion   

        #region Impression

        private void imprimer()
        { // impression standard
            PrintDocument Impression = new PrintDocument();
            Impression.PrintPage += Impression_PrintPage;
            Impression.Print();
        }
        private void imprimerToolStripMenuItem_Click(object sender, EventArgs e)
        { // impression paramétrée
            PrintDialog printDial = new PrintDialog();
            printDial.AllowSelection = false;
            printDial.AllowSomePages = true;
            printDial.PrintToFile = false;
            Margins margin = new Margins(35, 35, 35, 35);
            if (printDial.ShowDialog() == DialogResult.OK)
            {
                ImprimanteCourante = printDial.PrinterSettings.PrinterName;
                paperSize = new PaperSize("A4", printDial.PrinterSettings.DefaultPageSettings.Bounds.Width, printDial.PrinterSettings.DefaultPageSettings.Bounds.Height);
                margin = printDial.PrinterSettings.DefaultPageSettings.Margins;
                numberOfPages = 1;

                if (printDial.PrinterSettings.FromPage > 0)
                    nombreDePages = printDial.PrinterSettings.ToPage;

                imprimer();
            }

            
        }
        private void imprimerToolStripButton_Click(object sender, EventArgs e)
        { // bouton imprimer
            imprimer();             
        }
        private void Impression_PrintPage(object sender, PrintPageEventArgs e)
        { // gestion des pages
            if (numéroPage < nombreDePages)
                e.HasMorePages = true;
            else e.HasMorePages = false;
            foreach (Noeud n in collectionNoeud)
                n.Dessine(e.Graphics, origin);
            foreach (Trait t in collectionTrait)
                t.Dessine(e.Graphics);
        }
        private void aperçuavantimpressionToolStripMenuItem_Click(object sender, EventArgs e)
        { // bouton aperçu avant impression
            PrintPreviewDialog ptPrev = new PrintPreviewDialog();
            PrintDocument Impression = new PrintDocument();
            Impression.PrintPage += Impression_PrintPage;
            ptPrev.Document = Impression;
            ptPrev.ShowDialog();
        }

        // accesseurs
        public string ImprimanteCourante { get; set; }
        public PaperSize paperSize { get; set; }
        public int nombreDePages { get; set; }
        public int numberOfPages { get; set; }
        public int numéroPage { get; set; }
        #endregion
     

        #region Graphe&Langage
        // foncitons de l'algorithme firsr fit
        public void coloration(int[,] matrice)
        {
            int[] tabC = new int[cptSommet];
            int nbCouleur = 0;
            firstFit(matrice, cptSommet, ref nbCouleur, ref tabC);
            int cpt = 0;
            foreach (Noeud s in collectionNoeud)
            {
                s.Texte = tabC[cpt].ToString();
                cpt++;
            }

        }


        private void firstFit(int[,] G, int n, ref int nbCouleur, ref int[] tabCouleur)
        {
            for (int i = 0; i < n; i++)
            {
                tabCouleur[i] = plusPetiteCouleurDispo(G, n, i, ref tabCouleur);
                nbCouleur = tabCouleur[i];
            }
        }

        private int plusPetiteCouleurDispo(int[,] G, int n, int sommet, ref int[] tabCouleur)
        {
            bool[] dispo = new bool[n];
            for (int i = 0; i < sommet; i++)
                dispo[i] = true;
            for (int i = 0; i < sommet; i++)
            {
                if (G[sommet, i] == 1)
                    dispo[tabCouleur[i]] = false;
            }

            int cpt = 0;
            while (dispo[cpt] == false && cpt < sommet)
            {
                cpt++;
            }
            return cpt;

        }

        public void DFS(int[,] matrice, int nbSommet)
        {
            bool[] marque = new bool[nbSommet];

            for (int s = 0; s < nbSommet; s++)
                marque[s] = false;
            for (int s = 0; s < nbSommet; s++)
                if (!marque[s])
                    Tremaux(matrice, nbSommet, ref marque, s);
        }

        public void Tremaux(int[,] matrice, int nbSommet, ref bool[] marque, int s)
        {
            marque[s] = true;
            for (int i = 0; i < nbSommet; i++)
                if (matrice[s, i] == 1 && !marque[i])
                    Tremaux(matrice, nbSommet, ref marque, s);

        }

        #endregion

        private void PaintWindow_Load(object sender, EventArgs e)
        {

        }

        #region défilement et zoom
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            switch(e.Type)
            {
                case ScrollEventType.First :
                    break;
                
                case ScrollEventType.Last :
                    break;

                case ScrollEventType.EndScroll :
                    origin.Y = -e.NewValue;
                    Refresh();
                    break;

            }


            Refresh();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            switch (e.Type)
            {
                case ScrollEventType.First:
                    break;

                case ScrollEventType.Last:
                    break;

                case ScrollEventType.EndScroll:
                    origin.X = -e.NewValue;
                    Refresh();
                    break;

            }
        }

        #endregion


        private void PaintWindow_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        




        #region Undo/Redo

     


        private void annulerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (actions.Count > 0)
            {
                Action action = actions.Pop();
                action.Undo();
                actionsAnnulées.Push(action);
                Refresh();

            }
        }
        #endregion

        private void rétablirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (actionsAnnulées.Count > 0)
            {
                Action action = actionsAnnulées.Pop();
                action.Redo();
                actions.Push(action);
                Refresh();
            }
        }


















    }
}

