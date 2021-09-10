using Api.CasaDoCodigo.Models.Validations;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class Usuario : Base
    {

        public Usuario(string login, string senha)
        {
            Login = login;
            Senha = senha;
            CreatedAt = DateTime.Now;
        }

        public string Login { get; private set; }
        public string Senha { get; private set; }

        public readonly DateTime CreatedAt;

        public void SetSenha(string senha)
        {
            Senha = senha;
        }


        public async Task<ValidationResult> Validar()
        {
            var validationResult = await new UsuarioValidation().ValidateAsync(this);
            return validationResult;
        }

    }
}
