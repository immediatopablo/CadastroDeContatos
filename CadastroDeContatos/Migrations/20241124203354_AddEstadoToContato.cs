using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroDeContatos.Migrations
{
    /// <inheritdoc />
    public partial class AddEstadoToContato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Contatos",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Contatos");
        }
    }
}
