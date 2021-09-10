using Api.CasaDoCodigo.Models.Validations;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class UsuarioLoginReturnViewModel
    {
        public string Token { get; set; }
        public double ExpiraEmSegundos { get; set; }
        public Guid Id { get; set; }
        public string Login { get; set; }

    }
}
