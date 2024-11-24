using System.ComponentModel.DataAnnotations;

namespace CadastroDeContatos.ViewModels
{
    public class RegisterViewModel
    {
        // Validação de email
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
        public string Email { get; set; }

        // Validação de senha
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "{0} deve ter no mínimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Validação de confirmação de senha
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmPassword { get; set; }
    }
}
