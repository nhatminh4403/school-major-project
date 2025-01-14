document.addEventListener('DOMContentLoaded', function () {
    fetch('/api/admin/countries')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(countries => {
            const countrySelect = document.getElementById('country');
            countries.forEach(country => {
                const option = document.createElement('option');
                option.value = country.id;
                option.textContent = country.name;
                countrySelect.appendChild(option);
            });
        })
        .catch(error => console.error('Error fetching countries:', error));
});
document.addEventListener('DOMContentLoaded', function () {
    fetch('/api/admin/categories')
        .then(response => response.json())
        .then(categories => {
            const categorySelect = document.getElementById('category');

            // Thêm từng option cho từng category
            categories.forEach(category => {
                const option = document.createElement('option');
                option.value = category.id;
                option.textContent = category.name;

                // Kiểm tra xem category đã có trong categoryIds hay chưa
                const isSelected = document.querySelector('select[name="categoryIds"]')
                    .options.namedItem(category.id) !== null;

                if (isSelected) {
                    option.selected = true; // Đánh dấu là đã chọn
                }

                categorySelect.appendChild(option);
            });
        })
        .catch(error => console.error('Error fetching categories:', error));
});
document.getElementById('addFilmForm').addEventListener('submit', function(event) {
    event.preventDefault();

    let formData = new FormData(this);

    fetch('/api/admin/films', {
        method: 'POST',
        body: formData
    }).then(response => {
        if (response.ok) {
            // Nếu phản hồi thành công, chuyển hướng đến trang dashboard
            window.location.href = '/admin/films';
        } else {
            // Xử lý lỗi ở đây nếu có
            return response.json().then(data => {
                console.error('Error:', data);
            });
        }
    }).catch(error => {
        console.error('Error:', error);
    });
});
