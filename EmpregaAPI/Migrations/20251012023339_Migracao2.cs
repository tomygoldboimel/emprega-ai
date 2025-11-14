using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpregaAI.Migrations
{
    public partial class Migracao2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cargo",
                table: "Curriculos");

            migrationBuilder.DropColumn(
                name: "Certificacao",
                table: "Curriculos");

            migrationBuilder.DropColumn(
                name: "Curso",
                table: "Curriculos");

            migrationBuilder.DropColumn(
                name: "DescricaoAtividade",
                table: "Curriculos");

            migrationBuilder.DropColumn(
                name: "Empresa",
                table: "Curriculos");

            migrationBuilder.DropColumn(
                name: "Endereço",
                table: "Curriculos");

            migrationBuilder.RenameColumn(
                name: "PeriodoInstituto",
                table: "Curriculos",
                newName: "ResumoProfissional");

            migrationBuilder.RenameColumn(
                name: "PeriodoCertificacao",
                table: "Curriculos",
                newName: "NomeCompleto");

            migrationBuilder.RenameColumn(
                name: "PeriodoCargo",
                table: "Curriculos",
                newName: "LinkedIn");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Curriculos",
                newName: "GitHub");

            migrationBuilder.RenameColumn(
                name: "NivelEducacional",
                table: "Curriculos",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "Instituto",
                table: "Curriculos",
                newName: "Endereco");

            migrationBuilder.RenameColumn(
                name: "InstituicaoCertificacao",
                table: "Curriculos",
                newName: "Cidade");

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "Curriculos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Certificacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurriculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instituicao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CargaHoraria = table.Column<int>(type: "int", nullable: true),
                    UrlCertificado = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificacoes_Curriculos_CurriculoId",
                        column: x => x.CurriculoId,
                        principalTable: "Curriculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experiencias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurriculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Empresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmpregoAtual = table.Column<bool>(type: "bit", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiencias_Curriculos_CurriculoId",
                        column: x => x.CurriculoId,
                        principalTable: "Curriculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Formacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurriculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Instituicao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Curso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nivel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Formacoes_Curriculos_CurriculoId",
                        column: x => x.CurriculoId,
                        principalTable: "Curriculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Curriculos_UsuarioId",
                table: "Curriculos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificacoes_CurriculoId",
                table: "Certificacoes",
                column: "CurriculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiencias_CurriculoId",
                table: "Experiencias",
                column: "CurriculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Formacoes_CurriculoId",
                table: "Formacoes",
                column: "CurriculoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Curriculos_Usuarios_UsuarioId",
                table: "Curriculos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Curriculos_Usuarios_UsuarioId",
                table: "Curriculos");

            migrationBuilder.DropTable(
                name: "Certificacoes");

            migrationBuilder.DropTable(
                name: "Experiencias");

            migrationBuilder.DropTable(
                name: "Formacoes");

            migrationBuilder.DropIndex(
                name: "IX_Curriculos_UsuarioId",
                table: "Curriculos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Curriculos");

            migrationBuilder.RenameColumn(
                name: "ResumoProfissional",
                table: "Curriculos",
                newName: "PeriodoInstituto");

            migrationBuilder.RenameColumn(
                name: "NomeCompleto",
                table: "Curriculos",
                newName: "PeriodoCertificacao");

            migrationBuilder.RenameColumn(
                name: "LinkedIn",
                table: "Curriculos",
                newName: "PeriodoCargo");

            migrationBuilder.RenameColumn(
                name: "GitHub",
                table: "Curriculos",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Curriculos",
                newName: "NivelEducacional");

            migrationBuilder.RenameColumn(
                name: "Endereco",
                table: "Curriculos",
                newName: "Instituto");

            migrationBuilder.RenameColumn(
                name: "Cidade",
                table: "Curriculos",
                newName: "InstituicaoCertificacao");

            migrationBuilder.AddColumn<string>(
                name: "Cargo",
                table: "Curriculos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Certificacao",
                table: "Curriculos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Curso",
                table: "Curriculos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescricaoAtividade",
                table: "Curriculos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Empresa",
                table: "Curriculos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endereço",
                table: "Curriculos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
