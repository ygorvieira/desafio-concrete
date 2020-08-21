using System;

namespace Desafio_Concrete.Domain.Entities
{
    public class Telefone
    {
        public int ID { get; set; }
        public long Numero { get; set; }
        public int DDD { get; set; }
        public Guid UsuarioGuid { get; set; }
    }
}
