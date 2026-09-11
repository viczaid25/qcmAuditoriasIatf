using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class AddInformeAuditoriaCampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeguimientoEntregaAnalisis",
                table: "qmcAudHallazgo",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeguimientoImplementacionCierre",
                table: "qmcAudHallazgo",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeguimientoVerificacion",
                table: "qmcAudHallazgo",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Conclusiones",
                table: "qmcAudAuditoria",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeguimientoEntregaAnalisis",
                table: "qmcAudHallazgo");

            migrationBuilder.DropColumn(
                name: "SeguimientoImplementacionCierre",
                table: "qmcAudHallazgo");

            migrationBuilder.DropColumn(
                name: "SeguimientoVerificacion",
                table: "qmcAudHallazgo");

            migrationBuilder.DropColumn(
                name: "Conclusiones",
                table: "qmcAudAuditoria");
        }
    }
}
