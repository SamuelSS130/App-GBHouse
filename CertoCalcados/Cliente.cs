using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace CertoCalcados
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF_CNPJ { get; set; }
        public string InscricaoEstadual { get; set; }
        public Endereco Endereco { get; set; }
        public Cliente()
        {
            Id = 0;
            Nome = string.Empty;
            CPF_CNPJ = string.Empty;
            InscricaoEstadual = string.Empty;
            Endereco = new Endereco();
        }
        DataTable dataTable = new DataTable();
        List<SqlParameter> parameters = new List<SqlParameter>();
        AcessoBD acesso = new AcessoBD();
        string sql = string.Empty;

        public DataTable Consultar()
        {
            try
            {
                parameters.Clear();
                sql = "select \n";
                sql += "cliente_id, \n";
                sql += "nome, \n";
                sql += "cpf_cnpj, \n";
                sql += "inscricao_estadual \n";
                sql += "from tb_cliente \n";

                if (Id != 0)
                {
                    sql += "where cliente_id = @id \n";
                    parameters.Add(new SqlParameter("@id", Id));
                }
                else if (CPF_CNPJ != string.Empty)
                {
                    sql += "where cpf_cnpj = @cpf_cnpj \n";
                    parameters.Add(new SqlParameter("@cpf_cnpj", CPF_CNPJ));
                }
                else if (Nome != string.Empty)
                {
                    sql += "where nome like @nome \n";

                    parameters.Add(new SqlParameter("@nome", "%" + Nome + "%"));
                }
                sql += "order by nome";
                dataTable = acesso.Consultar(sql, parameters);

                if ((Id != 0 || CPF_CNPJ != string.Empty) &&
                    dataTable.Rows.Count > 0)
                {
                    Id = Convert.ToInt32(dataTable.Rows[0]["cliente_id"]);
                    Nome = dataTable.Rows[0]["nome"].ToString();
                    CPF_CNPJ = dataTable.Rows[0]["cpf_cnpj"].ToString();
                    InscricaoEstadual =
                        dataTable.Rows[0]["inscricao_estadual"].ToString();
                    Endereco.ClienteId = Id;
                    Endereco.Consultar();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Gravar()
        {
            try
            {
                using (TransactionScope transacao = new TransactionScope())
                {
                    parameters.Clear();
                    if (Id ==0)
                    {
                        sql = "insert tb_cliente \n";
                        sql += "(nome, cpf_cnpj, incricao_estadual)\n";
                        sql += "values (@nome, @cpf_cnpj, @inscricao_estadual); \n";
                        sql += "select @@identity";

                    }
                    else
                    {
                        sql = "update tb_cliente set \n";
                        sql += "nome = @nome, \n";
                        sql += "cpf_cnpj = cpf_cnpj,\n";
                        sql += "inscricao_estadual = @inscricao_estadual \n";
                        sql += " where clinte_Id = @Id \n";
                        parameters.Add(new SqlParameter("@id", Id));
                    }
                    parameters.Add(new SqlParameter("@nome", Nome));
                    parameters.Add(new SqlParameter("@cpf_cnpj", CPF_CNPJ));
                    parameters.Add(new SqlParameter("inscricao_estadual", InscricaoEstadual));

                    if (Id == 0)
                    {
                        Id = acesso.Executar(sql, parameters);
                        Endereco.ClienteId = Id;
                    }
                    else
                    {
                        acesso.Executar(sql, parameters);
                    }
                    Endereco.Gravar();

                    transacao.Complete();

                }
            }
            catch (Exception ex)
            {

            }
        }
       
    }
}
