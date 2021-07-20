using Microsoft.EntityFrameworkCore.Migrations;

namespace RpgApi.Migrations
{
    public partial class MigracaoAtualizacaoArmaPersonagem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Armas_Personagens_personagemId",
                table: "Armas");

            migrationBuilder.DropForeignKey(
                name: "FK_Personagens_Usuarios_Usuarioid",
                table: "Personagens");

            migrationBuilder.DropIndex(
                name: "IX_Armas_personagemId",
                table: "Armas");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Usuarios",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Usuarioid",
                table: "Personagens",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Personagens_Usuarioid",
                table: "Personagens",
                newName: "IX_Personagens_UsuarioId");

            migrationBuilder.RenameColumn(
                name: "personagemId",
                table: "Armas",
                newName: "PersonagemId");

            migrationBuilder.AlterColumn<int>(
                name: "PersonagemId",
                table: "Armas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Armas_PersonagemId",
                table: "Armas",
                column: "PersonagemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Armas_Personagens_PersonagemId",
                table: "Armas",
                column: "PersonagemId",
                principalTable: "Personagens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Personagens_Usuarios_UsuarioId",
                table: "Personagens",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Armas_Personagens_PersonagemId",
                table: "Armas");

            migrationBuilder.DropForeignKey(
                name: "FK_Personagens_Usuarios_UsuarioId",
                table: "Personagens");

            migrationBuilder.DropIndex(
                name: "IX_Armas_PersonagemId",
                table: "Armas");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Usuarios",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Personagens",
                newName: "Usuarioid");

            migrationBuilder.RenameIndex(
                name: "IX_Personagens_UsuarioId",
                table: "Personagens",
                newName: "IX_Personagens_Usuarioid");

            migrationBuilder.RenameColumn(
                name: "PersonagemId",
                table: "Armas",
                newName: "personagemId");

            migrationBuilder.AlterColumn<int>(
                name: "personagemId",
                table: "Armas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Armas_personagemId",
                table: "Armas",
                column: "personagemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Armas_Personagens_personagemId",
                table: "Armas",
                column: "personagemId",
                principalTable: "Personagens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Personagens_Usuarios_Usuarioid",
                table: "Personagens",
                column: "Usuarioid",
                principalTable: "Usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
