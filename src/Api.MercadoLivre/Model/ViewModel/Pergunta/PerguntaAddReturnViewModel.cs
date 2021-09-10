using Api.MercadoLivre.Model.Validations;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class PerguntaAddReturnViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid ProdutoId { get; set; }

    }
}
