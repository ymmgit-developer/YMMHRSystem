var selectedRole;
$(document).ready(function () {
    //Tabla de Roles
    if ($.fn.dataTable.isDataTable("#dttRoles")) {
        var table = $("#dttRoles").DataTable();
        table.destroy();
    }
    $("#dttRoles").DataTable({   
        scrollY: "50vh",
        scrollCollapse: true,
        searching: true,
        info: false,
        ordering: false
    });
});

function DeleteRol() {
    $.ajax({
        method: "POST",
        url: window.$DeleteRole,
        data: { roleId: selectedRole },
        success: function (result) {
            if (result === "true") {
                CloseDeleteRolDialog();
                Metro.toast.create("Se eliminó el rol correctamente..", null, null, "bg-green fg-white");
                setTimeout(function() {
                    location.reload();
                }, 1000);
            } else {
                CloseDeleteRolDialog();
                Metro.toast.create("Ya existen usuarios asignados a este rol, no se puede eliminar..", null, null, "bg-red fg-white");
            }

        }
    });
}

function OpenDeleteRolDialog(id) {
    selectedRole = id;
    var dialog = Metro.getPlugin('#DeleteRol', 'dialog');
    dialog.open(); 
}

function CloseDeleteRolDialog() {
    var dialog = Metro.getPlugin('#DeleteRol', 'dialog');
    dialog.close(); 
}

