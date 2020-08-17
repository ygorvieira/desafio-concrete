using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Desafio_Concrete.Domain.Entities;
using Desafio_Concrete.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_Concrete.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository = new UsuarioRepository();

        [HttpPost]
        [Route("SignUp/")]
        public JsonResult SignUp()
        {
            var status = HttpStatusCode.OK;
            string mensagem = "";
            Usuario usuario = new Usuario();
            usuario = _usuarioRepository.SignUp();

            return new JsonResult(new { statusCode = status, mensagem, usuario });
        }

        [HttpPost]
        [Route("Login/")]
        public JsonResult Login(string email, string senha)
        {
            var status = HttpStatusCode.OK;
            string mensagem = "";
            Usuario usuario = new Usuario();
            usuario = _usuarioRepository.Login(email, senha);

            return new JsonResult(new { statusCode = status, mensagem, usuario });
        }

        [HttpPost]
        [Route("Profile/")]
        public JsonResult Profile(int usuarioId)
        {
            var status = HttpStatusCode.OK;
            string mensagem = "";
            Usuario usuario = new Usuario();
            usuario = _usuarioRepository.Profile(usuarioId);

            return new JsonResult(new { statusCode = status, mensagem, usuario });
        }
    }
}