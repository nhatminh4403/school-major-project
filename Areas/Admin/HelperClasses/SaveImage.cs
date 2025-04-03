namespace school_major_project.Areas.Admin.HelperClasses
{
    public static class SaveImage
    {
        public static async Task<string> SaveImageAsync(IFormFile image, string folderPath)
        {
            if (image == null || image.Length == 0)
            {
                throw new ArgumentException("Ảnh không hợp lệ.");
            }

            // Tạo thư mục nếu chưa tồn tại
            var uploadPath = Path.Combine("wwwroot", folderPath);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Tạo đường dẫn lưu file
            var fileName = Path.GetFileName(image.FileName);
            var savePath = Path.Combine(uploadPath, fileName);

            // Lưu file vào server
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            // Trả về đường dẫn tương đối
            return $"/{folderPath}/{fileName}";
        }

    }
}
