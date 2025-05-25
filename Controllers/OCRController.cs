using Microsoft.AspNetCore.Mvc;
using school_major_project.HelperClass;
using System.Text.RegularExpressions;
using Tesseract;

namespace school_major_project.Controllers
{
    public class OCRController : Controller
    {
        private readonly string _tesseractDataPath = @"E:\Csharp\project\school-major-project\wwwroot\OCR\Tesseract-OCR\tessdata";
        [HttpPost]
        public IActionResult ExtractStudentInfo(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Lưu tạm hình ảnh vào thư mục uploads
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/cards", file.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Thực hiện OCR để trích xuất văn bản
                string extractedText;
                using (var engine = new TesseractEngine(_tesseractDataPath,
                    "vie+eng+fra+por+spa+pol+deu+hun+tur+ita", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(filePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            extractedText = page.GetText();
                            Console.WriteLine("extracted Text: " + extractedText);
                        }
                    }
                }

                // Phân tích văn bản để lấy thông tin
                var studentInfo = ParseStudentInfo(extractedText);

                // Trả về kết quả dưới dạng JSON
                return Json(studentInfo);
            }
            return BadRequest("No file uploaded.");
        }

        private object ParseStudentInfo(string text)
        {
            // Sử dụng các phương thức trích xuất thông tin
            string studentId = ExtractStudentId(text);
            string name = ExtractFullName(text);
            string birthday = ExtractBirthday(text);

            // Chuyển đổi ngày sinh thành DateTime (nếu có)
            DateTime? dob = null;
            if (!string.IsNullOrEmpty(birthday) && DateTime.TryParse(birthday, out var parsedDob))
            {
                dob = parsedDob;
            }

            // Tính tuổi dựa trên ngày sinh
            int? age = null;
            if (!string.IsNullOrEmpty(birthday))
            {
                age = CalculateAge(birthday);
            }

            // Trả về thông tin dưới dạng object
            return new
            {
                StudentId = studentId,
                Name = StringHelper.RemoveDiacritics(name),
                DateOfBirth = dob?.ToString("dd/MM/yyyy"),
                Age = age,
                FullText = text
            };
        }

        private int CalculateAge(string birthday)
        {
            if (DateTime.TryParse(birthday, out DateTime dob))
            {
                int age = DateTime.Now.Year - dob.Year;
                if (DateTime.Now.DayOfYear < dob.DayOfYear)
                    age--;
                return age;
            }
            return 0;
        }

        private string ExtractStudentId(string text)
        {
            Regex mssv = new Regex(@"(?:MSSV:|Mã SV:|Ma SV:|Student ID:|"" MãSV: - ')(?:\s*)(\d{8,12})(?=\s|$)");
            Match match = mssv.Match(text);

            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            return string.Empty;
        }
        private string ExtractFullName(string text)
        {
            Regex name = new Regex(@"(?:Họ tên:|Họ ten:|Ho ten:|Ho tên:|Ho va ten:|Họ va ten:|Ho và ten:|Ho va tên:|Họ và ten:|Họ va tên:|Ho và tên:|Họ và tên:|Ho fen:|Họ tén:|Ho tén:|Ho tèn:|Họ tèn)\s*(.+)(?=\s|$)");
            Match match = name.Match(text);

            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            return string.Empty;
        }


        private string ExtractBirthday(string text)
        {
            Regex birthday = new Regex(@"(?:Ngày sinh:|Ngay sinh:|Ngey sinh:|Ngèy sinh:|Ngoy sinh:|Ngòy sinh:)\s*(\d{2}/\d{2}/\d{4})(?=\s|$)");
            Match match = birthday.Match(text);

            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            return string.Empty;
        }
    }
}
