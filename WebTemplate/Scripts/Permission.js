var userSelected;
var CompleteName;
var lastUser;
$(document).ready(function () {

    //Tabla de Roles - Usuario
    if ($.fn.dataTable.isDataTable("#dttRolUsuario")) {
        var table = $("#dttRolUsuario").DataTable();
        table.destroy();
    }
    $("#dttRolUsuario").DataTable({
        scrollY: "50vh",
        scrollCollapse: true,
        paging: false,
        info: false,
        ordering: false
    });

    //Method when a user is selected
    $("#dttRolUsuario tbody").on("click", ".selectedUser", function () {
        CompleteName = $(this).parent().children("#CompleteName").text();
        if (CompleteName !== undefined) {
            $("#PermissionTree").css("display", "block");
            $("#dttRolUsuario tbody").children("#" + lastUser).removeClass('selected');
            lastUser = userSelected;
            userSelected = $(this).parent().prop("id");
            if ($changesMade) {
                var dialogChanges = Metro.getPlugin('#UserChanges', 'dialog');
                dialogChanges.open();
            } else {
                $("#UserName").text($(this).parent().children("#CompleteName").text());
                var dialogPreloader = Metro.getPlugin('#preloaderPermission', 'dialog');
                dialogPreloader.open();
                $.ajax({
                    method: "POST",
                    url: window.$LoadTreeByUser,
                    data: { userId: userSelected },
                    success: function (result) {
                        $("#PermissionTree").children().remove();
                        $("#PermissionTree").append(result);
                        dialogPreloader.close();
                    }
                });
            }
        }
    });
    $("li > label > input").change(function () {
        if (CompleteName !== undefined) {
            $changesMade = true;
        }
    });

    $("#dttRolUsuario tbody").on("click", ".ReturnDefaultRole", function () {
        userSelected = $(this).parent().prop("id");
        $("#UserName").text($(this).parent().children("#CompleteName").text());
    });

});

function SaveTree() {
    $changesMade = false;
    var tarea = $(".taskList");
    var dialogPreloader = Metro.getPlugin('#preloaderSave', 'dialog');
    dialogPreloader.open();
    $.ajax({
        method: "POST",
        url: window.$SaveTreePermissions + "?userId=" + userSelected,
        data: tarea.serialize(),
        success: function (result) {
            dialogPreloader.close();
            if (result === "true") {
                Metro.toast.create("Changes saved", null, null, "bg-green fg-white");
            } else {
                Metro.toast.create("Changes could not be saved", null, null, "bg-red fg-white");
            }

        }
    });
    setTimeout(function () {
        location.reload();
    }, 2000);
}

function ReturnDefaultRole(roleid) {

    var idRole = roleid;
    var dialogPreloader = Metro.getPlugin('#preloaderRole', 'dialog');
    dialogPreloader.open();
    $.ajax({
        method: "POST",
        url: window.$ReturnDefaultRole,
        data: { roleId: idRole },
        success: function (result) {
            $("#PermissionTree").children().remove();
            $("#PermissionTree").append(result);
            dialogPreloader.close();
        }
    });
}

function ChangeUser() {
    $changesMade = false;
    var dialogUser = Metro.getPlugin('#UserChanges', 'dialog');
    dialogUser.close();
    $("#UserName").text(CompleteName);
    var dialogPreloader = Metro.getPlugin('#preloaderPermission', 'dialog');
    dialogPreloader.open();
    $.ajax({
        method: "POST",
        url: window.$LoadTreeByUser,
        data: { userId: userSelected },
        success: function (result) {
            $("#PermissionTree").children().remove();
            $("#PermissionTree").append(result);
            dialogPreloader.close();
        }
    });
    $("#dttRolUsuario tbody").children("#" + userSelected).click();
    lastUser = userSelected;
}

function CancelChange() {
    var dialogChanges = Metro.getPlugin('#UserChanges', 'dialog');
    dialogChanges.close();
    $("#dttRolUsuario tbody").children("#" + userSelected).click();
    userSelected = lastUser;
}

