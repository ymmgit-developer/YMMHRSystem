var table, tableAssociates;
$(document).ready(function () {

    $('#dttTransports').DataTable({
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6],
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6],
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6],
                }
            },
        ],
        dom: 'Bfrtip',
        responsive: true,
        searching: true,
        info: true,
        paging: true,
        ordering: false,
        scrollY: "60vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#TransportTable").show();
        }
    });

    $('#dttExtraordinaryTransports').DataTable({
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6],
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6],
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6],
                }
            },
        ],
        dom: 'Bfrtip',
        responsive: true,
        searching: true,
        info: true,
        paging: true,
        ordering: false,
        scrollY: "70vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#ExtraordinaryTransportTable").show();
        }
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
            if (result.success === true) {
                Metro.toast.create("Transport saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#TransportDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                showErrorDialog(result.message);
            }

            $('#SaveTransport').attr('disabled', false);
            $("#SaveTransport").addClass('button my-control-colors');
            $("#SaveTransport").addClass('button');
            $("#Preloader").css("visibility", "hidden");
        }
    });
}

function showErrorDialog(message) {
    var dialogContent = '<div class="dialog-title">Error</div>' +
        '<div class="dialog-content">' +
        '<p>' + message + '</p>' +
        '</div>' +
        '<div class="dialog-actions ml-auto">' +
        '<button class="button alert" onclick="Metro.dialog.close(\'#Error\')">OK</button>' +
        '</div>';
    console.log(message);
    $("#Error").html(dialogContent);
    Metro.dialog.open('#Error');
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

function ExtraordinaryTransportAddDialog() {

    $.ajax({
        method: "POST",
        url: window.$AddExtraordinaryTransport,
        success: function (result) {
            var dialog = Metro.getPlugin('#ExtraordinaryTransportAddDialog', 'dialog');
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

    $.ajax({
        method: "POST",
        url: window.$SaveExtraordinaryTransport,
        cache: false,
        data: $("#ExtraTransportForm").serialize(),
        success: function (result) {
            console.log(result);
            
            if (result === "true") {
                Metro.toast.create("Transport saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#ExtraordinaryTransportDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error');
                console.log("#####################");
            }

            $('#SaveTransport').attr('disabled', false);
            $("#SaveTransport").addClass('button my-control-colors');
            $("#SaveTransport").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmCancelExtraordinaryTransport(transportId) {
    var dialog = Metro.getPlugin('#CancelExtraordinaryTransport', 'dialog');
    dialog.open();
    window.$transportId = transportId;
}

function CancelExtraordinaryTransport() {


    $.ajax({
        method: "POST",
        url: window.$CancelExtraordinaryTransport,
        data: { extraordinaryTransportId: window.$transportId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Transport cancelled.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error cancelling transport", null, null, "bg-red fg-white");
            }

        }
    });

}

function SelectAssociates(names, process, id) {

    var same;
    $("#dttAssociateList td").each(function () {
        if ($(this).text() === names) {
            same = "true";
        }
    });
    if (same === "true") {
        Metro.toast.create("Associate already added.", null, null, "bg-red fg-white");
    } else {
        tableAssociates.row($('#'+id)).remove().draw();

        var node = table.row.add([names, process, '<a class="button small bg-red fg-white" style="cursor: pointer;" onclick="RemoveAssociate(\'' + names + '\')"><span class="mif-bin"></span></a>']).draw(false).node();
        $(node).css('text-align', 'center');
        $("#dttAssociateList").find("tr").last().append("<input type='hidden' name='WorkerList[" + ($("#dttAssociateList").find("tr").length - 2) + "].Names' value='" + names + "'>");
        $("#dttAssociateList").find("tr").last().append("<input type='hidden' name='WorkerList[" + ($("#dttAssociateList").find("tr").length - 2) + "].Process' value='" + process + "'>");
        Metro.toast.create("Associate added.", null, null, "bg-green fg-white");
    }
}

function SelectAssociate(names, process) {
    $('#AssociateName').val(names);
    $('#Process').val(process);
    Metro.dialog.close('#AssociateImporter');

}




