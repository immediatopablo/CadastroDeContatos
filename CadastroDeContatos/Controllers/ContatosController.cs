using Microsoft.AspNetCore.Mvc;
using CadastroDeContatos.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace MeuProjeto.Controllers
{
    public class ContatosController : Controller
    {
        private readonly AppDbContext _context;

        // Construtor para injeção de dependência
        public ContatosController(AppDbContext context)
        {
            _context = context;
        }

        // Ação para listar os contatos
        public async Task<IActionResult> Index(string? searchNome, string? searchCPF)
        {
            // Passa os valores para a ViewData para uso na view
            ViewData["CurrentNomeFilter"] = searchNome;
            ViewData["CurrentCPFFilter"] = searchCPF;

            // Consulta inicial
            var query = _context.Contatos.AsQueryable();

            // Filtro por Nome
            if (!string.IsNullOrWhiteSpace(searchNome))
            {
                query = query.Where(c => c.Nome.ToLower().Contains(searchNome.ToLower()));
            }

            // Filtro por CPF
            if (!string.IsNullOrWhiteSpace(searchCPF))
            {
                query = query.Where(c => c.CPF.Contains(searchCPF));
            }

            // Ordenação alfabética
            query = query.OrderBy(c => c.Nome);

            // Obtém a lista filtrada e ordenada
            var contatos = await query.ToListAsync();
            return View(contatos);
        }




        // Ação para criar um novo contato (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Ação para criar um novo contato (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CPF,Telefone,Cidade,Latitude,Longitude")] Contato contato)
        {
            // Verifica se o CPF já existe
            if (_context.Contatos.Any(c => c.CPF == contato.CPF))
            {
                ModelState.AddModelError("CPF", "O CPF informado já está cadastrado.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(contato);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(contato);
        }

        // Ação para editar um contato (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contato = await _context.Contatos.FindAsync(id);
            if (contato == null)
            {
                return NotFound();
            }
            return View(contato);
        }

        // Ação para editar um contato (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CPF,Telefone,Cidade,Latitude,Longitude")] Contato contato)
        {
            if (id != contato.Id)
            {
                return NotFound();
            }

            // Verifica se outro contato já utiliza o mesmo CPF
            if (_context.Contatos.Any(c => c.CPF == contato.CPF && c.Id != contato.Id))
            {
                ModelState.AddModelError("CPF", "O CPF informado já está cadastrado.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContatoExists(contato.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(contato);
        }

        // Ação para excluir um contato (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contato = await _context.Contatos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contato == null)
            {
                return NotFound();
            }

            return View(contato);
        }

        // Ação para excluir um contato (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contato = await _context.Contatos.FindAsync(id);
            _context.Contatos.Remove(contato);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContatoExists(int id)
        {
            return _context.Contatos.Any(e => e.Id == id);
        }
    }
}
