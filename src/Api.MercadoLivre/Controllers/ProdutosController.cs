using Api.MercadoLivre.Data;
using Api.MercadoLivre.Extensions;
using Api.MercadoLivre.Model;
using Api.MercadoLivre.Model.ViewModel.Caracteristica;
using Api.MercadoLivre.Model.ViewModel.Produto;
using Api.MercadoLivre.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Api.MercadoLivre.Controllers
{
    [Route("api/v1/produtos")]
    public class ProdutosController : BaseController
    {

        private readonly CaracteristicaRepository _caracteristicaRepository;
        private readonly ProdutoRepository _produtoRepository;
        private readonly OpiniaoRepository _opiniaoRepository;
        private readonly PerguntaRepository _perguntaRepository;
        private readonly ImagemRepository _imagemRepository;
        private readonly CategoriaRepository _categoriaRepository;

        public ProdutosController(ApiDbContext context,
                                  IMapper mapper,
                                  CaracteristicaRepository caracteristicaRepository,
                                  ProdutoRepository produtoRepository,
                                  OpiniaoRepository opiniaoRepository,
                                  PerguntaRepository perguntaRepository,
                                  ImagemRepository imagemRepository,
                                  CategoriaRepository categoriaRepository) : base(context, mapper)
        {
            _caracteristicaRepository = caracteristicaRepository;
            _produtoRepository = produtoRepository;
            _opiniaoRepository = opiniaoRepository;
            _perguntaRepository = perguntaRepository;
            _imagemRepository = imagemRepository;
            _categoriaRepository = categoriaRepository;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<ActionResult<List<Produto>>> GetAll()
        {
            var produtos = await GetProdutoCategoriaCaracteristicasUsuarioAll();

            var produtoReturnSimpleViewModel = _mapper.Map<List<ProdutoReturnSimpleViewModel>>(produtos);

            return Response(result: produtoReturnSimpleViewModel);
        }


        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoReturnViewModel>> GetById(Guid id)
        {
            var produto = await GetProdutoCategoriaCaracteristicasUsuarioById(id);
            if (produto == null) return Response(result: new { msg = "Produto não foi encontrado." }, 404, sucess: false);

            var produtoReturnViewModel = _mapper.Map<ProdutoReturnViewModel>(produto);
            produtoReturnViewModel.Imagens = _mapper.Map<List<ImagemAddReturnViewModel>>(await _imagemRepository.Buscar(o => o.ProdutoId == id));
            produtoReturnViewModel.Opinioes = _mapper.Map<List<OpiniaoReturnViewModel>>(await _opiniaoRepository.GetOpinioesUsuario(id));
            produtoReturnViewModel.Perguntas = _mapper.Map<List<PerguntaReturnViewModel>>(await _perguntaRepository.GetPerguntasUsuario(id));
            produtoReturnViewModel.TotalDeOpinioes = produtoReturnViewModel.Opinioes.Count;
            produtoReturnViewModel.MediaOpinioes = CalcularMediaNotaOpnioes(produtoReturnViewModel.Opinioes);
            produtoReturnViewModel.CategoriasRelacionadas = _mapper.Map<CategoriaDetalhesReturnViewModel>(await GetCategoriaRelacionadas(produtoReturnViewModel.CategoriaId));
            produtoReturnViewModel.TotalDePerguntas = produtoReturnViewModel.Perguntas.Count;



            return Response(result: produtoReturnViewModel);
        }


        [Authorize]
        [HttpPost("")]
        public async Task<ActionResult<ProdutoAddReturnViewModel>> Add(ProdutoAddViewModel produtoAddViewModel)
        {
            if (!ModelState.IsValid) return ModelErrors(ModelState);
            if (User.Claims.ToList()[0].Value != produtoAddViewModel.UsuarioId.ToString())
                return Response(result: new { msg = "Ação não autorizada!" }, 403, sucess: false, erro: true);
            if (await _produtoRepository.Existe(o => o.UsuarioId == produtoAddViewModel.UsuarioId))
                return Response("O usuario já possui um produto cadastrado, não podendo cadastrar mais!");
            if (!await _categoriaRepository.Existe(o => o.Id == produtoAddViewModel.CategoriaId))
                return Response("A categoria fornecida não existe!");


            var produto = _mapper.Map<Produto>(produtoAddViewModel);
            var caracteristicas = MapperCaracteristicas(produtoAddViewModel, produto.Id);

            var modelProduto = await produto.Validar();
            if (!modelProduto.IsValid) return ValidationErrors(modelProduto);
            for (int i = 0; i < caracteristicas.Count; i++)
            {
                var modelCaracteristica = await caracteristicas[i].Validar();
                if (!modelCaracteristica.IsValid) return ValidationErrors(modelCaracteristica);
            }

            _context.Produtos.Add(produto);
            await _context.Caracteristicas.AddRangeAsync(caracteristicas);
            await _context.SaveChangesAsync();

            var produtoAddReturnViewModel = _mapper.Map<ProdutoAddReturnViewModel>(produto);
            var caracteristicaReturnViewModel = _mapper.Map<List<CaracteristicaReturnViewModel>>(caracteristicas);
            produtoAddReturnViewModel.Caracteristicas = caracteristicaReturnViewModel;
            produtoAddReturnViewModel.CategoriaNome = await GetCategoriaNome(produtoAddReturnViewModel.CategoriaId);

            return Response(result: produtoAddReturnViewModel, 201, nameof(GetById), produtoAddReturnViewModel.Id);

        }

        [Authorize]
        [HttpPost("{id:guid}/opinioes")]
        public async Task<ActionResult<OpiniaoAddReturnViewModel>> Add([FromRoute] Guid id, [FromBody] OpiniaoAddViewModel opiniaoAddViewModel)
        {
            if (!ModelState.IsValid) return ModelErrors(ModelState);
            if (User.Claims.ToList()[0].Value != opiniaoAddViewModel.UsuarioId.ToString())
                return Response(result: new { msg = "Ação não autorizada!" }, 403, sucess: false, erro: true);
            if (!await _produtoRepository.Existe(o => o.Id == id))
                return Response(result: new { msg = "Produto não foi encontrado." }, 404, sucess: false, erro: true);

            opiniaoAddViewModel.SetProdutoId(id);
            var opiniao = _mapper.Map<Opiniao>(opiniaoAddViewModel);

            var validate = await opiniao.Validar();
            if (!validate.IsValid) return ValidationErrors(validate);

            _context.Opinioes.Add(opiniao);
            await _context.SaveChangesAsync();

            var opiniaoAddReturnViewModel = _mapper.Map<OpiniaoAddReturnViewModel>(opiniao);

            return Response(result: opiniaoAddReturnViewModel, 201, nameof(GetAllOpinioes), opiniaoAddReturnViewModel.Id);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}/opinioes")]
        public async Task<ActionResult<OpiniaoReturnViewModel>> GetAllOpinioes(Guid id)
        {
            if (!await _produtoRepository.Existe(o => o.Id == id))
                return Response(result: new { msg = "Produto não foi encontrado." }, 404, sucess: false, erro: true);

            var opinioes = await _opiniaoRepository.GetOpinioesUsuario(id);

            var opinioesAddReturnViewModel = _mapper.Map<List<OpiniaoReturnViewModel>>(opinioes);

            return Response(result: opinioesAddReturnViewModel);
        }



        [Authorize]
        [HttpPost("{id:guid}/perguntas")]
        public async Task<ActionResult<PerguntaAddReturnViewModel>> Add([FromRoute] Guid id, [FromBody] PerguntaAddViewModel perguntaAddViewModel)
        {
            if (!ModelState.IsValid) return ModelErrors(ModelState);
            if (User.Claims.ToList()[0].Value != perguntaAddViewModel.UsuarioId.ToString())
                return Response(result: new { msg = "Ação não autorizada!" }, 403, sucess: false, erro: true);
            if (!await _produtoRepository.Existe(o => o.Id == id))
                return Response(result: new { msg = "Produto não foi encontrado." }, 404, sucess: false, erro: true);

            perguntaAddViewModel.SetProdutoId(id);
            var pergunta = _mapper.Map<Pergunta>(perguntaAddViewModel);

            var validate = await pergunta.Validar();
            if (!validate.IsValid) return ValidationErrors(validate);

            _context.Perguntas.Add(pergunta);
            await _context.SaveChangesAsync();

            //var emailEnviado = await EmailFake(User.Claims.ToList()[1].Value, pergunta);

            var emailEnviado = await EnviarEmailPergunta(User.Claims.ToList()[1].Value, pergunta);
            if (!emailEnviado) Console.WriteLine("Falha no envio do email.");

            var perguntaAddReturnViewModel = _mapper.Map<PerguntaAddReturnViewModel>(pergunta);

            return Response(result: perguntaAddReturnViewModel, 201, nameof(GetAllPerguntas), perguntaAddReturnViewModel.Id);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}/perguntas")]
        public async Task<ActionResult<PerguntaReturnViewModel>> GetAllPerguntas(Guid id)
        {
            if (!await _produtoRepository.Existe(o => o.Id == id))
                return Response(result: new { msg = "Produto não foi encontrado." }, 404, sucess: false, erro: true);

            var perguntas = await _perguntaRepository.GetPerguntasUsuario(id);

            var perguntasReturnViewModel = _mapper.Map<List<PerguntaReturnViewModel>>(perguntas);

            return Response(result: perguntasReturnViewModel);
        }

        [Authorize]
        [DisableRequestSizeLimit]
        [HttpPost("{id:guid}/imagens")]
        public async Task<ActionResult<ImagemAddReturnViewModel>> Add(IFormFile imagemRecebida, Guid id)
        {
            if (imagemRecebida == null) return Response("Imagem invalida ou ausente.");
            if (imagemRecebida.Length < 0) return Response("Imagem invalida ou ausente.");
            var produto = await _produtoRepository.ObterPorId(id);
            if (produto == null) return Response(result: new { msg = "Produto não foi encontrado." }, 404, sucess: false, erro: true);
            if (User.Claims.ToList()[0].Value != produto.UsuarioId.ToString()) return Response(result: new { msg = "Ação não autorizada!" }, 403, sucess: false, erro: true);

            // armazena em cloud
            var imageJson = await ArmazenarImagem(imagemRecebida);
            if (imageJson == null) return Response(result: new { msg = "Imagem não pode ser salva, tente mais tarde." }, 500, sucess: false, erro: true);
            var imagem = new Imagem(imageJson.data.image.url, id, imageJson.data.image.filename);

            ////armazena fake
            //var addPadrao = "_" + Guid.NewGuid().ToString();
            //var ulrFake = $"https://i.ibb.co/{imagemRecebida.FileName}{addPadrao}";
            //var imagem = new Imagem(ulrFake, id, imagemRecebida.FileName);

            var validate = await imagem.Validar();
            if (!validate.IsValid) return ValidationErrors(validate);

            _context.Imagens.Add(imagem);
            await _context.SaveChangesAsync();

            var imagemAddReturnViewModel = _mapper.Map<ImagemAddReturnViewModel>(imagem);

            return Response(result: imagemAddReturnViewModel, 201, nameof(GetById), imagemAddReturnViewModel.Id); ;
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}/imagens")]
        public async Task<ActionResult<List<ImagemAddReturnViewModel>>> GetAllImagens(Guid id)
        {
            if (!await _produtoRepository.Existe(o => o.Id == id))
                return Response(result: new { msg = "Produto não foi encontrado." }, 404, sucess: false, erro: true);

            var imagens = await _imagemRepository.Buscar(o => o.ProdutoId == id);

            var perguntasReturnViewModel = _mapper.Map<List<ImagemAddReturnViewModel>>(imagens);

            return Response(result: perguntasReturnViewModel);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<string> GetCategoriaNome(Guid id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(o => o.Id == id);

            return categoria.Nome == null ? "Erro na busca." : categoria.Nome;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> EmailFake(string Email, Pergunta pergunta)
        {
            Console.WriteLine(@$"Olá {Email}, você recebeu uma pergunta:
                 Usuario: {Email}
                 Pergunta: {pergunta.Titulo}
                 Link para o produto: https://localhost:5001/api/v1/produtos/{pergunta.ProdutoId}");

            return true;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<Caracteristica> MapperCaracteristicas(ProdutoAddViewModel produtoAddViewModel, Guid produtoId)
        {
            /////// --- Tudo isso para um bind de caracteristicas --- ////////////
            var caracteristicasAdd = produtoAddViewModel.Caracteristicas;
            var bindCaracteristicas = new List<CaracteristicaBindViewModel>();

            foreach (var caracteristica in caracteristicasAdd)
            {
                bindCaracteristicas.Add(_mapper.Map<CaracteristicaBindViewModel>(caracteristica));
            }

            foreach (var caracteristica in bindCaracteristicas)
            {
                caracteristica.ProdutoId = produtoId;
            }

            var caracteristicas = _mapper.Map<List<Caracteristica>>(bindCaracteristicas);

            return caracteristicas;
            /////// --- Tudo isso para um bind de caracteristicas --- ////////////
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Produto> GetProdutoCategoriaCaracteristicasUsuarioById(Guid id)
        {
            var produto = await _context.Produtos.AsNoTrackingWithIdentityResolution().Include(o => o.Caracteristicas)
                                                                                      .Include(o => o.Categoria)
                                                                                      .Include(o => o.Usuario)
                                                                                      .FirstOrDefaultAsync(o => o.Id == id);

            return produto;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<List<Produto>> GetProdutoCategoriaCaracteristicasUsuarioAll()
        {
            var produtos = await _context.Produtos.AsNoTrackingWithIdentityResolution().Include(o => o.Caracteristicas)
                                                                                       .Include(o => o.Categoria)
                                                                                       .Include(o => o.Usuario).ToListAsync();

            return produtos;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> EnviarEmailPergunta(string Email, Pergunta pergunta)
        {

            var produto = await GetProdutoCategoriaCaracteristicasUsuarioById(pergunta.ProdutoId);
            if (produto == null) return false;

            // Credentials
            var credentials = new NetworkCredential("desafiomercadolivre22@gmail.com", "lucasvaivoa");
            // Mail message
            var mail = new MailMessage()
            {
                From = new MailAddress("desafiomercadolivre22@gmail.com"),
                Subject = "Desafio 2 - Mercado Livre",
                Body = @$"Olá {produto.Usuario.Login}, o produto {produto.Nome} recebeu uma pergunta:
                 Usuario: {Email}
                 Pergunta: {pergunta.Titulo}
                 Link para o produto: https://localhost:5001/api/v1/produtos/{pergunta.ProdutoId}"
            };
            mail.IsBodyHtml = true;
            mail.To.Add(new MailAddress(produto.Usuario.Login));
            mail.CC.Add(new MailAddress("desafiomercadolivre22@gmail.com"));
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
        public async Task<Root> ArmazenarImagem(IFormFile imagemRecebida)
        {
            var image64 = "";
            using (var ms = new MemoryStream())
            {
                imagemRecebida.CopyTo(ms);
                var fileBytes = ms.ToArray();
                image64 = Convert.ToBase64String(fileBytes);
            }

            HttpClient httpClient = new HttpClient();
            var multiForm = new MultipartFormDataContent();
            multiForm.Add(new StringContent(image64), "image");
            multiForm.Add(new StringContent(imagemRecebida.FileName + "_" + Guid.NewGuid().ToString()), "name");
            var url = "https://api.imgbb.com/1/upload?key=8da220510687693589617194b14c9d43";
            var response = await httpClient.PostAsync(url, multiForm);

            if (!response.IsSuccessStatusCode) return null;

            var reponseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<Root>(reponseString);

            return jsonResponse;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public double CalcularMediaNotaOpnioes(List<OpiniaoReturnViewModel> opinioes)
        {

            double contagemDeNotas = 0;

            if (opinioes.Count == 0) return contagemDeNotas;

            foreach (var opiniao in opinioes)
            {
                contagemDeNotas += opiniao.Nota;
            }

            double mediaNotaOpnioes = contagemDeNotas / opinioes.Count;

            return mediaNotaOpnioes;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Categoria> GetCategoriaRelacionadas(Guid id)
        {
            var categoria = await _context.Categorias.Include(o => o.CategoriaPrincipal).ThenInclude(o => o.CategoriaPrincipal).DefaultIfEmpty().FirstOrDefaultAsync(o => o.Id == id);

            return categoria;
        }

    }

}

