using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class AddAccionesToCincoPorQue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM sys.columns
                    WHERE object_id = OBJECT_ID(N'[qmcAudHallazgoCincoPorQue]')
                    AND name = N'Acciones'
                )
                ALTER TABLE [qmcAudHallazgoCincoPorQue] ADD [Acciones] nvarchar(2000) NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Acciones",
                table: "qmcAudHallazgoCincoPorQue");
        }
    }
}
