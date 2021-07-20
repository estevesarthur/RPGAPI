using Microsoft.EntityFrameworkCore.Migrations;

namespace RpgApi.Migrations
{
    public partial class MigrationPerfil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonagemHabilidades_PersonagemHabilidades_PersonagemId1_PersonagemHabilidadeId",
                table: "PersonagemHabilidades");

            migrationBuilder.DropIndex(
                name: "IX_PersonagemHabilidades_PersonagemId1_PersonagemHabilidadeId",
                table: "PersonagemHabilidades");

            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FotoPersonagem",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "PersonagemHabilidadeId",
                table: "PersonagemHabilidades");

            migrationBuilder.DropColumn(
                name: "PersonagemId1",
                table: "PersonagemHabilidades");

            migrationBuilder.AddColumn<string>(
                name: "Perfil",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Jogador");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Perfil",
                table: "Usuarios");

            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoPersonagem",
                table: "Personagens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonagemHabilidadeId",
                table: "PersonagemHabilidades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonagemId1",
                table: "PersonagemHabilidades",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonagemHabilidades_PersonagemId1_PersonagemHabilidadeId",
                table: "PersonagemHabilidades",
                columns: new[] { "PersonagemId1", "PersonagemHabilidadeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PersonagemHabilidades_PersonagemHabilidades_PersonagemId1_PersonagemHabilidadeId",
                table: "PersonagemHabilidades",
                columns: new[] { "PersonagemId1", "PersonagemHabilidadeId" },
                principalTable: "PersonagemHabilidades",
                principalColumns: new[] { "PersonagemId", "HabilidadeId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
