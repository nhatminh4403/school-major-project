using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_major_project.Migrations
{
    /// <inheritdoc />
    public partial class readdSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeatImage",
                table: "Seats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatImage",
                table: "Seats");
        }
    }
}
