﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model.ViewModel.Caracteristica
{
    public class CaracteristicaBindViewModel
    {
        public Guid? ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

    }
}
