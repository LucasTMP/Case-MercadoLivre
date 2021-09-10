using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class OpiniaoAddViewModel
    {
        public int Nota { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public Guid ProdutoId { get; private set; }
        public Guid UsuarioId { get; set; }

        public void SetProdutoId(Guid id)
        {
            ProdutoId = id;
        }

    }
}
