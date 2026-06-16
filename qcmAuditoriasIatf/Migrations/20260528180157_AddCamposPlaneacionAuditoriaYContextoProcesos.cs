using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposPlaneacionAuditoriaYContextoProcesos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            



            migrationBuilder.AddColumn<string>(
                name: "ClausulasIatfTop3NcmMayoresPasadas",
                table: "qmcAudAuditoriaProceso",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClausulasIatfTop3NcmMenoresPasadas",
                table: "qmcAudAuditoriaProceso",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReclamosAuditoriasPasadas",
                table: "qmcAudAuditoriaProceso",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResultadosAuditoriasInternasPrevias",
                table: "qmcAudAuditoriaProceso",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeguimientoAccionesCorrectivasIatf",
                table: "qmcAudAuditoriaProceso",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Criterios",
                table: "qmcAudAuditoria",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Metodos",
                table: "qmcAudAuditoria",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

    
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qmcAudUsuarioPerfil");

            migrationBuilder.DropColumn(
                name: "ComentarioRevisionSgc",
                table: "qmcAudHallazgo");

            migrationBuilder.DropColumn(
                name: "CreadoPorId",
                table: "qmcAudHallazgo");

            migrationBuilder.DropColumn(
                name: "EstatusValidacionSgc",
                table: "qmcAudHallazgo");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "qmcAudHallazgo");

            migrationBuilder.DropColumn(
                name: "FechaRevisionSgc",
                table: "qmcAudHallazgo");

            migrationBuilder.DropColumn(
                name: "JustificacionNoConformidad",
                table: "qmcAudHallazgo");

            migrationBuilder.DropColumn(
                name: "RevisadoPorSgcId",
                table: "qmcAudHallazgo");

            migrationBuilder.DropColumn(
                name: "ClausulasIatfTop3NcmMayoresPasadas",
                table: "qmcAudAuditoriaProceso");

            migrationBuilder.DropColumn(
                name: "ClausulasIatfTop3NcmMenoresPasadas",
                table: "qmcAudAuditoriaProceso");

            migrationBuilder.DropColumn(
                name: "ReclamosAuditoriasPasadas",
                table: "qmcAudAuditoriaProceso");

            migrationBuilder.DropColumn(
                name: "ResultadosAuditoriasInternasPrevias",
                table: "qmcAudAuditoriaProceso");

            migrationBuilder.DropColumn(
                name: "SeguimientoAccionesCorrectivasIatf",
                table: "qmcAudAuditoriaProceso");

            migrationBuilder.DropColumn(
                name: "Criterios",
                table: "qmcAudAuditoria");

            migrationBuilder.DropColumn(
                name: "Metodos",
                table: "qmcAudAuditoria");
        }
    }
}
