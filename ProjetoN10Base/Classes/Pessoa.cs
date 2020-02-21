using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace ProjetoN10Base.Classes
{
    public class Pessoa : AcessoDados
    {
        #region Propriedades

        public int IdPessoa { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Cidade { get; set; }
        public string EstadoUF { get; set; }
        public int Ativo { get; set; }
        public int Administrador { get; set; }
        public string FotoProfile { get; set; }
        public byte[] FotoProfileArquivo { get; set; }

        #endregion

        #region Construtores

        public Pessoa()
        {
        }

        public Pessoa(int idPessoa, string nome, string email, string senha, string cidade, string estadoUF, int ativo, int administrador)
        {
            IdPessoa = idPessoa;
            Nome = nome;
            Email = email;
            Senha = senha;
            Cidade = cidade;
            EstadoUF = estadoUF;
            Ativo = ativo;
            Administrador = administrador;
        }

        public Pessoa(string nome, string email, string senha, string cidade, string estadoUF, int ativo, int administrador)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            Cidade = cidade;
            EstadoUF = estadoUF;
            Ativo = ativo;
            Administrador = administrador;
        }

        #endregion

        #region Métodos

        //TODO - Criar método para realizar o login de uma pessoa
        public Pessoa RealizarLogin(string email, string senha)
        {
            List<MySqlParameter> parametros = new List<MySqlParameter>
            {
                new MySqlParameter("Vemail", email)
                //new MySqlParameter("Senha", senha)
            };

            try
            {
                DataSet ds = Consultar("SP_LoginPessoa", parametros);
                //TODO - Realizar a validação dos dados da pessoa
                //Verificar se o email digitado existe
                //Verificar se o usuário é ativo
                //Verificar se a senha confere
                //Se tudo der certo: "return pessoaLogada;"
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Pessoa pessoaLogada = new Pessoa
                        {
                            IdPessoa = (int)ds.Tables[0].Rows[0][0],
                            Nome = ds.Tables[0].Rows[0][1].ToString(),
                            Email = ds.Tables[0].Rows[0][2].ToString(),
                            Senha = ds.Tables[0].Rows[0][3].ToString(),
                            Cidade = ds.Tables[0].Rows[0][4].ToString(),
                            EstadoUF = ds.Tables[0].Rows[0][5].ToString(),
                            Ativo = (int)ds.Tables[0].Rows[0][6],
                            Administrador = (int)ds.Tables[0].Rows[0][7],
                            FotoProfile = ds.Tables[0].Rows[0][8].ToString()
                            //FotoProfileArquivo = (byte[])ds.Tables[0].Rows[0][9]
                        };
                        if (pessoaLogada.Ativo == 1)
                        {
                            if (pessoaLogada.Senha == senha)
                            {
                                return pessoaLogada;
                            }
                            else
                            {
                                throw new Exception("Senha não confere!");
                            }
                        }
                        else
                        {
                            throw new Exception("Usuário inativo!");
                        }
                    }
                    else
                    {
                        throw new Exception("E-mail não existe!");
                    }
                    
                }
                throw new Exception("Não há retorno do banco");
            }
            catch (Exception)
            {
                throw;
            }           
        }

        #endregion
    }
}