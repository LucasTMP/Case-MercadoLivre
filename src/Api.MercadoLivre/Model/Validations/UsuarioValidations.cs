using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.MercadoLivre.Model;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;

namespace Api.CasaDoCodigo.Models.Validations
{
    public class UsuarioValidation : AbstractValidator<Usuario>
    {

        public UsuarioValidation()
        {
            RuleFor(parameter => parameter.Id)
                .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");


            RuleFor(parameter => parameter.Senha)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(60).WithMessage("A {PropertyName} não pode ser armazenada devido a falha de segurança.");

            RuleFor(parameter => parameter.Login)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .EmailAddress(EmailValidationMode.Net4xRegex).WithMessage("O {PropertyName} precisa ser um email válido.")
                .Length(1, 50).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(parameter => parameter.CreatedAt)
                .NotEmpty().WithMessage("O campo data de criação não pode estar em branco.")
                .NotNull().WithMessage("O campo data de criação não pode ser nulo.")
                .Must(date => date != default(DateTime)).WithMessage("A data de criação precisa ser válida.")
                .LessThanOrEqualTo(p => DateTime.Now).WithMessage("A data de criação deve ser a presente.");

        }

    }

    public class UsuarioAddViewModelValidation : AbstractValidator<UsuarioAddViewModel>
    {

        public UsuarioAddViewModelValidation()
        {
            RuleFor(parameter => parameter.Senha)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(6, 12).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(parameter => parameter.Login)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .EmailAddress(EmailValidationMode.Net4xRegex).WithMessage("O {PropertyName} precisa ser um email válido.")
                .Length(1, 50).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

        }

    }

    public class UsuarioLoginViewModelValidation : AbstractValidator<UsuarioLoginViewModel>
    {

        public UsuarioLoginViewModelValidation()
        {
            RuleFor(parameter => parameter.Senha)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(6, 12).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(parameter => parameter.Login)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .EmailAddress(EmailValidationMode.Net4xRegex).WithMessage("O {PropertyName} precisa ser um email válido.")
                .Length(1, 50).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

        }

    }
}
