using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class AddSegundoAuditorYObservadorEnAuditoriaProceso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ObservadorId",
                table: "qmcAudAuditoriaProceso",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SegundoAuditorId",
                table: "qmcAudAuditoriaProceso",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObservadorId",
                table: "qmcAudAuditoriaProceso");

            migrationBuilder.DropColumn(
                name: "SegundoAuditorId",
                table: "qmcAudAuditoriaProceso");
        }
    }
}
