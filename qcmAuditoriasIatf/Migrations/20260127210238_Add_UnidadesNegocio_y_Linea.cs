using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class Add_UnidadesNegocio_y_Linea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

          

            migrationBuilder.AddColumn<string>(
                name: "LineaNombre",
                table: "qmcAudAuditoriaProceso",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnidadNegocioId",
                table: "qmcAudAuditoriaProceso",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "qmcAudUnidadNegocio",
                columns: table => new
                {
                    UnidadNegocioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcesoId = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qmcAudUnidadNegocio", x => x.UnidadNegocioId);
                    table.ForeignKey(
                        name: "FK_qmcAudUnidadNegocio_qmcAudProceso_ProcesoId",
                        column: x => x.ProcesoId,
                        principalTable: "qmcAudProceso",
                        principalColumn: "ProcesoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_qmcAudAuditoriaProceso_UnidadNegocioId",
                table: "qmcAudAuditoriaProceso",
                column: "UnidadNegocioId");

            migrationBuilder.CreateIndex(
                name: "IX_qmcAudUnidadNegocio_ProcesoId",
                table: "qmcAudUnidadNegocio",
                column: "ProcesoId");

            migrationBuilder.AddForeignKey(
                name: "FK_qmcAudAuditoriaProceso_qmcAudUnidadNegocio_UnidadNegocioId",
                table: "qmcAudAuditoriaProceso",
                column: "UnidadNegocioId",
                principalTable: "qmcAudUnidadNegocio",
                principalColumn: "UnidadNegocioId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_qmcAudAuditoriaProceso_qmcAudUnidadNegocio_UnidadNegocioId",
                table: "qmcAudAuditoriaProceso");

            migrationBuilder.DropTable(
                name: "qmcAudUnidadNegocio");

            migrationBuilder.DropIndex(
                name: "IX_qmcAudAuditoriaProceso_UnidadNegocioId",
                table: "qmcAudAuditoriaProceso");

            migrationBuilder.DropColumn(
                name: "LineaNombre",
                table: "qmcAudAuditoriaProceso");

            migrationBuilder.DropColumn(
                name: "UnidadNegocioId",
                table: "qmcAudAuditoriaProceso");

            migrationBuilder.AddColumn<string>(
                name: "Linea",
                table: "qmcAudAuditoriaProceso",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnidadNegocio",
                table: "qmcAudAuditoriaProceso",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
