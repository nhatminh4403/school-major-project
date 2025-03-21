using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_major_project.Migrations
{
    /// <inheritdoc />
    public partial class addCategoryFilm : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CategoryFilm",
                columns: new[] { "CategoriesId", "FilmsId" },
                values: new object[,]
                {
                    {1,2 },
                    {2,3 },
                    {3,4 },
                    {4,5 },
                    {5,6 },
                    {6,7 },
                    {7,8 },
                    {8,2 },
                    {9,1},
                    {10,1 }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
