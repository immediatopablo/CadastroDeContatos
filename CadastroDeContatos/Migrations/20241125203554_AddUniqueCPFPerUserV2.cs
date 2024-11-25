using Microsoft.EntityFrameworkCore.Migrations;
using System;

public partial class AddUniqueCPFPerUserV2 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Certifique-se de que o índice existe antes de tentar removê-lo
        migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Contato_CPF");

        // Remover a coluna UserId, se ela existir
        migrationBuilder.DropColumn(
            name: "UserId",
            table: "Contatos");

        // Alterar o tipo de UsuarioId para Guid
        migrationBuilder.AlterColumn<Guid>(
            name: "UsuarioId",
            table: "Contatos",
            type: "TEXT",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "INTEGER");

        // Criar o índice composto para garantir CPF único por usuário
        migrationBuilder.CreateIndex(
            name: "IX_Contato_Usuario_CPF",
            table: "Contatos",
            columns: new[] { "UsuarioId", "CPF" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Remover o índice composto
        migrationBuilder.DropIndex(
            name: "IX_Contato_Usuario_CPF",
            table: "Contatos");

        // Reverter a coluna UsuarioId para int
        migrationBuilder.AlterColumn<int>(
            name: "UsuarioId",
            table: "Contatos",
            type: "INTEGER",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "TEXT");

        // Adicionar a coluna UserId novamente
        migrationBuilder.AddColumn<string>(
            name: "UserId",
            table: "Contatos",
            type: "TEXT",
            nullable: true);

        // Criar o índice único apenas para CPF
        migrationBuilder.CreateIndex(
            name: "IX_Contato_CPF",
            table: "Contatos",
            column: "CPF",
            unique: true);
    }
}
