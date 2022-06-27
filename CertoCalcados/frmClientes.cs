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
    public partial class frmClientes : Form
    {
        public frmClientes()
        {
            InitializeComponent();
        }
        bool load = false;
        Cliente cliente = new Cliente();
        private void frmClientes_Load(object sender, EventArgs e)
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
            CarregarUF();
            load = true;
        }
        private void CarregarGrid()
        {
            try
            {
                grdPesquisa.DataSource = cliente.Consultar();

                //Escondendo colunas 
                grdPesquisa.Columns[0].Visible = false;
                grdPesquisa.Columns[3].Visible = false;

                //Ajustando cabeçalhos
                grdPesquisa.Columns[1].HeaderText = "Nome";
                grdPesquisa.Columns[2].HeaderText = "CPF/CNPJ";

                //Ajustar a largura das colunas
                grdPesquisa.Columns[1].Width = 200;
                grdPesquisa.Columns[2].Width = 125;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message,
                        "Certo Calçados", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        private void CarregarUF()
        {
            try
            {
                cboUF.DataSource = Global.ConsultarUF();
                cboUF.DisplayMember = "estado";
                cboUF.ValueMember = "estado_id";
                cboUF.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message,
                        "Certo Calçados", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        private void CarregarCidade()
        {
            if (!load)
            {
                return;
            }
            try
            {
                int estadoId = Convert.ToInt32(cboUF.SelectedValue);
                cboCidade.DataSource = Global.ConsultarCidade(estadoId);
                cboCidade.DisplayMember = "cidade";
                cboCidade.ValueMember = "cidade_id";
                cboCidade.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message,
                        "Certo Calçados", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        private void cboUF_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCidade();
        }
        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            cliente = new Cliente();
            cliente.Nome = txtPesquisa.Text;
            CarregarGrid();
        }
        private void grdPesquisa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                cliente = new Cliente();
                cliente.Id = Convert.ToInt32(
                    grdPesquisa.SelectedRows[0].Cells[0].Value);
                cliente.Consultar();
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
            txtNome.Text = cliente.Nome;
            txtCPF_CNPJ.Text = cliente.CPF_CNPJ;
            txtInscricaoEstadual.Text = cliente.InscricaoEstadual;
            txtEndereco.Text = cliente.Endereco.Logradouro;
            txtNumero.Text = cliente.Endereco.Numero;
            txtComplemento.Text = cliente.Endereco.Complemento;
            txtBairro.Text = cliente.Endereco.Bairro;
            txtCEP.Text = cliente.Endereco.CEP;
            cboUF.SelectedValue = Convert.ToInt32(
                Global.ConsultarUF(cliente.Endereco.CidadeId).
                Rows[0]["estado_id"]);
            cboCidade.SelectedValue =
                Convert.ToInt32(cliente.Endereco.CidadeId);
        }
        private void PreencherClasse()
        {
            cliente.Nome = txtNome.Text;
            cliente.CPF_CNPJ = txtCPF_CNPJ.Text;
            cliente.InscricaoEstadual = txtInscricaoEstadual.Text;
            cliente.Endereco.Logradouro = txtEndereco.Text;
            cliente.Endereco.Numero = txtNumero.Text;
            cliente.Endereco.Complemento = txtComplemento.Text;
            cliente.Endereco.Bairro = txtBairro.Text;
            cliente.Endereco.CEP = txtCEP.Text;
            cliente.Endereco.CidadeId = 
                Convert.ToInt32(cboCidade.SelectedValue);
        }
        private string ValidarPreenchimento()
        {
            string msgErro = string.Empty;
            try
            {
                if (txtNome.Text == string.Empty)
                {
                    msgErro = "Preencher o campo NOME.\n";
                }
                if (txtCPF_CNPJ.Text == string.Empty)
                {
                    msgErro += "Preencher o campo CPF/CNPJ.\n";
                }
                else
                {
                    Cliente c = new Cliente();
                    c.CPF_CNPJ = txtCPF_CNPJ.Text;
                    c.Consultar();
                    if (c.Id != 0 && c.Id != cliente.Id)
                    {
                        msgErro = "CPF/CNPJ já cadastrado.\n";
                    }
                }
                if (txtEndereco.Text == string.Empty)
                {
                    msgErro += "Preencher o campo ENDEREÇO.\n";
                }
                if (txtBairro.Text==string.Empty)
                {
                    msgErro += "Preencher o campo BAIRRO.\n";
                }
                if (txtCEP.Text == string.Empty)
                {
                    msgErro += "Preencher o campo CEP.\n";
                }
                if (cboCidade.SelectedIndex==-1)
                {
                    msgErro += "Selecionar o campo CIDADE.\n";
                }
                if (cboUF.SelectedIndex==-1)
                {
                    msgErro += "Selecionar o campo UF.\n";
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message,
                        "Certo Calçados", 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
            return msgErro;
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LimparCampos()
        {
            cliente = new Cliente();
            txtPesquisa.Clear();
            txtNome.Clear();
            txtCPF_CNPJ.Clear();
            txtInscricaoEstadual.Clear();
            txtEndereco.Clear();
            txtNumero.Clear();
            txtComplemento.Clear();
            txtBairro.Clear();
            txtCEP.Clear();
            cboCidade.SelectedIndex = -1;
            cboUF.SelectedIndex = -1;
            txtPesquisa.Focus();
            CarregarGrid();
        }
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        private void btnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                String mensage = ValidarPreenchimento();
                if (mensage != string.Empty)
                {
                    MessageBox.Show(mensage,
                        "Certos Calçados", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                PreencherClasse();
                cliente.Gravar();
                MessageBox.Show("Cliente gravado com sucesso",
                    "Certos Calçados", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LimparCampos();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Erro--> " + ex.Message,
                    "Certo Calçados", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
