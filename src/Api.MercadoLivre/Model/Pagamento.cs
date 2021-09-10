using Api.MercadoLivre.Model.Enums;
using Api.MercadoLivre.Model.Validations;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class Pagamento : Base
    {

        public Pagamento(Guid compraId, Guid pagamentoGatewayId, StatusPagamento status)
        {
            CompraId = compraId;
            PagamentoGatewayId = pagamentoGatewayId;
            Status = status;
            CreatedAt = DateTime.Now;
        }

        public Guid CompraId { get; private set; }
        public Guid PagamentoGatewayId { get; private set; }
        public StatusPagamento Status { get; private set; }

        public readonly DateTime CreatedAt;


        /* EF Relations */

        public Compra Compra { get; set; }



        public void SetStatus(StatusPagamento status)
        {
            if (Status == StatusPagamento.SUCESSO) return;

            Status = status;
        }


        public async Task<ValidationResult> Validar()
        {
            var validationResult = await new PagamentoValidation().ValidateAsync(this);
            return validationResult;
        }

    }
}
