function getCategoryIdFromURL() {
    const urlParams = window.location.pathname.split('/');
    return urlParams[urlParams.length - 1];
}

// Function to fetch category data and bind to the form
function fetchCategoryData(categoryId) {
    fetch(`http://localhost:8080/api/admin/categories/${categoryId}`)
        .then(response => response.json())
        .then(data => {
            document.getElementById("updatedName").value = data.updatedName;
            // Handle the icon data if needed
        })
        .catch(error => console.error('Error fetching category:', error));
}

// Function to update category data
function updateCategory(event) {
    event.preventDefault(); // Prevent form from submitting the traditional way
    const categoryId = getCategoryIdFromURL();
    const name = document.getElementById("updatedName").value;
    const nameInput = document.getElementById("updatedName");
    const formData = new FormData();
    formData.append("updatedName", name);
    nameInput.value = '';
    fetch(`http://localhost:8080/api/admin/categories/${categoryId}`, {
        method: 'PUT',
        body: formData
    })
        .then(response => response.json())
        .then(data => {

            alert(data.message)
            // console.log('Category updated successfully:', data);
            // Handle the response after updating the category
            window.location.href = '/admin/categories';
        })
        .catch(error => {
            console.error('Error updating category:', error);
            alert('Lỗi' + error.message);
        });
}

// Event listener for DOMContentLoaded to fetch data when the form loads
document.addEventListener("DOMContentLoaded", function () {
    const categoryId = getCategoryIdFromURL();
    if (categoryId) {
        fetchCategoryData(categoryId);
    }

    // Add event listener to form submit button
    document.getElementById("updateCategoryForm").addEventListener("submit", updateCategory);
});