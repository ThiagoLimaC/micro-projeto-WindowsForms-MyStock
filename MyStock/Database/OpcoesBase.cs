﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class OpcoesBase
    {
        public bool UsarNoBancoDeDados { get; set; }
        public bool UsarParaBuscar { get; set; }
        public bool ChavePrimaria { get; set; }
        public bool AutoIncrement { get; set; }
    }
}
