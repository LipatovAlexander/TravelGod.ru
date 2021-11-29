﻿function toggleEditable(editFlag) {
    let inputs = $('#profile-form input:not([type="submit"]), #wizard-picture');
    inputs.attr('readonly', (_, attr) => !attr);
    inputs.attr('disabled', (_, attr) => !attr);
    inputs.toggleClass('form-control-plaintext');
    inputs.toggleClass('form-control');
    $('#profile-form #toggle-edit').attr('hidden', (_, attr) => !attr);
    $('#profile-form input[type="submit"]').attr('hidden', (_, attr) => !attr);
    $('.picture-container').toggleClass('active');
}

function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();

        reader.onload = function (e) {
            $('#wizardPicturePreview').attr('src', e.target.result).fadeIn('slow');
        }
        reader.readAsDataURL(input.files[0]);
    }
}

$(document).ready(function () {
    $('#profile-form #toggle-edit').on('click', function (e) {
        toggleEditable()
    })

    $('#profile-form input[type="submit"]').on('click', function () {
        let form = $('#profile-form');
        let validator = form.validate();
        if (form.valid()) {
            $.ajax({
                type: "POST",
                url: '/Profile/Index/' + $('#UserId').val() + '?handler=Edit', // Replace YOUR_CUSTOM_HANDLER with your handler.
                data: new FormData(document.forms[0]),
                contentType: false,
                processData: false,
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


    $("#wizard-picture").change(function () {
        readURL(this);
    });
})