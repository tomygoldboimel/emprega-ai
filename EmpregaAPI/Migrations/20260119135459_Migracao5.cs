using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpregaAI.Migrations
{
    public partial class Migracao5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Curriculos");

            migrationBuilder.DropColumn(
                name: "GitHub",
                table: "Curriculos");

            migrationBuilder.DropColumn(
                name: "LinkedIn",
                table: "Curriculos");

            migrationBuilder.RenameColumn(
                name: "ResumoProfissional",
                table: "Curriculos",
                newName: "Objetivo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Objetivo",
                table: "Curriculos",
                newName: "ResumoProfissional");

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Curriculos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHub",
                table: "Curriculos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedIn",
                table: "Curriculos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
