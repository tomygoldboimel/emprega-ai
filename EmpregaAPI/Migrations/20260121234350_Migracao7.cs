using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpregaAI.Migrations
{
    public partial class Migracao7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Curriculos",
                newName: "Telefone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Telefone",
                table: "Curriculos",
                newName: "Email");
        }
    }
}
