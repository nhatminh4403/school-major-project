using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_major_project.Migrations
{
    /// <inheritdoc />
    public partial class updateReceiptModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MoMoTransactionId",
                table: "Receipts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoMoTransactionId",
                table: "Receipts");
        }
    }
}
