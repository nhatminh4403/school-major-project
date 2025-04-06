using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_major_project.Migrations
{
    /// <inheritdoc />
    public partial class updateSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatImage",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatPrice",
                table: "Seats");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "SeatTypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Price",
                table: "SeatTypes",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "SeatImage",
                table: "Seats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SeatPrice",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
