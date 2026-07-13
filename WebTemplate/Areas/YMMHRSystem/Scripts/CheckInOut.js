$(document).ready(function () {
    $('#dttCheckIO').DataTable({
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
        order:[[6,'desc']],
        scrollY: "50vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#CheckInOutTable").show();
        }
    });
});

function CheckIODetailAddDialog() {
    $.ajax({
        method: "POST",
        url: window.$AddRecordCheckInOut,
        success: function (result) {
            var dialog = Metro.getPlugin("#CheckInOutAddDialog", "dialog");
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}




$(document).ready(function () {

    table = $('#dttAssociateList').DataTable({
        responsive: true,
        retrieve: true,
        searching: false,
        info: false,
        compact: true,
        paging: false,
        ordering: false,
        scrollY: "33vh",
        scrollCollapse: true,
    });
});

function RemoveAssociate(names) {
    var aux;
    $("#dttAssociateList td").each(function () {
        if ($(this).text() === names) {
            aux = $(this).parents('tr');
        }
    });
    table.row(aux).remove().draw();
    Metro.toast.create("Associate removed.", null, null, "bg-green fg-white");
}

function AddRegister() {
    var $button = $("#SaveCheckInOut");

    if ($button.data("busy") === true) {
        return;
    }

    $button.data("busy", true);
    $button.prop("disabled", true);
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveRecordCheckInOut,
        cache: false,
        data: $("#CheckInOutForm").serialize(),
        success: function (result) {
            if (result.success === true) {
                Metro.toast.create(result.message || "Successful registration.", null, null, "bg-green fg-white");
                Metro.dialog.close('#CheckInOutAddDialog');
                setTimeout(function () {
                    location.reload();
                }, 1500);
            }
            else {
                $("#Error .dialog-content").text(result.message || "The access request could not be saved.");
                Metro.dialog.open('#Error');
            }
        },
        error: function () {
            $("#Error .dialog-content").text("The access request could not be saved.");
            Metro.dialog.open('#Error');
        },
        complete: function () {
            $button.data("busy", false);
            $button.prop("disabled", false);
            $("#Preloader").css("visibility", "hidden");
        }
    });
}

function UpdateCheckInOut()
{
    $("#Preloader").css("visibility", "visible");
    $.ajax({
        method: "POST",
        url: window.$UpdateRecordCheckInOut,
        cache: false,
        data: $("#CheckInOutForm").serialize(),
        success: function (result) {
            if (result) {
                Metro.toast.create("Successful Update.", null, null, "bg-green fg-white");
                Metro.dialog.close('#CheckInOutDetailDialog');
                setTimeout(function () {
                    location.reload();
                }, 1500);
            }
            else {
                $("#Error .dialog-content").html("<p>" + result.message + "</p>");
                Metro.dialog.open('#Error');
            }
        }
    });
    console.log("UpdateCheckInOut");
}

function RecordCheckInOutDetailDialog(IdRecordsInOut)
{
    $.ajax({
        method: "POST",
        url: window.$DetailRecordCheckInOut,
        cache: false,
        data: { IdRecordsInOut: IdRecordsInOut },
        success: function (result) {
            var dialog = Metro.getPlugin("#CheckInOutDetailDialog", "dialog");
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);  
        }
    });
}

function ConfirmDeleteRecordCheckInOut(IdRecordsInOut)
{
    var dialog = Metro.getPlugin('#CancelCheckInOut', 'dialog');
    dialog.open();
    window.$IdRecordsInOut = IdRecordsInOut;
}

function CancelCheckInOut() {
    $.ajax({
        method: "POST",
        url: window.$DeleteRecordCheckInOut,
        data: { IdRecordsInOut: window.$IdRecordsInOut },
        success: function (result) {
            if (result == true) {
                Metro.toast.create("Register cancelled.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error cancelling registration", null, null, "bg-red fg-white");
            }

        }
    });
}

function RecordApproveCheckInOut(IdRecordsInOut, approvalLevel, button) {

    var $button = $(button);

    if ($button.data("busy") === true) {
        return;
    }

    $button.data("busy", true);
    $button.addClass("disabled");
    $button.css("pointer-events", "none");

    $.ajax({
        method: "POST",
        url: window.$ApprovedRecordCheckInOut,
        data: {
            IdRecordsInOut: IdRecordsInOut,
            approvalLevel: approvalLevel
        },
        success: function (result) {
            console.log(result);

            if (result.success === true) {
                Metro.toast.create(result.message || "Register approved.", null, null, "bg-green fg-white");

                setTimeout(function () {
                    location.reload();
                }, 1000);
            }
            else {
                Metro.toast.create(result.message || "Error approving registration.", null, null, "bg-red fg-white");

                $button.data("busy", false);
                $button.removeClass("disabled");
                $button.css("pointer-events", "auto");
            }
        },
        error: function () {
            Metro.toast.create("Error approving registration.", null, null, "bg-red fg-white");

            $button.data("busy", false);
            $button.removeClass("disabled");
            $button.css("pointer-events", "auto");
        }
    });
}
