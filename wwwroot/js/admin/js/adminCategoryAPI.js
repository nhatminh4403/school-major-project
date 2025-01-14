document.addEventListener("DOMContentLoaded", function () {
    // Handle modal dismiss button
    fetchCategories();
    $('.modal__btn--dismiss').on('click', function (e) {
        e.preventDefault();
        $.magnificPopup.close();
    });

});
async function fetchCategories() {

    const response = await fetch('/api/admin/categories');
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }
    const categories = await response.json();
    const category_body =  document.getElementById('category_body');
    category_body.innerHTML = categories.map(category => displayCategories(category)).join('');
    initializeModalTriggers()
    try{

    }catch(error ){
        console.error('There was a problem with the fetch operation:', error);

    }
}

function displayCategories(item) {
        return `
<tr>
<td>
                      <div class="main__table-text">${item.id}</div>
                  </td>
                  <td>
                      <div class="main__table-text">${item.name}</div>
                  </td>
                  <td>
                      <div class="main__table-btns">
                          <a href="/admin/categories/edit/${item.id}" 
                             class="main__table-btn main__table-btn--edit">
                              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
                                  <path d="M5,18H9.24a1,1,0,0,0,.71-.29l6.92-6.93h0L19.71,8a1,1,0,0,0,0-1.42L15.47,2.29a1,1,0,0,0-1.42,0L11.23,5.12h0L4.29,12.05a1,1,0,0,0-.29.71V17A1,1,0,0,0,5,18ZM14.76,4.41l2.83,2.83L16.17,8.66,13.34,5.83ZM6,13.17l5.93-5.93,2.83,2.83L8.83,16H6ZM21,20H3a1,1,0,0,0,0,2H21a1,1,0,0,0,0-2Z"/>
                              </svg>
                          </a>
                          <a href="#modal-delete" onclick="setDeleteItem(${item.id})"
                             class="main__table-btn main__table-btn--delete open-modal">
                              <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
                                  <path d="M20,6H16V5a3,3,0,0,0-3-3H11A3,3,0,0,0,8,5V6H4A1,1,0,0,0,4,8H5V19a3,3,0,0,0,3,3h8a3,3,0,0,0,3-3V8h1a1,1,0,0,0,0-2ZM10,5a1,1,0,0,1,1-1h2a1,1,0,0,1,1,1V6H10Zm7,14a1,1,0,0,1-1,1H8a1,1,0,0,1-1-1V8H17Z"/>
                              </svg>
                          </a>
                      </div>
                  </td>
</tr>
                  
              `;
    }
function initializeModalTriggers() {
    $('.open-modal').magnificPopup({
        type: 'inline',
        fixedContentPos: true,
        fixedBgPos: true,
        overflowY: 'auto',
        preloader: false,
        focus: '#username',
        modal: true,
        removalDelay: 300,
        mainClass: 'my-mfp-zoom-in',
    });
}
document.addEventListener('DOMContentLoaded', function() {
    const form = document.getElementById('createCategoryForm');
    form.addEventListener('submit', function(event) {
        event.preventDefault(); // Prevent traditional form submission

        const formData = new FormData(form);
        fetch('/api/admin/categories', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (!response.ok) {
                    return response.json().then(error => {
                        throw new Error(error.message || 'Network response was not ok');
                    });
                }
                return response.json();
            })
            .then(data => {
                if (data.message) {
                    alert(data.message); // Display success message
                    form.reset(); // Reset form after successful submission
                    window.location.href = '/admin/categories';
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Đã xảy ra lỗi trong quá trình tạo thể loại');
            });
    });
});

