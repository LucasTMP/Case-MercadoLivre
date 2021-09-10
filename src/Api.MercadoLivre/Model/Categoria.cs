using Api.CasaDoCodigo.Models.Validations;
using Api.MercadoLivre.Model.Validations;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class Categoria : Base
    {

        public Categoria(string nome)
        {
            Nome = nome;
        }

        public string Nome { get; private set; }
        public Guid? CategoriaPrincipalId { get; private set; }

        /* EF RELATIONS */

        public Categoria CategoriaPrincipal { get; set; }


        public void SetCategoriaPrincipal(Guid categoriaId)
        {
            CategoriaPrincipalId = categoriaId;
        }


        public async Task<ValidationResult> Validar()
        {
            return await new CategoriaValidation().ValidateAsync(this);
        }

    }
}
