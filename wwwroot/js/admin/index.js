let deleteItemId = 0;

function setDeleteItem(id) {
    console.log('Setting delete item ID: ' + id);
    deleteItemId = id;
}

function deleteItem(apiUrl, redirectUrl) {
    console.log('Deleting item ID: ' + deleteItemId + ' using API: ' + apiUrl);

    // Thay thế placeholder với ID thực tế
    const url = apiUrl.replace('{id}', deleteItemId);

    $.ajax({
        url: url,
        type: 'POST',
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            window.location.href = redirectUrl;
        },
        error: function (xhr, status, error) {
            alert('Có lỗi xảy ra khi xoá dữ liệu: ' + error);
        }
    });
}
document.querySelectorAll('.toggle-btn').forEach((button) => {
    button.addEventListener('click', function (e) {
        e.preventDefault(); // Ngăn chặn hành động mặc định của thẻ <a>
        const navItem = this.parentElement; // Lấy thẻ cha của nút nhấn
        const subnav = this.nextElementSibling;

        // Đóng các subnav khác trước khi mở
        document.querySelectorAll('.sidebar__nav-item').forEach((item) => {
            if (item !== navItem) {
                item.classList.remove('active');
                const sub = item.querySelector('.subNav');
                if (sub) {
                    sub.style.maxHeight = null;
                    sub.style.padding = null;
                }
            }
        });

        // Toggle trạng thái active
        if (navItem.classList.contains('active')) {
            navItem.classList.remove('active');
            subnav.style.maxHeight = null;
            subnav.style.padding = null;
        } else {
            navItem.classList.add('active');
            subnav.style.display = "flex";
            subnav.style.textAlign = "center";
            subnav.style.flexWrap = "wrap";
            subnav.style.alignItems = 'center';
            subnav.style.backgroundColor = "#28282d";
            subnav.style.maxHeight = (subnav.scrollHeight) * 2 + 'px';
            subnav.style.padding = '6px 0 2px';
        }
    });
});


