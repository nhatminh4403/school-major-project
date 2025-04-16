using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_major_project.Migrations
{
    /// <inheritdoc />
    public partial class changeSeatName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatName",
                table: "Receipts");

            migrationBuilder.AddColumn<string>(
                name: "SeatName",
                table: "ReceiptDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatName",
                table: "ReceiptDetails");

            migrationBuilder.AddColumn<string>(
                name: "SeatName",
                table: "Receipts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
