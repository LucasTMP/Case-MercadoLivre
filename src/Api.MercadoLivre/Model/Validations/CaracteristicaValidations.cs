using Api.MercadoLivre.Model.ViewModel.Caracteristica;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.Validations
{
    public class CaracteristicaValidation : AbstractValidator<Caracteristica>
    {

        public CaracteristicaValidation()
        {

            RuleFor(parameter => parameter.Id)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos."); ;

            RuleFor(parameter => parameter.ProdutoId)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(1, 50).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(parameter => parameter.Descricao)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(1, 50).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

        }

    }

    public class CaracteristicaAddViewModelValidation : AbstractValidator<CaracteristicaAddViewModel>
    {

        public CaracteristicaAddViewModelValidation()
        {

            RuleFor(parameter => parameter.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(1, 50).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(parameter => parameter.Descricao)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(1, 50).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

        }

    }

}
