using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class AddVerificacionQmsHallazgo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComentarioVerificacionQms",
                table: "qmcAudHallazgo",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaVerificacionQms",
                table: "qmcAudHallazgo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificadoPorId",
                table: "qmcAudHallazgo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VerificadoQms",
                table: "qmcAudHallazgo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComentarioVerificacionQms",
                table: "qmcAudHallazgo");

            migrationBuilder.DropColumn(
                name: "FechaVerificacionQms",
                table: "qmcAudHallazgo");

            migrationBuilder.DropColumn(
                name: "VerificadoPorId",
                table: "qmcAudHallazgo");

            migrationBuilder.DropColumn(
                name: "VerificadoQms",
                table: "qmcAudHallazgo");
        }
    }
}
