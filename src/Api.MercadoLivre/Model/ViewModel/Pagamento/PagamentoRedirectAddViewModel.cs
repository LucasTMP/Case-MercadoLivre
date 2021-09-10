using Api.CasaDoCodigo.Models.Validations;
using Api.MercadoLivre.Model.Enums;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class PagamentoRedirectAddViewModel
    {

        public Guid CompraId { get; set; }
        public Guid PagamentoGatewayId { get; set; }
        public string Status { get; set; }

    }

}
