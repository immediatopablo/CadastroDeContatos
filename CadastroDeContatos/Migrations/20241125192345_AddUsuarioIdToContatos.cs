using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroDeContatos.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioIdToContatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Contatos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Contatos");
        }
    }
}
