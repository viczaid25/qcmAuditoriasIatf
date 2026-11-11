using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class ConvertirCompetenciaASiNo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calificacion",
                table: "qmcAudAuditorCompetencia");

            migrationBuilder.AddColumn<bool>(
                name: "TieneCompetencia",
                table: "qmcAudAuditorCompetencia",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TieneCompetencia",
                table: "qmcAudAuditorCompetencia");

            migrationBuilder.AddColumn<int>(
                name: "Calificacion",
                table: "qmcAudAuditorCompetencia",
                type: "int",
                nullable: true);
        }
    }
}
