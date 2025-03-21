using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_major_project.Migrations
{
    /// <inheritdoc />
    public partial class initBlogData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var today = DateTime.Now;
            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "BlogTitle", "BlogContent", "BlogCreatedDate", "BlogPoster" },
                values: new object[,]
                {
                { 1, "Xe máy trên 5 năm sẽ phải kiểm định khí thải","Xe máy trên 5 năm sẽ phải kiểm định khí thải\r\nChủ sở hữu xe mô tô, xe gắn máy sản xuất từ 5 năm trở lên bắt buộc phải mang xe đi kiểm định khí thải tại các trung tâm đăng kiểm.\r\n\r\nTheo Thông tư 47/2024 của Bộ Giao thông Vận tải, xe mô tô, xe gắn máy có tuổi đời dưới 5 năm được miễn kiểm định khí thải. Xe từ 5 đến 12 năm tuổi phải kiểm định hai năm một lần, còn xe trên 12 năm tuổi phải kiểm định hàng năm.\r\n",
                today,"https://bcp.cdnchinhphu.vn/thumb_w/777/334894974524682240/2024/12/16/82-1692853155-khithaixemay-1689138006805856281099-0-0-436-698-crop-16891380152921359861940-17289016068801282848539-17343301831681947437874.jpg"  },
                }
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
