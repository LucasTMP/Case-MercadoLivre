using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.MercadoLivre.Data;
using Api.MercadoLivre.Model;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Api.MercadoLivre.Repository;

namespace Api.MercadoLivre.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/categorias")]
    public class CategoriasController : BaseController
    {

        private readonly CategoriaRepository _categoriaRepository;
        public CategoriasController(ApiDbContext context,
                                  IMapper mapper,
                                  CategoriaRepository categoriaRepository) : base(context, mapper)
        {
            _categoriaRepository = categoriaRepository;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<ActionResult<List<CategoriaReturnViewModel>>> GetAll()
        {
            var categorias = await _categoriaRepository.ObterTodos();

            var categoriasReturnViewModel = _mapper.Map<List<CategoriaReturnViewModel>>(categorias);

            return Response(result: categoriasReturnViewModel);
        }


        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoriaDetalhesReturnViewModel>> GetById(Guid id)
        {
            var categoria = await GetCategoriaRelacionadas(id);
            if (categoria == null) return Response(result: new { msg = "Categoria não foi encontrada." }, 404, sucess: false);

            var categoriaDetalhesReturnViewModel = _mapper.Map<CategoriaDetalhesReturnViewModel>(categoria);

            return Response(result: categoriaDetalhesReturnViewModel);
        }


        [AllowAnonymous]
        [HttpPost("")]
        public async Task<ActionResult<CategoriaAddReturnViewModel>> Add(CategoriaAddViewModel categoriaAddViewModel)
        {
            if (!ModelState.IsValid) return ModelErrors(ModelState);
            if (await _categoriaRepository.Existe(o => o.Nome == categoriaAddViewModel.Nome)) return Response("Categoria já cadastrada.");

            if (categoriaAddViewModel.CategoriaPrincipalId != null)
                if (!await _categoriaRepository.Existe(o => o.Id == categoriaAddViewModel.CategoriaPrincipalId))
                    return Response("Categoria principal não possui cadastro.");

            var categoria = _mapper.Map<Categoria>(categoriaAddViewModel);

            var model = await categoria.Validar();
            if (!model.IsValid) return ValidationErrors(model);

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            var categoriaAddReturnViewModel = _mapper.Map<CategoriaAddReturnViewModel>(categoria);

            return Response(result: categoriaAddReturnViewModel, 201, nameof(GetById), categoria.Id);

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Categoria> GetCategoriaRelacionadas(Guid id)
        {
            var categoria = await _context.Categorias.Include(o => o.CategoriaPrincipal).ThenInclude(o => o.CategoriaPrincipal).DefaultIfEmpty().FirstOrDefaultAsync(o => o.Id == id);

            return categoria;
        }

    }
}
