using Api.MercadoLivre.Model.ViewModel.Caracteristica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.ViewModel.Produto
{
    public class ProdutoAddReturnViewModel
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public string Descricao { get; set; }
        public Guid CategoriaId { get; set; }
        public string CategoriaNome { get; set; }
        public List<CaracteristicaReturnViewModel>? Caracteristicas { get; set; }

    }
}
