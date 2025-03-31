document.addEventListener("DOMContentLoaded", function () {
    const imageInput = document.getElementById("imageInput");
    const previewContainer = document.querySelector(".preview-container");
    const imagePreview = document.getElementById("imagePreview");
    const previewMessage = document.querySelector(".preview-message");
    const scanButton = document.getElementById("scanButton");

    // Xử lý sự kiện khi chọn hình ảnh
    imageInput.addEventListener("change", function (event) {
        console.log("Image input changed:", event.target.files);
        const fileInput = event.target;

        if (fileInput.files && fileInput.files[0] && fileInput.files[0].type.startsWith("image/")) {
            // Hiển thị preview-container và kích hoạt nút Quét Thẻ
            previewContainer.style.display = "block";
            scanButton.disabled = false;

            // Hiển thị hình ảnh preview
            const reader = new FileReader();
            reader.onload = function (e) {
                imagePreview.src = e.target.result;
                imagePreview.style.display = "block";
                previewMessage.textContent = "Ảnh đã được tải lên.";
            };
            reader.onerror = function () {
                previewMessage.textContent = "Không thể đọc file hình ảnh.";
            };
            reader.readAsDataURL(fileInput.files[0]);
        } else {
            // Ẩn preview-container và vô hiệu hóa nút Quét Thẻ
            previewContainer.style.display = "none";
            scanButton.disabled = true;
            imagePreview.src = "";
            previewMessage.textContent = "Vui lòng chọn một file hình ảnh hợp lệ.";
        }
    });

    // Xử lý sự kiện khi nhấn nút Quét Thẻ
    scanButton.addEventListener("click", function () {
        const fileInput = document.getElementById("imageInput");
        const loadingIndicator = document.getElementById("loadingIndicator");
        const previewMessage = document.querySelector(".preview-message");
        const isStudentCheckbox = document.getElementById("isStudent");
        if (fileInput.files && fileInput.files[0]) {
            // Hiển thị thông báo đang xử lý
            loadingIndicator.style.display = "block";

            // Tạo FormData để gửi hình ảnh
            const formData = new FormData();
            formData.append("file", fileInput.files[0]);

            // Gửi yêu cầu AJAX đến phương thức OCR
            fetch('/Ocr/ExtractStudentInfo', {
                method: 'POST',
                body: formData
            })
                .then(response => response.json())
                .then(data => {
                    loadingIndicator.style.display = "none";
                    if (data) {
                        console.log("OCR result:", data);
                        // Cập nhật các trường thông tin từ kết quả OCR
                        document.getElementById("fullname").value = data.name || "";
                        document.getElementById("birthday").value = data.dateOfBirth || "";
                        document.getElementById("age").value = data.age || "";
                        //document.getElementById("fullInfo").value = data.fullText || "";
                        if (data.studentId) {
                            isStudentCheckbox.checked = true;
                            isStudentCheckbox.dispatchEvent(new Event('change'));
                        }
                        else {
                            isStudentCheckbox.checked = false;
                            isStudentCheckbox.dispatchEvent(new Event('change'));
                        }
                        previewMessage.textContent = "Quét thẻ thành công";
                    } else {
                        previewMessage.textContent = "Không thể trích xuất thông tin.";
                    }
                })
                .catch(error => {
                    loadingIndicator.style.display = "none";
                    previewMessage.textContent = "Có lỗi xảy ra khi xử lý hình ảnh.";
                    console.error('Error:', error);
                });
        }
    });
});