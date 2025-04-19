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

            // Fetch booking data from the API - Fix URL string syntax
            const response = await fetch(`/admin/hoa-don/chi-tiet/${bookingId}`);
            if (!response.ok) {
                throw new Error('Booking not found');
            }

            const data = await response.json();
            console.log('API response data:', data); // Log the full response

            // Check the structure of the returned data
            if (!data || !data.Receipt) {
                throw new Error('Invalid response format: Receipt data missing');
            }

            const receipt = data.Receipt;
            console.log('Receipt data:', receipt);

            // Check if id exists before using it
            if (!receipt.id) {
                console.log('Warning: receipt.id is undefined, using manual ID lookup');
                // The id might be capitalized or named differently
                const receiptId = receipt.Id || receipt.ID || receipt.id || bookingId;

                // Hide all rows first
                const rows = document.querySelectorAll('#bookingTableBody tr');
                rows.forEach(row => {
                    row.style.display = 'none';
                });

                // Show only the matched row - Fix selector string syntax
                const matchedRow = document.querySelector(`tr[data-booking-id="${receiptId}"]`);
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
                    const newRow = createBookingRow(receipt); // You'll need to implement this
                    tableBody.insertBefore(newRow, tableBody.firstChild);
                    newRow.style.backgroundColor = '#e8f5e9';
                    setTimeout(() => {
                        newRow.style.backgroundColor = '';
                    }, 3000);
                }
            } else {
                // Original code path if id exists
                // Hide all rows first
                const rows = document.querySelectorAll('#bookingTableBody tr');
                rows.forEach(row => {
                    row.style.display = 'none';
                });

                // Show only the matched row - Fix selector string syntax
                const matchedRow = document.querySelector(`tr[data-booking-id="${receipt.id}"]`);
                if (matchedRow) {
                    matchedRow.style.display = 'table-row';
                    matchedRow.style.backgroundColor = '#e8f5e9';
                    setTimeout(() => {
                        matchedRow.style.backgroundColor = '';
                    }, 3000);
                } else {
                    // If the row doesn't exist in the current table, create a new row
                    const tableBody = document.getElementById('bookingTableBody');
                    const newRow = createBookingRow(receipt);
                    tableBody.insertBefore(newRow, tableBody.firstChild);
                    newRow.style.backgroundColor = '#e8f5e9';
                    setTimeout(() => {
                        newRow.style.backgroundColor = '';
                    }, 3000);
                }
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
    function createBookingRow(receipt) {
        const row = document.createElement('tr');
        row.setAttribute('data-booking-id', receipt.Id || receipt.id);

        // Add detailed logging to help debug
        console.log('Creating row for receipt:', receipt);

        // First, check if we have receipt details
        if (!receipt.ReceiptDetails && !receipt.receiptDetails) {
            console.warn('Receipt has no details property');
            // Create a simplified row without details
            row.innerHTML = `
            <td><div class="main__table-text">${receipt.Id || receipt.id || 'N/A'}</div></td>
            <td><div class="main__table-text">${receipt.GetUser?.FullName || receipt.getUser?.fullName || 'N/A'}</div></td>
            <td colspan="5"><div class="main__table-text">Details not available</div></td>
            <td><div class="main__table-text">${parseFloat(receipt.TotalPrice || receipt.totalPrice || 0).toLocaleString()}</div></td>
            <td>
                <div class="main__table-btns">
                    <a class="main__table-btn main__table-btn--edit print-ticket" data-booking-id="${receipt.Id || receipt.id}" href="#">
                        <svg fill="#000000" height="20px" width="20px" version="1.1" id="Layer_1"
                            xmlns="http://www.w3.org/2000/svg"
                            xmlns:xlink="http://www.w3.org/1999/xlink"
                            viewBox="0 0 502 502" xml:space="preserve">
                            <!-- SVG path data omitted for brevity -->
                            <path d="M492,158.5h-41v-135c0-5.522-4.478-10-10-10H61c-5.522,0-10,4.478-10,10v135H10c-5.522,0-10,4.478-10,10v220c0,5.522,4.478,10,10,10h41v80c0,5.522,4.478,10,10,10h380c5.522,0,10-4.478,10-10v-80h41c5.522,0,10-4.478,10-10v-220C502,162.978,497.522,158.5,492,158.5z"></path>
                        </svg>
                    </a>
                </div>
            </td>
        `;
            return row;
        }

        // Get receipt details (handle case sensitivity)
        const details = receipt.ReceiptDetails?.[0] || receipt.receiptDetails?.[0];

        // Format the row with available data
        row.innerHTML = `
        <td><div class="main__table-text">${receipt.Id || receipt.id || 'N/A'}</div></td>
        <td><div class="main__table-text">${receipt.GetUser?.FullName || receipt.getUser?.fullName || 'N/A'}</div></td>
        <td><div class="main__table-text">${details?.FilmName || details?.filmName || 'N/A'}</div></td>
        <td><div class="main__table-text">${details?.StartTime || details?.startTime || 'N/A'}</div></td>
        <td><div class="main__table-text">${receipt.Date || receipt.date || 'N/A'}</div></td>
        <td><div class="main__table-text">${details?.CinemaName || details?.cinemaName || 'N/A'}</div></td>
        <td><div class="main__table-text">${details?.RoomName || details?.roomName || 'N/A'}</div></td>
        <td><div class="main__table-text">${parseFloat(receipt.TotalPrice || receipt.totalPrice || 0).toLocaleString()}</div></td>
        <td>
            <div class="main__table-btns">
                <a class="main__table-btn main__table-btn--edit print-ticket" data-booking-id="${receipt.Id || receipt.id}" href="#">
                    <svg fill="#000000" height="20px" width="20px" version="1.1" id="Layer_1"
                        xmlns="http://www.w3.org/2000/svg"
                        xmlns:xlink="http://www.w3.org/1999/xlink"
                        viewBox="0 0 502 502" xml:space="preserve">
                        <!-- SVG path data omitted for brevity -->
                        <path d="M492,158.5h-41v-135c0-5.522-4.478-10-10-10H61c-5.522,0-10,4.478-10,10v135H10c-5.522,0-10,4.478-10,10v220c0,5.522,4.478,10,10,10h41v80c0,5.522,4.478,10,10,10h380c5.522,0,10-4.478,10-10v-80h41c5.522,0,10-4.478,10-10v-220C502,162.978,497.522,158.5,492,158.5z"></path>
                    </svg>
                </a>
            </div>
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