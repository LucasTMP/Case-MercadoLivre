using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.MercadoLivre.Model;
using Api.MercadoLivre.Model.Enums;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;

namespace Api.CasaDoCodigo.Models.Validations
{
    public class CompraValidation : AbstractValidator<Compra>
    {

        public CompraValidation()
        {
            RuleFor(parameter => parameter.Id)
                .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.ProdutoId)
                .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.Quantidade)
                .NotEqual(0).WithMessage("A compra tem que possuir ao menos 1 produto.")
                .NotNull().WithMessage("A {PropertyName} da compra não pode ser nula.")
                .GreaterThan(0).WithMessage("O campo {PropertyName} tem que ser positivo e maior que 0 unidades.");

            RuleFor(parameter => parameter.Valor)
                .NotEqual(0).WithMessage("A compra não pode ser gratuita.")
                .NotNull().WithMessage("O {PropertyName} da compra não pode ser nulo.")
                .GreaterThan(0).WithMessage("O campo {PropertyName} tem que ser positivo e maior que R$ 0.");

            RuleFor(parameter => parameter.Comprador)
                .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.Status)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .IsInEnum();

            RuleFor(parameter => parameter.GatewayPagamento)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .IsInEnum();

            RuleFor(parameter => parameter.CreatedAt)
                .NotEmpty().WithMessage("O campo data de criação não pode estar em branco.")
                .NotNull().WithMessage("O campo data de criação não pode ser nulo.")
                .Must(date => date != default(DateTime)).WithMessage("A data de criação precisa ser válida.")
                .LessThanOrEqualTo(p => DateTime.Now).WithMessage("A data de criação deve ser a presente.");

            RuleFor(parameter => parameter.UpdatedAt)
                .NotEmpty().WithMessage("O campo data de criação não pode estar em branco.")
                .NotNull().WithMessage("O campo data de criação não pode ser nulo.")
                .Must(date => date != default(DateTime)).WithMessage("A data de edição precisa ser válida.")
                .GreaterThanOrEqualTo(p => p.CreatedAt).WithMessage("A data de edição deve ser uma data futura.");

        }

    }

    public class CompraAddViewModelValidation : AbstractValidator<CompraAddViewModel>
    {

        public CompraAddViewModelValidation()
        {


            RuleFor(parameter => parameter.ProdutoId)
                .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.Quantidade)
                .NotEqual(0).WithMessage("A compra tem que possuir ao menos 1 produto.")
                .NotNull().WithMessage("A {PropertyName} da compra não pode ser nula.")
                .GreaterThan(0).WithMessage("O campo {PropertyName} tem que ser positivo e maior que 0 unidades.");

            RuleFor(parameter => parameter.Comprador)
                .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");


            RuleFor(parameter => parameter.GatewayPagamento)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .IsInEnum();

        }

    }

}
