using Api.CasaDoCodigo.Models.Validations;
using Api.MercadoLivre.Model.Enums;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class Compra : Base
    {

        public Compra(Guid produtoId, int quantidade, decimal valor, StatusCompra status, GatewayPagamento gatewayPagamento, Guid comprador)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
            Valor = valor;
            Status = status;
            GatewayPagamento = gatewayPagamento;
            Comprador = comprador;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; private set; }
        public decimal Valor { get; private set; }
        public StatusCompra Status { get; private set; }
        public GatewayPagamento GatewayPagamento { get; private set; }
        public Guid Comprador { get; private set; }
        public readonly DateTime CreatedAt;
        public DateTime UpdatedAt { get; private set; }

        /* EF Relations */

        public Produto Produto { get; set; }
        public Usuario Usuario { get; set; }


        public void SetStatus(StatusCompra status)
        {
            Status = status;
            SetUpdate();
        }

        public void SetValorCompra(decimal valorCompra)
        {
            Valor = valorCompra;
            SetUpdate();
        }

        public void SetUpdate()
        {
            UpdatedAt = DateTime.Now;
        }

        public async Task<ValidationResult> Validar()
        {
            var validationResult = await new CompraValidation().ValidateAsync(this);
            return validationResult;
        }

    }
}
