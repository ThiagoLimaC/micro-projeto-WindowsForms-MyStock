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

namespace UI.Telas
{
    public partial class FrmCadastro : Form
    {
        public FrmCadastro()
        {
            InitializeComponent();
        }

        private void FrmCadastro_Load(object sender, EventArgs e)
        {
            LoadAll();
        }

        private void LimparCampos()
        {
            txtCodigo.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtValor.Text = string.Empty;
            txtDescricao.Text = string.Empty;
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

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            var produto = new Produto();

            produto.CodigoProd = txtCodigo.Text;
            produto.Nome = txtNome.Text;

            var preco = txtValor.Text.Replace("R$", "").Replace(".", ",");
            produto.Valor = Decimal.Parse(preco);

            produto.Descricao = txtDescricao.Text;

            produto.Salvar(1);

            LimparCampos();
            LoadAll();

            MessageBox.Show("Produto cadastrado com sucesso!");
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            dgProduto.Rows.Clear();

            foreach (Produto prod in new Produto().Busca(txtPesquisar.Text))
            {
                string[] row1 = new string[]
                {
                        prod.CodigoProd,
                        prod.Nome,
                        "R$ " + prod.Valor.ToString(),
                        prod.Descricao
                };

                dgProduto.Rows.Add(row1);
            }
        }
    }
}
