using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertoCalcados
{
    public class Produto
    {
        public int Id { get; set; }
        public long Sku { get; set; }
        public string Descricao { get; set; }
        public decimal ValorUnitario { get; set; }

        public Produto()
        {
            Id = 0;
            Sku = 0;
            Descricao = string.Empty;
            ValorUnitario = 0;
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
                sql += "produto_Id, \n";
                sql += "sku, \n";
                sql += "descricao, \n";
                sql += "valor_unitario \n";
                sql += "from tb_produto \n";
                if (Id != 0)
                {
                    sql += "where produto_Id = @id";
                    parameters.Add(new SqlParameter("@id", Id));
                }
                else if (Sku != 0)
                {
                    sql += "where sku = @sku";
                    parameters.Add(new SqlParameter("@sku", Sku));
                }
                else if (Descricao != string.Empty)
                {
                    sql += "where descricao like @descricao \n";
                    sql += "order by descricao";
                    parameters.Add(new SqlParameter(
                        "@descricao", "%" + Descricao + "%"));
                }
                dataTable = acesso.Consultar(sql, parameters);

                if ((Id != 0 || Sku != 0) && dataTable.Rows.Count > 0)
                {
            Id = Convert.ToInt32(dataTable.Rows[0]["produto_Id"]);
            Sku = Convert.ToInt64(dataTable.Rows[0]["sku"]);
            Descricao = dataTable.Rows[0]["descricao"].ToString();
            ValorUnitario = 
             Convert.ToDecimal(dataTable.Rows[0]["valor_unitario"]);
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
                parameters.Clear();
                if (Id == 0)
                {
                    sql = "insert into tb_produto \n";
                    sql += "(sku, descricao, valor_unitario)\n";
                   sql += "values (@sku, @descricao, @valor_unitario)";
                }
                else
                {
                    sql = "update tb_produto \n";
                    sql += "set \n";
                    sql += "sku = @sku, \n";
                    sql += "descricao = @descricao, \n";
                    sql += "valor_unitario = @valor_unitario \n";
                    sql += "where produto_Id = @id\n";
                    parameters.Add(new SqlParameter("@id", Id));
                }
                parameters.Add(new SqlParameter("@sku", Sku));
                parameters.Add(new SqlParameter(
                    "@descricao", Descricao));
                parameters.Add(new SqlParameter(
                    "@valor_unitario", ValorUnitario));

                acesso.Executar(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
