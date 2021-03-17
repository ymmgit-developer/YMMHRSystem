$(document).ready(function () {

    $('#dttLegalProcedures').DataTable({
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

function LegalProcedureDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadLegalProcedure,
        data: { legalProcedureId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#LegalProcedureDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddLegalProcedure() {

    $('#SaveLegalProcedure').attr('disabled', true);
    $("#SaveLegalProcedure").removeClass('button my-control-colors');
    $("#SaveLegalProcedure").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveLegalProcedure,
        cache: false,
        data: $("#LegalProcedureForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Legal procedure saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#LegalProcedureDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveLegalProcedure').attr('disabled', false);
            $("#SaveLegalProcedure").addClass('button my-control-colors');
            $("#SaveLegalProcedure").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteLegalProcedure(legalProcedureId) {
    var dialog = Metro.getPlugin('#DeleteLegalProcedure', 'dialog');
    dialog.open();
    window.$legalProcedureId = legalProcedureId;
}

function DeleteLegalProcedure() {

    $.ajax({
        method: "POST",
        url: window.$DeleteLegalProcedure,
        data: { legalProcedureId: window.$legalProcedureId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Legal procedure deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting legal procedure", null, null, "bg-red fg-white");
            }

        }
    });

}





