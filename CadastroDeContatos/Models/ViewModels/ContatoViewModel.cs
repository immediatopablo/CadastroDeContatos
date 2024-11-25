using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace CadastroDeContatos.Models.ViewModels
{
    [Table("Contatos")]  // Tabela no banco de dados que vai gravar os contatos!
    public class ContatoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [CustomValidationCPF(ErrorMessage = "O CPF informado não é válido.")]
        public string CPF { get; set; }

        [Phone(ErrorMessage = "Insira um número de telefone válido.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O logradouro é obrigatório.")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        public string Cep { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Insira um e-mail válido.")]
        public string Email { get; set; }

        // Propriedade para exibição do sucesso no cadastro (opcional)
        public string SuccessMessage { get; set; }

        public string Estado { get; set; }

        // Corrigido: Propriedade para associar o contato ao usuário logado (do tipo Guid)
        public Guid UserId { get; set; }
    }

    // Classe para validação personalizada do CPF
    public class CustomValidationCPF : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var cpf = value as string;
            if (cpf == null || !ValidarCPF(cpf))
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }

        // Método para validar o CPF
        private bool ValidarCPF(string cpf)
        {
            cpf = Regex.Replace(cpf, "[^0-9]", "");

            if (cpf.Length != 11 || new string(cpf[0], cpf.Length) == cpf)
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            string digito = resto.ToString();

            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}
