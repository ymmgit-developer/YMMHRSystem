$(document).ready(function () {

    $('#dttPrizeAssociates').DataTable({
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
        responsive: true,
        compact: true,
        searching: true,
        info: true,
        paging: true,
        ordering: false,
        scrollY: "50vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#PrizeAssociateTable").show();
        }
    });

});

function ConfirmDeletePrizeAssociate(prizeAssociateId) {
    var dialog = Metro.getPlugin('#DeletePrizeAssociate', 'dialog');
   setTimeout(function () { dialog.open(); }, 100);
    window.$prizeAssociateId = prizeAssociateId;
}

function DeletePrizeAssociate() {

    $.ajax({
        method: "POST",
        url: window.$DeletePrizeAssociate,
        data: { prizeAssociateId: window.$prizeAssociateId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Associate deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting Associate", null, null, "bg-red fg-white");
            }

        }
    });

}

function ImportWorkers() {
    Metro.dialog.open('#preloader');
    $.ajax({
        url: window.$ImportWorkers,
        method: "POST",
        success: function (result) {
            Metro.dialog.close('#preloader');
            if (result == false) {
                Metro.toast.create("Error, could not import associates", null, null, "bg-red fg-white");
            } else {
                Metro.toast.create("Workers imported", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            }
        },
        error: function (err) {
            Metro.dialog.close('#preloader');
            Metro.toast.create("Error, please contact IT administrator", null, null, "bg-red fg-white");
        }
    });
}

function OpenAssociateMonthDialog() {

    $.ajax({
        method: "POST",
        url: window.$AssociateMonthDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#AssociateMonthDialog', 'dialog');
            dialog.setContent(result);
           setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SaveAssociateMonth() {
    $.ajax({
        method: "POST",
        url: window.$SaveAssociateMonth,
        data: $('#AssociateMonthForm').serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Points added", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error adding points", null, null, "bg-red fg-white");
            }

        }
    });

}

function OpenAssociateYearDialog() {

    $.ajax({
        method: "POST",
        url: window.$AssociateYearDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#AssociateYearDialog', 'dialog');
            dialog.setContent(result);
           setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SaveAssociateYear() {

    $.ajax({
        method: "POST",
        url: window.$SaveAssociateYear,
        data: $('#AssociateYearForm').serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Points added", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error adding points", null, null, "bg-red fg-white");
            }

        }
    });

}

function OpenFindingDialog() {

    $.ajax({
        method: "POST",
        url: window.$FindingDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#FindingDialog', 'dialog');
            dialog.setContent(result);
           setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SaveAssociateFinding() {

    $.ajax({
        method: "POST",
        url: window.$SaveAssociateFinding,
        data: $('#FindingForm').serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Points added", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error adding points", null, null, "bg-red fg-white");
            }

        }
    });

}

function OpenPrizeDialog() {

    $.ajax({
        method: "POST",
        url: window.$PrizeDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#PrizeDialog', 'dialog');
            dialog.setContent(result);
           setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SaveAssociatePrize() {

    $("#Prize").val($("#PrizeId option:selected").text());
    $.ajax({
        method: "POST",
        url: window.$SaveAssociatePrize,
        data: $('#PrizeForm').serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Points added", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error adding points", null, null, "bg-red fg-white");
            }

        }
    });

}

function OpenAttendanceDialog() {

    $.ajax({
        method: "POST",
        url: window.$AttendanceDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#AttendanceDialog', 'dialog');
            dialog.setContent(result);
           setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SaveAttendance() {

    $.ajax({
        method: "POST",
        url: window.$SaveAttendance,
        data: $('#AttendanceForm').serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Points added", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error adding points", null, null, "bg-red fg-white");
            }

        }
    });

}

function OpenPointConfigurationDialog() {

    $.ajax({
        method: "POST",
        url: window.$PointConfigurationDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#PointConfigurationDialog', 'dialog');
            dialog.setContent(result);
           setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SavePointConfiguration() {

    $.ajax({
        method: "POST",
        url: window.$SavePointConfiguration,
        data: $('#PointForm').serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Points saved", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error saving points", null, null, "bg-red fg-white");
            }

        }
    });

}

function PrizeSelected() {
    $("#Quantity").val($("#PrizeId option:selected").prop("id"));
}

function PrizeAssociatePointLogDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$PrizeAssociatePointLogDialog,
        data: { prizeAssociateId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#PrizeAssociatePointLogDialog', 'dialog');
            dialog.setContent(result);
           setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AdjustmentDialog() {

    $.ajax({
        method: "POST",
        url: window.$AdjustmentDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#AdjustmentDialog', 'dialog');
            dialog.setContent(result);
           setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SaveAdjustment() {

    Metro.dialog.open('#preloader');

    $.ajax({
        method: "POST",
        url: window.$SaveAdjustment,
        data: $('#AdjustmentForm').serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Adjustment saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#preloader');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create(result, null, null, "bg-red fg-white");
            }

        }
    });

}






