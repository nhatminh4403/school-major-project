

function redeemPromotion(promotionId) {


    fetch(`/khuyen-mai/doi-khuyen-mai/${promotionId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            
        },
    })
        .then(response => {
            return response.json().then(data => {
                if (!response.ok) {
                    const error = new Error(data.message || `Lỗi ${response.status}`);
                    error.data = data;
                    error.status = response.status;
                    return Promise.reject(error);
                }
                return data;
            }).catch(jsonError => {
                console.error("Lỗi parse JSON:", jsonError);
                const error = new Error(`Yêu cầu thất bại với mã lỗi ${response.status}. Không thể đọc chi tiết lỗi.`);
                error.status = response.status;
                return Promise.reject(error);
            });
        })
        .then(data => {
            console.log("Success data:", data);

            Toastify({
                text: data.message || "Quy đổi thành công!",
                duration: 3000, 
                close: true, 
                gravity: "top", 
                position: "right", 
                stopOnFocus: true, 
                style: {
                   
                    background: "linear-gradient(to right, #00b09b, #96c93d)",
                },
                onClick: function () { }
            }).showToast();

            const promotionElement = document.querySelector(`#promotion-${promotionId}`);
            if (promotionElement) {
                const actionButton = promotionElement.querySelector('.btn-primary');
                if (actionButton) {
                    actionButton.textContent = 'Đã quy đổi';
                    actionButton.disabled = true;
                    actionButton.classList.remove('btn-primary');
                    actionButton.classList.add('btn-success', 'disabled');
                }
            }

            // Cập nhật số điểm
            const userPointsElement = document.getElementById('user-points-display');
            if (userPointsElement && data.newPoints !== undefined) {
                userPointsElement.textContent = data.newPoints;
            }

           
        })
        .catch(error => {
            // ---- XỬ LÝ KHI THẤT BẠI ----
            console.error('Lỗi quy đổi:', error);

           
            Toastify({
                text: error.message || "Quy đổi thất bại! Vui lòng thử lại.", 
                duration: 5000, 
                close: true,
                gravity: "top",
                position: "right",
                stopOnFocus: true,
                style: {
                    background: "linear-gradient(to right, #ff5f6d, #ffc371)",
                },
            }).showToast();
        });
}



