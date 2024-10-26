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
    public partial class frmCadastroUsuario : Form
    {

        //Criando variáveis para controle do menu
        const int MF_BYCOMMAND = 0X400;
        [DllImport("user32")]
        static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern int GetMenuItemCount(IntPtr hWnd);

        public frmCadastroUsuario()
        {
            InitializeComponent();
            carregaFuncionarios();
            desabilitarCampos();
        }

        private void frmCadastroUsuario_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int MenuCount = GetMenuItemCount(hMenu) - 1;
            RemoveMenu(hMenu, MenuCount, MF_BYCOMMAND);
        }

        public void carregaFuncionarios()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "select * from tbFuncionarios;";
            comm.CommandType = CommandType.Text;
            comm.Connection = Conexao.obterConexao();

            MySqlDataReader DR;

            DR = comm.ExecuteReader();

            cbbFuncionarios.Items.Clear();

            while (DR.Read())
            {
                cbbFuncionarios.Items.Add(DR.GetString(1));
            }

            Conexao.fecharConexao();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            frmMenuPrincipal abrir = new frmMenuPrincipal();
            abrir.Show();
            this.Hide();

        }

        public int selecionaCodigoFuncionario(string nome)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "select codFunc from tbfuncionarios where nome = @nome;";
            comm.CommandType = CommandType.Text;

            comm.Connection = Conexao.obterConexao();
            comm.Parameters.Clear();

            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = nome;

            MySqlDataReader DR;
            DR = comm.ExecuteReader();
            DR.Read();

            codFunc = DR.GetInt32(0);

            return codFunc;

            Conexao.fecharConexao();
        }

        private void cbbFuncionarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMostraCodigoFuncionario.Text = selecionaCodigoFuncionario(cbbFuncionarios.SelectedItem.ToString()).ToString();
            selecionaUsuarios(Convert.ToInt32(lblMostraCodigoFuncionario.Text));
        }

        public void selecionaUsuarios(int codFunc)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "select codUsu,nome,senha from tbUsuarios where codFunc = @codFunc;";
            comm.CommandType = CommandType.Text;

            comm.Connection = Conexao.obterConexao();

            comm.Parameters.Clear();
            comm.Parameters.Add("@codFunc", MySqlDbType.Int32, 11).Value = codFunc;

            MySqlDataReader DR;
            DR = comm.ExecuteReader();

            DR.Read();
            try
            {
                txtCodigo.Text = DR.GetInt32(0).ToString();
                txtUsuario.Text = DR.GetString(1);
                txtSenha.Text = DR.GetString(2);

                Conexao.fecharConexao();
            }
            catch (Exception)
            {
                MessageBox.Show("Funcionário sem usuário.");
                txtUsuario.Clear();
                txtSenha.Clear();
                txtCodigo.Clear();
                txtUsuario.Focus();
                habilitarCamposCadastrar();
            }
        }

        //variavel global
        public int codFunc;

        public int cadastrarUsuario(int codFunc)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "insert into tbUsuarios(nome,senha,codFunc) values(@nome,@senha,@codFunc);";
            comm.CommandType = CommandType.Text;
            comm.Connection = Conexao.obterConexao();

            comm.Parameters.Clear();

            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 25).Value = txtUsuario.Text;
            comm.Parameters.Add("@senha", MySqlDbType.VarChar, 10).Value = txtSenha.Text;
            comm.Parameters.Add("@codFunc", MySqlDbType.Int32, 11).Value = codFunc;

            int res = comm.ExecuteNonQuery();

            return res;

            Conexao.fecharConexao();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (txtSenha.Text.Equals(txtRepetirSenha.Text))
            {
                int resultado = cadastrarUsuario(codFunc);

                if (resultado == 1)
                {
                    MessageBox.Show("Cadastrado com sucesso!!");
                    desabilitarCampos();
                    limparCampos();
        
                }
                else
                {
                    MessageBox.Show("Erro ao cadastrar!!");
                }
            }
            else
            {
                MessageBox.Show("A senha não é igual!!!");
                txtSenha.Clear();
                txtRepetirSenha.Clear();
                txtSenha.Focus();
            }
        }

        public void limparCampos()
        {
            txtCodigo.Clear();
            txtUsuario.Clear();
            txtSenha.Clear();
            txtRepetirSenha.Clear();
            cbbFuncionarios.Text = "";
        }
        public void desabilitarCampos()
        {
            txtUsuario.Enabled = false;
            txtSenha.Enabled = false;
            txtRepetirSenha.Enabled = false;
            btnCadastrar.Enabled = false;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnLimpar.Enabled = false;
            btnNovo.Enabled = false;
        }
        public void habilitarCamposCadastrar()
        {
            txtUsuario.Enabled = true;
            txtSenha.Enabled = true;
            txtRepetirSenha.Enabled = true;
            btnCadastrar.Enabled = true;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnLimpar.Enabled = true;
            btnNovo.Enabled = false;
            txtUsuario.Focus();

        }
        private void btnNovo_Click(object sender, EventArgs e)
        {
            habilitarCamposCadastrar();
        }
    }
}
