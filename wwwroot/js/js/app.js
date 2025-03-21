function rate(value) {
    var stars = document.querySelectorAll('.rating-stars i');
    for (var i = 0; i < stars.length; i++) {
        if (i < value) {
            stars[i].classList.add('selected');
        } else {
            stars[i].classList.remove('selected');
        }
    }
    document.getElementById('rating').value = value;
}

function validateForm() {
    var rating = document.getElementById('rating').value;
    var content = document.getElementsByName('content')[0].value;

    if (!rating || rating == 0) {
        alert("Vui lòng chọn số sao để đánh giá.");
        return false;
    }

    if (!content.trim()) {
        alert("Vui lòng nhập nội dung đánh giá.");
        return false;
    }
    return true;
}

function handleChange(selectElement) {
    console.log(selectElement);
    const countryId = selectElement.value;
    if (countryId) {
        window.location.href = `/phim-theo-quoc-gia-${countryId}/trang-1`;
    }
}