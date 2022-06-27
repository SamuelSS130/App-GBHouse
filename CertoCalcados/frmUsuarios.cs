using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CertoCalcados
{
    public partial class frmUsuarios : Form
    {
        public frmUsuarios()
        {
            InitializeComponent();
        }

        Usuario usuario = new Usuario();

        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            grdPesquisa.AllowUserToAddRows = false;
            grdPesquisa.AllowUserToDeleteRows = false;
            grdPesquisa.AllowUserToResizeRows = false;
            grdPesquisa.AllowUserToResizeColumns = false;
            grdPesquisa.ReadOnly = true;
            grdPesquisa.MultiSelect = false;
            grdPesquisa.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            grdPesquisa.RowHeadersVisible = false;

            CarregarGrid();

        }
        private void CarregarGrid()
        {
            try
            {
                grdPesquisa.DataSource = usuario.Consultar();

                //Escondendo colunas 
                grdPesquisa.Columns[0].Visible = false;
                grdPesquisa.Columns[3].Visible = false;
                grdPesquisa.Columns[5].Visible = false;
                grdPesquisa.Columns[6].Visible = false;

                //Ajustando cabeçalhos
                grdPesquisa.Columns[1].HeaderText = "Usuário";
                grdPesquisa.Columns[2].HeaderText = "Nome";
                grdPesquisa.Columns[4].HeaderText = "Cadastrado em";

                //Ajustar a largura das colunas
                grdPesquisa.Columns[1].Width = 105;
                grdPesquisa.Columns[2].Width = 200;
                grdPesquisa.Columns[4].Width = 105;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message,
                        "Certo Calçados", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            usuario = new Usuario();
            usuario.Nome = txtPesquisa.Text;
            CarregarGrid();
        }
        private void grdPesquisa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                usuario = new Usuario();
                usuario.Id =
                    Convert.ToInt32(grdPesquisa.SelectedRows[0].Cells[0].Value);
                usuario.Consultar();

                PreencherFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message,
                        "Certo Calçados", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        private void PreencherFormulario()
        {
            txtUsuario.Text = usuario.Login;
            txtNome.Text = usuario.Nome;
            txtSenha.Text = usuario.Senha;
            txtConfirmacao.Text = txtSenha.Text;
            txtDataCadastro.Text =
                usuario.DataCadastro.ToShortDateString();
            chkAlterarSenha.Checked = usuario.AlterarSenha;
            rdbAtivo.Checked = usuario.Ativo;
        }
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        private void LimparCampos()
        {
            usuario = new Usuario();
            txtPesquisa.Clear();
            txtUsuario.Clear();
            txtNome.Clear();
            txtSenha.Clear();
            txtConfirmacao.Clear();
            txtDataCadastro.Clear();
            chkAlterarSenha.Checked = true;
            rdbAtivo.Checked = true;
            txtPesquisa.Focus();
            CarregarGrid();
        }
        private void PreencherClasse()
        {
            usuario.Login = txtUsuario.Text;
            usuario.Nome = txtNome.Text;
            if (usuario.Senha != txtSenha.Text)
            {
                usuario.Senha = Global.Encrypt(txtSenha.Text);
            }
            usuario.AlterarSenha = chkAlterarSenha.Checked;
            usuario.Ativo = rdbAtivo.Checked;
        }
        private string ValidarPreenchimento()
        {
            string msgErro = string.Empty;
            try
            {
                if (txtUsuario.Text == string.Empty)
                {
                    msgErro = "Preencher USUÁRIO.\n";
                }
                else
                {
                    Usuario u = new Usuario();
                    u.Login = txtUsuario.Text;
                    u.Consultar();

                    if (u.Id != 0 && u.Id != usuario.Id)
                    {
                        msgErro = "Usuário já existente.\n";
                    }
                }
                if (txtNome.Text == string.Empty)
                {
                    msgErro += "Preencher NOME.\n";
                }
                if (txtSenha.Text == string.Empty)
                {
                    msgErro += "Preencher SENHA.\n";
                }
                else if (txtSenha.Text != txtConfirmacao.Text)
                {
                    msgErro += "Confirmação da senha incorreta.\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message,
                        "Certo Calçados", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
            return msgErro;
        }
        private void btnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                string mensagem = ValidarPreenchimento();
                if (mensagem != string.Empty)
                {
                    MessageBox.Show(mensagem,
                            "Certo Calçados", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    return;
                }
                PreencherClasse();
                usuario.Gravar();

                MessageBox.Show("Usuário gravado com sucesso",
                       "Certo Calçados", MessageBoxButtons.OK,
                       MessageBoxIcon.Information);

                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message,
                        "Certo Calçados", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
    }
}
