using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Desafio_Concrete.Domain.Entities;
using Desafio_Concrete.Domain.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_Concrete.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository = new UsuarioRepository();

        [HttpPost]
        [Route("SignUp/")]
        public JsonResult SignUp()
        {
            string mensagem = "";
            Usuario usuario = new Usuario();

            try
            {
                if (_usuarioRepository.GetUsers().FirstOrDefault(x => x.Email.Equals(usuario.Email.Trim())) != null)
                {
                    mensagem = "E-mail já existente";
                    return new JsonResult(new { statusCode = HttpStatusCode.BadRequest, mensagem});
                }

                mensagem = "Operação realizada com sucesso.";
                usuario = _usuarioRepository.SignUp(usuario);
                return new JsonResult(new { statusCode = HttpStatusCode.OK, mensagem, usuario });
            }
            catch (Exception e)
            {
                mensagem = "Erro ao efetuar operação";

                return new JsonResult(new { statusCode = HttpStatusCode.InternalServerError, mensagem});
                throw e;
            }
        }

        [HttpPost]
        [Route("Login/")]
        public JsonResult Login(string email, string senha)
        {
            var status = HttpStatusCode.OK;
            string mensagem = "";
            Usuario usuario = new Usuario();            

            try
            {                
                usuario = _usuarioRepository.Login(email, senha, ref status);

                if (usuario != null)
                {
                    mensagem = "Login efetuado com sucesso.";
                    return new JsonResult(new { statusCode = status, mensagem, usuario });
                }
                else
                {
                    mensagem = "Usuário e/ou senha inválidos";
                    return new JsonResult(new { statusCode = status, mensagem, usuario });
                }
            }
            catch (Exception e)
            {
                mensagem = "Erro ao efetuar login";
                return new JsonResult(new { statusCode = HttpStatusCode.InternalServerError, mensagem});
                throw e;
            }
        }

        [HttpPost]
        [Route("Profile/")]
        public JsonResult Profile(string email, string senha)
        {
            var status = HttpStatusCode.OK;
            string mensagem = "";
            Usuario usuario = new Usuario();

            try
            {
                usuario = _usuarioRepository.Profile(email, senha, ref status);

                if (usuario.Token == null)
                {
                    mensagem = "Não autorizado.";
                    return new JsonResult(new { statusCode = HttpStatusCode.Forbidden, mensagem, usuario });
                }
                else
                {
                    switch (status)
                    {
                        case HttpStatusCode.Forbidden:
                            mensagem = "Não autorizado.";
                            break;
                        case HttpStatusCode.GatewayTimeout:
                            mensagem = "Sessão inválida";
                            break;
                        case HttpStatusCode.NotFound:
                            mensagem = "Usuário e/ou senha inválidos.";
                            break;
                        default:
                            mensagem = "Perfil de usuário localizado com sucesso.";
                            break;
                    }

                    return new JsonResult(new { statusCode = status, mensagem, usuario });
                }              

            }
            catch (Exception e)
            {
                mensagem = "Erro ao obter perfil de usuário.";

                return new JsonResult(new { statusCode = HttpStatusCode.InternalServerError, mensagem});
                throw e;
            }
        }
    }
}