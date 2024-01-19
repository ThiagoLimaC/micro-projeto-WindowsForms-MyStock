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
using UI.Telas;

namespace UI
{
    public partial class FrmProduto : Form
    {
        FrmCadastro frmCadastro;
         
        public FrmProduto()
        {
            InitializeComponent();
            this.frmCadastro = new FrmCadastro();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            pnlDesktop.Controls.Clear();
            frmCadastro.TopLevel = false;
            frmCadastro.TopMost = false;
            frmCadastro.Dock = DockStyle.Fill;
            pnlDesktop.Controls.Add(frmCadastro);
            frmCadastro.Show();
        }
    }
}
