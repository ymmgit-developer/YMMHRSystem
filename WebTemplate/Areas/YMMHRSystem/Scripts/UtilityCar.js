$(document).ready(function () {

    $('#dttUtilityCars').DataTable({
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

function UtilityCarDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadUtilityCar,
        data: { utilityCarId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#UtilityCarDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddUtilityCar() {

    Metro.dialog.open('#preloaderUtilityCar');
    $("#UtilityCar").val($("#Model option:selected").text());
    $.ajax({
        method: "POST",
        url: window.$SaveUtilityCar,
        cache: false,
        data: $("#UtilityCarForm").serialize(),
        success: function (result) {
            Metro.dialog.close('#preloaderUtilityCar');
            if (result !== "false") {
                Metro.toast.create("Utility car loan saved.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    window.location.replace(window.$UtilityCarIndex);
                }, 1000);
            } else {
                Metro.toast.create("Departure - License Expiration dates are not correct / No available cars at the moment", null, null, "bg-red fg-white");
            }
        }
    });

}

function ConfirmDeleteUtilityCar(utilityCarId) {
    var dialog = Metro.getPlugin('#DeleteUtilityCar', 'dialog');
    dialog.open();
    window.$utilityCarId = utilityCarId;
}

function DeleteUtilityCar() {

    $.ajax({
        method: "POST",

        url: window.$DeleteUtilityCar,
        data: { utilityCarId: window.$utilityCarId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Utility car loan deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting utility car loan", null, null, "bg-red fg-white");
            }

        }
    });

}

function StartUtilityCar() {
    if ($("#Model").val() == "" || $("#LicensePlate").val() == "") {
        Metro.toast.create("Please select a vehicle", null, null, "bg-red fg-white");
    } else {
        Metro.dialog.open('#preloaderUtilityCar');
        $.ajax({
            method: "POST",
            url: window.$StartUtilityCar,
            cache: false,
            data: $("#UtilityCarForm").serialize(),
            success: function (result) {
                Metro.dialog.close('#preloaderUtilityCar');
                if (result === "true") {
                    Metro.toast.create("Utility car loan started.", null, null, "bg-green fg-white");
                    setTimeout(function () {
                        window.location.replace(window.$UtilityCarIndex);
                    }, 1000);
                } else {
                    Metro.toast.create("Utility car loan could not be started.", null, null, "bg-red fg-white");
                }

            }
        });
    }

}

function EndUtilityCar() {
    
    let arrivalParts = $("#ArrivalDate").val().split('-');
    let arrivalDate = new Date(arrivalParts[2], arrivalParts[1] - 1, arrivalParts[0]); // YYYY, MM, DD

    let today = new Date();
    // Normalizamos la fecha de hoy (sin horas)
    today.setHours(0, 0, 0, 0);
    arrivalDate.setHours(0, 0, 0, 0);
    if (arrivalDate > today) { // Validamos que la fecha de llegada no sea mayor a la fecha actual
        Metro.toast.create("Arrival date is higher than the current date, update the “Arrival Date” parameter to match the closing date.", null, 6000, "bg-red fg-white");
    } else {
        Metro.dialog.open('#preloaderUtilityCar');

        $.ajax({
            method: "POST",
            url: window.$EndUtilityCar,
            cache: false,
            data: $("#UtilityCarForm").serialize(),
            success: function (result) {
                Metro.dialog.close('#preloaderUtilityCar');
                if (result === "true") {
                    Metro.toast.create("Utility car loan ended.", null, 2000, "bg-green fg-white");
                    setTimeout(function () {
                        window.location.replace(window.$UtilityCarIndex);
                    }, 1000);
                } else {
                    Metro.toast.create("Utility car loan could not be ended.", null, 2000, "bg-red fg-white");
                }
            }
        });
    }
}

function ModelSelected() {
    $("#LicensePlate").val($("#Model option:selected").prop("id"));
}






