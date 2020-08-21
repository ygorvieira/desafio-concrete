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
                    mensagem = "Usuário já cadastrado.";
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
            string mensagem = "";
            Usuario usuario = new Usuario();            

            try
            {
                mensagem = "Login efetuado com sucesso.";
                usuario = _usuarioRepository.Login(email, senha);

                return new JsonResult(new { statusCode = HttpStatusCode.OK, mensagem, usuario });
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
            string mensagem = "";
            Usuario usuario = new Usuario();

            try
            {
                mensagem = "Perfil de usuário localizado com sucesso.";
                usuario = _usuarioRepository.Profile(email, senha);

                return new JsonResult(new { statusCode = HttpStatusCode.OK, mensagem, usuario });

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