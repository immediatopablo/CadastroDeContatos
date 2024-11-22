using Microsoft.EntityFrameworkCore;
using CadastroDeContatos.Models;

public class AppDbContext : DbContext
{
    public DbSet<Contato> Contatos { get; set; }

    // Construtor que aceita DbContextOptions<AppDbContext>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Aqui você pode definir uma string de conexão se não estiver usando o DI
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=App_Data/Contatos.db");
        }
    }
}
