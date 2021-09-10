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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/usuarios")]
    public class UsuariosController : BaseController
    {

        private readonly JwtSettings _jwtSettings;
        private readonly UsuarioRepository _usuarioRepository;
        public UsuariosController(ApiDbContext context,
                                  IMapper mapper,
                                  IOptions<JwtSettings> jwtSettings,
                                  UsuarioRepository usuarioRepository) : base(context, mapper)
        {
            _jwtSettings = jwtSettings.Value;
            _usuarioRepository = usuarioRepository;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<ActionResult<List<UsuarioReturnViewModel>>> GetAll()
        {
            var usuarios = await _usuarioRepository.ObterTodos();
            if (!usuarios.Any()) return Response("Nenhum usuario está cadastrado no momento.");


            var usuariosReturnViewModel = _mapper.Map<List<UsuarioReturnViewModel>>(usuarios);

            return usuariosReturnViewModel;
        }


        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UsuarioReturnViewModel>> GetById(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null) return Response(result: new { msg = "Usuario não foi encontrado." }, 404, sucess: false);


            var usuariosReturnViewModel = _mapper.Map<UsuarioReturnViewModel>(usuario);

            return Response(result: usuariosReturnViewModel);
        }

        //return User.Claims.ToList()[0].Value; obter dados do jwt<<<<<<

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioLoginViewModel>> Login(UsuarioLoginViewModel usuarioLoginViewModel)
        {
            if (!ModelState.IsValid) return ModelErrors(ModelState);
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(o => o.Login == usuarioLoginViewModel.Login);
            if (usuario == null) return Response("Senha ou email inválidos.");
            if (!SenhaValida(usuarioLoginViewModel.Senha, usuario.Senha)) return Response("Senha ou email inválidos.");

            var usuarioLoginReturnViewModel = _mapper.Map<UsuarioLoginReturnViewModel>(usuario);
            usuarioLoginReturnViewModel.ExpiraEmSegundos = TimeSpan.FromHours(_jwtSettings.ExpiracaoHoras).TotalSeconds;

            var jwt = await GerarJwt(usuario);

            usuarioLoginReturnViewModel.Token = jwt;


            return Response(result: usuarioLoginReturnViewModel);
        }

        [AllowAnonymous]
        [HttpPost("")]
        public async Task<ActionResult<UsuarioReturnViewModel>> Add(UsuarioAddViewModel usuarioAddViewModel)
        {
            if (!ModelState.IsValid) return ModelErrors(ModelState);
            if (await _usuarioRepository.EmailDuplicado(o => o.Login == usuarioAddViewModel.Login)) return Response("Email já cadastrado.");

            var usuario = _mapper.Map<Usuario>(usuarioAddViewModel);

            var passwordHash = GerarHash(usuarioAddViewModel.Senha);
            usuario.SetSenha(passwordHash);

            var model = await usuario.Validar();
            if (!model.IsValid) return ValidationErrors(model);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var usuarioReturnViewModel = _mapper.Map<UsuarioReturnViewModel>(usuario);

            return Response(result: usuarioReturnViewModel, 201, nameof(GetById), usuarioReturnViewModel.Id);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public bool SenhaValida(string senhaUsuario, string senhaHash)
        {
            return BCrypt.Net.BCrypt.Verify(senhaUsuario + _pepper, senhaHash);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GerarHash(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha + _pepper);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<string> GerarJwt(Usuario usuario)
        {

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, usuario.Login));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Emissor,
                Audience = _jwtSettings.ValidoEm,
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private static long ToUnixEpochDate(DateTime date)
           => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    }
}
