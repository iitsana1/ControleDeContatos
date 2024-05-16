using ControleDeContatos.Data;
using ControleDeContatos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleDeContatos.Controllers
{
    public class ContatoController : Controller
    {
        private readonly BancoContext _context;
        public ContatoController(BancoContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var contato = await _context.Contato.ToListAsync();
            if(contato == null)
            {
                throw new Exception("Dados não encontrado"!);
            }
            return View(contato);
        }
        public IActionResult Criar()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Criar(ContatoModels contato) 
        {
            if (contato != null)
            {
                await _context.Contato.AddAsync(contato);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            throw new Exception("Preencha todos os campos!");
            
        }
        public IActionResult Editar()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Editar(ContatoModels contato)
        {
            if (contato != null)
            {
                var busca = _context.Contato.FirstOrDefault(x => x.id == contato.id);

                if (busca == null)
                {
                    throw new Exception("Dados não encontrados!");
                }
                busca.Nome = contato.Nome;
                busca.Email = contato.Email;
                busca.Celular = contato.Celular;

                await _context.Contato.AddAsync(busca);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult ApagarConfirmacao()
        {
            return View();
        }
    }
}
