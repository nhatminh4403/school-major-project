document.addEventListener("DOMContentLoaded", function () {
    const cinemaTabs = document.getElementById("cinemaTabs");

    // Hàm hiển thị toast (giữ nguyên từ lần trước)
    function showToast(message, type = 'info') {
        // ... (code hàm showToast)
        let backgroundColor;
        switch (type.toLowerCase()) {
            case 'success': backgroundColor = "linear-gradient(to right, #00b09b, #96c93d)"; break;
            case 'error': backgroundColor = "linear-gradient(to right, #ff5f6d, #ffc371)"; break;
            case 'warning': backgroundColor = "linear-gradient(to right, #ffc371, #ff5f6d)"; break;
            case 'info': default: backgroundColor = "linear-gradient(to right, #007bff, #0056b3)"; break;
        }
        Toastify({ text: message, duration: 3000, close: true, gravity: "top", position: "right", stopOnFocus: true, style: { background: backgroundColor } }).showToast();
    }

    function createRoomCardHtml(roomData) {
        // Sử dụng template literals để dễ đọc và chèn biến
        return `
            <div class="col-md-6 mb-4" id="room-card-${roomData.id}">
                <div class="card">
                    <div class="card-header">
                        <h5 class="card-title">${roomData.name || 'Phòng mới'}</h5>
                    </div>
                    <div class="card-body">
                        <p class="card-text">
                            Địa chỉ: <span>${roomData.cinemaLocation || ''}</span>
                        </p>
                        <p class="card-text">
                            Vị trí: <span>${roomData.cinemaMap || ''}</span>
                        </p>
                        <a href="${roomData.detailsUrl || '#'}" class="btn btn-info">Chi tiết phòng</a>
                        <a href="${roomData.editUrl || '#'}" class="btn btn-primary">Chỉnh sửa</a>
                        <a href="#modal-delete" onclick="setDeleteItem(${roomData.id})" class="btn btn-danger open-modal">Xóa</a>
                    </div>
                </div>
            </div>
        `;
    }
    if (!cinemaTabs) {
        console.error("Không tìm thấy các phần tử DOM cần thiết cho cinemaTabs");
    } else {
        cinemaTabs.addEventListener("click", function (event) {
            event.preventDefault();
            const target = event.target;

            if (target.classList.contains("nav-link")) {
                // ... (code xử lý active tab, cập nhật link, fetch info giữ nguyên) ...
                document.querySelectorAll("#cinemaTabs .nav-link").forEach(link => { /*...*/ });
                target.classList.add("active"); target.setAttribute("aria-selected", "true");
                const cinemaId = target.getAttribute("data-cinema-id");
                document.querySelectorAll("#cinemaTabsContent .tab-pane").forEach(pane => { /*...*/ });
                const selectedPane = document.getElementById(`tab-${cinemaId}`);
                if (selectedPane) selectedPane.classList.add("show", "active");
                const addRoomLink = document.getElementById("add-room-link"); // Đảm bảo có ID này hoặc bỏ qua
                const editCinemaLink = document.getElementById("edit-cinema-link");
                // if (addRoomLink) addRoomLink.setAttribute("href", `/admin/phong-chieu/tao-moi/${cinemaId}`); // Có thể không cần link này nữa
                if (editCinemaLink) editCinemaLink.setAttribute("href", `/admin/rap-phim/chinh-sua/${cinemaId}`);

                // Cập nhật data-cinema-id cho nút Quick Create khi chuyển tab
                const quickCreateBtn = document.getElementById("quick-create-room");
                if (quickCreateBtn) {
                    quickCreateBtn.setAttribute("data-cinema-id", cinemaId);
                }


                // Fetch cinema info (giữ nguyên)
                fetch(`/admin/rap-phim/lay-thong-tin/${cinemaId}`)
                    .then(/* ... */)
                    .then(/* ... */)
                    .catch(/* ... */);
            }
        });
    }

    const quickCreateButton = document.getElementById("quick-create-room");
    if (quickCreateButton) {
        quickCreateButton.addEventListener("click", function () {
            // Lấy cinemaId từ data attribute của nút (đã được cập nhật khi chuyển tab)
            const cinemaId = this.getAttribute("data-cinema-id");

            if (!cinemaId) {
                showToast("Vui lòng chọn một rạp phim từ danh sách.", 'warning');
                return;
            }

            this.disabled = true;
            this.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Đang xử lý...';
            const originalButtonText = "Thêm phòng mới"; // Lưu lại text gốc

            const formData = new FormData();
            const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
            if (!tokenInput) {
                showToast("Lỗi bảo mật: Không tìm thấy Anti-Forgery Token.", 'error');
                this.disabled = false; this.innerHTML = originalButtonText;
                return;
            }
            const token = tokenInput.value;

            fetch(`/admin/phong-chieu/tao-moi/${cinemaId}`, {
                method: 'POST', headers: { 'RequestVerificationToken': token }, body: formData
            })
                .then(response => {
                    const contentType = response.headers.get("content-type");
                    if (contentType && contentType.indexOf("application/json") !== -1) {
                        return response.json().then(data => ({ ok: response.ok, status: response.status, data }));
                    } else {
                        return response.text().then(text => { throw new Error("Server không trả về JSON: " + text) });
                    }
                })
                .then(({ ok, status, data }) => {
                    if (ok && data.success && data.room) { // <<< Kiểm tra có data.room
                        showToast(data.message || `Đã tạo phòng thành công!`, 'success');

                        // <<< START: Cập nhật DOM >>>
                        const targetPaneId = `tab-${cinemaId}`;
                        const targetContainer = document.querySelector(`#${targetPaneId} .row.mt-3`);

                        if (targetContainer) {
                            // Tạo HTML cho card mới
                            const newRoomHtml = createRoomCardHtml(data.room);
                            // Thêm vào container
                            targetContainer.insertAdjacentHTML('beforeend', newRoomHtml);

                            // Xóa thông báo "chưa có phòng" nếu có
                            const noRoomsMessage = targetContainer.querySelector('.no-rooms-message');
                            if (noRoomsMessage) {
                                noRoomsMessage.remove();
                            }

                        } else {
                            console.error(`Không tìm thấy container phòng cho tab ID: ${targetPaneId}`);
                            showToast("Tạo phòng thành công nhưng lỗi cập nhật giao diện.", 'warning');
                            // Có thể cân nhắc reload lại trang trong trường hợp này
                            // setTimeout(() => location.reload(), 1500);
                        }

                        this.disabled = false;
                        this.innerHTML = originalButtonText;

                    } else {
                        const errorMessage = data?.message || `Lỗi không xác định (HTTP ${status})`;
                        showToast(`Lỗi: ${errorMessage}`, 'error');
                        this.disabled = false; this.innerHTML = originalButtonText;
                    }
                })
                .catch(error => {
                    console.error("Lỗi khi tạo phòng:", error);
                    showToast(`Có lỗi xảy ra khi tạo phòng mới: ${error.message}`, 'error');
                    this.disabled = false; this.innerHTML = originalButtonText;
                });
        });
    }

    const initialActiveTab = document.querySelector("#cinemaTabs .nav-link.active");
    const initialCinemaId = initialActiveTab ? initialActiveTab.getAttribute("data-cinema-id") : null;
    if (initialCinemaId && quickCreateButton) {
        quickCreateButton.setAttribute("data-cinema-id", initialCinemaId);
    }
    // ===

    document.querySelectorAll("#cinemaTabsContent .tab-pane").forEach(pane => {
        const roomContainer = pane.querySelector('.row.mt-3');
        if (roomContainer && !roomContainer.querySelector('.col-md-6')) { // Nếu không có card phòng nào
            roomContainer.innerHTML = '<p class="no-rooms-message col-12 text-center text-muted fst-italic">Rạp này chưa có phòng nào.</p>';
        }
    });
    // ===


});