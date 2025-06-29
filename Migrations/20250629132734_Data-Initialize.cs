using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_major_project.Migrations
{
    /// <inheritdoc />
    public partial class DataInitialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var today = DateTime.Now;
            var enddate = today.AddDays(30);

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Name", "Location", "Map" },
                values: new object[,]
                    {
                       { 1, "Cinema 1","12, Võ Văn Ngân","dadsa"},
                       { 2, "Cinema 2","Lầu 9 Crescent Mall","dadada"},
                    }
            );

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name", "Description", "CinemaId" },
                values: new object[,]
                    {
                       { 1, "Room 1","Đi thẳng tới cuối hành lang rẽ trái" ,1},
                       { 2, "Room 2","Đi thẳng tới cuối hành lang rẽ phải" ,1},

                       { 3, "Room 1","Đi thẳng tới cuối hành lang rẽ trái",2},
                       { 4, "Room 2","Đi thẳng tới cuối hành lang rẽ phải",2},

                    }
            );

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "Code", "Description", "StartDate", "EndDate", "DiscountRate", "RedemptionPoint" },
                values: new object[,]
                {
   { 1, "TEST20", "Test",
   today,enddate,0.2,20 },
                }
                );
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
            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "ScheduleTime", "FilmId", "RoomId" },
                values: new object[,]
                {
            // Lịch chiếu cho phim 1
                    { 1, new DateTime(2025, 4, 1, 10, 30, 0), 1, 1 },
                    { 2, new DateTime(2025, 4, 2, 14, 00, 0), 1, 2 },
                    { 3, new DateTime(2025, 4, 3, 18, 45, 0), 1, 3 },

                    // Lịch chiếu cho phim 2
                    { 4, new DateTime(2025, 4, 2, 12, 15, 0), 2, 2 },
                    { 5, new DateTime(2025, 4, 3, 20, 00, 0), 2, 3 },
                    { 6, new DateTime(2025, 4, 4, 16, 30, 0), 2, 4 },

                    // Lịch chiếu cho phim 3
                    { 7, new DateTime(2025, 4, 3, 11, 45, 0), 3, 1 },
                    { 8, new DateTime(2025, 4, 4, 15, 30, 0), 3, 2 },
                    { 9, new DateTime(2025, 4, 5, 19, 00, 0), 3, 3 },

                    // Lịch chiếu cho phim 4
                    { 10, new DateTime(2025, 4, 4, 09, 30, 0), 4, 1 },
                    { 11, new DateTime(2025, 4, 5, 13, 45, 0), 4, 2 },
                    { 12, new DateTime(2025, 4, 6, 17, 30, 0), 4, 4 },

                    // Lịch chiếu cho phim 5
                    { 13, new DateTime(2025, 4, 5, 10, 00, 0), 5, 1 },
                    { 14, new DateTime(2025, 4, 6, 14, 15, 0), 5, 3 },
                    { 15, new DateTime(2025, 4, 7, 18, 00, 0), 5, 4 },

                    // Lịch chiếu cho phim 6
                    { 16, new DateTime(2025, 4, 6, 11, 15, 0), 6, 2 },
                    { 17, new DateTime(2025, 4, 7, 15, 45, 0), 6, 3 },
                    { 18, new DateTime(2025, 4, 8, 20, 30, 0), 6, 4 },

                    // Lịch chiếu cho phim 7
                    { 19, new DateTime(2025, 4, 7, 09, 45, 0), 7, 1 },
                    { 20, new DateTime(2025, 4, 8, 13, 00, 0), 7, 3 },
                    { 21, new DateTime(2025, 4, 8, 19, 30, 0), 7, 4 },

                    // Lịch chiếu cho phim 8
                    { 22, new DateTime(2025, 4, 8, 10, 30, 0), 8, 2 },
                    { 23, new DateTime(2025, 4, 8, 14, 45, 0), 8, 3 },
                    { 24, new DateTime(2025, 4, 8, 18, 00, 0), 8, 4 }
               }
            );

            migrationBuilder.InsertData(
                 table: "SeatTypes",
                columns: new[] { "Id", "TypeDescription", "Price", "PointGiving", "ImageDescription" },
                values: new object[,]
                {
                    {1,"Regular",80000,1,"/imgs/img/seat/regular.png" },
                    {2,"VIP",80000,1,"/imgs/img/seat/VIP.png" },
                    {3,"Couple",80000,1,"/imgs/img/seat/Couple.png" }
                });
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
