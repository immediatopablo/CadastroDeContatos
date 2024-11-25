using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using CadastroDeContatos.Models;
using CadastroDeContatos.Models.ViewModels;
using System.Security.Claims;
using System.Collections.Generic;

namespace CadastroDeContatos.Controllers
{
    [Authorize]
    public class ContatosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ContatosController> _logger;

        public ContatosController(AppDbContext context, ILogger<ContatosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Ação para exibir o formulário de criação (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ContatoViewModel());
        }

        // Ação para salvar o contato no banco (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContatoViewModel model)
        {
            _logger.LogInformation($"Dados recebidos para o contato: Nome={model.Nome}, CPF={model.CPF}");

            // Obtendo o ID do usuário logado
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid usuarioId))
            {
                ModelState.AddModelError("UserId", "Não foi possível identificar o usuário logado.");
                _logger.LogWarning("UserId inválido.");
                return View(model);
            }

            // Verificação de duplicados somente para o usuário logado
            if (_context.Contatos.Any(c => c.CPF == model.CPF && c.UsuarioId == usuarioId))
            {
                ModelState.AddModelError("CPF", "Você já possui um contato cadastrado com este CPF.");
                _logger.LogWarning($"CPF duplicado para o mesmo usuário: {model.CPF}");
            }

            if (_context.Contatos.Any(c => c.Email == model.Email && c.UsuarioId == usuarioId))
            {
                ModelState.AddModelError("Email", "Você já possui um contato cadastrado com este e-mail.");
                _logger.LogWarning($"E-mail duplicado para o mesmo usuário: {model.Email}");
            }

            if (!ValidarCEP(model.Cep))
            {
                ModelState.AddModelError("Cep", "O CEP informado não é válido.");
                _logger.LogWarning($"CEP inválido encontrado: {model.Cep}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var contato = new Contato
                    {
                        Nome = model.Nome,
                        CPF = model.CPF,
                        Telefone = model.Telefone,
                        Cidade = model.Cidade,
                        Logradouro = model.Logradouro,
                        Bairro = model.Bairro,
                        Cep = model.Cep,
                        Email = model.Email,
                        Estado = model.Estado,
                        UsuarioId = usuarioId // Associa o contato ao usuário logado
                    };

                    _context.Add(contato);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Contato salvo com sucesso!");

                    TempData["SuccessMessage"] = "Contato cadastrado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao salvar o contato.");
                    _logger.LogError($"Erro ao salvar o contato: {ex.Message}");
                }
            }

            return View(model);
        }


        // Ação para listar contatos (GET)
        public async Task<IActionResult> Index(string searchNome, string searchCPF)
        {
            _logger.LogInformation($"Pesquisando contatos com Nome: {searchNome}, CPF: {searchCPF}");

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier); // Obtendo o UserId do usuário logado
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                _logger.LogWarning("UserId inválido para o filtro.");
                return View(new List<ContatoViewModel>());
            }

            var contatosQuery = _context.Contatos.AsQueryable().Where(c => c.UsuarioId == userId);

            if (!string.IsNullOrEmpty(searchNome))
            {
                contatosQuery = contatosQuery.Where(c => c.Nome.ToLower().Contains(searchNome.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchCPF))
            {
                contatosQuery = contatosQuery.Where(c => c.CPF.Contains(searchCPF));
            }

            var contatosList = await contatosQuery.ToListAsync();

            var contatosViewModel = contatosList.Select(c => new ContatoViewModel
            {
                Id = c.Id,
                Nome = c.Nome,
                CPF = c.CPF,
                Telefone = c.Telefone,
                Cidade = c.Cidade,
                Logradouro = c.Logradouro,
                Bairro = c.Bairro,
                Cep = c.Cep,
                Email = c.Email,
                Estado = c.Estado
            }).ToList();

            ViewData["TotalContatos"] = contatosList.Count;
            ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            return View(contatosViewModel);
        }

        // Ação para exibir o formulário de edição (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("ID nulo ao tentar editar contato.");
                return NotFound();
            }

            var contato = await _context.Contatos.FindAsync(id);

            if (contato == null)
            {
                _logger.LogWarning($"Contato com ID={id} não encontrado.");
                return NotFound();
            }

            var model = new ContatoViewModel
            {
                Id = contato.Id,
                Nome = contato.Nome,
                CPF = contato.CPF,
                Telefone = contato.Telefone,
                Cidade = contato.Cidade,
                Logradouro = contato.Logradouro,
                Bairro = contato.Bairro,
                Cep = contato.Cep,
                Email = contato.Email,
                Estado = contato.Estado
            };

            return View(model);
        }

        // Ação para salvar as alterações no contato (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContatoViewModel model)
        {
            if (id != model.Id)
            {
                _logger.LogWarning("ID do modelo não corresponde ao ID do contato.");
                return BadRequest();
            }

            // Obtendo o ID do usuário logado
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid usuarioId))
            {
                ModelState.AddModelError("UserId", "Não foi possível identificar o usuário logado.");
                _logger.LogWarning("UserId inválido.");
                return View(model);
            }

            // Verificação de duplicados somente para o usuário logado
            if (_context.Contatos.Any(c => c.CPF == model.CPF && c.Id != id && c.UsuarioId == usuarioId))
            {
                ModelState.AddModelError("CPF", "Você já possui outro contato cadastrado com este CPF.");
                _logger.LogWarning($"CPF duplicado ao editar para o mesmo usuário: {model.CPF}");
            }

            if (_context.Contatos.Any(c => c.Email == model.Email && c.Id != id && c.UsuarioId == usuarioId))
            {
                ModelState.AddModelError("Email", "Você já possui outro contato cadastrado com este e-mail.");
                _logger.LogWarning($"E-mail duplicado ao editar para o mesmo usuário: {model.Email}");
            }

            if (!ValidarCEP(model.Cep))
            {
                ModelState.AddModelError("Cep", "O CEP informado não é válido.");
                _logger.LogWarning($"CEP inválido encontrado: {model.Cep}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var contato = await _context.Contatos.FindAsync(id);

                    if (contato == null || contato.UsuarioId != usuarioId)
                    {
                        _logger.LogWarning("Contato não encontrado ou não pertence ao usuário logado.");
                        return NotFound();
                    }

                    contato.Nome = model.Nome;
                    contato.CPF = model.CPF;
                    contato.Telefone = model.Telefone;
                    contato.Cidade = model.Cidade;
                    contato.Logradouro = model.Logradouro;
                    contato.Bairro = model.Bairro;
                    contato.Cep = model.Cep;
                    contato.Email = model.Email;
                    contato.Estado = model.Estado;

                    _context.Update(contato);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Contato atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao atualizar o contato: {ex.Message}");
                    TempData["ErrorMessage"] = "Erro ao atualizar o contato.";
                    return View(model);
                }
            }

            return View(model);
        }


        // Ação para exibir a página de confirmação de exclusão (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var contato = await _context.Contatos.FindAsync(id);
            if (contato == null)
            {
                _logger.LogWarning($"Contato com ID={id} não encontrado.");
                return NotFound();
            }

            var model = new ContatoViewModel
            {
                Id = contato.Id,
                Nome = contato.Nome,
                CPF = contato.CPF,
                Telefone = contato.Telefone,
                Cidade = contato.Cidade,
                Logradouro = contato.Logradouro,
                Bairro = contato.Bairro,
                Cep = contato.Cep,
                Email = contato.Email,
                Estado = contato.Estado
            };

            return View(model);
        }

        // Ação para excluir um contato (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var contato = await _context.Contatos.FindAsync(id);

                if (contato == null)
                {
                    _logger.LogWarning($"Contato com ID={id} não encontrado para exclusão.");
                    return NotFound();
                }

                _context.Contatos.Remove(contato);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Contato excluído com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao excluir o contato com ID={id}: {ex.Message}");
                TempData["ErrorMessage"] = "Erro ao excluir o contato. Tente novamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ValidarCEP(string cep)
        {
            return Regex.IsMatch(cep, @"^\d{5}-?\d{3}$");
        }
    }
}
