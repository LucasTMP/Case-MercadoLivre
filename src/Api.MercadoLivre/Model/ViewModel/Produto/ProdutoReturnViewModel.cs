using Api.MercadoLivre.Model.ViewModel.Caracteristica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.ViewModel.Produto
{
    public class ProdutoReturnViewModel
    {
        public Guid Id { get; set; }//
        public Guid UsuarioId { get; set; }//
        public string Nome { get; set; }//
        public decimal Valor { get; set; }//
        public int Quantidade { get; set; }//
        public string Descricao { get; set; }//
        public Guid CategoriaId { get; set; }//
        public string CategoriaNome { get; set; }//
        public double? MediaOpinioes { get; set; }//
        public int? TotalDeOpinioes { get; set; }//
        public int? TotalDePerguntas { get; set; }//
        public List<OpiniaoReturnViewModel>? Opinioes { get; set; }//
        public List<PerguntaReturnViewModel>? Perguntas { get; set; }//
        public List<CaracteristicaReturnViewModel> Caracteristicas { get; set; }//
        public UsuarioReturnViewModel Usuario { get; set; } //
        public List<ImagemAddReturnViewModel>? Imagens { get; set; }//
        public CategoriaDetalhesReturnViewModel? CategoriasRelacionadas { get; set; }

    }
}
