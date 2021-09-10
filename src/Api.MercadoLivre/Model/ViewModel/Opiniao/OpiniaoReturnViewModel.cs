using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class OpiniaoReturnViewModel
    {
        public Guid Id { get; set; }
        public int Nota { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public UsuarioReturnViewModel Usuario { get; set; }

    }
}
