using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CertoCalcados
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool AlterarSenha { get; set; }
        public bool Ativo { get; set; }

        public Usuario()
        {
            Id = 0;
            Login = string.Empty;
            Nome = string.Empty;
            Senha = string.Empty;
            DataCadastro = DateTime.Now;
            AlterarSenha = false;
            Ativo = false;
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
                sql += "usuarioId, \n";
                sql += "usuario, \n";
                sql += "nome, \n";
                sql += "senha, \n";
                sql += "data_cadastro, \n";
                sql += "alterar_senha, \n";
                sql += "ativo \n";
                sql += "from tb_usuario \n";
                if (Id != 0)
                {
                    sql += "where usuarioId = @id";
                    parameters.Add(new SqlParameter("@id", Id));
                }
                else if (Login != string.Empty)
                {
                    sql += "where usuario = @usuario";
                    parameters.Add(new SqlParameter("@usuario", Login));
                }
                else if (Nome != string.Empty)
                {
                    sql += "where nome like @nome \n";
                    sql += "order by nome";
                    parameters.Add(new SqlParameter("@nome", "%" + Nome + "%"));
                }
                dataTable = acesso.Consultar(sql, parameters);

                if ((Login != string.Empty || Id != 0) &&
                    dataTable.Rows.Count > 0)
                {
                    Id = Convert.ToInt32(dataTable.Rows[0]["usuarioId"]);
                    Login = dataTable.Rows[0]["usuario"].ToString();
                    Nome = dataTable.Rows[0]["nome"].ToString();
                    Senha = dataTable.Rows[0]["senha"].ToString();
                    DataCadastro = Convert.ToDateTime(dataTable.Rows[0]["data_cadastro"]);
                    AlterarSenha = Convert.ToBoolean(dataTable.Rows[0]["alterar_senha"]);
                    Ativo = Convert.ToBoolean(dataTable.Rows[0]["ativo"]);
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
                    sql = "insert into tb_usuario \n";
                    sql += "(usuario, nome, senha, \n";
                    sql += "alterar_senha, data_cadastro, ativo)\n";
                    sql += "values (@usuario, @nome, @senha, \n";
                    sql += "@alterar_senha, @data_cadastro, @ativo)";
                    parameters.Add(new SqlParameter("@data_cadastro",
                        DataCadastro));
                }
                else
                {
                    sql = "update tb_usuario \n";
                    sql += "set \n";
                    sql += "usuario = @usuario, \n";
                    sql += "nome = @nome, \n";
                    sql += "senha = @senha, \n";
                    sql += "alterar_senha = @alterar_senha, \n";
                    sql += "ativo = @ativo \n";
                    sql += "where usuarioId = @id\n";
                    parameters.Add(new SqlParameter("@id", Id));
                }
                parameters.Add(new SqlParameter("@usuario", Login));
                parameters.Add(new SqlParameter("@nome", Nome));
                parameters.Add(new SqlParameter("@senha", Senha));
                parameters.Add(new SqlParameter("@alterar_senha", 
                    AlterarSenha));
                parameters.Add(new SqlParameter("@ativo", Ativo));
                
                acesso.Executar(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
