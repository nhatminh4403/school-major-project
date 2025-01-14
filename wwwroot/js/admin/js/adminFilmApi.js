// Function to format date
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('vi-VN');
}

// Function to initialize modal triggers
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

// Function to create table row HTML
function createFilmRow(film) {
    return `
        <tr>
            <td>
                <div class="main__table-text">${film.id}</div>
            </td>
            <td>
                <div class="main__table-text">${film.name}</div>
            </td>
            <td>
                <img src="${film.poster}" alt="Film Poster" style="max-width: 100px;"/>
            </td>
            <td>
                <div class="main__table-text">${formatDate(film.openingday)}</div>
            </td>
            <td>
                <div class="main__table-text">${film.duration} phút</div>
            </td>
            <td>
                <div class="main__table-text">${film.country.name}</div>
            </td>
            <td>
                <div class="main__table-text">${film.limit_age}</div>
            </td>
            <td>
                <div class="main__table-btns">
                    <a href="/admin/films/edit/${film.id}" 
                       class="main__table-btn main__table-btn--edit">
                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
                            <path d="M5,18H9.24a1,1,0,0,0,.71-.29l6.92-6.93h0L19.71,8a1,1,0,0,0,0-1.42L15.47,2.29a1,1,0,0,0-1.42,0L11.23,5.12h0L4.29,12.05a1,1,0,0,0-.29.71V17A1,1,0,0,0,5,18ZM14.76,4.41l2.83,2.83L16.17,8.66,13.34,5.83ZM6,13.17l5.93-5.93,2.83,2.83L8.83,16H6ZM21,20H3a1,1,0,0,0,0,2H21a1,1,0,0,0,0-2Z"/>
                        </svg>
                    </a>
                    <a href="#modal-delete" 
                       onclick="setDeleteItem(${film.id})"
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

// Function to fetch films and update table
async function fetchAndDisplayFilms() {
    try {
        const response = await fetch('/api/admin/films'); // Adjust the API endpoint as needed
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const films = await response.json();

        const filmBody = document.getElementById('film_body');
        filmBody.innerHTML = films.map(film => createFilmRow(film)).join('');

        // Reinitialize modal triggers after updating content
        initializeModalTriggers();
    } catch (error) {
        console.error('Error fetching films:', error);
    }
}

// Initialize when the page loads
document.addEventListener('DOMContentLoaded', () => {
    fetchAndDisplayFilms();

    // Handle modal dismiss button
    $('.modal__btn--dismiss').on('click', function (e) {
        e.preventDefault();
        $.magnificPopup.close();
    });
});