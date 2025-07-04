document.addEventListener("DOMContentLoaded", function () {
    const cinemaTabs = document.getElementById("cinemaTabs");

    // Hàm hiển thị toast
    function showToast(message, type = 'info') {
        let backgroundColor;
        switch (type.toLowerCase()) {
            case 'success': backgroundColor = "linear-gradient(to right, #00b09b, #96c93d)"; break;
            case 'error': backgroundColor = "linear-gradient(to right, #ff5f6d, #ffc371)"; break;
            case 'warning': backgroundColor = "linear-gradient(to right, #ffc371, #ff5f6d)"; break;
            case 'info': default: backgroundColor = "linear-gradient(to right, #007bff, #0056b3)"; break;
        }
        Toastify({
            text: message, duration: 3000, close: true, gravity: "top", position: "right", stopOnFocus: true, style: { background: backgroundColor }
        }).showToast();
    }

    // Hàm tạo HTML cho card phòng chiếu mới
    function createRoomCardHtml(roomData) {
        return `
            <div class="room-card" id="room-card-${roomData.id}">
                <div class="room-card-header">
                    <h4 class="room-card-title">${roomData.name || 'Phòng mới'}</h4>
                    <span class="room-card-id">#${roomData.id}</span>
                </div>
                <div class="room-card-content">
                    <div class="room-card-info">
                        <i class="fas fa-chair"></i>
                        <span>Số ghế: ${roomData.seats ? roomData.seats.length : 0}</span>
                    </div>
                    <div class="room-card-info">
                        <i class="fa-solid fa-phone"></i>
                        <span>Điện thoại: ${roomData.phone || 'Chưa cập nhật'}</span>
                    </div>
                    <div class="room-card-status ${roomData.isActive ? 'active' : 'inactive'}">
                        <i class="fas ${roomData.isActive ? 'fa-check-circle' : 'fa-times-circle'}"></i>
                        <span>${roomData.isActive ? 'Đang hoạt động' : 'Không hoạt động'}</span>
                    </div>
                </div>
                <div class="room-card-actions">
                    <a href="/admin/phong-chieu/chi-tiet/${roomData.id}" class="view">
                        <i class="fas fa-eye"></i>
                        <span>Xem</span>
                    </a>
                    <a href="/admin/phong-chieu/chinh-sua/${roomData.id}" class="edit">
                        <i class="fas fa-edit"></i>
                        <span>Sửa</span>
                    </a>
                    <a href="#" class="delete" data-toggle="modal" data-target="#modal-delete-room-${roomData.id}">
                        <i class="fas fa-trash"></i>
                        <span>Xóa</span>
                    </a>
                </div>
            </div>
        `;
    }

    // Xử lý chuyển tab
    if (!cinemaTabs) {
        console.error("Không tìm thấy các phần tử DOM cần thiết cho cinemaTabs");
    } else {
        cinemaTabs.addEventListener("click", function (event) {
            event.preventDefault();
            const target = event.target;

            if (target.classList.contains("nav-link")) {
                // Cập nhật active tab
                document.querySelectorAll("#cinemaTabs .nav-link").forEach(link => {
                    link.classList.remove("active");
                    link.setAttribute("aria-selected", "false");
                });
                target.classList.add("active");
                target.setAttribute("aria-selected", "true");

                // Cập nhật active tab content
                const cinemaId = target.getAttribute("data-cinema-id");
                document.querySelectorAll("#cinemaTabsContent .tab-pane").forEach(pane => {
                    pane.classList.remove("show", "active");
                });
                const selectedPane = document.getElementById(`tab-${cinemaId}`);
                if (selectedPane) {
                    selectedPane.classList.add("show", "active");
                }

                // Fetch cinema info
                fetch(`/admin/rap-phim/lay-thong-tin/${cinemaId}`)
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Network response was not ok');
                        }
                        return response.json();
                    })
                    .then(data => {
                        // Cập nhật thông tin rạp phim nếu cần
                        console.log("Cinema info updated:", data);
                    })
                    .catch(error => {
                        console.error('Error fetching cinema info:', error);
                    });
            }
        });
    }

    // Xử lý nút thêm phòng mới
    document.querySelectorAll('.quick-create-room').forEach(button => {
        button.addEventListener("click", function () {
            const cinemaId = this.getAttribute("data-cinema-id");

            if (!cinemaId) {
                showToast("Vui lòng chọn một rạp phim từ danh sách.", 'warning');
                return;
            }

            this.disabled = true;
            this.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Đang xử lý...';
            const originalButtonText = '<i class="fas fa-plus"></i> Thêm phòng mới';

            const formData = new FormData();
            const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
            if (!tokenInput) {
                showToast("Lỗi bảo mật: Không tìm thấy Anti-Forgery Token.", 'error');
                this.disabled = false;
                this.innerHTML = originalButtonText;
                return;
            }
            const token = tokenInput.value;

            fetch(`/admin/phong-chieu/tao-moi/${cinemaId}`, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': token
                },
                body: formData
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(data => {
                    if (data.success) {
                        showToast("Thêm phòng mới thành công!", 'success');

                        // Tạo card phòng mới và thêm vào grid
                        const roomsGrid = document.querySelector(`#tab-${cinemaId} .rooms-grid`);
                        if (roomsGrid) {
                            // Nếu chưa có phòng nào, xóa thông báo "Chưa có phòng chiếu nào"
                            const emptyMessage = document.querySelector(`#tab-${cinemaId} .rooms-empty`);
                            if (emptyMessage) {
                                emptyMessage.remove();
                            }

                            // Thêm card phòng mới vào đầu grid
                            roomsGrid.insertAdjacentHTML('afterbegin', createRoomCardHtml(data.room));
                        }
                    } else {
                        showToast(data.message || "Có lỗi xảy ra khi thêm phòng mới.", 'error');
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    showToast("Có lỗi xảy ra khi thêm phòng mới.", 'error');
                })
                .finally(() => {
                    this.disabled = false;
                    this.innerHTML = originalButtonText;
                });
        });
    });

    // Xử lý xóa phòng chiếu
    document.addEventListener('click', function (event) {
        if (event.target.closest('.room-card-actions .delete')) {
            event.preventDefault();
            const deleteLink = event.target.closest('.room-card-actions .delete');
            const roomId = deleteLink.getAttribute('data-room-id');

            if (roomId) {
                // Hiển thị modal xác nhận xóa
                const modalId = `#modal-delete-room-${roomId}`;
                const modal = document.querySelector(modalId);
                if (modal) {
                    $(modal).modal('show');
                }
            }
        }
    });

    // Xử lý xóa rạp phim
    document.addEventListener('click', function (event) {
        if (event.target.closest('.cinema-details-delete')) {
            event.preventDefault();
            const deleteLink = event.target.closest('.cinema-details-delete');
            const modalId = deleteLink.getAttribute('data-target');

            if (modalId) {
                $(modalId).modal('show');
            }
        }
    });
});

$(document).ready(function () {
    // Xử lý modal xóa rạp phim
    $('.cinema-details-delete').on('click', function (e) {
        e.preventDefault();
        const cinemaId = $(this).data('cinema-id');
        const cinemaName = $(this).data('cinema-name');

        // Cập nhật nội dung modal
        $('#modal-delete-cinema-label').text(`Xác nhận xóa rạp phim: ${cinemaName}`);
        $('.custom-modal-body').html(`Bạn có chắc chắn muốn xóa rạp phim <strong>${cinemaName}</strong> không? Hành động này sẽ xóa tất cả các phòng chiếu thuộc rạp và không thể hoàn tác.`);

        // Cập nhật action của form
        $('#delete-cinema-form').attr('action', `/admin/rap-phim/xoa/${cinemaId}`);

        // Hiển thị modal
        $('#modal-delete-cinema').modal('show');
    });
});