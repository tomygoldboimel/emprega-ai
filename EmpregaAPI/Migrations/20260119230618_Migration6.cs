using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpregaAI.Migrations
{
    public partial class Migration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataConclusao",
                table: "Formacoes");

            migrationBuilder.DropColumn(
                name: "DataInicio",
                table: "Formacoes");

            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "Formacoes");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Formacoes",
                type: "bit",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Formacoes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataConclusao",
                table: "Formacoes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicio",
                table: "Formacoes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nivel",
                table: "Formacoes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
