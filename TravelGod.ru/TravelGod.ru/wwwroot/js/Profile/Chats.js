function toggleActiveChatLink(link) {
    let prevActive = $('.chat-link.active');
    prevActive.removeClass('text-white');
    prevActive.addClass('list-group-item-light');
    prevActive.find('p').addClass('text-muted');
    prevActive.removeClass('active');

    link.addClass('active');
    link.addClass('text-white');
    link.removeClass('list-group-item-light');
    link.find('p').removeClass('text-muted');
}

$(document).ready(function () {
    let chatLinks = $('.chat-link');
    let form = $('#message-form');

    toggleActiveChatLink(chatLinks.first());

    form.find('[type="submit"]').on('click', function () {
        let validator = form.validate();
        if (form.valid()) {
            $.ajax({
                type: "POST",
                url: '/Profile/Chats?handler=SendMessage', // Replace YOUR_CUSTOM_HANDLER with your handler.
                data: new FormData(document.forms[0]),
                contentType: false,
                processData: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                dataType: "html",
                success: function (html) {
                    $('#messages-box').append(html);
                    $('#message-input').val('');
                }
            })
        } else {
            validator.showErrors();
        }
        return false;
    })

    chatLinks.on('click', function () {
        let chatId = $(this).attr('chat-id');
        $.ajax({
            type: "GET",
            url: '/Profile/Chats?handler=GetChat&id=' + chatId,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            dataType: "html",
            success: function (html) {
                $('#messages-box').remove();
                $('#chat-box').prepend(html);
            }
        })
        toggleActiveChatLink($(this));
        return false;
    })
})