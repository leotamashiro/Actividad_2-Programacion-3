﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CATALOGO
{
    public class Imagen
    {
        private int codigo;
        private string url;

        public int Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
    }   
}
