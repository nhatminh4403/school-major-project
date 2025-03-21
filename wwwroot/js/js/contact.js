// wwwroot/js/counter-cycler.js
document.addEventListener('DOMContentLoaded', function () {
    // Get all counter items
    const counterItems = document.querySelectorAll('.contact-counter-item');
    let currentActiveIndex = 0;

    // Set the first item as active initially
    counterItems[0].classList.add('active');

    // Function to cycle the active class
    function cycleActiveClass() {
        // Remove active class from current item
        counterItems[currentActiveIndex].classList.remove('active');

        // Move to next item, loop back to first if at the end
        currentActiveIndex = (currentActiveIndex + 1) % counterItems.length;

        // Add active class to new current item
        counterItems[currentActiveIndex].classList.add('active');
    }

    // Set interval to cycle every 3 seconds (adjust time as needed)
    setInterval(cycleActiveClass, 3000);
});