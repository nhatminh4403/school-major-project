 document.addEventListener("DOMContentLoaded", function () {
        // Lấy tất cả các phần tử mã QR trong trang
        const qrImages = document.querySelectorAll(".comboFood");

        // Duyệt qua từng ảnh QR và gọi API để lấy mã QR
        qrImages.forEach((img) => {
            const bookingId = img.getAttribute("data-booking-id");

            // Gọi API để lấy mã QR và gán vào src của ảnh
            fetch(`/tai-khoan/chi-tiet-hoa-don/${bookingId}`)
                .then((response) => response.blob())
                .then((blob) => {
                    const url = URL.createObjectURL(blob);
                    img.src = url;
                })
                .catch((error) => console.error("Lỗi khi tải mã QR:", error));
        });
    });