using Api.MercadoLivre.Model.ViewModel.Produto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.Validations
{
    public class ProdutoValidation : AbstractValidator<Produto>
    {

        public ProdutoValidation()
        {

            RuleFor(parameter => parameter.Id)
                .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.UsuarioId)
                .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");


            RuleFor(parameter => parameter.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(1, 50).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(parameter => parameter.Valor)
                .NotEqual(0).WithMessage("O produto não pode ser gratuito.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .GreaterThan(0).WithMessage("O campo {PropertyName} tem que ser positivo e maior que R$ 0.");

            RuleFor(parameter => parameter.Quantidade)
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .GreaterThanOrEqualTo(0).WithMessage("O campo {PropertyName} tem que ser um valor positivo ou zero.");

            RuleFor(parameter => parameter.Descricao)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(1, 1000).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(parameter => parameter.CategoriaId)
                .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.CreatedAt)
                .NotEmpty().WithMessage("O campo data de criação não pode estar em branco.")
                .NotNull().WithMessage("O campo data de criação não pode ser nulo.")
                .Must(date => date != default(DateTime)).WithMessage("A data de criação precisa ser válida.")
                .LessThanOrEqualTo(p => DateTime.Now).WithMessage("A data de criação deve ser a presente.");

        }


    }



    public class ProdutoAddViewModelValidation : AbstractValidator<ProdutoAddViewModel>
    {

        public ProdutoAddViewModelValidation()
        {

            RuleFor(parameter => parameter.UsuarioId)
                .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.");


            RuleFor(parameter => parameter.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(1, 50).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(parameter => parameter.Valor)
                .NotEqual(0).WithMessage("O produto não pode ser gratuito.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .GreaterThan(0).WithMessage("O campo {PropertyName} tem que ser positivo e maior que R$ 0.");

            RuleFor(parameter => parameter.Quantidade)
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .GreaterThanOrEqualTo(0).WithMessage("O campo {PropertyName} tem que ser um valor positivo ou zero.");

            RuleFor(parameter => parameter.Descricao)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Length(1, 1000).WithMessage("O campo {PropertyName} tem que possuir entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(parameter => parameter.CategoriaId)
                .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.");

            RuleFor(parameter => parameter.Caracteristicas)
                .NotEmpty().WithMessage("A lista de {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("A lista de {PropertyName} não pode ser nulo.");

            When(parameter => parameter.Caracteristicas != null, () =>
            {
                RuleFor(parameter => parameter.Caracteristicas)
                .Must(o => o.Count >= 3).WithMessage("A lista de {PropertyName} tem que possuir 3 ou mais caracteristicas.");

            });



        }


    }
}
