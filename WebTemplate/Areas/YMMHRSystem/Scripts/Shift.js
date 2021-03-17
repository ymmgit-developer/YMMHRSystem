$(document).ready(function () {

    $('#dttShifts').DataTable({
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

function ShiftDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadShift,
        data: { shiftId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#ShiftDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddShift() {

    $('#SaveShift').attr('disabled', true);
    $("#SaveShift").removeClass('button my-control-colors');
    $("#SaveShift").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveShift,
        cache: false,
        data: $("#ShiftForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Shift saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#ShiftDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveShift').attr('disabled', false);
            $("#SaveShift").addClass('button my-control-colors');
            $("#SaveShift").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteShift(shiftId) {
    var dialog = Metro.getPlugin('#DeleteShift', 'dialog');
    dialog.open();
    window.$shiftId = shiftId;
}

function DeleteShift() {

    $.ajax({
        method: "POST",
        url: window.$DeleteShift,
        data: { shiftId: window.$shiftId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Shift deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting shift", null, null, "bg-red fg-white");
            }

        }
    });

}





