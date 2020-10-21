using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.Models
{
    public class UsuarioModel
    {
        public static bool ValidaUsuario(string login, string senha)
        {

            using (var conexao = new MySqlConnection())
            {
                MySqlDataReader dr;
                bool ret = false;
                //string de conexao
                conexao.ConnectionString = "server=localhost; port=3306; user id=root; database=db_estoque; password=";
                if (conexao.State == System.Data.ConnectionState.Closed)
                {
                    conexao.Open();
                }

                using (var comando = new MySqlCommand())
                {
                    try
                    {
                        comando.Connection = conexao;
                        //comando executado no banco
                        comando.CommandText = string.Format(
                            "SELECT COUNT(*) FROM usuario WHERE login = '{0}' AND senha = '{1}'", login, senha);
                        dr = comando.ExecuteReader();
                        dr.Read();
                        int v = Convert.ToInt32(dr.GetString(0));
                        if (v > 0)
                        {
                            ret = true;
                        }

                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        if (conexao.State == System.Data.ConnectionState.Open)
                        {
                            conexao.Close();
                        }
                    }

                }
                return ret;
            }
        }
    }
}