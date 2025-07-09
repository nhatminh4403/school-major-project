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
                columns: new[] { "Id", "CinemaName", "CinemaAddress", "CinemaPhoneNumber", "Map" },
                values: new object[,]
                {
                    { 1, "CGV Vincom Center B", "171 Đ. Đồng Khởi, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh", "028 3821 2012", "test" },
                    { 2, "CGV Crescent Mall", "101 Tôn Dật Tiên, Tân Phú, Quận 7, Thành phố Hồ Chí Minh", "028 5413 3333", "test" }
                }
            );

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name", "Description", "CinemaId" },
                values: new object[,]
                {
                    { 1, "Room 1", "Đi thẳng tới cuối hành lang rẽ trái", 1 },
                    { 2, "Room 2", "Đi thẳng tới cuối hành lang rẽ phải", 1 },
                    { 3, "Room 1", "Đi thẳng tới cuối hành lang rẽ trái", 2 },
                    { 4, "Room 2", "Đi thẳng tới cuối hành lang rẽ phải", 2 }
                }
            );
            migrationBuilder.InsertData(
                table: "SeatTypes",
                columns: new[] { "Id", "TypeDescription", "Price", "PointGiving", "ImageDescription" },
                values: new object[,]
                {
                    { 1, "Regular", 80000, 1, "/imgs/img/seat/regular.png" },
                    { 2, "VIP", 80000, 1, "/imgs/img/seat/VIP.png" },
                    { 3, "Couple", 80000, 1, "/imgs/img/seat/Couple.png" },
                }
            );

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "SeatId", "SeatNumber", "Status", "SeatImage", "RoomId", "SeatTypeId" },
                values: new object[,]
                {
                    { 1, "A1", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 2, "A2", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 3, "A3", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 4, "A4", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 5, "A5", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 6, "A6", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 7, "A7", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 8, "A8", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 9, "A9", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 10, "A10", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 11, "A11", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 12, "A12", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 13, "B1", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 14, "B2", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 15, "B3", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 16, "B4", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 17, "B5", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 18, "B6", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 19, "B7", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 20, "B8", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 21, "B9", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 22, "B10", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 23, "B11", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 24, "B12", false, "/imgs/img/seat/regular.png", 1, 1 },

                    { 25, "C1", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 26, "C2", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 27, "C3", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 28, "C4", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 29, "C5", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 30, "C6", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 31, "C7", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 32, "C8", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 33, "C9", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 34, "C10", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 35, "C11", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 36, "C12", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 37, "D1", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 38, "D2", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 39, "D3", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 40, "D4", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 41, "D5", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 42, "D6", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 43, "D7", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 44, "D8", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 45, "D9", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 46, "D10", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 47, "D11", false, "/imgs/img/seat/regular.png", 1, 1 },
                    { 48, "D12", false, "/imgs/img/seat/regular.png", 1, 1 },

                    { 49, "E1", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 50, "E2", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 51, "E3", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 52, "E4", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 53, "E5", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 54, "E6", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 55, "E7", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 56, "E8", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 57, "E9", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 58, "E10", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 59, "E11", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 60, "E12", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 61, "F1", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 62, "F2", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 63, "F3", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 64, "F4", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 65, "F5", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 66, "F6", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 67, "F7", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 68, "F8", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 69, "F9", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 70, "F10", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 71, "F11", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 72, "F12", false, "/imgs/img/seat/VIP.png", 1, 2 },

                    { 73, "G1", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 74, "G2", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 75, "G3", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 76, "G4", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 77, "G5", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 78, "G6", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 79, "G7", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 80, "G8", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 81, "G9", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 82, "G10", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 83, "G11", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 84, "G12", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 85, "H1", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 86, "H2", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 87, "H3", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 88, "H4", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 89, "H5", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 90, "H6", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 91, "H7", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 92, "H8", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 93, "H9", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 94, "H10", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 95, "H11", false, "/imgs/img/seat/VIP.png", 1, 2 },
                    { 96, "H12", false, "/imgs/img/seat/VIP.png", 1, 2 },

                    // Couple seats (J1J2, J3J4, ..., J11J12)
                    { 97, "J1J2", false, "/imgs/img/seat/Couple.png", 1, 3 },
                    { 98, "J3J4", false, "/imgs/img/seat/Couple.png", 1, 3 },
                    { 99, "J5J6", false, "/imgs/img/seat/Couple.png", 1, 3 },
                    { 100, "J7J8", false, "/imgs/img/seat/Couple.png", 1, 3 },
                    { 101, "J9J10", false, "/imgs/img/seat/Couple.png", 1, 3 },
                    { 102, "J11J12", false, "/imgs/img/seat/Couple.png", 1, 3 },

                            // A1-A12
                    { 103, "A1", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 104, "A2", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 105, "A3", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 106, "A4", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 107, "A5", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 108, "A6", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 109, "A7", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 110, "A8", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 111, "A9", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 112, "A10", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 113, "A11", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 114, "A12", false, "/imgs/img/seat/regular.png", 2, 1 },

                    // B1-B12
                    { 115, "B1", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 116, "B2", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 117, "B3", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 118, "B4", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 119, "B5", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 120, "B6", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 121, "B7", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 122, "B8", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 123, "B9", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 124, "B10", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 125, "B11", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 126, "B12", false, "/imgs/img/seat/regular.png", 2, 1 },

                    // C1-C12
                    { 127, "C1", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 128, "C2", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 129, "C3", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 130, "C4", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 131, "C5", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 132, "C6", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 133, "C7", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 134, "C8", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 135, "C9", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 136, "C10", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 137, "C11", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 138, "C12", false, "/imgs/img/seat/regular.png", 2, 1 },

                    // D1-D12
                    { 139, "D1", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 140, "D2", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 141, "D3", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 142, "D4", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 143, "D5", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 144, "D6", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 145, "D7", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 146, "D8", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 147, "D9", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 148, "D10", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 149, "D11", false, "/imgs/img/seat/regular.png", 2, 1 },
                    { 150, "D12", false, "/imgs/img/seat/regular.png", 2, 1 },

                            // E1-E12
                    { 151, "E1", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 152, "E2", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 153, "E3", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 154, "E4", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 155, "E5", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 156, "E6", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 157, "E7", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 158, "E8", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 159, "E9", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 160, "E10", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 161, "E11", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 162, "E12", false, "/imgs/img/seat/VIP.png", 2, 2 },

                    // F1-F12
                    { 163, "F1", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 164, "F2", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 165, "F3", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 166, "F4", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 167, "F5", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 168, "F6", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 169, "F7", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 170, "F8", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 171, "F9", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 172, "F10", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 173, "F11", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 174, "F12", false, "/imgs/img/seat/VIP.png", 2, 2 },

                    // G1-G12
                    { 175, "G1", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 176, "G2", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 177, "G3", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 178, "G4", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 179, "G5", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 180, "G6", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 181, "G7", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 182, "G8", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 183, "G9", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 184, "G10", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 185, "G11", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 186, "G12", false, "/imgs/img/seat/VIP.png", 2, 2 },

                    // H1-H12
                    { 187, "H1", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 188, "H2", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 189, "H3", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 190, "H4", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 191, "H5", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 192, "H6", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 193, "H7", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 194, "H8", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 195, "H9", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 196, "H10", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 197, "H11", false, "/imgs/img/seat/VIP.png", 2, 2 },
                    { 198, "H12", false, "/imgs/img/seat/VIP.png", 2, 2 },


                            // A1-A12
                    { 205, "A1", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 206, "A2", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 207, "A3", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 208, "A4", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 209, "A5", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 210, "A6", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 211, "A7", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 212, "A8", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 213, "A9", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 214, "A10", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 215, "A11", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 216, "A12", false, "/imgs/img/seat/regular.png", 3, 1 },

                    // B1-B12
                    { 217, "B1", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 218, "B2", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 219, "B3", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 220, "B4", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 221, "B5", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 222, "B6", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 223, "B7", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 224, "B8", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 225, "B9", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 226, "B10", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 227, "B11", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 228, "B12", false, "/imgs/img/seat/regular.png", 3, 1 },

                    // C1-C12
                    { 229, "C1", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 230, "C2", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 231, "C3", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 232, "C4", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 233, "C5", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 234, "C6", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 235, "C7", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 236, "C8", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 237, "C9", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 238, "C10", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 239, "C11", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 240, "C12", false, "/imgs/img/seat/regular.png", 3, 1 },

                    // D1-D12
                    { 241, "D1", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 242, "D2", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 243, "D3", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 244, "D4", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 245, "D5", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 246, "D6", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 247, "D7", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 248, "D8", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 249, "D9", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 250, "D10", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 251, "D11", false, "/imgs/img/seat/regular.png", 3, 1 },
                    { 252, "D12", false, "/imgs/img/seat/regular.png", 3, 1 },

        // E1-E12
                    { 253, "E1", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 254, "E2", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 255, "E3", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 256, "E4", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 257, "E5", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 258, "E6", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 259, "E7", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 260, "E8", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 261, "E9", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 262, "E10", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 263, "E11", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 264, "E12", false, "/imgs/img/seat/VIP.png", 3, 2 },

                    // F1-F12
                    { 265, "F1", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 266, "F2", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 267, "F3", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 268, "F4", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 269, "F5", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 270, "F6", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 271, "F7", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 272, "F8", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 273, "F9", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 274, "F10", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 275, "F11", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 276, "F12", false, "/imgs/img/seat/VIP.png", 3, 2 },

                    // G1-G12
                    { 277, "G1", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 278, "G2", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 279, "G3", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 280, "G4", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 281, "G5", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 282, "G6", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 283, "G7", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 284, "G8", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 285, "G9", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 286, "G10", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 287, "G11", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 288, "G12", false, "/imgs/img/seat/VIP.png", 3, 2 },

                    // H1-H12
                    { 289, "H1", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 290, "H2", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 291, "H3", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 292, "H4", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 293, "H5", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 294, "H6", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 295, "H7", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 296, "H8", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 297, "H9", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 298, "H10", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 299, "H11", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 300, "H12", false, "/imgs/img/seat/VIP.png", 3, 2 },
                    { 301, "J1J2", false, "/imgs/img/seat/Couple.png", 3, 3 },
                    { 302, "J3J4", false, "/imgs/img/seat/Couple.png", 3, 3 },
                    { 303, "J5J6", false, "/imgs/img/seat/Couple.png", 3, 3 },
                    { 304, "J7J8", false, "/imgs/img/seat/Couple.png", 3, 3 },
                    { 305, "J9J10", false, "/imgs/img/seat/Couple.png", 3, 3 },
                    { 306, "J11J12", false, "/imgs/img/seat/Couple.png", 3, 3 },
                    // A1–A12
                    { 307, "A1", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 308, "A2", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 309, "A3", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 310, "A4", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 311, "A5", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 312, "A6", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 313, "A7", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 314, "A8", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 315, "A9", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 316, "A10", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 317, "A11", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 318, "A12", false, "/imgs/img/seat/regular.png", 4, 1 },

                    // B1–B12
                    { 319, "B1", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 320, "B2", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 321, "B3", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 322, "B4", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 323, "B5", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 324, "B6", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 325, "B7", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 326, "B8", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 327, "B9", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 328, "B10", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 329, "B11", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 330, "B12", false, "/imgs/img/seat/regular.png", 4, 1 },

                    // C1–C12
                    { 331, "C1", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 332, "C2", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 333, "C3", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 334, "C4", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 335, "C5", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 336, "C6", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 337, "C7", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 338, "C8", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 339, "C9", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 340, "C10", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 341, "C11", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 342, "C12", false, "/imgs/img/seat/regular.png", 4, 1 },

                    // D1–D12
                    { 343, "D1", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 344, "D2", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 345, "D3", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 346, "D4", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 347, "D5", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 348, "D6", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 349, "D7", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 350, "D8", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 351, "D9", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 352, "D10", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 353, "D11", false, "/imgs/img/seat/regular.png", 4, 1 },
                    { 354, "D12", false, "/imgs/img/seat/regular.png", 4, 1 },
                    // E1–E12
                    { 355, "E1", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 356, "E2", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 357, "E3", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 358, "E4", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 359, "E5", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 360, "E6", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 361, "E7", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 362, "E8", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 363, "E9", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 364, "E10", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 365, "E11", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 366, "E12", false, "/imgs/img/seat/VIP.png", 4, 2 },

                    // F1–F12
                    { 367, "F1", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 368, "F2", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 369, "F3", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 370, "F4", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 371, "F5", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 372, "F6", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 373, "F7", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 374, "F8", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 375, "F9", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 376, "F10", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 377, "F11", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 378, "F12", false, "/imgs/img/seat/VIP.png", 4, 2 },

                    // G1–G12
                    { 379, "G1", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 380, "G2", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 381, "G3", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 382, "G4", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 383, "G5", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 384, "G6", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 385, "G7", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 386, "G8", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 387, "G9", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 388, "G10", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 389, "G11", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 390, "G12", false, "/imgs/img/seat/VIP.png", 4, 2 },

                    // H1–H12
                    { 391, "H1", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 392, "H2", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 393, "H3", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 394, "H4", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 395, "H5", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 396, "H6", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 397, "H7", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 398, "H8", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 399, "H9", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 400, "H10", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 401, "H11", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 402, "H12", false, "/imgs/img/seat/VIP.png", 4, 2 },
                    { 403, "J1J2", false, "/imgs/img/seat/Couple.png", 4, 3 },
                    { 404, "J3J4", false, "/imgs/img/seat/Couple.png", 4, 3 },
                    { 405, "J5J6", false, "/imgs/img/seat/Couple.png", 4, 3 },
                    { 406, "J7J8", false, "/imgs/img/seat/Couple.png", 4, 3 },
                    { 407, "J9J10", false, "/imgs/img/seat/Couple.png", 4, 3 },
                    { 408, "J11J12", false, "/imgs/img/seat/Couple.png", 4, 3 },

                }
            );
            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "Id", "Code", "Description", "StartDate", "EndDate", "DiscountRate", "RedemptionPoint" },
                values: new object[,]
                {
                    { 1, "TEST20", "Test", today, enddate, 0.2, 20 }
                }
            );

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "BlogTitle", "BlogContent", "BlogCreatedDate", "BlogPoster" },
                values: new object[,]
                {
                    {
                        1,
                        "Xe máy trên 5 năm sẽ phải kiểm định khí thải",
                        "Xe máy trên 5 năm sẽ phải kiểm định khí thải\r\nChủ sở hữu xe mô tô, xe gắn máy sản xuất từ 5 năm trở lên bắt buộc phải mang xe đi kiểm định khí thải tại các trung tâm đăng kiểm.\r\n\r\nTheo Thông tư 47/2024 của Bộ Giao thông Vận tải, xe mô tô, xe gắn máy có tuổi đời dưới 5 năm được miễn kiểm định khí thải. Xe từ 5 đến 12 năm tuổi phải kiểm định hai năm một lần, còn xe trên 12 năm tuổi phải kiểm định hàng năm.\r\n",
                        today,
                        "https://bcp.cdnchinhphu.vn/thumb_w/777/334894974524682240/2024/12/16/82-1692853155-khithaixemay-1689138006805856281099-0-0-436-698-crop-16891380152921359861940-17289016068801282848539-17343301831681947437874.jpg"
                    },

                    { 2, "Top phim chiếu rạp tháng 7", "Danh sách các phim đáng xem trong tháng 7 bao gồm...", DateTime.Now,
                      "https://i.imgur.com/blog2.jpg" },

                }
            );

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Việt Nam" },
                    { 2, "Hoa Kỳ" },
                }
            );

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryDescription" },
                values: new object[,]
                {
                    { 1, "Hành động" },
                    { 2, "Phiêu lưu" },
                    { 3, "Khoa học viễn tưởng" },
                    { 4, "Kinh dị" },
                    { 5, "Hài" },
                    { 6, "Tâm lý" },
                    { 7, "Hoạt hình" },
                    { 8, "Phiêu bạt" },
                    { 9, "Tình cảm" },
                    { 10, "Hành động phiêu lưu" },
                    { 11, "Giả tưởng" },
                    { 12, "Gia đình" },
                    { 13, "Hành động khoa học viễn tưởng" },
                    { 14, "Tội phạm" },
                    { 15, "Thần thoại" }
                }
            );

            migrationBuilder.InsertData(
                table: "Films",
                columns: new[]
                {
                    "Id", "Name", "Description", "PosterUrl", "TrailerUrl",
                    "DirectorName", "Language", "FilmRated", "FilmDuration", "Actors", "Quality",
                    "StartTime", "CountryId"
                },
                values: new object[,]
                {
                    {
                        1,
                        "LẬT MẶT 7: MỘT ĐIỀU ƯỚC",
                        "Một câu chuyện lần đầu được kể bằng tất cả tình yêu, và từ tất cả những hồi ức xao xuyến nhất của đấng sinh thành",
                        "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/l/a/lat-mat-7.jpg",
                        "https://youtu.be/d1ZHdosjNX8?si=f6_pea9cJ5HtvnJy",
                        "Lý Hải",
                        "Tiếng Việt - Phụ đề Tiếng Anh",
                        "K - Phim được phổ biến đến người xem dưới 13 tuổi và có người bảo hộ đi kèm",
                        138,
                        "Thanh Hiền, Trương Minh Cường, Đinh Y Nhung, Quách Ngọc Tuyên, Trâm Anh, Trần Kim Hải,...",
                        "3D",
                        new DateTime(2024, 4, 26, 10, 15, 0),
                        1
                    },
                    {
                        2,
                        "Avengers: Endgame",
                        "Trận chiến cuối cùng của các Avengers chống lại Thanos.",
                        "https://i.ebayimg.com/images/g/wmUAAOSwasZcw2Ts/s-l1600.webp",
                        "https://youtu.be/TcMBFSGVi1c",
                        "Anthony Russo",
                        "English",
                        "PG-13",
                        181,
                        "Robert Downey Jr., Chris Evans, Scarlett Johansson",
                        "4K",
                        new DateTime(2019, 4, 26, 18, 0, 0),
                        2
                    },
                    {
                        3,
                        "Con Nhót mót chồng",
                        "Một bộ phim hài tình cảm đầy cảm xúc của Việt Nam.",
                        "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/3/image/c5f0a1eff4c394a251036189ccddaacd/7/0/700x1000_2_.jpg",
                        "https://youtu.be/hpK8I8eAY5g",
                        "Vũ Ngọc Đãng",
                        "Vietnamese",
                        "K",
                        102,
                        "Thu Trang, Tiến Luật",
                        "HD",
                        new DateTime(2024, 6, 1, 20, 0, 0),
                        1
                    },
                    {
                        4,
                        "Inside Out 2",
                        "Riley bước vào tuổi dậy thì, những cảm xúc mới xuất hiện trong trung tâm điều khiển.",
                        "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/1/0/1080x1350-insideout.jpg",
                        "https://youtu.be/LEjhY15eCx0",
                        "Kelsey Mann",
                        "English",
                        "PG",
                        100,
                        "Amy Poehler, Maya Hawke",
                        "HD",
                        new DateTime(2025, 7, 1, 18, 0, 0),
                        2
                    },
                    {
                        5,
                        "Dune: Part Two",
                        "Paul Atreides cùng tộc Fremen đứng lên chống lại kẻ thù của gia tộc.",
                        "https://upload.wikimedia.org/wikipedia/en/5/52/Dune_Part_Two_poster.jpeg",
                        "https://youtu.be/Way9Dexny3w",
                        "Denis Villeneuve",
                        "English",
                        "PG-13",
                        166,
                        "Timothée Chalamet, Zendaya",
                        "4K",
                        new DateTime(2025, 7, 10, 20, 0, 0),
                        2
                    },
                    {
                        6,
                        "Mai",
                        "Phim tình cảm lãng mạn của đạo diễn Trấn Thành.",
                        "https://vntravel.org.vn/uploads/images/2024/02/20/poster-mai-scaled-1708403724.jpg",
                        "https://youtu.be/Yz96EBNwMGw?si=T_m2CLgBxlS4Yuuy",
                        "Trấn Thành",
                        "Vietnamese",
                        "K",
                        128,
                        "Phương Anh Đào, Tuấn Trần",
                        "HD",
                        new DateTime(2025, 7, 5, 19, 0, 0),
                        1
                    },
                    {
                        7,
                        "Godzilla x Kong: The New Empire",
                        "Hai quái thú vĩ đại hợp lực chống lại mối đe dọa từ Hollow Earth.",
                        "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTZikAY7saYYBj8GKuwQkyXJPPXbzRRXNNCquOvlq9YHnTuj0jFjQGkRDNxupO-EECXDuds",
                        "https://youtu.be/qqrpMRDuPfc",
                        "Adam Wingard",
                        "English",
                        "PG-13",
                        115,
                        "Rebecca Hall, Brian Tyree Henry",
                        "4K",
                        new DateTime(2025, 7, 3, 21, 0, 0),
                        2
                    },
                    {
                        8,
                        "THANH GƯƠM DIỆT QUỶ: VÔ HẠN THÀNH",
                        "Trận chiến cuối cùng giữa Tanjiro và Muzan.",
                        "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/t/h/thanh-guom-diet-quy_up.jpg",
                        "https://youtu.be/rf0hW__Skow?si=BwoD_zIcL_cCInya",
                        "Haruo Sotozaki",
                        "Japanese",
                        "R",
                        123,
                        "Natsuki Hanae, Akari Kitō",
                        "HD",
                        new DateTime(2025, 7, 7, 17, 0, 0),
                        2
                    },
                    {
                        9,
                        "Kung Fu Panda 4",
                        "Po đối mặt với kẻ thù mới trong hành trình trở thành võ sư.",
                        "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/4/7/470x700-kungfupanda4.jpg",
                        "https://youtu.be/_inKs4eeHiI",
                        "Mike Mitchell",
                        "English",
                        "PG",
                        94,
                        "Jack Black, Awkwafina",
                        "HD",
                        new DateTime(2025, 7, 2, 15, 30, 0),
                        2
                    },
                    {
                        10,
                        "Nhà Bà Nữ",
                        "Câu chuyện hài hước, xúc động về gia đình và xung đột thế hệ.",
                        "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/n/b/nbn_main-poster_2_1_.jpg",
                        "https://youtu.be/IkaP0KJWTsQ?si=Bcq7zxx8q2aVuQm0",
                        "Trấn Thành",
                        "Vietnamese",
                        "K",
                        110,
                        "Lê Giang, Trấn Thành, Uyển Ân",
                        "HD",
                        new DateTime(2025, 7, 6, 18, 0, 0),
                        1
                    },
                    {
                        11,
                        "Spider-Man: No Way Home",
                        "Peter Parker đối đầu với các kẻ thù từ đa vũ trụ.",
                        "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/s/n/snwh_new_fb1080x1350_1_.jpg",
                        "https://youtu.be/JfVOs4VSpmA",
                        "Jon Watts",
                        "English",
                        "PG-13",
                        148,
                        "Tom Holland, Zendaya, Benedict Cumberbatch",
                        "4K",
                        new DateTime(2025, 7, 4, 14, 30, 0),
                        2
                    },
                    {
                        12,
                        "Yêu Lại Vợ Ngầu",
                        "Anh chàng bất ngờ xuyên không về quá khứ để thay đổi tương lai hôn nhân.",
                        "https://iguov8nhvyobj.vcdn.cloud/media/catalog/product/cache/1/image/c5f0a1eff4c394a251036189ccddaacd/l/r/lr-main-poster-printing.jpg",
                        "https://youtu.be/081I7DXNknc?si=cEfRqnrZvy4QrG4D",
                        "Nam Dae-jung",
                        "Tiếng Hàn - Phụ đề tiếng Việt, tiếng Anh",
                        "K",
                        105,
                        "Kang Ha-neul, Jung So-min, Kim Sun-young, Lim Chul-hyung, Yoon Kyung-ho, Jo Min-soo,....",
                        "HD",
                        new DateTime(2025, 7, 8, 19, 30, 0),
                        1
                    }
                }
            );

            migrationBuilder.InsertData(
                table: "CategoryFilm",
                columns: new[] { "CategoriesId", "FilmsId" },
                values: new object[,]
                {
                    { 12, 1 },
                    { 6, 1 },
                    { 1, 2 },
                    { 3, 2 },
                    { 5, 3 }, { 9, 3 },
                    { 4, 4 }, { 6, 4 },
                    { 1, 5 }, { 7, 5 }, { 8, 5 },
                    { 3, 6 }, { 5, 6 }, { 6, 6 },
                    { 1, 7 }, { 7, 7 }, { 9, 7 },
                    { 1, 8 }, { 2, 8 }, { 9, 8 },
                    { 1, 9 }, { 4, 9 }, { 5, 9 },
                    { 3, 10 }, { 5, 10 },
                    { 1, 11 }, { 7, 11 }, { 8, 11 },
                    { 3, 12 }, { 5, 12 }
                }
            );

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "ScheduleTime", "FilmId", "RoomId" },
                values: new object[,]
                {
                    { 1,  new DateTime(2025, 4, 1, 10, 30, 0),  1,  1 },
                    { 2,  new DateTime(2025, 4, 2, 14,  0, 0),  1,  2 },
                    { 3,  new DateTime(2025, 4, 3, 18, 45, 0),  1,  3 },
                    { 4,  new DateTime(2025, 4, 2, 12, 15, 0),  2,  2 },
                    { 5,  new DateTime(2025, 4, 3, 20,  0, 0),  2,  3 },
                    { 6,  new DateTime(2025, 4, 4, 16, 30, 0),  2,  4 },
                    { 7,  new DateTime(2025, 4, 3, 11, 45, 0),  3,  1 },
                    { 8,  new DateTime(2025, 4, 4, 15, 30, 0),  3,  2 },
                    { 9,  new DateTime(2025, 4, 5, 19,  0, 0),  3,  3 },
                    { 10, new DateTime(2025, 4, 4,  9, 30, 0),  4,  1 },
                    { 11, new DateTime(2025, 4, 5, 13, 45, 0),  4,  2 },
                    { 12, new DateTime(2025, 4, 6, 17, 30, 0),  4,  4 },
                    { 13, new DateTime(2025, 4, 5, 10,  0, 0),  5,  1 },
                    { 14, new DateTime(2025, 4, 6, 14, 15, 0),  5,  3 },
                    { 15, new DateTime(2025, 4, 7, 18,  0, 0),  5,  4 },
                    { 16, new DateTime(2025, 4, 6, 11, 15, 0),  6,  2 },
                    { 17, new DateTime(2025, 4, 7, 15, 45, 0),  6,  3 },
                    { 18, new DateTime(2025, 4, 8, 20, 30, 0),  6,  4 },
                    { 19, new DateTime(2025, 4, 7,  9, 45, 0),  7,  1 },
                    { 20, new DateTime(2025, 4, 8, 13,  0, 0),  7,  3 },
                    { 21, new DateTime(2025, 4, 8, 19, 30, 0),  7,  4 },
                    { 22, new DateTime(2025, 4, 8, 10, 30, 0),  8,  2 },
                    { 23, new DateTime(2025, 4, 8, 14, 45, 0),  8,  3 },
                    { 24, new DateTime(2025, 4, 8, 18,  0, 0),  8,  4 }
                });


            
            migrationBuilder.InsertData(
                table: "Foods",
                columns: new[] { "Id", "ComboName", "Price", "Description", "Poster" },
                values: new object[,]
                {
                    { 1, "Pizza", 88000, "pizza", "https://www.foodandwine.com/thmb/Wd4lBRZz3X_8qBr69UOu2m7I2iw=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/classic-cheese-pizza-FT-RECIPE0422-31a2c938fc2546c9a07b7011658cfd05.jpg" },
                    { 2, "Burger", 50000, "burger", "https://www.certifiedangusbeef.com/_next/image?url=https%3A%2F%2Fappetizing-cactus-7139e93734.media.strapiapp.com%2FClassic_Smashed_Burger_111c4bfdb7.jpeg&w=3840&q=75" },
                    { 3, "Pasta", 79000, "pasta", "https://www.goodnes.com/sites/g/files/jgfbjl321/files/styles/facebook_share/public/recipe-thumbnail/116751-0a0717810b73a1672a029c29788e557b_creamy_alfredo_pasta_long_left.jpg?itok=gZ6FDaar" },
                    { 4, "Salad", 49000, "Salad", "https://images.immediate.co.uk/production/volatile/sites/30/2014/05/Epic-summer-salad-hub-2646e6e.jpg?resize=1200%2C630" },
                    { 5, "Soda", 19000, "soda", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTS_FsenX3zvJZB78-xxCUxGH4-OvTWQChYxA&s" },
                    { 6, "Water", 7500, "water", "https://sonhawater.vn/wp-content/uploads/2020/10/aquafina-15-lit.jpg" },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
