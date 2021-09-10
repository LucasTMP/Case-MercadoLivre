using Api.CasaDoCodigo.Models.Validations;
using Api.MercadoLivre.Model.Enums;
using Api.MercadoLivre.Model.ViewModel.Produto;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class CompraReturnViewModel
    {

        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public StatusCompra Status { get; set; }
        public GatewayPagamento GatewayPagamento { get; set; }
        //public Guid Comprador { get; set; }

        public readonly DateTime CreatedAt;

        public readonly DateTime UpdatedAt;

        public ProdutoCompraReturnViewModel Produto { get; set; }
        public UsuarioReturnViewModel Comprador { get; set; }

    }
}
