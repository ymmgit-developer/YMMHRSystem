$(document).ready(function () {
    $('#dttBusinessTrips').DataTable({
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4],
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4],
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4],
                }
            },
        ],
        dom: 'Bfrtip',
        deferRender: true,
        responsive: true,
        compact: true,
        searching: true,
        info: true,
        paging: true,
        ordering: false,
        scrollY: "50vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#BusinessTripTable").show();
        }
    });
});

function SaveBusinessTrip() {
    var data = $('#FormDetail').find('select, input').serialize();

    var dialog = Metro.getPlugin('#preloaderBusinessTrip', 'dialog');
    dialog.open();

    $.ajax({
        method: "POST",
        url: window.$SaveBusinessTrip,
        cache: false,
        data: data,
        success: function (result) {
            dialog.close();
            if (result === "true") {
                Metro.toast.create("Business Trip saved.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                 Metro.toast.create("Please save your Business trip / Business trip is complete.", null, null, "bg-red fg-white");
            }

        }
    });
}

function ConfirmDeleteBusinessTrip(businessTripId) {
    var dialog = Metro.getPlugin('#DeleteBusinessTrip', 'dialog');
    dialog.open();
    window.$businessTripId = businessTripId;
}

function DeleteBusinessTrip() {

    $.ajax({
        method: "POST",
        url: window.$DeleteBusinessTrip,
        data: { businessTripId: window.$businessTripId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Business Trip deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting business trip", null, null, "bg-red fg-white");
            }

        }
    });

}

function OpenAttachmentDialog() {

    $.ajax({
        method: "POST",
        url: window.$AttachmentDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#AttachmentDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });

}

function AddAttachment() {
    var formdata = new FormData($('#BusinessTripAttachmentForm').get(0));
    $.ajax({
        method: "POST",
        url: window.$AddAttachment,
        data: formdata,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Attachment added", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error: Please attach PDF file only", null, null, "bg-red fg-white");
            }

        }
    });

}

function OpenPassengerDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$PassengerDialog,
        data: { passengerDetailId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#PassengerDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddPassenger() {

    $('#SavePassenger').attr('disabled', true);
    $("#SavePassenger").removeClass('button my-control-colors');
    $("#SavePassenger").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$AddPassenger,
        cache: false,
        data: $("#PassengerForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Passenger saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#PassengerDialog');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                 Metro.toast.create("Please save your Business trip / Business trip is complete.", null, null, "bg-red fg-white");
            }

            $('#SavePassenger').attr('disabled', false);
            $("#SavePassenger").addClass('button my-control-colors');
            $("#SavePassenger").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function OpenTaxiDialog(id) {
    $.ajax({
        method: "POST",
        url: window.$TaxiDialog,
        data: { taxiReservationId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#TaxiDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddTaxi() {

    $('#SaveTaxi').attr('disabled', true);
    $("#SaveTaxi").removeClass('button my-control-colors');
    $("#SaveTaxi").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$AddTaxi,
        cache: false,
        data: $("#TaxiForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Taxi saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#TaxiDialog');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                 Metro.toast.create("Please save your Business trip / Business trip is complete.", null, null, "bg-red fg-white");
            }

            $('#SaveTaxi').attr('disabled', false);
            $("#SaveTaxi").addClass('button my-control-colors');
            $("#SaveTaxi").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function OpenHotelDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$HotelDialog,
        data: { hotelReservationId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#HotelDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddHotel() {

    $('#SaveHotel').attr('disabled', true);
    $("#SaveHotel").removeClass('button my-control-colors');
    $("#SaveHotel").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$AddHotel,
        cache: false,
        data: $("#HotelForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Hotel saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#HotelDialog');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                 Metro.toast.create("Please save your Business trip / Business trip is complete.", null, null, "bg-red fg-white");
            }

            $('#SaveHotel').attr('disabled', false);
            $("#SaveHotel").addClass('button my-control-colors');
            $("#SaveHotel").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function OpenFlightDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$FlightDialog,
        data: { flightReservationId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#FlightDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddFlight() {

    $('#SaveFlight').attr('disabled', true);
    $("#SaveFlight").removeClass('button my-control-colors');
    $("#SaveFlight").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$AddFlight,
        cache: false,
        data: $("#FlightForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Flight saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#FlightDialog');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                 Metro.toast.create("Please save your Business trip / Business trip is complete.", null, null, "bg-red fg-white");
            }

            $('#SaveFlight').attr('disabled', false);
            $("#SaveFlight").addClass('button my-control-colors');
            $("#SaveFlight").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function OpenCarRentalDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$CarRentalDialog,
        data: { carRentalReservationId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#CarRentalDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddCarRental() {

    $('#SaveCarRental').attr('disabled', true);
    $("#SaveCarRental").removeClass('button my-control-colors');
    $("#SaveCarRental").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$AddCarRental,
        cache: false,
        data: $("#CarRentalForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Car Rental saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#CarRentalDialog');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                 Metro.toast.create("Please save your Business trip / Business trip is complete.", null, null, "bg-red fg-white");
            }

            $('#SaveCarRental').attr('disabled', false);
            $("#SaveCarRental").addClass('button my-control-colors');
            $("#SaveCarRental").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function RefreshTables() {
    $('#dttTaxiResponse').DataTable({
        retrieve: true,
        responsive: true,
        searching: false,
        info: false,
        paging: false,
        ordering: false,
        scrollY: "20vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#TaxiResponseTable").show();
        }
    }).draw();

    $('#dttHotelResponse').DataTable({
        retrieve: true,
        responsive: true,
        searching: false,
        info: false,
        paging: false,
        ordering: false,
        scrollY: "20vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#HotelResponseTable").show();
        }
    }).draw();

    $('#dttFlightResponse').DataTable({
        retrieve: true,
        responsive: true,
        searching: false,
        info: false,
        paging: false,
        ordering: false,
        scrollY: "20vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#FlightResponseTable").show();
        }
    }).draw();

    $('#dttCarRentalResponse').DataTable({
        retrieve: true,
        responsive: true,
        searching: false,
        info: false,
        paging: false,
        ordering: false,
        scrollY: "20vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#CarRentalResponseTable").show();
        }
    }).draw();
}

function ConfirmRespondBusinessTrip() {
    var dialog = Metro.getPlugin('#RespondBusinessTrip', 'dialog');
    dialog.open();
}

function RespondBusinessTrip() {

    var data = $('#FormDetail').find('select, input').serialize();

    $.ajax({
        method: "POST",
        url: window.$RespondBusinessTrip,
        cache: false,
        data: data, 
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Business Trip responded.", null, null, "bg-green fg-white");
                Metro.dialog.close('#RespondBusinessTrip');
                setTimeout(function () {
                    location.href = $BusinessTripIndex;
                }, 1000);
            } else {
                Metro.toast.create("Business trip could not be responded.", null, null, "bg-red fg-white");
            }
        }
    });
}

