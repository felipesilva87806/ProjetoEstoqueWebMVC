using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.Models
{
    public class GrupoProdutoModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }

        public static List<GrupoProdutoModel> RecuperLista()
        {
            var ret = new List<GrupoProdutoModel>();

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
                            "SELECT * FROM grupo_produto ORDER BY nome");
                        var reader = comando.ExecuteReader();
                        while (reader.Read())
                        {
                            ret.Add(new Models.GrupoProdutoModel
                            {
                                Id = (int)reader["id"],
                                Nome = (string)reader["nome"],
                                Ativo = (bool)reader["ativo"]
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

        public static GrupoProdutoModel RecuperarPeloId(int id)
        {
            GrupoProdutoModel ret = null;

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
                        comando.CommandText = string.Format(
                            "SELECT * FROM grupo_produto WHERE (id = {0})", id);
                        var reader = comando.ExecuteReader();
                        if (reader.Read())
                        {
                            ret = (new Models.GrupoProdutoModel
                            {
                                Id = (int)reader["id"],
                                Nome = (string)reader["nome"],
                                Ativo = (bool)reader["ativo"]
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
                            //comando executado no banco
                            comando.CommandText = string.Format(
                                "DELETE FROM grupo_produto WHERE (id = {0})", id);
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
                            comando.CommandText = string.Format(
                                "INSERT INTO grupo_produto (nome,ativo) VALUES ('{0}','{1}');", this.Nome, this.Ativo ? 1 : 0);
                            comando.ExecuteScalar();
                            comando.CommandText = "SELECT LAST_INSERT_ID();";
                            ret = Convert.ToInt32(comando.ExecuteScalar());
                        }
                        else
                        {
                            comando.Connection = conexao;
                            //comando executado no banco
                            comando.CommandText = string.Format(
                                "UPDATE grupo_produto SET nome='{1}', ativo={2} WHERE id = '{0}'",
                                this.Id, this.Nome, this.Ativo ? 1 : 0);
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