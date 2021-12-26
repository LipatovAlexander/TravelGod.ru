$(function () {
    function toggleEditable(row) {
        let inputs = row.find('input:not([type="submit"]), select');
        inputs.attr('readonly', (_, attr) => !attr);
        inputs.attr('disabled', (_, attr) => !attr);
        inputs.toggleClass('form-control-plaintext');
        inputs.toggleClass('form-control');
        row.find('.toggle-edit').attr('hidden', (_, attr) => !attr);
        row.find('.save').attr('hidden', (_, attr) => !attr);
        row.find('.remove-avatar').attr('hidden', (_, attr) => !attr)
    }

    $('.toggle-edit').on('click', function () {
        let row = $(this).parent().parent();
        toggleEditable(row);
        return false;
    })

    $('.save').on('click', function (e) {
        e.preventDefault();
        let row = $(this).parent().parent();
        let form = row.find('form');
        let id = row.find('.id').val();
        $.ajax({
            type: "POST",
            url: '?handler=Edit&id=' + id, // Replace YOUR_CUSTOM_HANDLER with your handler.
            data: form.serialize(),
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function () {
                toggleEditable(row)
            },
            dataType: 'json'
        })
    })

    $('.remove-form').on('submit', function (e) {
        e.preventDefault();
        let form = $(this);
        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize(),
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function () {
                form.remove()
            }
        })
    })
})