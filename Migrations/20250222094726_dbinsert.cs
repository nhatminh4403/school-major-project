using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_major_project.Migrations
{
    /// <inheritdoc />
    public partial class dbinsert : Migration
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

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                    {
                { 1,"Việt Nam" }
                    }
                );
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
            migrationBuilder.InsertData(
                table: "Films",
                columns: new[] { "Id", "Name", "Description", "PosterUrl", "TrailerUrl",
                         "DirectorName","Language","FilmRated","FilmDuration","Actors","Quality","StartTime","CountryId"},
                values: new object[,]
                {
             {1, "LẬT MẶT 7: MỘT ĐIỀU ƯỚC",
              "Một câu chuyện lần đầu được kể bằng tất cả tình yêu, và từ tất cả những hồi ức xao xuyến nhất của đấng sinh thành",
              "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/l/a/lat-mat-7.jpg",
              "https://youtu.be/d1ZHdosjNX8?si=f6_pea9cJ5HtvnJy",
               "Lý Hải",
                 "Tiếng Việt - Phụ đề Tiếng Anh",
                 "K - Phim được phổ biến đến người xem dưới 13 tuổi và có người bảo hộ đi kèm",
                 138,
                 "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...","3D",
                 new DateTime(2024, 4, 26, 10, 15, 0),1
             },
             {2, "LẬT MẶT 7: MỘT ĐIỀU ƯỚC",
              "Một câu chuyện lần đầu được kể bằng tất cả tình yêu, và từ tất cả những hồi ức xao xuyến nhất của đấng sinh thành",
              "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/l/a/lat-mat-7.jpg",
              "https://youtu.be/d1ZHdosjNX8?si=f6_pea9cJ5HtvnJy",
               "Lý Hải",
                 "Tiếng Việt - Phụ đề Tiếng Anh",
                 "K - Phim được phổ biến đến người xem dưới 13 tuổi và có người bảo hộ đi kèm",
                 138,
                 "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...","3D",
                 new DateTime(2024, 4, 26, 10, 15, 0),1
             },
             {3, "LẬT MẶT 7: MỘT ĐIỀU ƯỚC",
              "Một câu chuyện lần đầu được kể bằng tất cả tình yêu, và từ tất cả những hồi ức xao xuyến nhất của đấng sinh thành",
              "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/l/a/lat-mat-7.jpg",
              "https://youtu.be/d1ZHdosjNX8?si=f6_pea9cJ5HtvnJy",
               "Lý Hải",
                 "Tiếng Việt - Phụ đề Tiếng Anh",
                 "K - Phim được phổ biến đến người xem dưới 13 tuổi và có người bảo hộ đi kèm",
                 138,
                 "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...","3D",
                 new DateTime(2024, 4, 26, 10, 15, 0),1
             },
             {4, "LẬT MẶT 7: MỘT ĐIỀU ƯỚC",
              "Một câu chuyện lần đầu được kể bằng tất cả tình yêu, và từ tất cả những hồi ức xao xuyến nhất của đấng sinh thành",
              "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/l/a/lat-mat-7.jpg",
              "https://youtu.be/d1ZHdosjNX8?si=f6_pea9cJ5HtvnJy",
               "Lý Hải",
                 "Tiếng Việt - Phụ đề Tiếng Anh",
                 "K - Phim được phổ biến đến người xem dưới 13 tuổi và có người bảo hộ đi kèm",
                 138,
                 "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...","3D",
                 new DateTime(2024, 4, 26, 10, 15, 0),1
             },
             {5, "LẬT MẶT 7: MỘT ĐIỀU ƯỚC",
              "Một câu chuyện lần đầu được kể bằng tất cả tình yêu, và từ tất cả những hồi ức xao xuyến nhất của đấng sinh thành",
              "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/l/a/lat-mat-7.jpg",
              "https://youtu.be/d1ZHdosjNX8?si=f6_pea9cJ5HtvnJy",
               "Lý Hải",
                 "Tiếng Việt - Phụ đề Tiếng Anh",
                 "K - Phim được phổ biến đến người xem dưới 13 tuổi và có người bảo hộ đi kèm",
                 138,
                 "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...","3D",
                 new DateTime(2024, 4, 26, 10, 15, 0),1
             },
             {6, "LẬT MẶT 7: MỘT ĐIỀU ƯỚC",
              "Một câu chuyện lần đầu được kể bằng tất cả tình yêu, và từ tất cả những hồi ức xao xuyến nhất của đấng sinh thành",
              "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/l/a/lat-mat-7.jpg",
              "https://youtu.be/d1ZHdosjNX8?si=f6_pea9cJ5HtvnJy",
               "Lý Hải",
                 "Tiếng Việt - Phụ đề Tiếng Anh",
                 "K - Phim được phổ biến đến người xem dưới 13 tuổi và có người bảo hộ đi kèm",
                 138,
                 "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...","3D",
                 new DateTime(2024, 4, 26, 10, 15, 0),1
             },
             {7, "LẬT MẶT 7: MỘT ĐIỀU ƯỚC",
              "Một câu chuyện lần đầu được kể bằng tất cả tình yêu, và từ tất cả những hồi ức xao xuyến nhất của đấng sinh thành",
              "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/l/a/lat-mat-7.jpg",
              "https://youtu.be/d1ZHdosjNX8?si=f6_pea9cJ5HtvnJy",
               "Lý Hải",
                 "Tiếng Việt - Phụ đề Tiếng Anh",
                 "K - Phim được phổ biến đến người xem dưới 13 tuổi và có người bảo hộ đi kèm",
                 138,
                 "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...","3D",
                 new DateTime(2024, 4, 26, 10, 15, 0),1
             },
             {8, "LẬT MẶT 7: MỘT ĐIỀU ƯỚC",
              "Một câu chuyện lần đầu được kể bằng tất cả tình yêu, và từ tất cả những hồi ức xao xuyến nhất của đấng sinh thành",
              "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/l/a/lat-mat-7.jpg",
              "https://youtu.be/d1ZHdosjNX8?si=f6_pea9cJ5HtvnJy",
               "Lý Hải",
                 "Tiếng Việt - Phụ đề Tiếng Anh",
                 "K - Phim được phổ biến đến người xem dưới 13 tuổi và có người bảo hộ đi kèm",
                 138,
                 "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...","3D",
                 new DateTime(2024, 4, 26, 10, 15, 0),1
             }

                }
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
