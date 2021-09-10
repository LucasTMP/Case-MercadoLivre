using Api.MercadoLivre.Model.Validations;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Model
{
    public class ImagemAddReturnViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Link { get; set; }

    }
}
