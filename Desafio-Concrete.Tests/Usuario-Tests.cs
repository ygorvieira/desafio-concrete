using Desafio_Concrete.Domain.Repository;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Net;

namespace Desafio_Concrete.Tests
{
    [TestClass]
    public class Usuario_Tests
    {
        private readonly UsuarioRepository _usuarioRepository = new UsuarioRepository();

        [TestMethod]
        public void GetTokenTest()
        {
            var loginTest = "teste@teste.com";
            var senhaTest = "123";

            var resultado = _usuarioRepository.GetToken(loginTest, senhaTest);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void GetUsersTest()
        {
            var resultado = _usuarioRepository.GetUsers();
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void LoginTestComEmailNaoEncontrado()
        {
            var loginTest = "EmailInexistente@teste.com";
            var senhaTest = "123";
            var statusTest = HttpStatusCode.OK;

            var resultado = _usuarioRepository.Login(loginTest, senhaTest, ref statusTest);
            Assert.IsNull(resultado);
        }
    }
}
