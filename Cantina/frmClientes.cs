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

        //alterar clientes
        public int alterarClientes(int codCli)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "update tbclientes set nome = @nome, email = @email, telCelular = @telCelular where codCli = @codCli; ";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@nome", MySqlDbType.VarChar, 100).Value = txtNome.Text;
            comm.Parameters.Add("@email", MySqlDbType.VarChar, 100).Value = txtEmail.Text;
            comm.Parameters.Add("@telCelular", MySqlDbType.VarChar, 10).Value = mskTelefone.Text;
            comm.Parameters.Add("@codCli", MySqlDbType.Int32, 11).Value = codCli;

            comm.Connection = Conexao.obterConexao();

            int resp = comm.ExecuteNonQuery();

            return resp;

            Conexao.fecharConexao();
        }

        //excluir clientes
        public int excluirClientes(int codCli)
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "delete from tbClientes where codCli = @codCli";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Clear();
            comm.Parameters.Add("@codCli", MySqlDbType.Int32, 11).Value = codCli;

            comm.Connection = Conexao.obterConexao();

            int resp = comm.ExecuteNonQuery();

            return resp;
        }

        //carrega clientes
        public void carregaClientes()
        {
            MySqlCommand comm = new MySqlCommand();
            comm.CommandText = "select codCli as 'Código', nome as 'Nome', email as 'E-mail', telCelular as 'Telefone' from tbClientes";
            comm.CommandType = CommandType.Text;

            comm.Connection = Conexao.obterConexao();

            MySqlDataAdapter da = new MySqlDataAdapter(comm);

            DataTable clientes = new DataTable();

            da.Fill(clientes);

            dgvClientes.DataSource = clientes;

            Conexao.fecharConexao();


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

        public void habilitarCamposPesquisar()
        {
            txtCodigo.Enabled = false;
            txtEmail.Enabled = true;
            txtNome.Enabled = true;
            mskTelefone.Enabled = true;

            btnCadastrar.Enabled = false;
            btnAlterar.Enabled = true;
            btnExcluir.Enabled = true;
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

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (alterarClientes(Convert.ToInt32(txtCodigo.Text)) == 1)
            {
                MessageBox.Show("Alterado com sucesso!!!");
                desabilitarCampos();
                limparCampos();
                carregaClientes();
            }
            else
            {
                MessageBox.Show("Erro ao alterar");
                limparCampos();
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            carregaClientes();
            habilitarCamposPesquisar();
        }

        private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int indice = e.RowIndex;

            if (indice == -1)
            {
                return;
            }
            else
            {
                DataGridViewRow rowData = dgvClientes.Rows[indice];

                txtCodigo.Text = rowData.Cells[0].Value.ToString();
                txtNome.Text = rowData.Cells[1].Value.ToString();
                txtEmail.Text = rowData.Cells[2].Value.ToString();
                mskTelefone.Text = rowData.Cells[3].Value.ToString();

            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deseja excluir?",
                "Sistema", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.OK)
            {
                excluirClientes(Convert.ToInt32(txtCodigo.Text));
                carregaClientes();
                limparCampos();
                txtNome.Focus();
            }
            else
            {
                MessageBox.Show("Erro ao excluir");
            }
        }
    }
}
