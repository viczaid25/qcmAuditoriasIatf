using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class AddResponsableCierreToHallazgo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponsableCierreId",
                table: "qmcAudHallazgo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponsableCierreId",
                table: "qmcAudHallazgo");
        }
    }
}
