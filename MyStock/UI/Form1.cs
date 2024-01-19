using Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class FrmProduto : Form
    {
        public FrmProduto()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAll();
        }

        private void LoadAll()
        {
            dgProduto.Rows.Clear();
            dgProduto.AutoGenerateColumns = false;

            dgProduto.ColumnCount = 4;
            dgProduto.Columns[0].Name = "Codigo";
            dgProduto.Columns[1].Name = "Nome";
            dgProduto.Columns[2].Name = "Valor";
            dgProduto.Columns[3].Name = "Descricao";

            var rows = new List<string[]>();

            foreach (Produto prod in new Produto().Todos())
            {
                string[] row1 = new string[]
                {
                        prod.CodigoProd,
                        prod.Nome,
                        "R$ " + prod.Valor.ToString(),
                        prod.Descricao
                };

                rows.Add(row1);
            }

            foreach (string[] rowArray in rows)
            {
                dgProduto.Rows.Add(rowArray);
            }
        }
    }
}
