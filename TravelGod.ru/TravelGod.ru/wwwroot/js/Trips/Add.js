$(function () {
    $('.radio-group .radio[data-value="' + $('#radio-value').val() +'"]').toggleClass('selected')
    $('[data-toggle="tooltip"]').tooltip()

    $('.radio-group .radio').click(function () {
        $('.radio-group .radio').removeClass('selected');
        $(this).addClass('selected');
        let val = $(this).attr('data-value');
        $('#radio-value').val(val);
    });

    let form = $('#trip-form');

    $('.nav-item:has(.nav-link.active)').nextAll().find('.nav-link').click(function (e) {
        let tab = $('.tab-pane.active.show')
        let inputs = tab.find('input')
        let validator = form.validate();
        if (!inputs.valid()) {
            validator.showErrors();
            e.preventDefault();
            return false;
        }
    })

    $('.btnNext').click(function (e) {
        e.preventDefault();
        $('.nav-tabs .active').parent().next('li').find('a').trigger('click');
    });

    $('.btnPrevious').click(function (e) {
        e.preventDefault();
        $('.nav-tabs .active').parent().prev('li').find('a').trigger('click');
    });

})

$(function () {

    let searchDatesInput = $('#dateRangePicker');

    searchDatesInput.daterangepicker({
        autoUpdateInput: false,
        minDate: new Date(),
        autoApply: true,
        opens: 'left',
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

    searchDatesInput.on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD.MM.YYYY') + ' - ' + picker.endDate.format('DD.MM.YYYY'));
    });

    searchDatesInput.on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
    });

});