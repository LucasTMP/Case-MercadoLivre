using Api.CasaDoCodigo.Models.Validations;
using Api.MercadoLivre.Model.Enums;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class PagamentoReturnViewModel
    {
        public Guid Id { get; set; }
        public Guid CompraId { get; set; }
        public Guid PagamentoGatewayId { get; set; }
        public StatusPagamento Status { get; set; }
        public DateTime CreatedAt { get; set; }

    }

}
