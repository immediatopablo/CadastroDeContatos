using Microsoft.AspNetCore.Mvc;

namespace CadastroDeContatos.Controllers
{
    public class HomeController : Controller
    {
        // Método Index que renderiza a view Index.cshtml
        public IActionResult Index()
        {
            return View(); // Retorna a view Index.cshtml da pasta Views/Home
        }
    }
}
