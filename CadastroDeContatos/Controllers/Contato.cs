using System;
using System.ComponentModel.DataAnnotations;

public class Contato
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve estar no formato 000.000.000-00.")]
    public string CPF { get; set; }

    [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres.")]
    public string Telefone { get; set; }

    [StringLength(50, ErrorMessage = "A cidade deve ter no máximo 50 caracteres.")]
    public string Cidade { get; set; }

    [StringLength(100, ErrorMessage = "O logradouro deve ter no máximo 100 caracteres.")]
    public string Logradouro { get; set; }

    [StringLength(50, ErrorMessage = "O bairro deve ter no máximo 50 caracteres.")]
    public string Bairro { get; set; }

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "O CEP deve estar no formato 00000-000.")]
    public string Cep { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
    [StringLength(100, ErrorMessage = "O e-mail deve ter no máximo 100 caracteres.")]
    public string Email { get; set; }

    [StringLength(2, ErrorMessage = "O estado deve ter no máximo 2 caracteres.")]
    public string Estado { get; set; }

    // Relacionamento com o usuário
    [Required]
    public Guid UsuarioId { get; set; }
}
