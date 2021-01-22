$(document).ready(function () {

    $('#dttFindingTypes').DataTable({
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

function FindingTypeDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadFindingType,
        data: { findingTypeId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#FindingTypeDetail', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function AddFindingType() {

    $('#SaveFindingType').attr('disabled', true);
    $("#SaveFindingType").removeClass('button my-control-colors');
    $("#SaveFindingType").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveFindingType,
        cache: false,
        data: $("#FindingTypeForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("FindingType saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#FindingTypeDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveFindingType').attr('disabled', false);
            $("#SaveFindingType").addClass('button my-control-colors');
            $("#SaveFindingType").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteFindingType(findingTypeId) {
    var dialog = Metro.getPlugin('#DeleteFindingType', 'dialog');
    dialog.open();
    window.$findingTypeId = findingTypeId;
}

function DeleteFindingType() {

    $.ajax({
        method: "POST",
        url: window.$DeleteFindingType,
        data: { findingTypeId: window.$findingTypeId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("FindingType deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting findingType", null, null, "bg-red fg-white");
            }

        }
    });

}





