using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class AddHallazgoCincoPorQueFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "qmcAudHallazgoCincoPorQue",
                columns: table => new
                {
                    HallazgoCincoPorQueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HallazgoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioRegistroId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PorQue1 = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PorQue2 = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PorQue3 = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PorQue4 = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PorQue5 = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Estatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AprobacionAuditorAsignado = table.Column<bool>(type: "bit", nullable: true),
                    AprobadoPorAuditorAsignadoId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaAprobacionAuditorAsignado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ComentarioAuditorAsignado = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    AprobacionSegundoAuditor = table.Column<bool>(type: "bit", nullable: true),
                    AprobadoPorSegundoAuditorId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaAprobacionSegundoAuditor = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ComentarioSegundoAuditor = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qmcAudHallazgoCincoPorQue", x => x.HallazgoCincoPorQueId);
                    table.ForeignKey(
                        name: "FK_qmcAudHallazgoCincoPorQue_qmcAudHallazgo_HallazgoId",
                        column: x => x.HallazgoId,
                        principalTable: "qmcAudHallazgo",
                        principalColumn: "HallazgoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_qmcAudHallazgoCincoPorQue_HallazgoId",
                table: "qmcAudHallazgoCincoPorQue",
                column: "HallazgoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qmcAudHallazgoCincoPorQue");
        }
    }
}
