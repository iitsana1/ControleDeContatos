using ControleDeContatos.Data;
using ControleDeContatos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleDeContatos.Controllers
{
    [Authorize(Roles = "Usuario, Admin, Gerente")]
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
            return View();
        }
        public IActionResult Editar(int id)
        {
            var contato = _context.Contato.FirstOrDefault(x => x.id == id);
            if (contato == null)
            {
                return NotFound();
            }
            return View(contato);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(ContatoModels contato)
        {
            if (ModelState.IsValid)
            {
                var busca = await _context.Contato.FirstOrDefaultAsync(x => x.id == contato.id);
                if (busca == null)
                {
                    return NotFound();
                }

                busca.Nome = contato.Nome;
                busca.Email = contato.Email;
                busca.Celular = contato.Celular;

                _context.Update(busca);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contato);
        }
        public IActionResult ApagarConfirmacao(int id)
        {
            var contato = _context.Contato.FirstOrDefault(x => x.id == id);
            if (contato == null)
            {
                return NotFound();
            }
            return View(contato);
        }

        public async Task<IActionResult> Apagar(int id)
        {
            ContatoModels contato = _context.Contato.FirstOrDefault(x => x.id == id);

            if (contato == null)
            {
                throw new System.NotImplementedException("Houve um erro na deleção do contato!");
            }
            _context.Contato.Remove(contato);
            _context.SaveChanges();


            return RedirectToAction("Index");
            
        }
    }
}
