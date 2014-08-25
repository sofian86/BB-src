$(document).ready(function () {
    $('#email').tooltip();    

    /*Email Notification*/
    $('#btnNotify').click(function () {        
        var email = $('#inputEmail').val();
        if (email != "") {
            $('#emailError').addClass("hidden");
            var clubName = getParameterByName("court");
            if (clubName == "") {
                clubName = "Search";
            }
            $.get("EmailSubscription", { emailAddress: email, refferredClub: clubName });
            $('#aftersubmit-msg').removeClass("hidden");
            $('#aftersubmit-msg').addClass("show");
            $(this).attr("disabled", "disabled");
            $(this).removeClass("btn-success");            
        }
        else {
            $('#emailError').removeClass("hidden");
        }
    });

    /*Email Notification*/



    /*All related to partner search*/
    $('#frmLogin').hide();
    $('#back').hide();
    $('#area-options').hide();

    $('.btn-area').click(function () {
        if ($(this).attr("class").indexOf("btn-primary") >= 0) {

            $(this).attr("class", "btn btn-xs btn-area");
            $(this).children(':first').attr("class", "glyphicon glyphicon-plus");
            $('#area-options').append(this);
        }
        else {

            $(this).attr("class", "btn btn-xs btn-area btn-primary");
            $(this).children(':first').attr("class", "glyphicon glyphicon-remove-sign");
            $('#pnl-area').append(this);
        }
    });

    $('#pnl-area').click(function () {
        $('#area-options').toggle();
    });

    $("#close").click(function () {
        $('#area-options').hide();

    });

    $('#next').click(function () {
        $('#frmPreference').hide();
        $('#next').hide();
        $('#frmLogin').show();
        $('#back').show();
    });

    $('#back').click(function () {
        $('#frmPreference').show();
        $('#next').show();
        $('#frmLogin').hide();
        $('#back').hide();
    });

    $(".citydropdown li a").click(function () {

        $("#citydropdown").text($(this).text());
        $("#citydropdown").val($(this).text()).append(" <span class=\"caret\"></span>");
    });

    $(".preftime li a").click(function () {

        $("#preftime").text($(this).text());
        $("#preftime").val($(this).text()).append(" <span class=\"caret\"></span>");
    });

    $(".prefday li a").click(function () {

        $("#prefday").text($(this).text());
        $("#prefday").val($(this).text()).append(" <span class=\"caret\"></span>");
    });

    /*All related to partner search*/


});


function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

