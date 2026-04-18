using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G3CustomerAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "customer");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customers",
                newSchema: "customer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Customers",
                schema: "customer",
                newName: "Customers");
        }
    }
}
