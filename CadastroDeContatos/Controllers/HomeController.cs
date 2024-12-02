using Microsoft.AspNetCore.Mvc;

namespace CadastroDeContatos.Controllers
{
    public class HomeController : Controller // Classe que herda de Controller, responsável por gerenciar ações de requisição
    {
        // Método Index que renderiza a view Index.cshtml
        public IActionResult Index() 
        {
            return View(); // Retorna a view Index.cshtml da pasta Views/Home para ser renderizada na resposta
        }
    }
}
