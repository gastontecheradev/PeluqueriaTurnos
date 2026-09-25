// Menú colapsable del header (< 900px)
(function () {
    var header = document.querySelector('[data-nf-header]');
    var toggle = document.querySelector('[data-nf-nav-toggle]');
    if (!header || !toggle) return;

    toggle.addEventListener('click', function () {
        var open = header.classList.toggle('is-open');
        toggle.setAttribute('aria-expanded', open ? 'true' : 'false');
    });
})();
