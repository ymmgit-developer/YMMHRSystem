$(document).ready(function () {

    $('#dttStops').DataTable({
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

function StopDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadStop,
        data: { stopId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#StopDetail', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function AddStop() {

    $('#SaveStop').attr('disabled', true);
    $("#SaveStop").removeClass('button my-control-colors');
    $("#SaveStop").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveStop,
        cache: false,
        data: $("#StopForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Stop saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#StopDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveStop').attr('disabled', false);
            $("#SaveStop").addClass('button my-control-colors');
            $("#SaveStop").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteStop(stopId) {
    var dialog = Metro.getPlugin('#DeleteStop', 'dialog');
    dialog.open();
    window.$stopId = stopId;
}

function DeleteStop() {

    $.ajax({
        method: "POST",
        url: window.$DeleteStop,
        data: { stopId: window.$stopId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Stop deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting stop", null, null, "bg-red fg-white");
            }

        }
    });

}





