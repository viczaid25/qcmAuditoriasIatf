using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class AddValidacionAccionCorrectiva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComentarioSgc",
                table: "qmcAudAccionCorrectiva",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            // Los registros existentes ya pasaron evidencia bajo el proceso anterior (sin esta
            // validación); se marcan como Aprobado para no ocultar su evidencia histórica.
            migrationBuilder.AddColumn<string>(
                name: "EstatusValidacion",
                table: "qmcAudAccionCorrectiva",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Aprobado");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRevisionSgc",
                table: "qmcAudAccionCorrectiva",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevisadoPorSgcId",
                table: "qmcAudAccionCorrectiva",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComentarioSgc",
                table: "qmcAudAccionCorrectiva");

            migrationBuilder.DropColumn(
                name: "EstatusValidacion",
                table: "qmcAudAccionCorrectiva");

            migrationBuilder.DropColumn(
                name: "FechaRevisionSgc",
                table: "qmcAudAccionCorrectiva");

            migrationBuilder.DropColumn(
                name: "RevisadoPorSgcId",
                table: "qmcAudAccionCorrectiva");
        }
    }
}
