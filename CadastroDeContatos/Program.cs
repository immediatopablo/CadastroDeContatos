using CadastroDeContatos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext com a string de conexão do SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Adicionar o IHttpClientFactory para lidar com o proxy
builder.Services.AddHttpClient();

// Outros serviços
builder.Services.AddControllersWithViews();

// Configuração de autenticação e autorização
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Caminho para login
    options.AccessDeniedPath = "/Account/AccessDenied"; // Caminho para acesso negado
    options.LogoutPath = "/Account/Logout"; // Caminho para logout
    options.ReturnUrlParameter = "/Home/Index"; // Redirecionamento para a página inicial após login
});

var app = builder.Build();

// Configuração do pipeline de requisição HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middleware de autenticação e autorização
app.UseAuthentication();  // Necessário para autenticação
app.UseAuthorization();   // Necessário para autorização

// Configuração das rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contatos}/{action=Index}/{id?}");  // A rota padrão para "Contatos/Index"

app.MapControllerRoute(
    name: "login",
    pattern: "{controller=Account}/{action=Login}/{id?}");  // Caminho para login

app.Run();
