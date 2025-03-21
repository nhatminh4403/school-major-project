using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_major_project.Migrations
{
    /// <inheritdoc />
    public partial class dbInitCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryDescription" },
                values: new object[,]
                {
                { 1, "Hành động" },
                { 2, "Kinh dị" },
                { 3, "Tình cảm" },
                { 4, "Hoạt hình" },
                { 5, "Hài" },
                { 6, "Tâm lý" },
                { 7, "Phiêu lưu" },
                { 8, "Viễn tưởng" },
                { 9, "Thần thoại" },
                { 10, "Chiến tranh" }
                }
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
