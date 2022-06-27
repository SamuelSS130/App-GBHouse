using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CertoCalcados
{
    public static class Global
    {
        public static int UsuarioId;
        public static string Usuario;
        public static string Conexao = ConnectionString();

        public static string Encrypt(string strSenha)
        {
            Byte[] byteTamanhoOriginal;
            Byte[] byteTamanhoCriptografado;
            MD5 md5;

            // Conver the original password to bytes; then create the hash
            md5 = new MD5CryptoServiceProvider();
            byteTamanhoOriginal = ASCIIEncoding.Default.GetBytes(strSenha);
            byteTamanhoCriptografado = md5.ComputeHash(byteTamanhoOriginal);

            // Bytes to string
            return Regex.Replace(BitConverter.ToString(byteTamanhoCriptografado), "-", "").ToLower();
        }
        private static string ConnectionString()
        {
            string Servidor = string.Empty;
            string Banco = string.Empty;
            string Conexao = string.Empty;

            Servidor = ConfigurationManager.AppSettings["Servidor"];
            Banco = ConfigurationManager.AppSettings["Banco"];

            Conexao = string.Format("Data Source={0};" +
                "Initial Catalog={1};" +
                "Integrated Security=true;",
                Servidor, Banco);

            return Conexao;
        }

        public static DataTable ConsultarUF(int CidadeId = 0)
        {
            DataTable dataTable = new DataTable();
            List<SqlParameter> parameters = new List<SqlParameter>();
            AcessoBD acesso = new AcessoBD();
            string sql = string.Empty;

            try
            {
                parameters.Clear();
                if (CidadeId == 0)
                {
                    sql = "select estado_id, estado \n";
                    sql += "from tb_estado\n";
                }
                else
                {
                    sql = "select estado_id \n";
                    sql += "from tb_cidade \n";
                    sql += "where cidade_id = @cidade_id";
                    parameters.Add(new SqlParameter("@cidade_id", CidadeId));
                }
                dataTable = acesso.Consultar(sql, parameters);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataTable ConsultarCidade(int UFId)
        {
            DataTable dataTable = new DataTable();
            List<SqlParameter> parameters = new List<SqlParameter>();
            AcessoBD acesso = new AcessoBD();
            string sql = string.Empty;

            try
            {
                parameters.Clear();
                sql = "select cidade_id, cidade \n";
                sql += "from tb_cidade \n";
                sql += "where estado_id = @estado_id";
                parameters.Add(new SqlParameter("@estado_id", UFId));
                dataTable = acesso.Consultar(sql, parameters);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
