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
using Api.MercadoLivre.Model.Enums;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Api.MercadoLivre.Extensions;

namespace Api.MercadoLivre.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/compras")]
    public class ComprasController : BaseController
    {

        private readonly CompraRepository _compraRepository;
        private readonly ProdutoRepository _produtoRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly PagamentoRepository _pagamentoRepository;


        public ComprasController(ApiDbContext context,
                                  IMapper mapper,
                                  ProdutoRepository produtoRepository,
                                  UsuarioRepository usuarioRepository,
                                  CompraRepository compraRepository,
                                  PagamentoRepository pagamentoRepository) : base(context, mapper)
        {
            _compraRepository = compraRepository;
            _produtoRepository = produtoRepository;
            _usuarioRepository = usuarioRepository;
            _pagamentoRepository = pagamentoRepository;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<ActionResult<List<CompraReturnViewModel>>> GetAll()
        {
            var compras = await _compraRepository.GetComprasProdutoComprador();

            var comprasReturnViewModel = _mapper.Map<List<CompraReturnViewModel>>(compras);

            return Response(result: comprasReturnViewModel);
        }


        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CompraReturnViewModel>> GetById(Guid id)
        {
            var compra = await _compraRepository.GetCompraProdutoComprador(id);
            if (compra == null) return Response(result: new { msg = "Compra não foi encontrada." }, 404, sucess: false);

            var compraReturnViewModel = _mapper.Map<CompraReturnViewModel>(compra);

            return Response(result: compraReturnViewModel);
        }


        [AllowAnonymous]
        [HttpPost("")]
        public async Task<ActionResult<CompraAddViewModel>> Add(CompraAddViewModel compraAddViewModel)
        {
            if (!ModelState.IsValid) return ModelErrors(ModelState);

            if (!await _produtoRepository.Existe(o => o.Id == compraAddViewModel.ProdutoId))
                return Response(result: new { msg = "Produto não foi encontrado." }, 404, sucess: false, erro: true);

            if (!await _usuarioRepository.Existe(o => o.Id == compraAddViewModel.Comprador))
                return Response(result: new { msg = "O comprador não foi encontrado no sistema." }, 404, sucess: false, erro: true);

            var produto = await _produtoRepository.ObterPorId(compraAddViewModel.ProdutoId);
            if (produto == null) return Response("O Produto não está mais disponivel para compra.");

            if (produto.Quantidade < compraAddViewModel.Quantidade || produto.Quantidade == 0)
                return Response("O Produto não possui estoque para essa compra.");

            compraAddViewModel.SetValorCompra(compraAddViewModel.Quantidade * produto.Valor);
            compraAddViewModel.SetStatus(StatusCompra.Iniciada);
            var compra = _mapper.Map<Compra>(compraAddViewModel);

            var modelCompra = await compra.Validar();
            if (!modelCompra.IsValid) return ValidationErrors(modelCompra);

            produto.RetirarEstoque(compraAddViewModel.Quantidade);

            _context.Compras.Add(compra);
            var result = await _context.SaveChangesAsync();

            if (result <= 0) return Response(result: new { msg = "Falha no servidor, compra não efetuada." }, 500, sucess: false, erro: true);

            if (!await EnviarEmailCompra(compraAddViewModel.Comprador, produto, compra)) Console.WriteLine("Não foi possivel enviar o email.");
            //if (!await EmailFake(compraAddViewModel.Comprador, produto, compra)) Console.WriteLine("Não foi possivel enviar o email.");

            var ulrResponse = "";
            if (compraAddViewModel.GatewayPagamento == GatewayPagamento.Paypal) ulrResponse =
                    $"https://paypal.com/{compra.Id}?redirectUrl=https://localhost:5001/api/v1/compras/{compra.Id}/pagamentos";

            if (compraAddViewModel.GatewayPagamento == GatewayPagamento.Pagseguro) ulrResponse =
                    $"https://pagseguro.com?returnId={compra.Id}&redirectUrl=https://localhost:5001/api/v1/compras/{compra.Id}/v2/pagamentos";

            return Redirect(ulrResponse);

        }



        [AllowAnonymous]
        [HttpPost("{id:guid}/pagamentos")]
        public async Task<ActionResult<PagamentoReturnViewModel>> Add([FromRoute] Guid id, PagamentoAddViewModel pagamentoAddViewModel)
        {

            if (!ModelState.IsValid) return ModelErrors(ModelState);

            if (id != pagamentoAddViewModel.CompraId) return Response("A compra informada não confere com a url.");

            if (!await _compraRepository.Existe(o => o.Id == id))
                return Response(result: new { msg = "A compra não foi encontrada." }, 404, sucess: false, erro: true);

            var pagamentoSucesso = await _pagamentoRepository.BuscarTransacaoSucesso(pagamentoAddViewModel.PagamentoGatewayId);
            if (pagamentoSucesso != null)
                return Response("Essa transação já foi paga.");

            var compraPaga = await _pagamentoRepository.BuscarCompraPaga(pagamentoAddViewModel.CompraId);
            if (compraPaga != null)
                return Response("Essa compra já foi paga.");

            var pagamento = _mapper.Map<Pagamento>(pagamentoAddViewModel);

            var modelPagamento = await pagamento.Validar();
            if (!modelPagamento.IsValid) return ValidationErrors(modelPagamento);

            var compra = await _compraRepository.ObterPorId(id);
            compra.SetStatus(StatusCompra.EmSeparacao);

            _context.Add(pagamento);
            await _context.SaveChangesAsync();

            if (pagamentoAddViewModel.Status == StatusPagamento.ERRO)
            {
                if (!await EnviarEmailPagamentoFalhou(pagamento)) Console.WriteLine("Não foi possivel enviar o email.");
                //if (!await EmailFakePagamentoFalhou(pagamento)) Console.WriteLine("Não foi possivel enviar o email.");
                return Response(result: "Erro no pagamento da compra.");
            }

            if (!await EnviarEmailPagamentoConcluido(pagamento)) Console.WriteLine("Não foi possivel enviar o email.");
            //if (!await EmailFakePagamentoConcluido(pagamento)) Console.WriteLine("Não foi possivel enviar o email.");

            await EnviarFiscal(pagamento);
            await EnviarVendedorRank(pagamento);

            var pagamentoReturnViewModel = _mapper.Map<PagamentoReturnViewModel>(pagamento);


            return Response(pagamentoReturnViewModel);
        }



        [AllowAnonymous]
        [HttpPost("{id:guid}/v2/pagamentos")]
        public async Task<ActionResult> Add([FromRoute] Guid id, PagamentoRedirectAddViewModel pagamentoRedirectAddViewModel)
        {

            if (!ModelState.IsValid) return ModelErrors(ModelState);

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:5001/api/v1/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var jsonRequest = new PagamentoAddViewModel
            {
                CompraId = pagamentoRedirectAddViewModel.CompraId,
                PagamentoGatewayId = pagamentoRedirectAddViewModel.PagamentoGatewayId,
                Status = pagamentoRedirectAddViewModel.Status == "SUCESSO" ? StatusPagamento.SUCESSO : StatusPagamento.ERRO
            };

            var response = await httpClient.PostAsJsonAsync($"compras/{pagamentoRedirectAddViewModel.CompraId}/pagamentos", jsonRequest);

            if (!response.IsSuccessStatusCode)
            {
                return Response("Erro no pagamento da compra.");
            }

            var reponseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<Raiz>(reponseString);

            return Ok(jsonResponse);
        }




        [AllowAnonymous]
        [HttpGet("{id:guid}/pagamentos")]
        public async Task<ActionResult<List<CompraReturnViewModel>>> GetAll([FromRoute] Guid id)
        {
            if (!await _compraRepository.Existe(o => o.Id == id))
                return Response(result: new { msg = "A compra não foi encontrada." }, 404, sucess: false, erro: true);

            var pagamentos = await _pagamentoRepository.Buscar(o => o.CompraId == id);

            var pagamentosReturnViewModel = _mapper.Map<List<PagamentoReturnViewModel>>(pagamentos);

            return Response(result: pagamentosReturnViewModel);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<bool> EnviarEmailCompra(Guid compradorId, Produto produto, Compra compra)
        {

            var comprador = await _usuarioRepository.ObterPorId(compradorId);
            if (comprador == null) return false;

            var vendedor = await _usuarioRepository.ObterPorId(produto.UsuarioId);
            if (vendedor == null) return false;

            // Credentials
            var credentials = new NetworkCredential("desafiomercadolivre22@gmail.com", "lucasvaivoa");

            // Mail message
            var mail = new MailMessage()
            {
                From = new MailAddress("desafiomercadolivre22@gmail.com"),
                Subject = "Desafio 2 - Mercado Livre",
                Body = @$"Olá {vendedor.Login}, o produto {produto.Nome} recebeu um pedido de compra:
                 Comprador: {comprador.Login}
                 Data do Pedido: {compra.CreatedAt}
                 Quantidade: {compra.Quantidade}
                 Valor do Pedido: {compra.Valor}
                 Link para o produto: https://localhost:5001/api/v1/produtos/{produto.Id}"
            };

            mail.IsBodyHtml = true;
            mail.CC.Add(new MailAddress("desafiomercadolivre22@gmail.com"));
            //mail.To.Add(new MailAddress("desafiomercadolivre22@gmail.com"));
            mail.To.Add(new MailAddress(vendedor.Login));

            // Smtp client
            var client = new SmtpClient()
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Credentials = credentials
            };
            client.Send(mail);
            Console.WriteLine("Email enviado com sucesso!");
            return true;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<bool> EnviarEmailPagamentoConcluido(Pagamento pagamento)
        {

            var pagamentoWithInclude = await _pagamentoRepository.BuscarPagamentoCompraProdutoUsuario(pagamento.CompraId);
            if (pagamentoWithInclude == null) return false;

            // Credentials
            var credentials = new NetworkCredential("desafiomercadolivre22@gmail.com", "lucasvaivoa");

            // Mail message
            var mail = new MailMessage()
            {
                From = new MailAddress("desafiomercadolivre22@gmail.com"),
                Subject = "Desafio 2 - Mercado Livre",
                Body = @$"Olá {pagamentoWithInclude.Compra.Usuario.Login}, o pagamento da compra do produto {pagamentoWithInclude.Compra.Produto.Nome} foi concluida com sucesso:
                 Data do pagamento: {pagamentoWithInclude.CreatedAt}
                 Data da compra: {pagamentoWithInclude.Compra.CreatedAt}
                 Valor da compra: {pagamentoWithInclude.Compra.Valor}
                 Produto: {pagamentoWithInclude.Compra.Produto.Nome}
                 Quantidade: {pagamentoWithInclude.Compra.Quantidade}
                 Valor Unitario: {pagamentoWithInclude.Compra.Produto.Valor}
                 Status da compra: {pagamentoWithInclude.Compra.Status}"
            };

            mail.IsBodyHtml = true;
            mail.CC.Add(new MailAddress("desafiomercadolivre22@gmail.com"));
            //mail.To.Add(new MailAddress("desafiomercadolivre22@gmail.com"));
            mail.To.Add(new MailAddress(pagamentoWithInclude.Compra.Usuario.Login));

            // Smtp client
            var client = new SmtpClient()
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Credentials = credentials
            };
            client.Send(mail);
            Console.WriteLine("Email enviado com sucesso!");
            return true;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<bool> EmailFakePagamentoConcluido(Pagamento pagamento)
        {

            var pagamentoWithInclude = await _pagamentoRepository.BuscarPagamentoCompraProdutoUsuario(pagamento.CompraId);
            if (pagamentoWithInclude == null) return false;


            Console.WriteLine(@$"Olá {pagamentoWithInclude.Compra.Usuario.Login}, o pagamento da compra do produto {pagamentoWithInclude.Compra.Produto.Nome} foi concluida com sucesso:
                 Data do pagamento: {pagamentoWithInclude.CreatedAt}
                 Data da compra: {pagamentoWithInclude.Compra.CreatedAt}
                 Valor da compra: {pagamentoWithInclude.Compra.Valor}
                 Produto: {pagamentoWithInclude.Compra.Produto.Nome}
                 Quantidade: {pagamentoWithInclude.Compra.Quantidade}
                 Valor Unitario: {pagamentoWithInclude.Compra.Produto.Valor}
                 Status da compra: {pagamentoWithInclude.Compra.Status}");

            return true;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<bool> EnviarEmailPagamentoFalhou(Pagamento pagamento)
        {

            var pagamentoWithInclude = await _pagamentoRepository.BuscarPagamentoCompraProdutoUsuario(pagamento.CompraId);
            if (pagamentoWithInclude == null) return false;


            var uriPagamento = $"https://localhost:5001/api/v1/compras/{pagamentoWithInclude.CompraId}/pagamentos";
            var ulrResponse = "";
            if (pagamentoWithInclude.Compra.GatewayPagamento == GatewayPagamento.Paypal) ulrResponse =
                    $"https://paypal.com/{pagamentoWithInclude.CompraId}?redirectUrl=https://localhost:5001/api/v1/compras/{pagamentoWithInclude.CompraId}/pagamentos";

            if (pagamentoWithInclude.Compra.GatewayPagamento == GatewayPagamento.Pagseguro) ulrResponse =
                    $"https://pagseguro.com?returnId={pagamentoWithInclude.CompraId}&redirectUrl=https://localhost:5001/api/v1/compras/{pagamentoWithInclude.CompraId}/v2/pagamentos";


            // Credentials
            var credentials = new NetworkCredential("desafiomercadolivre22@gmail.com", "lucasvaivoa");

            // Mail message
            var mail = new MailMessage()
            {
                From = new MailAddress("desafiomercadolivre22@gmail.com"),
                Subject = "Desafio 2 - Mercado Livre",
                Body = @$"Olá {pagamentoWithInclude.Compra.Usuario.Login}, o pagamento da compra do produto {pagamentoWithInclude.Compra.Produto.Nome} não foi concluida com sucesso:
                 Tentativa de pagamento em: {pagamentoWithInclude.CreatedAt}
                 Link para realizar o pagamento novamente: {ulrResponse}"
            };

            mail.IsBodyHtml = true;
            mail.CC.Add(new MailAddress("desafiomercadolivre22@gmail.com"));
            //mail.To.Add(new MailAddress("desafiomercadolivre22@gmail.com"));
            mail.To.Add(new MailAddress(pagamentoWithInclude.Compra.Usuario.Login));

            // Smtp client
            var client = new SmtpClient()
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Credentials = credentials
            };
            client.Send(mail);
            Console.WriteLine("Email enviado com sucesso!");
            return true;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<bool> EmailFakePagamentoFalhou(Pagamento pagamento)
        {

            var pagamentoWithInclude = await _pagamentoRepository.BuscarPagamentoCompraProdutoUsuario(pagamento.CompraId);
            if (pagamentoWithInclude == null) return false;

            var ulrResponse = "";
            if (pagamentoWithInclude.Compra.GatewayPagamento == GatewayPagamento.Paypal) ulrResponse =
                    $"https://paypal.com/{pagamentoWithInclude.CompraId}?redirectUrl=https://localhost:5001/api/v1/compras/{pagamentoWithInclude.CompraId}/pagamentos";

            if (pagamentoWithInclude.Compra.GatewayPagamento == GatewayPagamento.Pagseguro) ulrResponse =
                    $"https://pagseguro.com?returnId={pagamentoWithInclude.CompraId}&redirectUrl=https://localhost:5001/api/v1/compras/{pagamentoWithInclude.CompraId}/v2/pagamentos";

            Console.WriteLine(@$"Olá {pagamentoWithInclude.Compra.Usuario.Login}, o pagamento da compra do produto {pagamentoWithInclude.Compra.Produto.Nome} não foi concluida com sucesso:
                 Tentativa de pagamento em: {pagamentoWithInclude.CreatedAt}
                 Link para realizar o pagamento novamente: {ulrResponse}");

            return true;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> EnviarFiscal(Pagamento pagamento)
        {

            var pagamentoWithInclude = await _pagamentoRepository.BuscarPagamentoCompraProdutoUsuario(pagamento.CompraId);
            if (pagamentoWithInclude == null) return false;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:5001/api/v1/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var jsonRequest = new FiscalAddViewModel
            {
                CompradorId = pagamentoWithInclude.Compra.Comprador,
                CompraId = pagamentoWithInclude.CompraId
            };

            var response = await httpClient.PostAsJsonAsync("fiscal/compras", jsonRequest);

            if (!response.IsSuccessStatusCode)
            {
                AddErrors("Erro ao enviar a requisição para o fiscal");
                return false;
            }

            return true;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> EnviarVendedorRank(Pagamento pagamento)
        {

            var pagamentoWithInclude = await _pagamentoRepository.BuscarPagamentoCompraProdutoUsuario(pagamento.CompraId);
            if (pagamentoWithInclude == null) return false;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:5001/api/v1/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var jsonRequest = new VendedorAddViewModel
            {
                VendedorId = pagamentoWithInclude.Compra.Produto.UsuarioId,
                CompraId = pagamentoWithInclude.CompraId
            };

            var response = await httpClient.PostAsJsonAsync("vendedores/rank/vendas", jsonRequest);

            if (!response.IsSuccessStatusCode)
            {
                AddErrors("Erro ao enviar a requisição para o rank de vendedores");
                return false;
            }

            return true;
        }

    }
}
