using Api.MercadoLivre.Data;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {

        protected readonly ApiDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly string _pepper = "ML";
        private List<string> _errors { get; set; }

        public BaseController(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _errors = new();
        }


        protected ActionResult ValidationErrors(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddErrors(error.ErrorMessage);
            }

            return Response();
        }

        protected ActionResult ModelErrors(ModelStateDictionary modelstate)
        {
            var erros = modelstate.Values.SelectMany(e => e.Errors);

            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                AddErrors(errorMsg);
            }

            return Response();
        }

        protected void AddErrors(string error)
        {
            _errors.Add(error);
        }

        protected ActionResult Response(object result = null, int statusCode = 0, string action = "", Guid? id = null, bool sucess = false, bool erro = false)
        {
            if (OperacaoInvalida())
            {
                return BadRequest(new
                {
                    sucess = false,
                    errors = _errors
                });
            }

            if (statusCode != 0 && statusCode != 201)
            {

                if (erro == false)
                    return StatusCode(statusCode, new
                    {
                        sucess = sucess,
                        response = result
                    });

                return StatusCode(statusCode, new
                {
                    sucess = sucess,
                    errors = result
                });
            }

            if (statusCode == 201)
            {
                return CreatedAtAction(
                    action,
                    new { id },
                    new
                    {
                        sucess = true,
                        response = result
                    });
            }

            return Ok(new
            {
                sucess = true,
                response = result
            });

        }

        protected ActionResult Response(string erroMsg)
        {
            AddErrors(erroMsg);

            return Response();
        }

        protected bool OperacaoInvalida()
        {
            return _errors.Any();
        }

    }
}
