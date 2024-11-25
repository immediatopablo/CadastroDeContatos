using System.ComponentModel.DataAnnotations;

namespace CadastroDeContatos.ViewModels
{
    public class ConfirmPasswordViewModel
    {
        [Required(ErrorMessage = "Por favor, insira sua senha.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
