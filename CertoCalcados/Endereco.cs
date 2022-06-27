using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CertoCalcados
{
    public class Endereco
    {
        public int Id { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public int CidadeId { get; set; }
        public int ClienteId { get; set; }
        public Endereco()
        {
            Id = 0;
            Logradouro = string.Empty;
            Numero = string.Empty;
            Complemento = string.Empty;
            Bairro = string.Empty;
            CEP = string.Empty;
            CidadeId = 0;
            ClienteId = 0;
        }

        DataTable dataTable = new DataTable();
        List<SqlParameter> parameters = new List<SqlParameter>();
        AcessoBD acesso = new AcessoBD();
        string sql = string.Empty;

        public void Consultar()
        {
            try
            {
                parameters.Clear();
                sql = "select \n";
                sql += "endereco, \n";
                sql += "numero, \n";
                sql += "complemento, \n";
                sql += "bairro, \n";
                sql += "cep, \n";
                sql += "cidade_id\n";
                sql += "from tb_endereco \n";
                sql += "where cliente_id = @clienteId";

                parameters.Add(new SqlParameter("@clienteId", ClienteId));
                dataTable = acesso.Consultar(sql, parameters);

                if (dataTable.Rows.Count > 0)
                {
                    Logradouro = dataTable.Rows[0]["endereco"].ToString();
                    Numero = dataTable.Rows[0]["numero"].ToString();
                    Complemento = dataTable.Rows[0]["complemento"].ToString();
                    Bairro = dataTable.Rows[0]["bairro"].ToString();
                    CEP = dataTable.Rows[0]["cep"].ToString();
                    CidadeId = Convert.ToInt32(dataTable.Rows[0]["cidade_id"]);
                }
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
                parameters.Clear();
                sql = "delete from tb_endereco where cliente_id = @cliente_id";
                parameters.Add(new SqlParameter("@cliente_id", ClienteId));
                acesso.Executar(sql, parameters);

                parameters.Clear();
                sql = "insert into tb_endereco \n";
                sql += "(endereco, numero, complemento, bairro, \n";
                sql += "cep, cliente_id, cidade_id)\n";
                sql += "values\n";
                sql += "(@endereco, @numero, @complemento, @bairro, \n";
                sql += "@cep, @cliente_id, @cidade_id)\n";

                parameters.Add(new SqlParameter("@endereco", Logradouro));
                parameters.Add(new SqlParameter("@numero", Numero));
                parameters.Add(new SqlParameter("@complemento", Complemento));
                parameters.Add(new SqlParameter("@bairro", Bairro));
                parameters.Add(new SqlParameter("@cep", CEP));
                parameters.Add(new SqlParameter("@cidade_id", CidadeId));
                parameters.Add(new SqlParameter("@cliente_id", ClienteId));

                acesso.Executar(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
