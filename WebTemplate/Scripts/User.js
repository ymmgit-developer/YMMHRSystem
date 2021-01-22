$(document).ready(function () {
    var table;

    if ($.fn.dataTable.isDataTable('#dttUsuarios')) {
        table = $('#dttUsuarios').DataTable();
        table.destroy();
    }

    table = $('#dttUsuarios').DataTable({
        scrollY: "50vh",
        scrollCollapse: true,
        paging: true,
        searching: true,
        info: true,
        ordering: false
    });

});

function AddUserDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadUser,
        data: { userId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#AddUser', 'dialog');
            dialog.setContent(result);
            dialog.open(); 
        }
    });
}

function NewUser() {

    //var user = $(".user");

    $('#SaveUser').attr('disabled', true);
    $("#SaveUser").removeClass('button primary');
    $("#SaveUser").addClass('button');
    $("#userPreloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveUser,
        cache:false,
        data: $("#UserForm").serialize(),
        success: function (result) {
            if (result === "true") {

                $('#Name').val('');
                $('#FirstSurname').val('');
                $('#LastSurname').val('');
                $('#Email').val('');
                $('#Password').val('');

                Metro.toast.create("User saved", null, null, "bg-green fg-white");
                Metro.dialog.close('#AddUser');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {

            }

            $('#SaveUser').attr('disabled', false);
            $("#SaveUser").addClass('button primary');
            $("#SaveUser").addClass('button');
            $("#userPreloader").css("visibility", "hidden");

        }
    });

}

function BlockUser(userId) {

    $.ajax({
        method: "POST",
        url: window.$BlockUser,
        data: { userId: userId, status: $("#Status" + userId).is(":checked") },
        success: function (result) {
            if (result === "true") {
                $("#Status" + userId).prop('checked', true);
                Metro.toast.create("User activated.", null, null, "bg-green fg-white");
            } else {
                $("#Status" + userId).prop('checked', false);
                Metro.toast.create("User blocked.", null, null, "bg-red fg-white");
            }

        }
    });

}


