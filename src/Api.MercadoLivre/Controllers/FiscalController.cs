using Api.MercadoLivre.Data;
using Api.MercadoLivre.Model;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Controllers
{
    [Route("api/v1/fiscal")]
    public class FiscalController : BaseController
    {
        public FiscalController(ApiDbContext context,
                                IMapper mapper) : base(context, mapper)
        {
        }


        [AllowAnonymous]
        [HttpPost("compras")]
        public async Task<ActionResult> GetAll(FiscalAddViewModel fiscalAddViewModel)
        {
            Console.WriteLine($"Fiscal recebeu: {fiscalAddViewModel.CompradorId} e {fiscalAddViewModel.CompraId}");

            return Ok($"Fiscal recebeu: {fiscalAddViewModel.CompradorId} e {fiscalAddViewModel.CompraId}");
        }

    }
}
