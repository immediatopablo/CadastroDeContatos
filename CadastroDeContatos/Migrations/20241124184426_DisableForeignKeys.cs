using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroDeContatos.Migrations
{
    /// <inheritdoc />
    public partial class DisableForeignKeys : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Desabilita as chaves estrangeiras no SQLite para a migração
            migrationBuilder.Sql("PRAGMA foreign_keys = 0;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Caso precise reverter, pode habilitar novamente as chaves estrangeiras
            migrationBuilder.Sql("PRAGMA foreign_keys = 1;");
        }


        }
}
