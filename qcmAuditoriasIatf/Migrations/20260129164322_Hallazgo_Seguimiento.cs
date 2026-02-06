using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class Hallazgo_Seguimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "qmcAudHallazgoSeguimiento",
                columns: table => new
                {
                    HallazgoSeguimientoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HallazgoId = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Evidencia = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    EstatusNuevo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qmcAudHallazgoSeguimiento", x => x.HallazgoSeguimientoId);
                    table.ForeignKey(
                        name: "FK_qmcAudHallazgoSeguimiento_qmcAudHallazgo_HallazgoId",
                        column: x => x.HallazgoId,
                        principalTable: "qmcAudHallazgo",
                        principalColumn: "HallazgoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_qmcAudHallazgoSeguimiento_HallazgoId_FechaRegistro",
                table: "qmcAudHallazgoSeguimiento",
                columns: new[] { "HallazgoId", "FechaRegistro" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qmcAudHallazgoSeguimiento");
        }
    }
}
