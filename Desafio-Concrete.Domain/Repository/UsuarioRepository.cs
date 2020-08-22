using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Desafio_Concrete.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Desafio_Concrete.Domain.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string connectionString = "Data Source=den1.mssql8.gear.host; Initial Catalog=desafioconcrete; User Id=desafioconcrete; Password=Uf2rX-oZ!Oi6";

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
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM USERS";
                var lstUsuarios = connection.Query<Usuario>(query).ToList();

                return lstUsuarios;
            }
        }

        public Usuario Login(string email, string senha, ref HttpStatusCode status)
        {
            var hash = new Hash(SHA512.Create());
            var usuario = GetUsers().FirstOrDefault(x => x.Email.Equals(email));

            try
            {
                if (usuario == null)
                {
                    status = HttpStatusCode.NotFound;
                    return null;
                }
                if (!hash.VerificarSenha(hash.CriptografarSenha(senha), usuario.Senha))
                {
                    status = HttpStatusCode.Forbidden;
                    return null;
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

        public Usuario Profile(string email, string senha, ref HttpStatusCode status)
        {
            var hash = new Hash(SHA512.Create());
            var usuario = GetUsers().FirstOrDefault(x => x.Email.Equals(email));

            try
            {
                if (usuario == null)
                {
                    status = HttpStatusCode.NotFound;
                    return null;
                }
                if (!hash.VerificarSenha(hash.CriptografarSenha(senha), usuario.Senha))
                {
                    status = HttpStatusCode.Forbidden;
                    return null;
                }

                CultureInfo culture = new CultureInfo("pt-BR");
                TimeSpan intervalo = DateTime.Now - usuario.UltimoLogin;

                if (decimal.Round(decimal.Parse(intervalo.TotalMinutes.ToString())) >= 30)
                {
                    status = HttpStatusCode.GatewayTimeout;
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return usuario;
        }

        public Usuario SignUp(Usuario usuario)
        {
            var hash = new Hash(SHA512.Create());
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    usuario.Token = GetToken(usuario.Email, usuario.Senha);
                    usuario.Senha = hash.CriptografarSenha(usuario.Senha);                    
                    usuario.DataCriacao = DateTime.Today;
                    usuario.DataAtualizacao = DateTime.Today;
                    usuario.UltimoLogin = DateTime.Today;

                    connection.Open();
                    var queryUsuario = @"INSERT INTO USERS(Nome, Email, Senha, DataCriacao, DataAtualizacao, UltimoLogin, Token) VALUES(@Nome, 
                                                            @Email, @Senha, @DataCriacao, @DataAtualizacao, @UltimoLogin, @Token)";

                    connection.Execute(queryUsuario, usuario);

                    foreach (var item in usuario.Telefones)
                    {                        
                        var getUserID = "SELECT ID FROM USERS WHERE Email = '" + usuario.Email + "'";
                        var queryID = connection.Query(getUserID).FirstOrDefault();
                        item.UsuarioID = queryID.ID;

                        var telefone = new Telefone()
                        {
                            Numero = item.Numero,
                            DDD = item.DDD,
                            UsuarioID = item.UsuarioID
                        };

                        var queryTelefones = string.Format(@"INSERT INTO TELEFONES(Numero, DDD, UsuarioID) VALUES(@Numero, @DDD, @UsuarioID)");
                        connection.Execute(queryTelefones, telefone);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return usuario;
        }
    }
}
