using System.ComponentModel.DataAnnotations;

namespace CadastroDeContatos.ViewModels
{
    public class ConfirmPasswordViewModel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
