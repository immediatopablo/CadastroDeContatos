using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CadastroDeContatos.Models;
using CadastroDeContatos.Controllers;

public class AppDbContext : IdentityDbContext<ApplicationUser> // Classe de contexto do banco de dados, herda de IdentityDbContext para suportar a autenticação do usuário
{
    public DbSet<Contato> Contatos { get; set; } // A referência para a tabela de Contatos

    // Construtor do DbContext, recebe as opções de configuração do banco de dados
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Índice composto para garantir CPF único por usuário
        builder.Entity<Contato>()
            .HasIndex(c => new { c.UsuarioId, c.CPF }) // CPF único por usuário
            .IsUnique()
            .HasDatabaseName("IX_Contato_Usuario_CPF"); // Nome do índice composto

        // Configurando a propriedade CPF
        builder.Entity<Contato>()
            .Property(c => c.CPF)
            .HasMaxLength(11)
            .IsRequired();

        // Configurando a propriedade Email
        builder.Entity<Contato>()
            .Property(c => c.Email)
            .HasMaxLength(100)
            .IsRequired();

        // Configurando índice único para Email
        builder.Entity<Contato>()
            .HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("IX_Contato_Email");
    }

}
