document.addEventListener('DOMContentLoaded', function () {
    const promotionSelect = document.getElementById('promotionSelect');
    const totalPriceElement = document.querySelector('.totalCheckout');
    const originalPriceElement = document.getElementById('originalPrice');
    const discountInfoElement = document.getElementById('discountInfo');
    const discountAmountElement = document.getElementById('discountAmount');
    const comboFoodSelect = document.getElementById('comboFoodSelect');
    const promoCodeBtn = document.getElementById('checkPromoBtn');
    const promoCodeInput = document.getElementById('promoCodeInput');
    const checkoutForm = document.querySelector('form[action*="/purchase/checkout"]');
    const codeAppliedElement = document.getElementById('discountCodeApplied');
    const appliedPromoCodeInput = document.getElementById('appliedPromoCode');
    const appliedDiscountRateInput = document.getElementById('appliedDiscountRate');
    let originalPrice = parseFloat(originalPriceElement.textContent.replace('.', ''));
    let currentDiscountRate = 0;

    function updateTotalPrice(discountRate = 0) {
        let currentPrice = originalPrice;

        if (comboFoodSelect.value !== '0-0') {
            const comboPrice = parseFloat(comboFoodSelect.value.split('-')[1]);
            currentPrice += comboPrice;
        }

        if (promotionSelect.value !== '0-0') {
            const selectedOption = promotionSelect.options[promotionSelect.selectedIndex];
            const discountPercent = parseFloat(selectedOption.getAttribute('data-percent')) || 0;
            currentPrice -= currentPrice * discountPercent;
            appliedPromoCodeInput.value = promotionSelect.value;
            appliedDiscountRateInput.value = discountPercent;
        } else if (discountRate > 0) {
            currentPrice -= currentPrice * discountRate;
            appliedPromoCodeInput.value = promoCodeInput.value.trim();
            appliedDiscountRateInput.value = discountRate;
        } else {
            appliedPromoCodeInput.value = '';
            appliedDiscountRateInput.value = '0';
        }

        totalPriceElement.textContent = Math.round(currentPrice).toLocaleString() + ' VND';
        discountInfoElement.style.display = (discountRate > 0 || promotionSelect.value !== '0-0') ? 'block' : 'none';
        discountAmountElement.textContent = Math.round(originalPrice - currentPrice).toLocaleString();
    }

    promoCodeBtn.addEventListener('click', function () {
        const promotionCode = promoCodeInput.value.trim();
        console.log(promotionCode);
        if (!promotionCode) {
            alert("Vui lòng nhập mã giảm giá!");
            return;
        }
        promotionSelect.value = '0-0';
        fetch(`/khuyen-mai/doi-ma/${promotionCode}`).then(response => {
            if (!response.ok) throw new Error(`Error: ${response.statusText}`);
            return response.json();
        }).then(data => {
            if (data && data.promotion.discountRate) {
                currentDiscountRate = data.promotion.discountRate;
                codeAppliedElement.textContent = data.promotion.code;
                updateTotalPrice(currentDiscountRate);
            } else {
                alert(data.message || "Mã giảm giá không hợp lệ.");
                appliedPromoCodeInput.value = '';
                appliedDiscountRateInput.value = '0';
            }
        }).catch(error => {
            console.error('Error fetching promotion:', error);
            alert("Có lỗi xảy ra khi kiểm tra mã giảm giá.");
            appliedPromoCodeInput.value = '';
            appliedDiscountRateInput.value = '0';
        });
    });

    promotionSelect.addEventListener('change', () => {
        promoCodeInput.value = ''; // Reset promo code input
        updateTotalPrice();
    });

    promoCodeInput.addEventListener('focus', () => {
        promotionSelect.value = '0-0'; // Reset promotion select
        updateTotalPrice();
    });

    comboFoodSelect.addEventListener('change', () => {
        updateTotalPrice(currentDiscountRate);
    });

    checkoutForm.addEventListener('submit', function (e) {
        if (promotionSelect.value === '0-0' && !appliedPromoCodeInput.value) {
            appliedPromoCodeInput.value = '';
            appliedDiscountRateInput.value = '0';
        }
    });
})
});