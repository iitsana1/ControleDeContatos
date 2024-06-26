using ControleDeContatos.Data;
using ControleDeContatos.Migrations;
using ControleDeContatos.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ControleDeContatos.Controllers
{
    [Authorize (Roles = "Usuario, Admin, Gerente")]
    public class UsuarioController : Controller 
    {
        private readonly BancoContext _bancoContext;

        public UsuarioController (BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        [AllowAnonymous]
        public IActionResult Cadastro()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Cadastro (UsuarioModel usuario)
        {

            if (usuario == null)
            {
                throw new Exception("Usuário Nulo!");
            }

            usuario.Cargo = "Usuario";

            _bancoContext.Usuario.Add(usuario);
            await _bancoContext.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UsuarioModel usuario)
        {
            if (usuario == null)
            {
                throw new Exception("Usuário Nulo!");
            }

            var verificar = _bancoContext.Usuario.Where(x => x.Email == usuario.Email && x.Senha == usuario.Senha).FirstOrDefaultAsync();
            if (verificar == null)
            {
                throw new Exception("Usuário não encontrado!");
            }

            List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Nome),
                    new Claim(ClaimTypes.Role, usuario.Cargo)
                };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme
                );

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,

            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);

            criarSessao(usuario);
            return RedirectToAction("Index", "Contato", new { area = "" });
        }

        public void criarSessao(UsuarioModel usuario)
        {
            HttpContext.Session.SetString("nome", usuario.Nome);
            HttpContext.Session.SetString("email", usuario.Email);
            HttpContext.Session.SetInt32("Id", usuario.Id);
        }
    }
}
