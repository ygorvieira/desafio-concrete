using Desafio_Concrete.Domain.Entities;
using System.Collections.Generic;

namespace Desafio_Concrete.Domain.Repository
{
    public interface IUsuarioRepository
    {
        Usuario SignUp(Usuario usuario);
        Usuario Login(string email, string senha);
        Usuario Profile(string email, string senha);

        List<Usuario> GetUsers();
    }
}
