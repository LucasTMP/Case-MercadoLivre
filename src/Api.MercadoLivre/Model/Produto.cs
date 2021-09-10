using Api.MercadoLivre.Model.Validations;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class Produto : Base
    {

        public Produto(Guid usuarioId, string nome, decimal valor, int quantidade, string descricao,
                       Guid categoriaId)
        {
            UsuarioId = usuarioId;
            Nome = nome;
            Valor = valor;
            Quantidade = quantidade;
            Descricao = descricao;
            CategoriaId = categoriaId;
            CreatedAt = DateTime.Now;
        }

        public Guid UsuarioId { get; private set; }
        public string Nome { get; private set; }
        public decimal Valor { get; private set; }
        public int Quantidade { get; private set; }
        public string Descricao { get; private set; }
        public Guid CategoriaId { get; private set; }
        public readonly DateTime CreatedAt;


        /* EF RELATIONS */

        public Usuario Usuario { get; set; }
        public Categoria Categoria { get; set; }
        public List<Caracteristica> Caracteristicas { get; set; }

        public void ColocarEstoque(int quantidade)
        {
            Quantidade += quantidade;
        }

        public void RetirarEstoque(int quantidade)
        {
            Quantidade -= quantidade;
        }

        public async Task<ValidationResult> Validar()
        {
            var validationResult = await new ProdutoValidation().ValidateAsync(this);
            return validationResult;
        }

    }
}
