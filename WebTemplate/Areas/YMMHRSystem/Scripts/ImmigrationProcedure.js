$(document).ready(function () {

    $('#dttImmigrationProcedures').DataTable({
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3],
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3],
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3],
                }
            },
        ],
        dom: 'Bfrtip',
        responsive: true,
        searching: true,
        info: true,
        paging: true,
        ordering: false,
        scrollY: "50vh",
        scrollCollapse: true,
    });

});

function ImmigrationProcedureDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadImmigrationProcedure,
        data: { immigrationProcedureId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#ImmigrationProcedureDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () {
                dialog.open();
            }, 100);
        }
    });
}

function AddImmigrationProcedure() {

    $('#SaveImmigrationProcedure').attr('disabled', true);
    $("#SaveImmigrationProcedure").removeClass('button my-control-colors');
    $("#SaveImmigrationProcedure").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveImmigrationProcedure,
        cache: false,
        data: $("#ImmigrationProcedureForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("ImmigrationProcedure saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#ImmigrationProcedureDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveImmigrationProcedure').attr('disabled', false);
            $("#SaveImmigrationProcedure").addClass('button my-control-colors');
            $("#SaveImmigrationProcedure").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteImmigrationProcedure(immigrationProcedureId) {
    var dialog = Metro.getPlugin('#DeleteImmigrationProcedure', 'dialog');
    dialog.open();
    window.$immigrationProcedureId = immigrationProcedureId;
}

function DeleteImmigrationProcedure() {

    $.ajax({
        method: "POST",
        url: window.$DeleteImmigrationProcedure,
        data: { immigrationProcedureId: window.$immigrationProcedureId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("ImmigrationProcedure deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting immigrationProcedure", null, null, "bg-red fg-white");
            }

        }
    });

}

function SelectAssociate(names, process) {
    $('#Associate').val(names);
    $('#Process').val(process);
    Metro.dialog.close('#AssociateImporter');
}

function ApproveImmigrationProcedure() {

    $('#ApproveImmigrationProcedure').attr('disabled', true);
    $("#ApproveImmigrationProcedure").removeClass('bg-green fg-white');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$ApproveImmigrationProcedure,
        cache: false,
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Immigration procedure approved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#ImmigrationProcedureDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Immigration procedure could not be approved.", null, null, "bg-red fg-white");
            }

            $('#ApproveImmigrationProcedure').attr('disabled', false);
            $("#ApproveImmigrationProcedure").addClass('bg-green fg-white');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function RejectImmigrationProcedure() {

    var rejectionMotive = $("#RejectionMotive").val();
    if (rejectionMotive === "") {
        Metro.toast.create("Please enter a rejection motive.", null, null, "bg-red fg-white");
    } else {

        $('#RejectImmigrationProcedure').attr('disabled', true);
        $("#RejectImmigrationProcedure").removeClass('bg-red fg-white');
        $("#Preloader").css("visibility", "visible");

        $.ajax({
            method: "POST",
            url: window.$RejectImmigrationProcedure,
            cache: false,
            data: { rejectionMotive: rejectionMotive },
            success: function (result) {
                if (result === "true") {
                    Metro.toast.create("Immigration procedure rejected.", null, null, "bg-green fg-white");
                    Metro.dialog.close('#ImmigrationProcedureDetail');
                    setTimeout(function () {
                        location.reload();
                    }, 1000);
                } else {
                    Metro.toast.create("Immigration procedure could not be rejected.", null, null, "bg-red fg-white");
                }

                $('#RejectImmigrationProcedure').attr('disabled', false);
                $("#RejectImmigrationProcedure").addClass('bg-red fg-white');
                $("#Preloader").css("visibility", "hidden");

            }
        });
    }
}




