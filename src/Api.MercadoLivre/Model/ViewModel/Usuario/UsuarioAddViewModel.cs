using Api.CasaDoCodigo.Models.Validations;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class UsuarioAddViewModel
    {
        public string Login { get; set; }
        public string Senha { get; set; }


        public async Task<ValidationResult> IsValid()
        {
            var validationResult = await new UsuarioAddViewModelValidation().ValidateAsync(this);
            return validationResult;
        }

    }
}
