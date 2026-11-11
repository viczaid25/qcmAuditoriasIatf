using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class AddMatrizCompetenciasAuditores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsAuditorLider",
                table: "qmcAudAuditor",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "qmcAudCompetenciaAuditor",
                columns: table => new
                {
                    CompetenciaAuditorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Grupo = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Obligatoria = table.Column<bool>(type: "bit", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qmcAudCompetenciaAuditor", x => x.CompetenciaAuditorId);
                });

            migrationBuilder.CreateTable(
                name: "qmcAudAuditorCompetencia",
                columns: table => new
                {
                    AuditorCompetenciaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditorId = table.Column<int>(type: "int", nullable: false),
                    CompetenciaAuditorId = table.Column<int>(type: "int", nullable: false),
                    Calificacion = table.Column<int>(type: "int", nullable: true),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualizadoPorId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qmcAudAuditorCompetencia", x => x.AuditorCompetenciaId);
                    table.ForeignKey(
                        name: "FK_qmcAudAuditorCompetencia_qmcAudAuditor_AuditorId",
                        column: x => x.AuditorId,
                        principalTable: "qmcAudAuditor",
                        principalColumn: "AuditorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_qmcAudAuditorCompetencia_qmcAudCompetenciaAuditor_CompetenciaAuditorId",
                        column: x => x.CompetenciaAuditorId,
                        principalTable: "qmcAudCompetenciaAuditor",
                        principalColumn: "CompetenciaAuditorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_qmcAudAuditorCompetencia_AuditorId_CompetenciaAuditorId",
                table: "qmcAudAuditorCompetencia",
                columns: new[] { "AuditorId", "CompetenciaAuditorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_qmcAudAuditorCompetencia_CompetenciaAuditorId",
                table: "qmcAudAuditorCompetencia",
                column: "CompetenciaAuditorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qmcAudAuditorCompetencia");

            migrationBuilder.DropTable(
                name: "qmcAudCompetenciaAuditor");

            migrationBuilder.DropColumn(
                name: "EsAuditorLider",
                table: "qmcAudAuditor");
        }
    }
}
