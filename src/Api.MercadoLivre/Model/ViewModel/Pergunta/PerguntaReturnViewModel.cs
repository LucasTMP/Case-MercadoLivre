using Api.MercadoLivre.Model.Validations;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class PerguntaReturnViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public UsuarioReturnViewModel Usuario { get; set; }

    }
}
