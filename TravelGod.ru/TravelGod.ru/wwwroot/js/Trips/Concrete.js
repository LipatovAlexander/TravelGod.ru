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

$(document).ready(function () {
    let ratings = $('.owl-carousel .card');
    let count = ratings.length;
    let items = 0, mdItems = 0, xlItems = 0;
    if (count >= 1) {
        items = 1;
        mdItems = 1;
        xlItems = 1;
    }
    if (count >= 2) {
        mdItems = 2;
        xlItems = 2;
    }
    if (count >= 3) {
        xlItems = 3;
    }

    $('.owl-carousel').owlCarousel({
        margin: 10,
        nav: true,
        loop: true,
        navText:
            ['<svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" class="bi bi-arrow-left-circle-fill" viewBox="0 0 16 16">\n' +
            '  <path d="M8 0a8 8 0 1 0 0 16A8 8 0 0 0 8 0zm3.5 7.5a.5.5 0 0 1 0 1H5.707l2.147 2.146a.5.5 0 0 1-.708.708l-3-3a.5.5 0 0 1 0-.708l3-3a.5.5 0 1 1 .708.708L5.707 7.5H11.5z"/>\n' +
            '</svg>',
                '<svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" class="bi bi-arrow-right-circle-fill" viewBox="0 0 16 16">\n' +
                '  <path d="M8 0a8 8 0 1 1 0 16A8 8 0 0 1 8 0zM4.5 7.5a.5.5 0 0 0 0 1h5.793l-2.147 2.146a.5.5 0 0 0 .708.708l3-3a.5.5 0 0 0 0-.708l-3-3a.5.5 0 1 0-.708.708L10.293 7.5H4.5z"/>\n' +
                '</svg>'],
        responsive: {
            0: {
                items: items
            },
            768: {
                items: mdItems
            },
            1200: {
                items: xlItems
            }
        }
    })
});