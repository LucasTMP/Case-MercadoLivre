using Api.MercadoLivre.Model.Validations;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class Caracteristica : Base
    {

        public Caracteristica(Guid produtoId, string nome, string descricao)
        {
            ProdutoId = produtoId;
            Nome = nome;
            Descricao = descricao;

        }

        public Guid ProdutoId { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }

        /* EF RELATIONS */

        public Produto Produto { get; set; }

        public async Task<ValidationResult> Validar()
        {
            var validationResult = await new CaracteristicaValidation().ValidateAsync(this);
            return validationResult;
        }

    }
}
