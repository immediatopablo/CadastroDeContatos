using System;

public class Contato
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string CPF { get; set; }
    public string Telefone { get; set; }
    public string Cidade { get; set; }
    public string Logradouro { get; set; }
    public string Bairro { get; set; }
    public string Cep { get; set; }
    public string Email { get; set; }
    public string Estado { get; set; }

    // Relacionamento com o usuário
    public Guid UsuarioId { get; set; } // Alterado para Guid

    // Não é necessário ter o campo "UserId" se o relacionamento já é feito pelo "UsuarioId"
    // public string UserId { get; internal set; } // Removido
}
