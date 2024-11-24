using System.ComponentModel.DataAnnotations;

namespace CadastroDeContatos.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "A confirmação da senha não corresponde.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; } // O token de redefinição de senha
    }
}
