using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Desafio_Concrete.Domain.Entities;

namespace Desafio_Concrete.Domain.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string connectionString = "";

        public List<Usuario> GetUsers()
        {
            using(var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM USERS";
                var lstUsuarios = connection.Query<Usuario>(query).ToList();

                return lstUsuarios;
            }
        }

        public Usuario Login(string email, string senha)
        {
            var usuario = GetUsers().FirstOrDefault(x => x.Email.Equals(email));

            try
            {
                if (usuario == null)
                {

                }
                else
                {
                    usuario.UltimoLogin = DateTime.Today;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return usuario;
        }

        public Usuario Profile(string email, string senha)
        {
            throw new System.NotImplementedException();
        }

        public Usuario SignUp(Usuario usuario)
        {
            try
            {
                if (GetUsers().FirstOrDefault(x => x.Email.Equals(usuario.Email.Trim())) == null) 
                {

                }
                else
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        usuario.DataCriacao = DateTime.Today;
                        usuario.DataAtualizacao = DateTime.Today;
                        usuario.UltimoLogin = DateTime.Today;

                        connection.Open();
                        var query = @"INSERT INTO USERS() VALUES()";
                        connection.Execute(query, usuario);
                    }

                }
            }
            catch (Exception)
            {
                return null;
            }

            return usuario;
        }
    }
}
