// Reservar turno: resumen "Tu turno" y habilitación de "Confirmar turno"
(function () {
    var form = document.querySelector('[data-nf-booking]');
    if (!form) return;

    var MISSING = 'Sin definir';
    var submit = form.querySelector('[data-nf-submit]');
    var radios = form.querySelectorAll('input[name="ServiceId"]');
    var stylist = form.querySelector('select[name="StylistId"]');
    var start = form.querySelector('input[name="Start"]');
    var clientName = form.querySelector('input[name="ClientName"]');
    var clientPhone = form.querySelector('input[name="ClientPhone"]');
    var startError = document.getElementById('Start-error');
    var pastMessage = form.getAttribute('data-past-message');

    var out = {};
    form.querySelectorAll('[data-summary]').forEach(function (el) {
        out[el.getAttribute('data-summary')] = el;
    });

    // Preselección desde "Reservar {servicio}" en Inicio (?serviceId=)
    var wanted = new URLSearchParams(window.location.search).get('serviceId');
    if (wanted && !form.querySelector('input[name="ServiceId"]:checked')) {
        var match = form.querySelector('input[name="ServiceId"][value="' + CSS.escape(wanted) + '"]');
        if (match) match.checked = true;
    }

    function setValue(el, value) {
        if (!el) return;
        el.textContent = value || MISSING;
        el.classList.toggle('is-missing', !value);
    }

    function parseStart() {
        if (!start || !start.value) return null;
        var d = new Date(start.value);
        return isNaN(d.getTime()) ? null : d;
    }

    // "Lunes 28 de septiembre, 14:00"
    function formatDate(d) {
        var day = d.toLocaleDateString('es-UY', { weekday: 'long', day: 'numeric', month: 'long' });
        var time = d.toLocaleTimeString('es-UY', { hour: '2-digit', minute: '2-digit', hour12: false });
        day = day.replace(',', '');
        return day.charAt(0).toUpperCase() + day.slice(1) + ', ' + time;
    }

    function isPast(d) {
        return d !== null && d.getTime() < Date.now();
    }

    function showPastError(show) {
        if (!startError || !pastMessage) return;
        if (show) {
            startError.textContent = pastMessage;
            startError.classList.remove('field-validation-valid');
            startError.classList.add('field-validation-error');
            start.setAttribute('aria-invalid', 'true');
        } else if (startError.textContent === pastMessage) {
            startError.textContent = '';
            startError.classList.remove('field-validation-error');
            startError.classList.add('field-validation-valid');
            start.removeAttribute('aria-invalid');
        }
    }

    function update() {
        var service = form.querySelector('input[name="ServiceId"]:checked');
        setValue(out.service, service ? service.getAttribute('data-name') : '');
        setValue(out.duration, service ? service.getAttribute('data-duration') : '');
        if (out.total) out.total.textContent = service ? service.getAttribute('data-price') : '—';

        var stylistName = stylist && stylist.value ? stylist.options[stylist.selectedIndex].text : '';
        setValue(out.stylist, stylistName);

        var d = parseStart();
        setValue(out.start, d ? formatDate(d) : '');

        var valid = !!service
            && !!stylistName
            && d !== null && !isPast(d)
            && !!clientName && clientName.value.trim().length > 0 && clientName.value.length <= 100
            && (!clientPhone || clientPhone.value.length <= 30);

        if (submit) submit.disabled = !valid;
        return d;
    }

    form.addEventListener('change', function (e) {
        var d = update();
        if (e.target === start) showPastError(isPast(d));
    });
    form.addEventListener('input', update);

    update();
})();
