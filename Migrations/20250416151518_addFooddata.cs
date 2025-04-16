using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_major_project.Migrations
{
    /// <inheritdoc />
    public partial class addFooddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Foods",
                columns: new[] { "Id", "ComboName", "Price", "Description", "Poster" },
                values: new object[,]
                {
                    { 1, "Pizza", 88000, "pizza","https://www.foodandwine.com/thmb/Wd4lBRZz3X_8qBr69UOu2m7I2iw=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/classic-cheese-pizza-FT-RECIPE0422-31a2c938fc2546c9a07b7011658cfd05.jpg" },
                    { 2, "Burger", 50000, "burger" ,"https://www.certifiedangusbeef.com/_next/image?url=https%3A%2F%2Fappetizing-cactus-7139e93734.media.strapiapp.com%2FClassic_Smashed_Burger_111c4bfdb7.jpeg&w=3840&q=75"},
                    { 3, "Pasta", 79000, "pasta" ,"https://www.goodnes.com/sites/g/files/jgfbjl321/files/styles/facebook_share/public/recipe-thumbnail/116751-0a0717810b73a1672a029c29788e557b_creamy_alfredo_pasta_long_left.jpg?itok=gZ6FDaar"},
                    { 4, "Salad", 49000, "Salad", "https://images.immediate.co.uk/production/volatile/sites/30/2014/05/Epic-summer-salad-hub-2646e6e.jpg?resize=1200%2C630"},
                    { 5, "Soda", 19000, "soda", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTS_FsenX3zvJZB78-xxCUxGH4-OvTWQChYxA&s"},
                    { 6, "Water", 7500, "water","https://sonhawater.vn/wp-content/uploads/2020/10/aquafina-15-lit.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
