
function SaveTree() {

    if (!$("#Name").val()) {
        $("#feedback").css("display", "block");
        return;
    }

    var task = $(".taskList");
    var roleId = $("#PermissionTree").attr("name");
    var description = $("#Description").val();
    var name = $("#Name").val();
    var dialog = Metro.getPlugin('#preloaderEditRole', 'dialog');
    dialog.open();    
    $("#preloaderSave").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveTreePermissions + "?roleId=" + roleId + "&name=" + name + "&description=" + description,
        data: task.serialize(),
        success: function (result) {
            $("#preloaderSave").css("visibility", "hidden");
            var dialog = Metro.getPlugin('#preloaderEditRole', 'dialog');
            dialog.close();   
            if (result === "true") {
                window.location.replace(window.$RoleIndex);
            } else {
                Metro.toast.create("Changes could not be saved.", null, null, "bg-red fg-white");
            }

        }
    });

}