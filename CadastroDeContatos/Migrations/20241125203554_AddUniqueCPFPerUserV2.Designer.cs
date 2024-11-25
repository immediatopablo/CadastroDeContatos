using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroDeContatos.Migrations
{
    public partial class AddUniqueCPFPerUserV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Caso o índice anterior tenha sido removido
            migrationBuilder.DropIndex(
                name: "IX_Contato_CPF",
                table: "Contatos");

            // Alteração para garantir que UsuarioId seja um Guid
            migrationBuilder.AlterColumn<Guid>(
                name: "UsuarioId",
                table: "Contatos",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            // Criação de novo índice composto para garantir CPF único por usuário
            migrationBuilder.CreateIndex(
                name: "IX_Contato_Usuario_CPF",
                table: "Contatos",
                columns: new[] { "UsuarioId", "CPF" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverter as alterações feitas na migração
            migrationBuilder.DropIndex(
                name: "IX_Contato_Usuario_CPF",
                table: "Contatos");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Contatos",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            // Recriar índice único para CPF sem considerar o usuário
            migrationBuilder.CreateIndex(
                name: "IX_Contato_CPF",
                table: "Contatos",
                column: "CPF",
                unique: true);
        }
    }
}
