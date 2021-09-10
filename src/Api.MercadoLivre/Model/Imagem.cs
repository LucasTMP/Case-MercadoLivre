using Api.MercadoLivre.Model.Validations;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class Imagem : Base
    {

        public Imagem(string link, Guid produtoId, string nome)
        {
            Link = link;
            ProdutoId = produtoId;
            Nome = nome;
        }

        public string Nome { get; private set; }
        public string Link { get; private set; }
        public Guid ProdutoId { get; private set; }

        /* EF RELATIONS */

        public Produto Produto { get; set; }


        public async Task<ValidationResult> Validar()
        {
            var validationResult = await new ImagemValidation().ValidateAsync(this);
            return validationResult;
        }

    }
}
