using Api.MercadoLivre.Model.ViewModel.Caracteristica;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.Validations
{
    public class PagamentoValidation : AbstractValidator<Pagamento>
    {

        public PagamentoValidation()
        {

            RuleFor(parameter => parameter.Id)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.CompraId)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.PagamentoGatewayId)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.Status)
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .IsInEnum();

            RuleFor(parameter => parameter.CreatedAt)
                .NotEmpty().WithMessage("O campo data de criação não pode estar em branco.")
                .NotNull().WithMessage("O campo data de criação não pode ser nulo.")
                .Must(date => date != default(DateTime)).WithMessage("A data de criação precisa ser válida.")
                .LessThanOrEqualTo(p => DateTime.Now).WithMessage("A data de criação deve ser a presente.");

        }

    }


    public class PagamentoAddViewModelValidation : AbstractValidator<PagamentoAddViewModel>
    {

        public PagamentoAddViewModelValidation()
        {

            RuleFor(parameter => parameter.CompraId)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.PagamentoGatewayId)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.Status)
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .IsInEnum();
        }

    }


    public class PagamentoRedirectAddViewModelValidation : AbstractValidator<PagamentoRedirectAddViewModel>
    {

        public PagamentoRedirectAddViewModelValidation()
        {

            RuleFor(parameter => parameter.CompraId)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.PagamentoGatewayId)
              .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} está invalido.")
              .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
              .Must(o => o.ToString().Length == 36).WithMessage("O campo {PropertyName} tem que possuir 36 digitos.");

            RuleFor(parameter => parameter.Status)
                .NotEmpty().WithMessage("O campo {PropertyName} não pode estar em branco.")
                .NotNull().WithMessage("O campo {PropertyName} não pode ser nulo.")
                .Must(o => o == "SUCESSO" || o == "ERRO").WithMessage("O campo {PropertyName} só possui 2 opçoes: SUCESSO ou ERRO");
        }

    }


}
