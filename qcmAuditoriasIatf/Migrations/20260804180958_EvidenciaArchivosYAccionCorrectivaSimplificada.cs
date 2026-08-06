using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class EvidenciaArchivosYAccionCorrectivaSimplificada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccionDefinida",
                table: "qmcAudAccionCorrectiva");

            migrationBuilder.DropColumn(
                name: "EvidenciaImplementacion",
                table: "qmcAudAccionCorrectiva");

            migrationBuilder.AddColumn<string>(
                name: "NombreOriginal",
                table: "qmcAudEvidencia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "AnalisisCausa",
                table: "qmcAudAccionCorrectiva",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreOriginal",
                table: "qmcAudEvidencia");

            migrationBuilder.AlterColumn<string>(
                name: "AnalisisCausa",
                table: "qmcAudAccionCorrectiva",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccionDefinida",
                table: "qmcAudAccionCorrectiva",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EvidenciaImplementacion",
                table: "qmcAudAccionCorrectiva",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
