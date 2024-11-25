using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CadastroDeContatos.ViewModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CadastroDeContatos.Models;
using Microsoft.AspNetCore.Authorization;

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

        // Página de registro faz o GET
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // Página de Google Maps faz o GET na api do Google
        [HttpGet]
        public IActionResult GoogleMaps()
        {
            return View();
        }

        // Página de login faz o GET
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // Se não houver returnUrl, redireciona para a página inicial
            ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
            return View();
        }

        // Método para registrar um novo usuário faz o POST
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
                    return RedirectToAction("Login");
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

        // Método de redefinir senha - Página de solicitação de redefinição (GET)
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // Método para enviar o link de redefinição de senha (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    TempData["InfoMessage"] = "Se o e-mail informado estiver correto, você receberá um link para redefinir sua senha.";
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                // Redireciona diretamente para a página de redefinição de senha
                return RedirectToAction("ResetPassword", new { email = model.Email });
            }

            return View(model);
        }

        // Página para redefinir senha (GET)
        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            if (email == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }

        // Método para processar a redefinição de senha (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Usuário não encontrado.";
                    return RedirectToAction("Login");
                }

                // Redefinir a senha diretamente
                var result = await _userManager.RemovePasswordAsync(user); // Remove a senha atual
                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = "Erro ao remover a senha atual.";
                    return View(model);
                }

                result = await _userManager.AddPasswordAsync(user, model.Password); // Adiciona a nova senha
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Sua senha foi redefinida com sucesso!";
                    return RedirectToAction("Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // Método para realizar login (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Verificar a senha do usuário
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Usuário logado com sucesso.");
                        return RedirectToLocal(returnUrl); // Redireciona para a URL desejada após o login
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Falha ao realizar login. E-mail ou senha incorretos.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuário não encontrado.");
                }
            }
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                // Garantir redirecionamento para a página inicial do HomeController
                return RedirectToAction("Index", "Home");
            }
        }

        // Método para realizar logout (GET)
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Usuário deslogado.");
            return RedirectToAction("Login", "Account");
        }

        // Página de confirmação de senha para exclusão da conta (GET)
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmPasswordBeforeDelete()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("Usuário não encontrado.");
                return NotFound();
            }

            return View(new ConfirmPasswordViewModel { Email = user.Email });
        }

        // Confirmação de senha (POST) para proceder com a exclusão
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ConfirmPasswordBeforeDelete(ConfirmPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Usuário não encontrado.");
                return View(model);
            }

            // Verificação de senha
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                ModelState.AddModelError(string.Empty, "Senha incorreta. Tente novamente.");
                return View(model); // Retorna a view com a mensagem de erro
            }

            // Se a senha estiver correta, redireciona para a exclusão da conta
            return RedirectToAction("DeleteAccount");
        }


        // Página de exclusão de conta (GET)
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("Usuário não encontrado.");
                return NotFound();
            }

            var model = new DeleteAccountViewModel
            {
                UserName = user.UserName
            };

            return View(model);
        }

        // Ação para excluir a conta (POST) após a confirmação de senha
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteAccountConfirmed()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("Usuário não encontrado.");
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("Usuário excluído com sucesso.");
                await _signInManager.SignOutAsync();
                TempData["SuccessMessage"] = "Sua conta foi excluída com sucesso!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _logger.LogError("Erro ao excluir o usuário.");
                TempData["ErrorMessage"] = "Erro ao excluir sua conta. Tente novamente.";
                return RedirectToAction("DeleteAccount");
            }
        }

    }
}
