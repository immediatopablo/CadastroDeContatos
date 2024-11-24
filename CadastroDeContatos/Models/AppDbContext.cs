using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CadastroDeContatos.Models;
using CadastroDeContatos.Controllers;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Contato> Contatos { get; set; } // A referência para a tabela de Contatos

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configurando índice único para CPF
        builder.Entity<Contato>()
            .HasIndex(c => c.CPF)
            .IsUnique()
            .HasDatabaseName("IX_Contato_CPF");

        // Configurando índice único para Email
        builder.Entity<Contato>()
            .HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("IX_Contato_Email");

        builder.Entity<Contato>()
            .Property(c => c.CPF)
            .HasMaxLength(11)
            .IsRequired();

        builder.Entity<Contato>()
            .Property(c => c.Email)
            .HasMaxLength(100)
            .IsRequired();
    }
}
