using System.ComponentModel.DataAnnotations; // Adiciona validações de dados às propriedades da classe

namespace CadastroDeContatos.ViewModels
{
    public class ConfirmPasswordViewModel // Uma classe que intermediára o Controller e a View
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")] // Senha Obrigatória
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
