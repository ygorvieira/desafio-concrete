using Desafio_Concrete.Domain.Entities;

namespace Desafio_Concrete.Domain.Repository
{
    public interface IUsuarioRepository
    {
        Usuario SignUp();
        Usuario Login(string email, string senha);
        Usuario Profile(int usuarioId);
    }
}
