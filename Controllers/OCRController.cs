using Microsoft.AspNetCore.Mvc;
using Tesseract;

namespace school_major_project.Controllers
{
    public class OCRController : Controller
    {
        private readonly string _tesseractDataPath = @"E:\C#\project\school-major-project\wwwroot\OCR\Tesseract-OCR\tessdata";
        [HttpPost]
        public IActionResult ExtractStudentInfo(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Lưu tạm hình ảnh vào thư mục uploads
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/cards", file.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // Đảm bảo thư mục tồn tại
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Thực hiện OCR để trích xuất văn bản
                string extractedText;
                using (var engine = new TesseractEngine(_tesseractDataPath, "vie+eng", EngineMode.Default)) // Thay "eng" bằng "vie" nếu cần tiếng Việt
                {
                    using (var img = Pix.LoadFromFile(filePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            extractedText = page.GetText();
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
            // Giả sử văn bản có định dạng như sau:
            // Student ID: 123456
            // Name: Nguyễn Văn A
            // Date of Birth: 01/01/2000

            var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string studentId = null;
            string name = null;
            DateTime? dob = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("Student ID:"))
                {
                    studentId = line.Replace("Student ID:", "").Trim();
                }
                else if (line.StartsWith("Name:"))
                {
                    name = line.Replace("Name:", "").Trim();
                }
                else if (line.StartsWith("Date of Birth:"))
                {
                    var dobStr = line.Replace("Date of Birth:", "").Trim();
                    if (DateTime.TryParse(dobStr, out var parsedDob))
                    {
                        dob = parsedDob;
                    }
                }
            }

            // Tính tuổi dựa trên ngày sinh
            int? age = null;
            if (dob.HasValue)
            {
                age = DateTime.Now.Year - dob.Value.Year;
                if (DateTime.Now < dob.Value.AddYears(age.Value)) age--; // Điều chỉnh nếu chưa đến sinh nhật
            }

            // Trả về thông tin dưới dạng object
            return new
            {
                StudentId = studentId,
                Name = name,
                DateOfBirth = dob?.ToString("dd/MM/yyyy"),
                Age = age,
                FullText = text
            };
        }
    }
}
