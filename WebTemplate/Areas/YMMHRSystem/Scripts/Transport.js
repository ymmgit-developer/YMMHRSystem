$(document).ready(function () {

    $('#dttTransports').DataTable({
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5],
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5],
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5],
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


    $('#dttExtraordinaryTransports').DataTable({
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5],
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5],
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5],
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

function TransportDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadTransport,
        data: { transportId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#TransportDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddTransport() {

    $('#SaveTransport').attr('disabled', true);
    $("#SaveTransport").removeClass('button my-control-colors');
    $("#SaveTransport").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $("#Route").val($("#RouteSelect option:selected").text());
    $.ajax({
        method: "POST",
        url: window.$SaveTransport,
        cache: false,
        data: $("#TransportForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Transport saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#TransportDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveTransport').attr('disabled', false);
            $("#SaveTransport").addClass('button my-control-colors');
            $("#SaveTransport").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteTransport(transportId) {
    var dialog = Metro.getPlugin('#DeleteTransport', 'dialog');
    dialog.open();
    window.$transportId = transportId;
}

function DeleteTransport() {


    $.ajax({
        method: "POST",
        url: window.$DeleteTransport,
        data: { transportId: window.$transportId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Transport deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting transport", null, null, "bg-red fg-white");
            }

        }
    });

}

function ExtraordinaryTransportDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadExtraordinaryTransport,
        data: { extraordinaryTransportId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#ExtraordinaryTransportDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddExtraordinaryTransport() {

    $('#SaveTransport').attr('disabled', true);
    $("#SaveTransport").removeClass('button my-control-colors');
    $("#SaveTransport").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $("#Route").val($("#RouteSelect option:selected").text());

    $.ajax({
        method: "POST",
        url: window.$SaveExtraordinaryTransport,
        cache: false,
        data: $("#ExtraTransportForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Transport saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#ExtraordinaryTransportDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveTransport').attr('disabled', false);
            $("#SaveTransport").addClass('button my-control-colors');
            $("#SaveTransport").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteExtraordinaryTransport(transportId) {
    var dialog = Metro.getPlugin('#DeleteExtraordinaryTransport', 'dialog');
    dialog.open();
    window.$transportId = transportId;
}

function DeleteExtraordinaryTransport() {


    $.ajax({
        method: "POST",
        url: window.$DeleteExtraordinaryTransport,
        data: { extraordinaryTransportId: window.$transportId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Transport deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting transport", null, null, "bg-red fg-white");
            }

        }
    });

}

function SelectAssociate(names, process){
    $('#AssociateName').val(names);
    $('#Process').val(process);
    Metro.dialog.close('#AssociateImporter');
}






