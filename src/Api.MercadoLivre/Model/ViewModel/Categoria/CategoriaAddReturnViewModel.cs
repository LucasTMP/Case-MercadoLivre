﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class CategoriaAddReturnViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public Guid? CategoriaPrincipalId { get; set; }

    }
}
