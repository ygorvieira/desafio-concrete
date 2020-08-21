using System;
using System.Collections.Generic;

namespace Desafio_Concrete.Domain.Entities
{
    public class Usuario
    {
        public Guid ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public List<Telefone> Telefones { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public DateTime UltimoLogin { get; set; }
        public string Token { get; set; }

    }
}
