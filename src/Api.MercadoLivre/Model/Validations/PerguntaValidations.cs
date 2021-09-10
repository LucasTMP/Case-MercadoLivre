using Api.MercadoLivre.Model.ViewModel.Caracteristica;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.Validations
{
    public class PerguntaValidation : AbstractValidator<Pergunta>
    {

        public PerguntaValidation()
        {

            RuleFor(parameter => parameter.Id)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.ProdutoId)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.UsuarioId)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.Titulo)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(1, 50).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(parameter => parameter.CreatedAt)
                .NotEmpty().WithMessage("O campo data de criação não pode estar em branco.")
                .NotNull().WithMessage("O campo data de criação não pode ser nulo.")
                .Must(date => date != default(DateTime)).WithMessage("A data de criação precisa ser válida.")
                .LessThanOrEqualTo(p => DateTime.Now).WithMessage("A data de criação deve ser a presente.");

        }

    }


    public class PerguntaAddViewModelValidation : AbstractValidator<PerguntaAddViewModel>
    {

        public PerguntaAddViewModelValidation()
        {


            RuleFor(parameter => parameter.UsuarioId)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.Titulo)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(1, 50).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

        }

    }

}
