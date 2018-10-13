using APICore.Helpers.Misc;
using APICore.Model;
using APICore.Model.Auth;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskTop.Authorization;
using TaskTop.Authorization.Model;
using TaskTop.Model;

namespace TaskTop.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly TaskTopContext DbContext;
        private readonly IMapper Mapper;

        public AuthController(TaskTopContext ctx, IMapper mapper)
        {
            DbContext = ctx;
            Mapper = mapper;
        }

        private object CreateToken(SigningConfig signConfig, TokenConfig tokenConfig, Operator operador)
        {
            var identity = new ClaimsIdentity(AuthExts.ToClaims(operador), tokenConfig.Type);
            var createdAt = DateTime.UtcNow;
            var expires = createdAt + TimeSpan.FromHours(tokenConfig.Hours);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfig.Issuer,
                Audience = tokenConfig.Audience,
                SigningCredentials = signConfig.SigningCredentials,
                Subject = identity,
                NotBefore = createdAt,
                Expires = expires
            });

            var token = handler.WriteToken(securityToken);
            return new
            {
                accessToken = token,
                expiresIn = expires,
                user = new
                {
                    id = operador.Id,
                    nome = operador.Nome,
                    email = operador.Email,
                    tipo = operador.Tipo.ToString().ToLower()
                }
            };
        }

        [HttpPost]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request,
            [FromServices] SigningConfig signConfig,
            [FromServices] TokenConfig tokenConfig)
        {
            var invalidExn = new ValidationExn("Nome de usuário ou senha está incorreto.");
            var username = request.usuario.ToTrim();
            var usuarioSalt = await DbContext.Usuario
                .Where(u => u.Login == username)
                .Select(u => u.Chave)
                .SingleOrDefaultAsync();

            if (usuarioSalt == null)
                throw invalidExn;

            var pass = Auth.GetPassword(request.senha, usuarioSalt);
            var usuario = await DbContext.Usuario
                .Where(u => u.Login == username && u.Senha == pass)
                .Select(u => new { u.Id, u.Nome, u.Email, u.Permissao })
                .SingleOrDefaultAsync();

            if (usuario == null)
                throw invalidExn;

            var op = new Operator
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Tipo = Enum.Parse(typeof(UsuarioTipo), usuario.Permissao).ChangeType<UsuarioTipo>()
            };

            return Ok(CreateToken(signConfig, tokenConfig, op));
        }
    }
}
