$(document).ready(function () {

    $('#dttVehicles').DataTable({
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

function VehicleDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadVehicle,
        data: { vehicleId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#VehicleDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddVehicle() {

    $('#SaveVehicle').attr('disabled', true);
    $("#SaveVehicle").removeClass('button my-control-colors');
    $("#SaveVehicle").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveVehicle,
        cache: false,
        data: $("#VehicleForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Vehicle saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#VehicleDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveVehicle').attr('disabled', false);
            $("#SaveVehicle").addClass('button my-control-colors');
            $("#SaveVehicle").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteVehicle(vehicleId) {
    var dialog = Metro.getPlugin('#DeleteVehicle', 'dialog');
    dialog.open();
    window.$vehicleId = vehicleId;
}

function DeleteVehicle() {

    $.ajax({
        method: "POST",
        url: window.$DeleteVehicle,
        data: { vehicleId: window.$vehicleId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Vehicle deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting vehicle", null, null, "bg-red fg-white");
            }

        }
    });

}

function TypeSelected() {
    if ($("#Type option:selected").val() == "Loan") {
        $('#Import').attr('disabled', false);
        $("#Associate").parent().addClass('required');
        $("#Associate").attr('data-validate','required');
    } else {
        $('#Import').attr('disabled', true);
        $("#Associate").parent().removeClass('required');
        $("#Associate").removeAttr('data-validate');
        $("#Associate").val("");
    }
}

function SelectAssociate(names, process) {
    $('#Associate').val(names);
    Metro.dialog.close('#AssociateImporter');
}





