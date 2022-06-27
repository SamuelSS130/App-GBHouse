using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CertoCalcados
{
    public class AcessoBD
    {
        SqlConnection connection;

        private void Conectar()
        {
            try
            {
                connection = new SqlConnection(Global.Conexao);
                connection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void Desconectar()
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable Consultar(string query, List<SqlParameter> parametros)
        {
            DataTable dt = new DataTable();
            try
            {
                Conectar();
                SqlCommand command = new SqlCommand(query, connection);

                foreach (SqlParameter p in parametros)
                {
                    command.Parameters.Add(p);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);             

                return dt;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
            finally
            {
                Desconectar();
            }
        }
        public int Executar(string query, List<SqlParameter> parametros)
        {
            try
            {
                Conectar();
                SqlCommand command = new SqlCommand(query, connection);

                foreach (SqlParameter p in parametros)
                {
                    command.Parameters.Add(p);
                }
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Desconectar();
            }
        }
    }
}
