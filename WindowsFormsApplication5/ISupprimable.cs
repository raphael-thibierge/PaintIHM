﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonPaint
{
    public interface ISupprimable
    {
       
        bool Supprimé { get; }
        void Supprime();
        void Restaure();
      

    }
}
