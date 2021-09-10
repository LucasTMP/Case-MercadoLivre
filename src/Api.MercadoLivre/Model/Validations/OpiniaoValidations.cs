using Api.MercadoLivre.Model.ViewModel.Caracteristica;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.Validations
{
    public class OpiniaoValidation : AbstractValidator<Opiniao>
    {

        public OpiniaoValidation()
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

            RuleFor(parameter => parameter.Descricao)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(1, 500).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(parameter => parameter.Nota)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco ou ser zero.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .NotEqual(0).WithMessage("A {PropertyName} não pode ser zero.")
                .InclusiveBetween(1, 5).WithMessage("A {PropertyName} tem que estar entre {From} e {To}.");

        }

    }


    public class OpiniaoAddViewModelValidation : AbstractValidator<OpiniaoAddViewModel>
    {

        public OpiniaoAddViewModelValidation()
        {


            //RuleFor(parameter => parameter.ProdutoId)
            //  .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
            //  .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
            //  .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.UsuarioId)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.Titulo)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(1, 50).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(parameter => parameter.Descricao)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(1, 500).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(parameter => parameter.Nota)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco ou ser zero.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .NotEqual(0).WithMessage("A {PropertyName} não pode ser zero.")
                .InclusiveBetween(1, 5).WithMessage("A {PropertyName} tem que estar entre {From} e {To}.");

        }

    }


}
