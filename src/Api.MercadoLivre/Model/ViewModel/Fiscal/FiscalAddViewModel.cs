using Api.MercadoLivre.Model.Validations;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class FiscalAddViewModel
    {
        public Guid CompraId { get; set; }
        public Guid CompradorId { get; set; }

    }
}
