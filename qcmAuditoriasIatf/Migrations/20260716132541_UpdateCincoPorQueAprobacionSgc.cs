using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCincoPorQueAprobacionSgc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AprobacionAuditorAsignado",
                table: "qmcAudHallazgoCincoPorQue");

            migrationBuilder.DropColumn(
                name: "AprobacionSegundoAuditor",
                table: "qmcAudHallazgoCincoPorQue");

            migrationBuilder.DropColumn(
                name: "AprobadoPorAuditorAsignadoId",
                table: "qmcAudHallazgoCincoPorQue");

            migrationBuilder.DropColumn(
                name: "ComentarioAuditorAsignado",
                table: "qmcAudHallazgoCincoPorQue");

            migrationBuilder.DropColumn(
                name: "FechaAprobacionAuditorAsignado",
                table: "qmcAudHallazgoCincoPorQue");

            migrationBuilder.RenameColumn(
                name: "FechaAprobacionSegundoAuditor",
                table: "qmcAudHallazgoCincoPorQue",
                newName: "FechaRevisionSgc");

            migrationBuilder.RenameColumn(
                name: "ComentarioSegundoAuditor",
                table: "qmcAudHallazgoCincoPorQue",
                newName: "ComentarioSgc");

            migrationBuilder.RenameColumn(
                name: "AprobadoPorSegundoAuditorId",
                table: "qmcAudHallazgoCincoPorQue",
                newName: "RevisadoPorSgcId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RevisadoPorSgcId",
                table: "qmcAudHallazgoCincoPorQue",
                newName: "AprobadoPorSegundoAuditorId");

            migrationBuilder.RenameColumn(
                name: "FechaRevisionSgc",
                table: "qmcAudHallazgoCincoPorQue",
                newName: "FechaAprobacionSegundoAuditor");

            migrationBuilder.RenameColumn(
                name: "ComentarioSgc",
                table: "qmcAudHallazgoCincoPorQue",
                newName: "ComentarioSegundoAuditor");

            migrationBuilder.AddColumn<bool>(
                name: "AprobacionAuditorAsignado",
                table: "qmcAudHallazgoCincoPorQue",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AprobacionSegundoAuditor",
                table: "qmcAudHallazgoCincoPorQue",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AprobadoPorAuditorAsignadoId",
                table: "qmcAudHallazgoCincoPorQue",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComentarioAuditorAsignado",
                table: "qmcAudHallazgoCincoPorQue",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAprobacionAuditorAsignado",
                table: "qmcAudHallazgoCincoPorQue",
                type: "datetime2",
                nullable: true);
        }
    }
}
