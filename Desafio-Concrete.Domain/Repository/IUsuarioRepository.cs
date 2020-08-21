using Desafio_Concrete.Domain.Entities;
using System.Collections.Generic;
using System.Net;

namespace Desafio_Concrete.Domain.Repository
{
    public interface IUsuarioRepository
    {
        Usuario SignUp(Usuario usuario);
        Usuario Login(string email, string senha, ref HttpStatusCode status);
        Usuario Profile(string email, string senha, ref HttpStatusCode status);

        List<Usuario> GetUsers();
        string GetToken(string email, string senha);
    }
}
