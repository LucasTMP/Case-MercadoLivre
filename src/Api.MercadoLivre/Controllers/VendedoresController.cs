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
    [Route("api/v1/vendedores")]
    public class VendedoresController : BaseController
    {
        public VendedoresController(ApiDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [AllowAnonymous]
        [HttpPost("rank/vendas")]
        public async Task<ActionResult> GetAll(VendedorAddViewModel vendedorAddViewModel)
        {
            Console.WriteLine($"O sistema de rank de vendedores recebeu: {vendedorAddViewModel.VendedorId} e {vendedorAddViewModel.CompraId}");

            return Ok($"O sistema de rank de vendedores recebeu: {vendedorAddViewModel.VendedorId} e {vendedorAddViewModel.CompraId}");
        }

    }
}
