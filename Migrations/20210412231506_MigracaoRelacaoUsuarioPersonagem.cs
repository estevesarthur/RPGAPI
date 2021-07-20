using Microsoft.EntityFrameworkCore.Migrations;

namespace RpgApi.Migrations
{
    public partial class MigracaoRelacaoUsuarioPersonagem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoPersonagem",
                table: "Personagens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Usuarioid",
                table: "Personagens",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personagens_Usuarioid",
                table: "Personagens",
                column: "Usuarioid");

            migrationBuilder.AddForeignKey(
                name: "FK_Personagens_Usuarios_Usuarioid",
                table: "Personagens",
                column: "Usuarioid",
                principalTable: "Usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personagens_Usuarios_Usuarioid",
                table: "Personagens");

            migrationBuilder.DropIndex(
                name: "IX_Personagens_Usuarioid",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "FotoPersonagem",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Usuarioid",
                table: "Personagens");
        }
    }
}
