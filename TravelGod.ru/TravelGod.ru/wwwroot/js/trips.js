$(function() {

    let searchDatesInput = $('#dateRangePicker');

    searchDatesInput.daterangepicker({
        autoUpdateInput: false,
        locale: {
            cancelLabel: 'Очистить',
            format: 'DD.MM.YYYY',
            separator: ' - ',
            applyLabel: 'Вставить',
            daysOfWeek: [
                "Вс",
                "Пн",
                "Вт",
                "Ср",
                "Чт",
                "Пт",
                "Сб"
            ],
            monthNames: [
                "Январь",
                "Февраль",
                "Март",
                "Апрель",
                "Май",
                "Июнь",
                "Июль",
                "Август",
                "Сентябрь",
                "Октябрь",
                "Ноябрь",
                "Декабрь"
            ],
            firstDay: 1,
        }
    });

    searchDatesInput.on('apply.daterangepicker', function(ev, picker) {
        $(this).val(picker.startDate.format('DD.MM.YYYY') + ' - ' + picker.endDate.format('DD.MM.YYYY'));
    });

    searchDatesInput.on('cancel.daterangepicker', function(ev, picker) {
        $(this).val('');
    });

});