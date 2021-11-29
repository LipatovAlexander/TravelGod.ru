$(function () {
    let currentPage = $('#CurrentPage').val();
    let activeLink = $('#' + currentPage);
    activeLink.addClass('active');

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
        let userId = row.find('.userId').val();
        $.ajax({
            type: "POST",
            url: '/Admin/Users?handler=EditUser&id=' + userId, // Replace YOUR_CUSTOM_HANDLER with your handler.
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

    $('.remove-avatar').on('click', function () {
        let row = $(this).parent().parent();
        let save = row.find('.save');
        if (save.is(':visible')) {
            row.find('.avatarId').attr('value', '1');
            row.find('img').attr('src', '/CustomFiles/Avatars/default-avatar.png');
            row.find('img').attr('alt', 'default-avatar.png');
        }
    })
})