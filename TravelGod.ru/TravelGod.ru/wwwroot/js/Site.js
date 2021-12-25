// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(function () {
    $(document).bind("ajaxSend", function(){
        $('#loaders').fadeIn(0);
        $("#loading").fadeIn(0);
    }).bind("ajaxSuccess", function(){
        $("#loading").fadeOut(0);
        $('#success').fadeIn(0);
    }).bind("ajaxError", function(){
        $("#loading").fadeOut(0);
        $('#failure').fadeIn(0);
    }).bind("ajaxComplete", function(){
        $('#loaders').fadeOut(2000);
        $('#success').fadeOut(2000);
        $('#failure').fadeOut(2000);
    });
})