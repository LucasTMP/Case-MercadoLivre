using Api.MercadoLivre.Model.Validations;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class Opiniao : Base
    {

        public Opiniao(int nota, string titulo, string descricao, Guid produtoId, Guid usuarioId)
        {
            Nota = nota;
            Titulo = titulo;
            Descricao = descricao;
            ProdutoId = produtoId;
            UsuarioId = usuarioId;

        }

        public int Nota { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public Guid ProdutoId { get; private set; }
        public Guid UsuarioId { get; private set; }

        /* EF RELATIONS */

        public Produto Produto { get; set; }
        public Usuario Usuario { get; set; }

        public async Task<ValidationResult> Validar()
        {

            return await new OpiniaoValidation().ValidateAsync(this);
        }


    }
}
