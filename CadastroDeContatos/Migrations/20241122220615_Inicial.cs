using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroDeContatos.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "UF",
                table: "Contatos");

            migrationBuilder.AlterColumn<string>(
                name: "CEP",
                table: "Contatos",
                type: "TEXT",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Contatos",
                type: "TEXT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                table: "Contatos",
                type: "TEXT",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                table: "Contatos");

            migrationBuilder.AlterColumn<string>(
                name: "CEP",
                table: "Contatos",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Contatos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Contatos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UF",
                table: "Contatos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
