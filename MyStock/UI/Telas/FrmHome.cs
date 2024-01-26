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

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            btnSalvar.Text = "Salvar";
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
            txtCodigo.Enabled = true;
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

            if (btnSalvar.Text == "Editar")
            {
                produto.Salvar(2);
                MessageBox.Show("Produto editado com sucesso!");
            }
            else if (btnSalvar.Text == "Excluir")
            {
                produto.Salvar(3);
                MessageBox.Show("Produto excluído com sucesso!");
            }
            else if (btnSalvar.Text == "Salvar")
            {
                produto.Salvar(1);
                MessageBox.Show("Produto cadastrado com sucesso!");
            }


            LimparCampos();
            LoadAll();
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

        private void btnEditar_Click(object sender, EventArgs e)
        {
            btnSalvar.Text = "Editar";
            txtCodigo.Enabled = false;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            btnSalvar.Text = "Excluir";
            txtCodigo.Enabled = false;
        }

        private void dgProduto_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            LimparCampos();

            var prod = new Produto();

            foreach (DataGridViewRow row in dgProduto.Rows)
            {
                if (row != null && row.Index == e.RowIndex)
                {
                    prod.CodigoProd = row.Cells["Codigo"].Value.ToString();
                    prod.Nome = row.Cells["Nome"].Value.ToString();

                    var p = row.Cells["Valor"].Value.ToString();
                    prod.Valor = Convert.ToDecimal(p.Replace("R$", ""));

                    prod.Descricao = row.Cells["Descricao"].Value.ToString();

                    break;
                }
            }

            txtCodigo.Text = prod.CodigoProd;
            txtNome.Text = prod.Nome;
            txtValor.Text = prod.Valor.ToString();
            txtDescricao.Text = prod.Descricao;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
