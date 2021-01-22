$(document).ready(function () {

    $('#dttProcesses').DataTable({
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

function ProcessDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadProcess,
        data: { processId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#ProcessDetail', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function AddProcess() {

    $('#SaveProcess').attr('disabled', true);
    $("#SaveProcess").removeClass('button my-control-colors');
    $("#SaveProcess").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveProcess,
        cache: false,
        data: $("#ProcessForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Process saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#ProcessDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveProcess').attr('disabled', false);
            $("#SaveProcess").addClass('button my-control-colors');
            $("#SaveProcess").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteProcess(processId) {
    var dialog = Metro.getPlugin('#DeleteProcess', 'dialog');
    dialog.open();
    window.$processId = processId;
}

function DeleteProcess() {

    $.ajax({
        method: "POST",
        url: window.$DeleteProcess,
        data: { processId: window.$processId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Process deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting process", null, null, "bg-red fg-white");
            }

        }
    });

}





