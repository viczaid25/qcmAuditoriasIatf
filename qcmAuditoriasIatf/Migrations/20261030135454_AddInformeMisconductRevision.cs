using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class AddInformeMisconductRevision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "qmcAudInformeMisconductRevision",
                columns: table => new
                {
                    InformeMisconductRevisionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditoriaId = table.Column<int>(type: "int", nullable: false),
                    RevisorId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SolicitadoPorId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Confirmada = table.Column<bool>(type: "bit", nullable: false),
                    FechaConfirmacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HashContenido = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qmcAudInformeMisconductRevision", x => x.InformeMisconductRevisionId);
                    table.ForeignKey(
                        name: "FK_qmcAudInformeMisconductRevision_qmcAudAuditoria_AuditoriaId",
                        column: x => x.AuditoriaId,
                        principalTable: "qmcAudAuditoria",
                        principalColumn: "AuditoriaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_qmcAudInformeMisconductRevision_AuditoriaId_RevisorId",
                table: "qmcAudInformeMisconductRevision",
                columns: new[] { "AuditoriaId", "RevisorId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qmcAudInformeMisconductRevision");
        }
    }
}
