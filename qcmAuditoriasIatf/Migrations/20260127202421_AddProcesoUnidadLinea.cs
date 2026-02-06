using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class AddProcesoUnidadLinea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LineaNombre",
                table: "qmcAudProceso",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnidadNegocio",
                table: "qmcAudProceso",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 1,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 2,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 3,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 4,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 5,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 6,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 7,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 8,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 9,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 10,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 11,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 12,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 13,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 14,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 15,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 16,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "qmcAudProceso",
                keyColumn: "ProcesoId",
                keyValue: 17,
                columns: new[] { "LineaNombre", "UnidadNegocio" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineaNombre",
                table: "qmcAudProceso");

            migrationBuilder.DropColumn(
                name: "UnidadNegocio",
                table: "qmcAudProceso");
        }
    }
}
