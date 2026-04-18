using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G3ReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialReservationUnified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "reservation");

            migrationBuilder.RenameTable(
                name: "Reservations",
                newName: "Reservations",
                newSchema: "reservation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Reservations",
                schema: "reservation",
                newName: "Reservations");
        }
    }
}
