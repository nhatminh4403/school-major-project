document.addEventListener('DOMContentLoaded', function () {
    let html5QrcodeScanner = null;
    window.toggleScanner = function () {
        const overlay = document.getElementById('scanner-overlay');
        if (overlay.style.display === 'none' || !overlay.style.display) {
            overlay.style.display = 'flex';
            startScanner();
        } else {
            overlay.style.display = 'none';
            if (html5QrcodeScanner) {
                html5QrcodeScanner.clear();
            }
        }
    }

    function startScanner() {
        if (html5QrcodeScanner) {
            html5QrcodeScanner.clear();
        }
        const config = {
            fps: 10,
            qrbox: {
                width: 250,
                height: 250
            },
            aspectRatio: 1.0,
            formatsToSupport: [Html5QrcodeSupportedFormats.QR_CODE],
            experimentalFeatures: {
                useBarCodeDetectorIfSupported: true
            },
            rememberLastUsedCamera: true,
            showTorchButtonIfSupported: true
        };
        html5QrcodeScanner = new Html5QrcodeScanner("reader", config, /* verbose= */ false);
        html5QrcodeScanner.render(onScanSuccess, onScanFailure);
    }

    async function onScanSuccess(decodedText, decodedResult) {
        console.log('Scanned text:', decodedText);
        console.log('Scanned type:', typeof decodedText);
        try {
            const match = decodedText.match(/BOOKING-(\d+)/);
            if (!match) {
                throw new Error('Invalid QR code format');
            }
            const bookingId = parseInt(match[1]);
            console.log('Scanned booking ID:', bookingId);
            // Show loading indicator
            const loadingDiv = document.createElement('div');
            loadingDiv.id = 'loadingIndicator';
            loadingDiv.innerHTML = 'Searching for booking...';
            loadingDiv.style.position = 'fixed';
            loadingDiv.style.top = '50%';
            loadingDiv.style.left = '50%';
            loadingDiv.style.transform = 'translate(-50%, -50%)';
            loadingDiv.style.background = 'white';
            loadingDiv.style.padding = '20px';
            loadingDiv.style.borderRadius = '5px';
            loadingDiv.style.zIndex = '2000';
            document.body.appendChild(loadingDiv);
            // Fetch booking data from the API
            const response = await fetch(`/api/admin/bookings/${bookingId}`);
            if (!response.ok) {
                throw new Error('Booking not found');
            }
            const booking = await response.json();
            // Hide all rows first
            const rows = document.querySelectorAll('#bookingTableBody tr');
            rows.forEach(row => {
                row.style.display = 'none';
            });
            // Show only the matched row
            const matchedRow = document.querySelector(`tr[data-booking-id="${booking.id}"]`);
            if (matchedRow) {
                matchedRow.style.display = 'table-row';
                matchedRow.style.backgroundColor = '#e8f5e9';
                setTimeout(() => {
                    matchedRow.style.backgroundColor = '';
                }, 3000);
            } else {
                // If the row doesn't exist in the current table, you might want to
                // either reload the page or add a new row with the booking data
                const tableBody = document.getElementById('bookingTableBody');
                const newRow = createBookingRow(booking); // You'll need to implement this
                tableBody.insertBefore(newRow, tableBody.firstChild);
                newRow.style.backgroundColor = '#e8f5e9';
                setTimeout(() => {
                    newRow.style.backgroundColor = '';
                }, 3000);
            }
            // Add reset button if it doesn't exist
            if (!document.getElementById('resetTable')) {
                const resetBtn = document.createElement('button');
                resetBtn.id = 'resetTable';
                resetBtn.className = 'btn btn-secondary ml-2';
                resetBtn.textContent = 'Show All Bookings';
                resetBtn.onclick = resetTable;
                document.querySelector('.main__title').appendChild(resetBtn);
            }
        } catch (error) {
            console.error('Error:', error);
            alert('Could not find the booking. Please try again.');
        } finally {
            // Close scanner overlay
            if (html5QrcodeScanner) {
                html5QrcodeScanner.clear();
            }
            document.getElementById('scanner-overlay').style.display = 'none';
            // Remove loading indicator
            const loadingDiv = document.getElementById('loadingIndicator');
            if (loadingDiv) {
                loadingDiv.remove();
            }
        }
    }

    // Helper function to create a new table row from booking data
    function createBookingRow(booking) {
        const row = document.createElement('tr');
        row.setAttribute('data-booking-id', booking.id);
        // Adjust these fields according to your booking object structure
        row.innerHTML = `



				<td>${booking.id}</td>
				<td>${booking.customerName}</td>
				<td>${booking.date}</td>
				<td>${booking.time}</td>
				<td>${booking.status}</td>
				<td>
					<button class="btn btn-primary btn-sm" onclick="editBooking(${booking.id})">Edit</button>
					<button class="btn btn-danger btn-sm" onclick="deleteBooking(${booking.id})">Delete</button>
				</td>
    `;
        return row;
    }

    function onScanFailure(error) {
        console.warn(`QR scan error = ${error}`);
    }

    function resetTable() {
        const rows = document.querySelectorAll('#bookingTableBody tr');
        rows.forEach(row => {
            row.style.display = 'table-row';
        });
        const resetBtn = document.getElementById('resetTable');
        if (resetBtn) {
            resetBtn.remove();
        }
    }

    // Close scanner when clicking outside the modal
    document.getElementById('scanner-overlay').addEventListener('click', function (e) {
        if (e.target === this) {
            toggleScanner();
        }
    });
    // Handle escape key to close scanner
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            const overlay = document.getElementById('scanner-overlay');
            if (overlay.style.display === 'flex') {
                toggleScanner();
            }
        }
    });
});