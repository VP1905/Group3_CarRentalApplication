using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G3MaintenanceAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMaintenanceUnified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "maintenance");

            migrationBuilder.RenameTable(
                name: "RepairHistories",
                newName: "RepairHistories",
                newSchema: "maintenance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "RepairHistories",
                schema: "maintenance",
                newName: "RepairHistories");
        }
    }
}
