using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qcmAuditoriasIatf.Migrations
{
    /// <inheritdoc />
    public partial class Add_Auditores_Catalogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "qmcAudAuditor",
                columns: table => new
                {
                    AuditorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeaxUserId = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Funcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PcLoginId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qmcAudAuditor", x => x.AuditorId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_qmcAudAuditor_MeaxUserId",
                table: "qmcAudAuditor",
                column: "MeaxUserId");

            migrationBuilder.CreateIndex(
                name: "IX_qmcAudAuditor_PcLoginId",
                table: "qmcAudAuditor",
                column: "PcLoginId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qmcAudAuditor");
        }
    }
}
