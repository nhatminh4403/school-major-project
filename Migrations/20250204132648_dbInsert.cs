using Microsoft.EntityFrameworkCore.Migrations;
using school_major_project.Models;

#nullable disable

namespace school_major_project.Migrations
{
    /// <inheritdoc />
    public partial class dbInsert : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                    {
                       { 1,"Việt Nam" }
                    }
                );
            migrationBuilder.InsertData(
                table: "Films",
                columns: new[] { "Id", "Name", "Description", "PosterUrl", "TrailerUrl",
                                "DirectorName","Language","FilmRated","FilmDuration","Actors","StartTime","CountryId"},
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
                        "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...",
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
                        "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...",
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
                        "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...",
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
                        "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...",
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
                        "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...",
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
                        "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...",
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
                        "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...",
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
                        "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...",
                        new DateTime(2024, 4, 26, 10, 15, 0),1
                    }

                }
                );
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
