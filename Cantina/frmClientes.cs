using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;

namespace Cantina
{
    public partial class frmClientes : Form
    {
        //Criando variáveis para controle do menu
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        public frmClientes()
        {
            InitializeComponent();
            desabilitarCampos();

        }

        private void frmClientes_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int MenuCount = GetMenuItemCount(hMenu) - 1;
            RemoveMenu(hMenu, MenuCount, MF_BYCOMMAND);
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.Equals("") || txtEmail.Text.Equals("") || mskTelefone.Text.Equals("     -"))
            {
                MessageBox.Show("Favor preencher os campos");
            }
            else
            {
                if (cadastrarClientes() == 1)
                {
                    MessageBox.Show("Cadastrado com sucesso");
                    limparCampos();
                    desabilitarCampos();

                }
                else
                {
                    MessageBox.Show("Erro ao cadastrar");

                }
            }
        }

        //cadastrar clientes
        public int cadastrarClientes()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "insert into tbClientes(nome,email,telCelular)values(@nome,@email,@telCelular);";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = txtNome.Text;
            comm.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = txtEmail.Text;
            comm.Parameters.Add("@telCelular", MySqlDbType.VarChar, 10).Value = mskTelefone.Text;

            comm.Connection = Conexao.obterConexao();

            int resp = comm.ExecuteNonQuery();

            return resp;
        }

        //desabilitar campos
        public void desabilitarCampos()
        {
            txtCodigo.Enabled = false;
            txtEmail.Enabled = false;
            txtNome.Enabled = false;
            mskTelefone.Enabled = false;

            btnCadastrar.Enabled = false;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnLimpar.Enabled = false;

        }
        public void habilitarCamposNovo()
        {
            txtCodigo.Enabled = false;
            txtEmail.Enabled = true;
            txtNome.Enabled = true;
            mskTelefone.Enabled = true;

            btnCadastrar.Enabled = true;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnLimpar.Enabled = true;
            btnNovo.Enabled = false;
            txtNome.Focus();

        }
        public void limparCampos()
        {
            txtCodigo.Clear();
            txtEmail.Clear();
            txtNome.Clear();
            mskTelefone.Clear();

            btnCadastrar.Enabled = false;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnLimpar.Enabled = false;
            btnNovo.Enabled = true;
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal();
            abrir.Show();
            this.Hide();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            habilitarCamposNovo();
        }
    }
}
