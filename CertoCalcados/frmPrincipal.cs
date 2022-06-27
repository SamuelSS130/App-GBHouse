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
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        DateTime login;
        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            login = DateTime.Now;
            tmrTempo.Enabled = true;
            lblUsuario.Text += Global.Usuario;
            this.Left = 0;
            this.Top = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void tmrTempo_Tick(object sender, EventArgs e)
        {
            TimeSpan tempo = DateTime.Now.Subtract(login);
            lblTempo.Text = string.Format("Tempo: {0}:{1}:{2}",
                tempo.Hours.ToString("00"),
                tempo.Minutes.ToString("00"),
                tempo.Seconds.ToString("00"));
        }

        private void mnuSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Deseja encerrar a aplicação?",
                "Certo Calçados",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2)== DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void mnuUsuarios_Click(object sender, EventArgs e)
        {
            frmUsuarios form = new frmUsuarios();
            AbrirFormulario(form);
        }

        private void mnuProdutos_Click(object sender, EventArgs e)
        {
            frmProdutos form = new frmProdutos();
            AbrirFormulario(form);
        }

        private void AbrirFormulario(Form form)
        {
            bool existe = false;
            foreach (Form filho in this.MdiChildren)
            {                
                if (filho.Name == form.Name)
                {
                    filho.BringToFront();
                    existe = true;
                    break;
                }
            }
            if (!existe)
            {
                form.MdiParent = this;
                form.Show();
            }
        }

        private void mnuClientes_Click(object sender, EventArgs e)
        {
            frmClientes form = new frmClientes();
            AbrirFormulario(form);
        }
    }
}
