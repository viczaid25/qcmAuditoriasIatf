using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class AddEvaluacionAuditores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "qmcAudEvaluacionAuditor",
                columns: table => new
                {
                    EvaluacionAuditorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditoriaId = table.Column<int>(type: "int", nullable: false),
                    AuditorEvaluadoId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Estatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEvaluacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponsablesEvaluacion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    TotalHorasAuditadas = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CalificacionGeneral = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    CompletadaPorId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaCompletada = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qmcAudEvaluacionAuditor", x => x.EvaluacionAuditorId);
                    table.ForeignKey(
                        name: "FK_qmcAudEvaluacionAuditor_qmcAudAuditoria_AuditoriaId",
                        column: x => x.AuditoriaId,
                        principalTable: "qmcAudAuditoria",
                        principalColumn: "AuditoriaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "qmcAudEvaluacionAuditorRespuesta",
                columns: table => new
                {
                    EvaluacionAuditorRespuestaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvaluacionAuditorId = table.Column<int>(type: "int", nullable: false),
                    NumeroPregunta = table.Column<int>(type: "int", nullable: false),
                    Calificacion = table.Column<int>(type: "int", nullable: true),
                    Comentario = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qmcAudEvaluacionAuditorRespuesta", x => x.EvaluacionAuditorRespuestaId);
                    table.ForeignKey(
                        name: "FK_qmcAudEvaluacionAuditorRespuesta_qmcAudEvaluacionAuditor_EvaluacionAuditorId",
                        column: x => x.EvaluacionAuditorId,
                        principalTable: "qmcAudEvaluacionAuditor",
                        principalColumn: "EvaluacionAuditorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_qmcAudEvaluacionAuditor_AuditoriaId_AuditorEvaluadoId",
                table: "qmcAudEvaluacionAuditor",
                columns: new[] { "AuditoriaId", "AuditorEvaluadoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_qmcAudEvaluacionAuditorRespuesta_EvaluacionAuditorId_NumeroPregunta",
                table: "qmcAudEvaluacionAuditorRespuesta",
                columns: new[] { "EvaluacionAuditorId", "NumeroPregunta" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qmcAudEvaluacionAuditorRespuesta");

            migrationBuilder.DropTable(
                name: "qmcAudEvaluacionAuditor");
        }
    }
}
