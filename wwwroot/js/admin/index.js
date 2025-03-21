let item_id = 0;

function setDeleteItem(id) {
    item_id = id;
}

function deleteItem(api, href) {
    $.get(api, function (data) {
        window.location.href = href;
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


