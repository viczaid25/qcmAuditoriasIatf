using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class Hallazgo_Link_Checklist_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.qmcAudHallazgo','AuditoriaProcesoId') IS NULL
BEGIN
    ALTER TABLE dbo.qmcAudHallazgo ADD AuditoriaProcesoId int NULL;
END

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_qmcAudHallazgo_AuditoriaProcesoId_PreguntaId'
      AND object_id = OBJECT_ID('dbo.qmcAudHallazgo')
)
BEGIN
    CREATE UNIQUE INDEX IX_qmcAudHallazgo_AuditoriaProcesoId_PreguntaId
    ON dbo.qmcAudHallazgo (AuditoriaProcesoId, PreguntaId)
    WHERE AuditoriaProcesoId IS NOT NULL AND PreguntaId IS NOT NULL;
END

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = 'FK_qmcAudHallazgo_qmcAudAuditoriaProceso_AuditoriaProcesoId'
)
BEGIN
    ALTER TABLE dbo.qmcAudHallazgo
    ADD CONSTRAINT FK_qmcAudHallazgo_qmcAudAuditoriaProceso_AuditoriaProcesoId
    FOREIGN KEY (AuditoriaProcesoId) REFERENCES dbo.qmcAudAuditoriaProceso(AuditoriaProcesoId);
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
