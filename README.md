> English translation below

# BaAnhEm - Website Đặt Vé Xem Phim - Hoàn Thiện

## Tổng quan dự án
BaAnhEm là website đặt vé xem phim trực tuyến cho phép người dùng duyệt phim, đặt vé và quản lý đặt vé của họ. Dự án này là phiên bản được phát triển lại từ ngôn ngữ Java sang C# ASP.NET.

> **Lưu ý:** Dự án đang trong quá trình phát triển  

## Dự án tham khảo
Dự án này là phiên bản C# ASP.NET dựa trên dự án Java gốc - một đồ án chuyên ngành tại trường mà tôi đóng góp và thực hiện. Bạn có thể tìm thấy dự án gốc tại đây:
[Dự án Java gốc](https://github.com/baobui166/Ba-Anh-Em-Movies)

## Tính năng
- Xác thực và phân quyền người dùng
- Duyệt và tìm kiếm phim
- Đặt vé và quản lý đặt vé
- Chọn ghế ngồi
- Xử lý thanh toán
- Bảng điều khiển quản trị cho quản lý phim và suất chiếu

## Công nghệ sử dụng
- C# ASP.NET
- Entity Framework Core
- SQL Server
- HTML5/CSS3
- JavaScript/jQuery
- Bootstrap

## Hướng dẫn cài đặt

### Yêu cầu hệ thống
- .NET 6.0 trở lên (khuyến nghị dùng .NET 9.0 vì project sử dụng các gói NuGet từ phiên bản 9.0 trở lên)
- SQL Server
- Visual Studio 2022 trở lên

### Các bước cài đặt
1. Clone repository
```bash
git clone https://github.com/nhatminh4403/school-major-project.git
```

2. Di chuyển vào thư mục dự án
```bash
cd school_major_project
```

3. Tạo mới file `appsettings.json` dựa vào bản mẫu `appsettings.example.json`

4. Chạy migration để tạo cơ sở dữ liệu (chọn một trong các cách sau):

   a. Sử dụng dotnet CLI:
   ```bash
   dotnet ef database update
   ```

   b. Sử dụng Package Manager Console:
   ```powershell
   Update-Database
   ```

5. Chạy dự án (chọn một trong các cách sau):

   a. Sử dụng dotnet CLI:
   ```bash
   dotnet run
   ```

   b. Sử dụng Visual Studio UI:
   - Nhấn F5 hoặc nút Start để chạy dự án
   - Hoặc nhấn Ctrl + F5 để chạy không debug

## Cấu trúc dự án
```
school_major_project/
├── Controllers/
├── Models/
├── Views/
├── Services/
├── Data/
└── wwwroot/
```

## Đóng góp
Mọi đóng góp đều được hoan nghênh! Hãy tự nhiên tạo Pull Request.

## Liên hệ
Phạm Trần Nhật Minh - nhatminh4403@gmail.com

---

# BaAnhEm - Movie Ticket Booking Website

## Project Overview
BaAnhEm is a movie ticket booking website that allows users to browse movies, book tickets, and manage their bookings online. This project is a C# ASP.NET implementation of a previously developed Java-based project.

> **Note:** This project is finished

## Reference Project
This project is a C# ASP.NET implementation based on the original Java project - a school major project that I contributed to and implemented. You can find the original project here:
[Original Java Project Link](https://github.com/baobui166/Ba-Anh-Em-Movies)

## Features
- User authentication and authorization
- Movie browsing and searching
- Ticket booking and management
- Seat selection
- Payment processing
- Admin dashboard for movie and show management

## Technologies Used
- C# ASP.NET
- Entity Framework Core
- SQL Server
- HTML5/CSS3
- JavaScript/jQuery
- Bootstrap

## Getting Started

### Prerequisites
- .NET 6.0 or later (recommended to use .NET 9.0 as the project uses NuGet packages from version 9.0 and above)
- SQL Server
- Visual Studio 2022 or later

### Installation
1. Clone the repository
```bash
git clone https://github.com/nhatminh4403/school-major-project.git
```

2. Navigate to the project directory
```bash
cd school_major_project
```

3. Create a new `appsettings.json` based on the example file `appsettings.example.json`

4. Run migrations to create the database (choose one of the following methods):

   a. Using dotnet CLI:
   ```bash
   dotnet ef database update
   ```

   b. Using Package Manager Console:
   ```powershell
   Update-Database
   ```

5. Run the project (choose one of the following methods):

   a. Using dotnet CLI:
   ```bash
   dotnet run
   ```

   b. Using Visual Studio UI:
   - Press F5 or Start button to run the project
   - Or press Ctrl + F5 to run without debugging

## Project Structure
```
school_major_project/
├── Controllers/
├── Models/
├── Views/
├── Services/
├── Data/
└── wwwroot/
```

## Contributing
Contributions are welcome! Please feel free to submit a Pull Request.

## Contact
Phạm Trần Nhật Minh - nhatminh4403@gmail.com
