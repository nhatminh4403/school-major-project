document.addEventListener('DOMContentLoaded', () => {
    const triggerTabList = document.querySelectorAll('#cinemaTabs button');
    triggerTabList.forEach(triggerEl => {
        triggerEl.addEventListener('shown.bs.tab', function (event) {
            const tabId = event.target.getAttribute('data-bs-target');
            console.log('Tab mở:', tabId); // Debug tab mở
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const cinemaTabs = document.getElementById("cinemaTabs");
    const infoTitle = document.querySelector(".cinema-info__title");
    const infoDetails = document.querySelectorAll(".cinema-info__detail");
    if (!cinemaTabs || !infoTitle || infoDetails.length === 0) {
        console.error("Không tìm thấy các phần tử DOM cần thiết");
        return;
    }
    cinemaTabs.addEventListener("click", function (event) {
        if (event.target.classList.contains('nav-link')) {
            const cinemaId = event.target.id.split("-")[2];
            fetch(`/api/admin/cinemas/${cinemaId}`).then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            }).then(data => {
                try {
                    infoTitle.innerText = "Thông tin chi nhánh";
                    // Kiểm tra và cập nhật từng phần tử
                    if (infoDetails[0]) {
                        infoDetails[0].innerText = `Tên rạp: ${data.name || ''}`;
                    }
                    if (infoDetails[1]) {
                        infoDetails[1].innerText = `Địa chỉ: ${data.address || ''}`;
                    }
                    if (infoDetails[2]) {
                        infoDetails[2].innerText = `Vị trí: ${data.map || ''}`;
                    }
                } catch (error) {
                    console.error("Lỗi khi cập nhật DOM:", error);
                }
            }).catch(error => {
                console.error("Lỗi khi lấy thông tin chi nhánh:", error);
                if (infoTitle) {
                    infoTitle.innerText = "Có lỗi xảy ra khi tải thông tin";
                }
            });
        }
    });
});