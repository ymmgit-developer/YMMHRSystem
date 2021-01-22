$(document).ready(function () {

    $("#SolicitudAccesoLink").click(function () {
        $("#preloaderRedirect").css("visibility", "visible");
        LoginDialog();
    });
});
function LoginDialog() {
    $("#PreloaderLogin").css("display", "block");
    $("#PreloaderLogin").data('dialog').open();
}


function PasswordRecoveryDialog() {
    $("#PasswordRecovery").css("display", "block");
    $("#PasswordRecovery").data('dialog').open();
}

function ClosePasswordRecoveryDialog() {
    $("#PasswordRecovery").css("display", "none");
    $("#PasswordRecovery").data('dialog').close();
}

function SendEmailPasswordRecovery() {
    var usuario = document.getElementById("UserPasswordRecovery").value;
    $('#SendEmail').attr('disabled', true);
    $("#SendEmail").removeClass('button primary');
    $("#SendEmail").addClass('button');
    $("#preloaderEmail").css("visibility", "visible");
    $.ajax({
        method: "POST",
        url: window.$SendEmailPasswordRecovery,
        data: { user: usuario },
        datatype: "html",
        success: function (result) {
            if (result === "True") {
                Metro.toast.create("Email sent.", null, null, "bg-green fg-white");
            } else {
                Metro.toast.create("That email does not exist.", null, null, "bg-red fg-white");
            }
            $("#preloaderEmail").css("visibility", "hidden");
            $('#SendEmail').attr('disabled', false);
            $("#SendEmail").addClass('button primary');
            $("#SendEmail").addClass('button');
            $('#UserPasswordRecovery').val('');
            ClosePasswordRecoveryDialog();
        }
    });
}