using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonPaint
{

    public enum Type_Action { Créer, Détruire, Déplacer };

    class Action
    {
        
        Type_Action type_action;
        Stack<Action> actions;

        List<Object> objets;

        public Action(Type_Action type, List<Object> objets)
        {
            type_action = type;
            this.objets = objets;
            actions = new Stack<Action>(50);

        }

        public void empiler(Object t)
        {
            Action action = new Action(Type_Action.Créer, new List<object>() { t });
            actions.Push(action);
        }


        public void Undo()
        {
            switch (type_action)
            {
                case Type_Action.Détruire:
                    foreach(object o in objets)
                        ((ISupprimable)o).Restaure();
                    break;
                case Type_Action.Créer:
                    foreach(object o in objets)
                        ((ISupprimable)o).Supprime();
                    break;
                   
            }
        }

        public void Redo()
        {
            switch (type_action)
            {
                case Type_Action.Détruire:
                    foreach (object o in objets)
                        ((ISupprimable)o).Supprime();
                    break;

                case Type_Action.Créer:
                    foreach (object o in objets)
                        ((ISupprimable)o).Restaure();
                    break;

            }
        }



    }
}
