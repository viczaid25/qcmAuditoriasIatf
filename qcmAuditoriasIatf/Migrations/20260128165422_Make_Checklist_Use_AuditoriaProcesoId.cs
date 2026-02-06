using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class Make_Checklist_Use_AuditoriaProcesoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_qmcAudAuditoriaChecklist_qmcAudAuditoria_AuditoriaId",
                table: "qmcAudAuditoriaChecklist");

            migrationBuilder.DropForeignKey(
                name: "FK_qmcAudAuditoriaChecklist_qmcAudChecklistPregunta_PreguntaId",
                table: "qmcAudAuditoriaChecklist");

            migrationBuilder.DropForeignKey(
                name: "FK_qmcAudAuditoriaChecklist_qmcAudProceso_ProcesoId",
                table: "qmcAudAuditoriaChecklist");

            migrationBuilder.DropIndex(
                name: "IX_qmcAudAuditoriaChecklist_ProcesoId",
                table: "qmcAudAuditoriaChecklist");

            migrationBuilder.RenameColumn(
                name: "ProcesoId",
                table: "qmcAudAuditoriaChecklist",
                newName: "AuditoriaProcesoId");

            migrationBuilder.AlterColumn<int>(
                name: "AuditoriaId",
                table: "qmcAudAuditoriaChecklist",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_qmcAudAuditoriaChecklist_AuditoriaProcesoId_PreguntaId",
                table: "qmcAudAuditoriaChecklist",
                columns: new[] { "AuditoriaProcesoId", "PreguntaId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_qmcAudAuditoriaChecklist_qmcAudAuditoriaProceso_AuditoriaProcesoId",
                table: "qmcAudAuditoriaChecklist",
                column: "AuditoriaProcesoId",
                principalTable: "qmcAudAuditoriaProceso",
                principalColumn: "AuditoriaProcesoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_qmcAudAuditoriaChecklist_qmcAudAuditoria_AuditoriaId",
                table: "qmcAudAuditoriaChecklist",
                column: "AuditoriaId",
                principalTable: "qmcAudAuditoria",
                principalColumn: "AuditoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_qmcAudAuditoriaChecklist_qmcAudChecklistPregunta_PreguntaId",
                table: "qmcAudAuditoriaChecklist",
                column: "PreguntaId",
                principalTable: "qmcAudChecklistPregunta",
                principalColumn: "PreguntaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_qmcAudAuditoriaChecklist_qmcAudAuditoriaProceso_AuditoriaProcesoId",
                table: "qmcAudAuditoriaChecklist");

            migrationBuilder.DropForeignKey(
                name: "FK_qmcAudAuditoriaChecklist_qmcAudAuditoria_AuditoriaId",
                table: "qmcAudAuditoriaChecklist");

            migrationBuilder.DropForeignKey(
                name: "FK_qmcAudAuditoriaChecklist_qmcAudChecklistPregunta_PreguntaId",
                table: "qmcAudAuditoriaChecklist");

            migrationBuilder.DropIndex(
                name: "IX_qmcAudAuditoriaChecklist_AuditoriaProcesoId_PreguntaId",
                table: "qmcAudAuditoriaChecklist");

            migrationBuilder.RenameColumn(
                name: "AuditoriaProcesoId",
                table: "qmcAudAuditoriaChecklist",
                newName: "ProcesoId");

            migrationBuilder.AlterColumn<int>(
                name: "AuditoriaId",
                table: "qmcAudAuditoriaChecklist",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_qmcAudAuditoriaChecklist_ProcesoId",
                table: "qmcAudAuditoriaChecklist",
                column: "ProcesoId");

            migrationBuilder.AddForeignKey(
                name: "FK_qmcAudAuditoriaChecklist_qmcAudAuditoria_AuditoriaId",
                table: "qmcAudAuditoriaChecklist",
                column: "AuditoriaId",
                principalTable: "qmcAudAuditoria",
                principalColumn: "AuditoriaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_qmcAudAuditoriaChecklist_qmcAudChecklistPregunta_PreguntaId",
                table: "qmcAudAuditoriaChecklist",
                column: "PreguntaId",
                principalTable: "qmcAudChecklistPregunta",
                principalColumn: "PreguntaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_qmcAudAuditoriaChecklist_qmcAudProceso_ProcesoId",
                table: "qmcAudAuditoriaChecklist",
                column: "ProcesoId",
                principalTable: "qmcAudProceso",
                principalColumn: "ProcesoId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
