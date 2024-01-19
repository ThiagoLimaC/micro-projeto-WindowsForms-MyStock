using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Produto : Base
    {
        [OpcoesBase(UsarNoBancoDeDados = true, ChavePrimaria = true, UsarParaBuscar = true)]
        public string CodigoProd { get; set; }

        [OpcoesBase(UsarNoBancoDeDados = true)]
        public string Nome { get; set; }

        [OpcoesBase(UsarNoBancoDeDados = true)]
        public string Descricao { get; set; }

        [OpcoesBase(UsarNoBancoDeDados = true)]
        public decimal Valor { get; set; }
    }
}
