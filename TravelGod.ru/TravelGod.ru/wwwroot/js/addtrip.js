$('.radio-group .radio').click(function(){
    $('.radio-group .radio').removeClass('selected');
    $(this).addClass('selected');
    var val = $(this).attr('data-value');
    //alert(val);
    $('#radio-value').val(val);
});

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

$('.btnNext').click(function(e){
    e.preventDefault();
    $('.nav-tabs .active').parent().next('li').find('a').trigger('click');
});

$('.btnPrevious').click(function(e){
    e.preventDefault();
    $('.nav-tabs .active').parent().prev('li').find('a').trigger('click');
});