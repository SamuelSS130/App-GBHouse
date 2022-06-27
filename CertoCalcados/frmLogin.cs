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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text == string.Empty ||
                txtSenha.Text == string.Empty)
            {
                MessageBox.Show("Usuário e/ou senha inválidos",
                    "Login", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            try
            {
                Usuario usuario = new Usuario();
                usuario.Login = txtUsuario.Text;
                usuario.Consultar();
                if (usuario.Senha == Global.Encrypt(txtSenha.Text))
                {
                    if (usuario.Ativo)
                    {
                        MessageBox.Show("Bem vindo " + usuario.Nome,
                            "Sucesso", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        Global.UsuarioId = usuario.Id;
                        Global.Usuario = usuario.Nome;
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Usuário inativo. Acesso não permitido",
                            "Login", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuário e/ou senha inválidos",
                        "Login", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->"+ex.Message,
                        "Login", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }          
    }
}
