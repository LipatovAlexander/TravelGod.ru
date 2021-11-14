$('#profile-form #toggle-edit').on('click', function (e) {
    toggleEditable()
})

$('#profile-form input[type="submit"]').on('click', function () {
    let form = $('#profile-form');
    let validator = form.validate();
    if (form.valid()) {
        $.ajax({
            type: "POST",
            url: '/Profile?handler=Edit&id=' + $('#UserId').val(), // Replace YOUR_CUSTOM_HANDLER with your handler.
            data: $('#profile-form').serialize(),
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            dataType: "json"
        }).done(function (data) {
            if (data.success)
                toggleEditable();
        })
    } else {
        validator.showErrors();
    }
    return false;
})

function toggleEditable(editFlag) {
    let inputs = $('#profile-form input:not([type="submit"])');
    inputs.attr('readonly', (_, attr) => !attr);
    inputs.attr('disabled', (_, attr) => !attr);
    inputs.toggleClass('form-control-plaintext');
    inputs.toggleClass('form-control');
    $('#profile-form #toggle-edit').attr('hidden', (_, attr) => !attr);
    $('#profile-form input[type="submit"]').attr('hidden', (_, attr) => !attr);
}