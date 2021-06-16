$(document).ready(function () {

    $('#dttRoutes').DataTable({
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

function SaveRoute() {
    if ($("#GetName").val() !== "") {
        $("#Cost").val($("#GetCost").val())
        $("#Name").val($("#GetName").val())

        var dialog = Metro.getPlugin('#preloaderRoute', 'dialog');
        dialog.open();

        $.ajax({
            method: "POST",
            url: window.$SaveRoute,
            cache: false,
            data: $(".route").serialize(),
            success: function (result) {
                if (result === "true") {
                    Metro.toast.create("Route saved.", null, null, "bg-green fg-white");
                } else {
                    Metro.dialog.open('#Error')
                }
                Metro.dialog.close('#preloaderRoute');
            }
        });
    } else {
        Metro.toast.create("Please enter a Name.", null, null, "bg-red fg-white");
    }
}

function AddStop(id) {
    var same;
    var RouteId = id;
    var stopId = $("#StopName option:selected").val();
    var stopName = $("#StopName option:selected").text();
    var reference = $("#Reference").val();

    $("#dttRouteStops td").each(function () {
        if ($(this).val() === reference) {
            same = "true";
        }
    });
    if (same === "true") {
        Metro.toast.create("Stop already exists.", null, null, "bg-red fg-white");
    } else {

        var node = table.row.add([stopName, reference, "<a class='button small bg-red fg-white' style='cursor: pointer;' onclick='ConfirmDeleteRouteStop(" + RouteId + "," + stopId + ",\"" + reference + "\")'><span class='mif-bin'></span><span class='actionButton'> Delete</span></a>"]).draw(false).node();
        $(node).css('text-align', 'center');
        $("#dttRouteStops").find("tr").last().append("<input type='hidden' name='StopList[" + ($("#dttRouteStops").find("tr").length - 2) + "].RouteStopId' class='route' value=''>");
        $("#dttRouteStops").find("tr").last().append("<input type='hidden' name='StopList[" + ($("#dttRouteStops").find("tr").length - 2) + "].RouteId' class='route' value='" + RouteId + "'>");
        $("#dttRouteStops").find("tr").last().append("<input type='hidden' name='StopList[" + ($("#dttRouteStops").find("tr").length - 2) + "].StopId' class='route' value='" + stopId + "'>");
        $("#dttRouteStops").find("tr").last().append("<input type='hidden' name='StopList[" + ($("#dttRouteStops").find("tr").length - 2) + "].StopName' class='route' value='" + stopName + "'>");
        $("#dttRouteStops").find("tr").last().append("<input type='hidden' name='StopList[" + ($("#dttRouteStops").find("tr").length - 2) + "].Reference' class='route' value='" + reference + "'>");
        Metro.toast.create("Stop added.", null, null, "bg-green fg-white");
    }
}

function ConfirmDeleteRoute(routeId) {
    var dialog = Metro.getPlugin('#DeleteRoute', 'dialog');
    dialog.open();
    window.$routeId = routeId;
}

function DeleteRoute() {

    $.ajax({
        method: "POST",
        url: window.$DeleteRoute,
        data: { routeId: window.$routeId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Route deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting route", null, null, "bg-red fg-white");
            }

        }
    });

}

function StopSelected() {

    $.ajax({
        method: "POST",
        url: window.$GetReference,
        data: { stopId: $("#StopName option:selected").val() },
        success: function (result) {
            if (result !== "false") {
                $("#Reference").val(result);
            }
        }
    });
}





