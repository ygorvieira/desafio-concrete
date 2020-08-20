using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Dapper;
using Desafio_Concrete.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Desafio_Concrete.Domain.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string connectionString = "";

        public string GetToken(string email, string senha)
        {
            var direitos = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email, senha),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("desafio-concrete-authentication-valid"));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: direitos,
                signingCredentials: credenciais,
                expires: DateTime.Now.AddMinutes(30)
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

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
                    usuario.Token = GetToken(email, senha);
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
                        usuario.Token = GetToken(usuario.Email, usuario.Senha);
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
