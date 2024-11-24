using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CadastroDeContatos.ViewModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CadastroDeContatos.Models;

namespace CadastroDeContatos.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // Página do Maps
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Página de registro (GET)
        [HttpGet]
        public IActionResult GoogleMaps()
        {
            return View();
        }

        // Método para registrar um novo usuário (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                {
                    ModelState.AddModelError(string.Empty, "Já existe um usuário com esse e-mail.");
                    return View(model);
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Cadastro realizado com sucesso! Você pode fazer login agora.";
                    return RedirectToAction("Login"); // Redireciona para a página de login
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        _logger.LogError($"Erro ao criar usuário: {error.Description}");
                    }
                }
            }

            return View(model);
        }

        // Página de login (GET)
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/"); // Retorna ao padrão (home) se não houver retorno
            return View();
        }

        // Método para realizar o login (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Redirecionar para a página de contatos, ou para a URL de retorno
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "Senha ou e-mail inválidos.");
            }

            return View(model);
        }

        // Método para realizar logout (GET)
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Usuário deslogado.");
            return RedirectToAction("Login", "Account"); // Redireciona para a página de login
        }

        // Método auxiliar para redirecionar para a URL de origem ou para a página de contatos
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl); // Redireciona para a URL de retorno
            }
            else
            {
                return RedirectToAction("Index", "Contatos"); // Redireciona para a página de contatos
            }
        }
    }
}
