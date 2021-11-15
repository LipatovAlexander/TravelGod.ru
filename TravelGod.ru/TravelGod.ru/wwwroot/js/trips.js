$(function() {

    let searchDatesInput = $('#dateRangePicker');

    searchDatesInput.daterangepicker({
        autoUpdateInput: false,
        locale: {
            cancelLabel: 'Отменить',
            format: 'DD.MM.YYYY',
            applyLabel: 'Вставить',
        }
    });

    searchDatesInput.on('apply.daterangepicker', function(ev, picker) {
        $(this).val(picker.startDate.format('DD.MM.YYYY') + ' - ' + picker.endDate.format('DD.MM.YYYY'));
    });

    searchDatesInput.on('cancel.daterangepicker', function(ev, picker) {
        $(this).val('');
    });

});