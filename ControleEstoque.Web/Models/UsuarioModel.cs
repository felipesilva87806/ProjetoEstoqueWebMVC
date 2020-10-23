using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe o login.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Informe a senha.")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Informe o nome.")]
        public string Nome { get; set; }

        public static UsuarioModel ValidaUsuario(string login, string senha)
        {

            using (var conexao = new MySqlConnection())
            {
                MySqlDataReader dr;
                UsuarioModel ret = null;
                //string de conexao que está dentro do arquivo Web.config
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
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
                        comando.CommandText = "SELECT * FROM usuario WHERE login = @login AND senha = MD5(@senha)";
                        comando.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
                        comando.Parameters.Add("@senha", MySqlDbType.VarChar).Value = senha;
                        dr = comando.ExecuteReader();
                        if (dr.Read())
                        {
                            ret = new UsuarioModel
                            {
                                Id = (int)dr["id"],
                                Nome = (string)dr["nome"],
                                Senha = (string)dr["senha"],
                                Login = (string)dr["login"]
                            };
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


        public static List<UsuarioModel> RecuperLista()
        {
            var ret = new List<UsuarioModel>();

            using (var conexao = new MySqlConnection())
            {

                //string de conexao que está dentro do arquivo Web.config
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
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
                        comando.CommandText = (
                            "SELECT * FROM usuario ORDER BY nome");
                        var reader = comando.ExecuteReader();
                        while (reader.Read())
                        {
                            ret.Add(new Models.UsuarioModel
                            {
                                Id = (int)reader["id"],
                                Nome = (string)reader["nome"],
                                Login = (string)reader["login"]
                            });
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

        public static UsuarioModel RecuperarPeloId(int id)
        {
            UsuarioModel ret = null;

            using (var conexao = new MySqlConnection())
            {

                //string de conexao que está dentro do arquivo Web.config
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                if (conexao.State == System.Data.ConnectionState.Closed)
                {
                    conexao.Open();
                }

                using (var comando = new MySqlCommand())
                {
                    try
                    {
                        comando.Connection = conexao;
                        comando.CommandText = "SELECT * FROM usuario WHERE id = @id";
                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                        var reader = comando.ExecuteReader();
                        if (reader.Read())
                        {
                            ret = (new Models.UsuarioModel
                            {
                                Id = (int)reader["id"],
                                Nome = (string)reader["nome"],
                                Login = (string)reader["login"]
                            });
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
        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            using (var conexao = new MySqlConnection())
            {

                //string de conexao
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                if (conexao.State == System.Data.ConnectionState.Closed)
                {
                    conexao.Open();
                }
                if (RecuperarPeloId(id) != null)
                {
                    using (var comando = new MySqlCommand())
                    {
                        try
                        {
                            comando.Connection = conexao;
                            comando.CommandText = "DELETE FROM usuario WHERE id = @id";
                            comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                            ret = (comando.ExecuteNonQuery() > 0);

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
                }

                return ret;
            }
        }

        public int Salvar()
        {
            var ret = 0;
            var model = RecuperarPeloId(this.Id);
            using (var conexao = new MySqlConnection())
            {

                //string de conexao que está dentro do arquivo Web.config
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                if (conexao.State == System.Data.ConnectionState.Closed)
                {
                    conexao.Open();
                }
                using (var comando = new MySqlCommand())
                {
                    try
                    {
                        if (model == null)
                        {
                            comando.Connection = conexao;
                            //comando executado no banco
                            comando.CommandText = "INSERT INTO usuario (login,senha,nome) VALUES (@login,MD5(@senha),@nome);";
                            comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                            comando.Parameters.Add("@senha", MySqlDbType.VarChar).Value = this.Senha;
                            comando.Parameters.Add("@login", MySqlDbType.VarChar).Value = this.Login;
                            comando.ExecuteScalar();
                            comando.CommandText = "SELECT LAST_INSERT_ID();";
                            ret = Convert.ToInt32(comando.ExecuteScalar());
                        }
                        else
                        {
                            comando.Connection = conexao;
                            comando.CommandText = "UPDATE usuario SET nome=@nome, login=@login " +
                               (!string.IsNullOrEmpty(this.Senha) ? ",senha = md5(@senha)" : "") +
                               " WHERE id = @id";
                            comando.Parameters.Add("@id", MySqlDbType.Int32).Value = this.Id;
                            comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                            comando.Parameters.Add("@login", MySqlDbType.VarChar).Value = this.Login;

                            if (!string.IsNullOrEmpty(this.Senha))
                            {
                                comando.Parameters.Add("@senha", MySqlDbType.VarChar).Value = this.Senha;
                            }


                            if (comando.ExecuteNonQuery() > 0)
                            {
                                ret = this.Id;
                            }
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