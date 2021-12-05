$(document).ready(function () {
    let ratingModal = $('#ratingModal');
    if (ratingModal) {
        ratingModal.modal('show');
    }

    $('#comment-form input[type="submit"]').on('click', function () {
        let form = $('#comment-form');
        let validator = form.validate();
        if (form.valid()) {
            $.ajax({
                type: "POST",
                url: '/Trips/Concrete/' + $('#TripId').val() + '?handler=AddComment', // Replace YOUR_CUSTOM_HANDLER with your handler.
                data: new FormData(document.querySelector('#comment-form')),
                contentType: false,
                processData: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                dataType: "html",
                success: function (html) {
                    $('#comments').prepend(html);
                    $('#comment-text').val('');
                }
            })
        } else {
            validator.showErrors();
        }
        return false;
    })
})