using Api.MercadoLivre.Model.Validations;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class Pergunta : Base
    {

        public Pergunta(string titulo, Guid produtoId, Guid usuarioId)
        {
            Titulo = titulo;
            ProdutoId = produtoId;
            UsuarioId = usuarioId;
            CreatedAt = DateTime.Now;
        }

        public string Titulo { get; private set; }
        public Guid ProdutoId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public readonly DateTime CreatedAt;

        /* EF RELATIONS */

        public Produto Produto { get; set; }
        public Usuario Usuario { get; set; }


        public async Task<ValidationResult> Validar()
        {
            return await new PerguntaValidation().ValidateAsync(this);
        }

    }
}
