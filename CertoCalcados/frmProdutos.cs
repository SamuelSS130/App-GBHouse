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
    public partial class frmProdutos : Form
    {
        public frmProdutos()
        {
            InitializeComponent();
        }
        Produto produto = new Produto();
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        private void LimparCampos()
        {
            produto = new Produto();
            txtPesquisa.Clear();
            txtSku.Clear();
            txtDescricao.Clear();
            txtValorUnitario.Clear();
            txtPesquisa.Focus();

            CarregarGrid();
        }
        private void frmProdutos_Load(object sender, EventArgs e)
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
                grdPesquisa.DataSource = produto.Consultar();

                //Escondendo colunas 
                grdPesquisa.Columns[0].Visible = false;

                //Ajustando cabeçalhos
                grdPesquisa.Columns[1].HeaderText = "SKU";
                grdPesquisa.Columns[2].HeaderText = "Descrição";
                grdPesquisa.Columns[3].HeaderText = "Valor Unitário";

                //Ajustar a largura das colunas
                grdPesquisa.Columns[1].Width = 105;
                grdPesquisa.Columns[2].Width = 350;
                grdPesquisa.Columns[3].Width = 75;
                //Formatando coluna como R$ 
                grdPesquisa.Columns[3].DefaultCellStyle.Format = "C";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message,
                        "Certo Calçados", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            produto = new Produto();
            produto.Descricao = txtPesquisa.Text;

            CarregarGrid();
        }
        private void grdPesquisa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                produto = new Produto();
                produto.Id =
                Convert.ToInt32(grdPesquisa.SelectedRows[0].Cells[0].Value);
                produto.Consultar();

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
            txtSku.Text = produto.Sku.ToString();
            txtDescricao.Text = produto.Descricao;
            txtValorUnitario.Text =
                produto.ValorUnitario.ToString("C");
        }
        private void PreencherClasse()
        {
            produto.Sku = Convert.ToInt64(txtSku.Text);
            produto.Descricao = txtDescricao.Text;
            produto.ValorUnitario =
           Convert.ToDecimal(txtValorUnitario.Text.Replace("R$", ""));
        }
        private string ValidarPreenchimento()
        {
            string msgErro = string.Empty;
            try
            {
                if (txtSku.Text == string.Empty)
                {
                    msgErro = "Preencher SKU.\n";
                }
                else if (!long.TryParse(txtSku.Text, out long l_aux))
                {
                    msgErro = "SKU inválido.\n";
                }
                else
                {
                    Produto p = new Produto();
                    p.Sku = l_aux;
                    p.Consultar();

                    if (p.Id != 0 && p.Id != produto.Id)
                    {
                        msgErro = "SKU já existente.\n";
                    }
                }
                if (txtDescricao.Text == string.Empty)
                {
                    msgErro += "Preencher DESCRIÇÃO.\n";
                }
                if (txtValorUnitario.Text == string.Empty)
                {
                    msgErro += "Preencher VALOR UNITÁRIO.\n";
                }
                else if (!decimal.TryParse(
                    txtValorUnitario.Text.Replace("R$", ""), out decimal d_aux))
                {
                    msgErro += "VALOR UNITÁRIO incorreto.\n";
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
                produto.Gravar();

                MessageBox.Show("Produto gravado com sucesso",
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
