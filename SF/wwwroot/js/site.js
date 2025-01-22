
document.addEventListener('DOMContentLoaded', () => {
    const spinner = document.getElementById('loadingSpinner');

    function showSpinner() {
        const spinner = document.getElementById('loadingSpinner');
        spinner.classList.remove('d-none');
        spinner.classList.add('d-flex'); // Enable flexbox for centering
    }

    function hideSpinner() {
        const spinner = document.getElementById('loadingSpinner');
        spinner.classList.add('d-none');
        spinner.classList.remove('d-flex'); // Remove flexbox when hiding
    }

    // Show spinner on all AJAX requests
    $(document).ajaxStart(showSpinner).ajaxStop(hideSpinner);

    // Show spinner when navigating to another page
    document.addEventListener('click', (e) => {
        const target = e.target;
        if (target.tagName === 'A' && target.getAttribute('href')) {
            showSpinner();
        }
    });

    // Hide spinner after the page has fully loaded
    window.addEventListener('load', hideSpinner);
});
