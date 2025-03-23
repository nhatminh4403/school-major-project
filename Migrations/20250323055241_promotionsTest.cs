using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_major_project.Migrations
{
    /// <inheritdoc />
    public partial class promotionsTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*name: "Promotions",
                            columns: table => new
                            {
                                Id = table.Column<int>(type: "int", nullable: false)
                                    .Annotation("SqlServer:Identity", "1, 1"),
                                Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                                StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                                EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                                DiscountRate = table.Column<double>(type: "float", nullable: false),
                                RedemptionPoint = table.Column<int>(type: "int", nullable: false)
                            },*/
            var today = DateTime.Now;
            var enddate = today.AddDays(30);
            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "Code", "Description", "StartDate", "EndDate", "DiscountRate", "RedemptionPoint" },
                values: new object[,]
                {
                { 1, "TEST20", "Test",
                today,enddate,0.2,20 },
                }
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
