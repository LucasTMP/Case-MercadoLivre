using Api.CasaDoCodigo.Models.Validations;
using Api.MercadoLivre.Model.Enums;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class CompraAddViewModel
    {

        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public GatewayPagamento GatewayPagamento { get; set; }
        public Guid Comprador { get; set; }
        public decimal Valor { get; private set; }
        public StatusCompra Status { get; private set; }


        public void SetStatus(StatusCompra status)
        {
            Status = status;
        }

        public void SetValorCompra(decimal valorCompra)
        {
            Valor = valorCompra;
        }

    }
}
