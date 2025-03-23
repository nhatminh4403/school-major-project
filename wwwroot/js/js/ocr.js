document.addEventListener('DOMContentLoaded', function() {
    const imageInput = document.getElementById('imageInput');
    const imagePreview = document.getElementById('imagePreview');
    const scanButton = document.getElementById('scanButton');
    const loadingIndicator = document.getElementById('loadingIndicator');
    const isStudentCheckbox = document.getElementById('isStudent');
    const isStudentHidden = document.createElement('input');
    // Preview ảnh
    imageInput.addEventListener('change', function(e) {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function(e) {
                imagePreview.style.display = 'block';
                imagePreview.src = e.target.result;
                scanButton.disabled = false;
            }
            reader.readAsDataURL(file);
        }
    });

    // Xử lý quét thẻ
    scanButton.addEventListener('click', async function() {
        const file = imageInput.files[0];
        if (!file) return;

        loadingIndicator.style.display = 'block';
        scanButton.disabled = true;

        const formData = new FormData();
        formData.append('imageFile', file);

        try {
            const response = await fetch('/tai-khoan/xu-ly-the-sinh-vien', {
                method: 'POST',
                body: formData
            });

            const data = await response.json();
            if (response.ok) {
                // Điền thông tin vào các trường
                document.getElementById('fullname').value = data.fullName || '';
                document.getElementById('birthday').value = data.birthday || '';
                document.getElementById('age').value = data.age || '';
                //kiem thu du lieu
                const fullInfoElement = document.getElementById('fullInfo');
                if (fullInfoElement) {
                    // Tạo chuỗi thông tin đầy đủ
                    let fullInfoText = '';

                    // Thêm từng trường thông tin vào chuỗi
                    if (data.StudentId) fullInfoText += `Mã sinh viên: ${data.StudentId}\n`;
                    if (data.FullName) fullInfoText += `Họ và tên: ${data.FullName}\n`;
                    if (data.Birthday) fullInfoText += `Ngày sinh: ${data.Birthday}\n`;
                }
                // document.getElementById('studentId').value = data.studentId ;
                if(data.studentId){
                    isStudentCheckbox.checked = true;
                    isStudentCheckbox.dispatchEvent(new Event('change'));                    // Trigger change event
                    // const event = new Event('change', {
                    //     bubbles: true,
                    //     cancelable: true,
                    // });
                    // isStudentCheckbox.dispatchEvent(event);
                }
                else {
                    isStudentCheckbox.checked = false;
                    isStudentCheckbox.dispatchEvent(new Event('change'));
                }

            } else {
                alert('Có lỗi xảy ra: ' + data.error);
            }
        } catch (error) {
            alert('Có lỗi xảy ra khi xử lý yêu cầu');
        } finally {
            loadingIndicator.style.display = 'none';
            scanButton.disabled = false;
        }
    });
});
//document.addEventListener('DOMContentLoaded', function () {
//    const imageInput = document.getElementById('imageInput');
//    const imagePreview = document.getElementById('imagePreview');
//    const scanButton = document.getElementById('scanButton');
//    const loadingIndicator = document.getElementById('loadingIndicator');
//    const isStudentCheckbox = document.getElementById('isStudent');

//    // Preview ảnh
//    imageInput.addEventListener('change', function (e) {
//        const file = e.target.files[0];
//        if (file) {
//            const reader = new FileReader();
//            reader.onload = function (e) {
//                imagePreview.style.display = 'block';
//                imagePreview.src = e.target.result;
//                scanButton.disabled = false;
//            }
//            reader.readAsDataURL(file);
//        }
//    });

//    // Xử lý quét thẻ
//    scanButton.addEventListener('click', async function () {
//        const file = imageInput.files[0];
//        if (!file) return;

//        loadingIndicator.style.display = 'block';
//        scanButton.disabled = true;

//        const formData = new FormData();
//        formData.append('imageFile', file); // Sửa tên tham số phù hợp với API

//        try {
//            const response = await fetch('/tai-khoan/xu-ly-the-sinh-vien', { // Giữ nguyên route đã định nghĩa
//                method: 'POST',
//                body: formData
//            });

//            const result = await response.json();

//            if (result.success) {
//                const data = result.data; // API trả về dữ liệu trong thuộc tính data

//                // Điền thông tin vào các trường
//                document.getElementById('fullname').value = data.FullName || '';
//                document.getElementById('birthday').value = data.Birthday || '';
//                document.getElementById('age').value = data.Age || '';
//                document.getElementById('')
//                // Kiểm tra xem có phải sinh viên không
//                if (data.IsStudent) {
//                    isStudentCheckbox.checked = true;
//                } else {
//                    isStudentCheckbox.checked = false;
//                }

//                // Trigger sự kiện change
//                isStudentCheckbox.dispatchEvent(new Event('change'));

               
//            } else {
//                alert('Có lỗi xảy ra: ' + result.message);
//            }
//        } catch (error) {
//            console.error('Lỗi:', error);
//            alert('Có lỗi xảy ra khi xử lý yêu cầu');
//        } finally {
//            loadingIndicator.style.display = 'none';
//            scanButton.disabled = false;
//        }
//    });
//});