document.addEventListener('DOMContentLoaded', () => {  

    document.querySelectorAll('.print-ticket').forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault();
            const buttonElement = this;  
             
            const receiptId = buttonElement.getAttribute('data-booking-id');

            if (!receiptId) {
                console.error('Missing data-receipt-id or data-booking-id attribute on button.');
                alert('Cannot print ticket: Receipt ID is missing.');
                return;
            }

             
            const apiUrl = `/admin/hoa-don/in-ve/${receiptId}`;

             
            const originalButtonText = buttonElement.textContent;


            console.log(`Fetching PDFs from: ${apiUrl}`);

            fetch(apiUrl)
                .then(response => {
                     
                    if (!response.ok) {
                         
                        return response.json()
                            .then(errData => {
                                 
                                throw new Error(errData.message || `Server error: ${response.status}`);
                            })
                            .catch(() => {
                                 
                                throw new Error(`Network response was not ok (${response.status})`);
                            });
                    }
                     
                    return response.json();
                })
                .then(urls => {
                    console.log("Received URLs:", urls);
                    if (!Array.isArray(urls)) {
                        console.error('Response from server was not an array:', urls);
                        throw new Error('Received invalid data from server.');
                    }
                    if (urls.length === 0) {
                        console.warn('PDF generation successful, but no URLs were provided (no details?).');
                        alert('No ticket files were generated. There might be no details associated with this receipt.');
                         
                        buttonElement.disabled = false;
                        buttonElement.textContent = originalButtonText;
                        return;  
                    }

                     
                    let totalDelay = 0;  

                    urls.forEach((url, index) => {
                        console.log(`Processing URL ${index + 1}: ${url}`);

                         
                         
                        const viewDelay = index * 600;  
                        setTimeout(() => {
                            console.log(`(Delay: ${viewDelay}ms) Opening view: ${url}`);
                            window.open(url, '_blank');
                        }, viewDelay);

                         
                         
                        const downloadDelay = viewDelay + 200;  
                        setTimeout(() => {
                            try {
                                console.log(`(Delay: ${downloadDelay}ms) Triggering download for: ${url}`);
                                const downloadLink = document.createElement('a');
                                downloadLink.href = url;

                                 
                                const filename = url.substring(url.lastIndexOf('/') + 1);
                                downloadLink.download = filename || `ticket-${receiptId}-${index + 1}.pdf`;  

                                document.body.appendChild(downloadLink);
                                downloadLink.click();
                                document.body.removeChild(downloadLink);  
                                console.log(`Download initiated for: ${filename}`);
                            } catch (dlError) {
                                console.error(`(Delay: ${downloadDelay}ms) Error triggering download for ${url}:`, dlError);
                                 
                            }
                        }, downloadDelay);

                         
                        totalDelay = downloadDelay;
                    });


                    const finalResetDelay = totalDelay + 500;
                    console.log(`Scheduling button reset after ${finalResetDelay}ms`);
                    setTimeout(() => {
                        buttonElement.disabled = false;
                        buttonElement.textContent = originalButtonText;
                        console.log('Button reset.');
                    }, finalResetDelay);

                })
                .catch(error => {
                     
                    console.error('Error fetching or processing ticket PDFs:', error);

                     
                    alert(`There was an issue with the ticket generation process, but some tickets may have been generated. Please check your downloads or the tickets folder.`);
]
                });
        });
    });

});  