using System; // Um dos principais Namespaces, fornece classes e tipos básicos para o C# 
using System.ComponentModel.DataAnnotations; // Fornece atributos para validação de dados e configuração de propriedades em classes

public class Contato
{
    // Propriedade que representa o identificador único do contato no banco de dados
    public int Id { get; set; }

    // Nome do contato
    // Obrigatório e limitado a 100 caracteres
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; }

    // CPF do contato
    // Campo Obrigatório
    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve estar no formato 000.000.000-00.")]
    public string CPF { get; set; }

    // Telefone do contato
    // Campo opcional, mas limitado a 15 caracteres
    [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres.")]
    public string Telefone { get; set; }

    // Cidade onde o contato reside
    // Campo opcional, mas limitado a 50 caracteres
    [StringLength(50, ErrorMessage = "A cidade deve ter no máximo 50 caracteres.")]
    public string Cidade { get; set; }

    // Logradouro (rua ou endereço) do contato
    // Campo opcional, mas limitado a 100 caracteres
    [StringLength(100, ErrorMessage = "O logradouro deve ter no máximo 100 caracteres.")]
    public string Logradouro { get; set; }

    // Bairro onde o contato reside
    // Campo opcional, mas limitado a 50 caracteres
    [StringLength(50, ErrorMessage = "O bairro deve ter no máximo 50 caracteres.")]
    public string Bairro { get; set; }

    // CEP do contato
    // Obrigatório e deve seguir o formato "00000-000"
    [Required(ErrorMessage = "O CEP é obrigatório.")]
    [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "O CEP deve estar no formato 00000-000.")]
    public string Cep { get; set; }

    // Email do contato.
    // Obrigatório, validado como um e-mail válido e limitado a 100 caracteres
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
    [StringLength(100, ErrorMessage = "O e-mail deve ter no máximo 100 caracteres.")]
    public string Email { get; set; }

    // Estado onde o contato reside.
    // Campo opcional, mas limitado a 2 caracteres (sigla do estado)
    [StringLength(2, ErrorMessage = "O estado deve ter no máximo 2 caracteres.")]
    public string Estado { get; set; }

    // Relacionamento com o usuário responsável pelo cadastro do contato
    // Representado pelo ID único do usuário
    [Required]
    public Guid UsuarioId { get; set; }
}
