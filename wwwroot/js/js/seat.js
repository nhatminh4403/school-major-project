﻿let selectedSeats = [];
let totalPrice = 0;
window.onload = function () {
    var seatImages = document.getElementsByClassName('seat-img');
    for (var i = 0; i < seatImages.length; i++) {
        seatImages[i].setAttribute('data-src-original', seatImages[i].src);
        seatImages[i].setAttribute('data-selected', 'false');

        seatImages[i].addEventListener('mouseover', function () {
            this.src = this.getAttribute('data-src2');
            console.log("tôi là sự kiện on over ");
        });

        seatImages[i].addEventListener('mouseout', function () {
            if (this.getAttribute('data-selected') === 'false') {
                this.src = this.getAttribute('data-src-original');
            }
            console.log("tôi là sự kiện on mouse out ");
        });

        seatImages[i].addEventListener('click', function () {
            const seatId = this.getAttribute('data-id');
            const seatSymbol = this.getAttribute('data-symbol');
            const seatPrice = parseInt(this.getAttribute('data-price'));

            if (this.getAttribute('data-selected') === 'false') {
                if (selectedSeats.length >= 7) {
                    document.querySelector('.error-message').textContent = 'Bạn chỉ có thể đặt tối đa 7 ghế 1 lần.';
                    document.querySelector('.error-message').style.display = 'block';
                    hienThiThongBao("Bạn chỉ được chọn tối đa 7 ghế 1 lần", 3000, 'bg-danger');

                    selectedSeats.forEach(seat => {
                        const seatElement = document.querySelector(`img[data-id="${seat.id}"]`);
                        seatElement.setAttribute('data-selected', 'false');
                        seatElement.src = seatElement.getAttribute('data-src-original');
                    });
                    selectedSeats = [];
                    totalPrice = 0;
                } else {
                    this.setAttribute('data-selected', 'true');
                    this.src = this.getAttribute('data-src2');
                    selectedSeats.push({ id: seatId, symbol: seatSymbol, price: seatPrice });
                    totalPrice += seatPrice;
                    hienThiThongBao("Bạn đã chọn ghế thành công", 2000, 'bg-success');
                    document.querySelector('.error-message').style.display = 'none';
                }
            } else {
                this.setAttribute('data-selected', 'false');
                this.src = this.getAttribute('data-src-original');
                selectedSeats = selectedSeats.filter(seat => seat.id !== seatId);
                totalPrice -= seatPrice;
                document.querySelector('.error-message').style.display = 'none';
            }

            updateUI();
        });
    }

    function updateUI() {
        const selectedSeatSymbols = selectedSeats.map(seat => seat.symbol).join(', ');
        document.querySelector('.selected-seats').textContent = selectedSeatSymbols;
        document.querySelector('.total-price').textContent = totalPrice.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
    }
    var checkoutForm = document.querySelector('#checkout-form');
    if (checkoutForm) {
        checkoutForm.addEventListener('submit', function (event) {
            if (selectedSeats.length === 0) {
                event.preventDefault();
                hienThiThongBao("Vui lòng chọn ít nhất một ghế trước khi tiếp tục", 3000, 'bg-danger');
                document.querySelector('.error-message').textContent = 'Bạn chưa chọn ghế nào.';
                document.querySelector('.error-message').style.display = 'block';
                return false;
            }
            return true;
        });
    }
    document.querySelector('#checkout-button').addEventListener('click', function (event) {
        event.preventDefault(); // Ngăn chặn hành vi submit mặc định

        if (selectedSeats.length === 0) {
            hienThiThongBao("Vui lòng chọn ít nhất một ghế trước khi tiếp tục", 3000, 'bg-danger');

            return false;
        } else {
            // Nếu đã chọn ghế, tiến hành cập nhật input và submit form
            const selectedSeatSymbols = selectedSeats.map(seat => ({
                id: seat.id,
                symbol: seat.symbol,
                price: seat.price
            }));
            document.getElementById('selectedSeatsInput').value = JSON.stringify(selectedSeatSymbols);
            document.getElementById('totalPriceInput').value = totalPrice;
            document.getElementById('checkout-form').submit();
        }
    });

    function hienThiThongBao(text, duration, className) {
        Toastify({
            text,
            duration,
            className,
            close: false,
            gravity: "top",
            position: "left",
            stopOnFocus: true,
            style: {
                width: 600,
                height: 250,
            },
            backgroundColor: "red",
        }).showToast();
    }
}