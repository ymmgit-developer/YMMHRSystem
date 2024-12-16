$(document).ready(function () {
    $('#dttFirstApprovers').DataTable({
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4],
                },
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
            }
        ],
        dom: 'Bfrtip',
        responsive: true,
        searching: true,
        info: true,
        paging: true,
        ordering: false,
        order: [[6, 'desc']],
        scrollY: "50vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#FirstApproversTable").show();
        }
    });
});

$(document).ready(function () {
    $('#dttSecondApprovers').DataTable({
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4],
                },
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
            }
        ],
        dom: 'Bfrtip',
        responsive: true,
        searching: true,
        info: true,
        paging: true,
        ordering: false,
        order: [[6, 'desc']],
        scrollY: "50vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#SecondApproversTable").show();
        }
    });
});

function FirstApproverAddDialog()
{
    $.ajax({
        method: "POST",
        url: window.$AddFirstApprover,
        success: function (result) {
            var dialog = Metro.getPlugin('#FirstApproversAddDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SecondApproverAddDialog()
{
    $.ajax({
        method: "POST",
        url: window.$AddSecondApprover,
        success: function (result) {
            var dialog = Metro.getPlugin('#SecondApproversAddDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddRegister() {
    $("#Preloader").css("visibility", "visible");
    $.ajax({
        method: "POST",
        url: window.$SaveFirstApproverRelation,
        cache: false,
        data: $("#FirstApproverForm").serialize(),
        success: function (result) {
            if (result) {
                Metro.toast.create("Successful registration.", null, null, "bg-green fg-white");
                Metro.dialog.close('#FirstApproversAddDialog');
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
}

function AddRegisterSecond()
{
    $("#Preloader").css("visibility", "visible");
    $.ajax({
        method: "POST",
        url: window.$SaveSecondApproverRelation,
        cache: false,
        data: $("#SecondApproverForm").serialize(),
        success: function (result) {
            if (result) {
                Metro.toast.create("Successful registration.", null, null, "bg-green fg-white");
                Metro.dialog.close('#SecondApproversAddDialog');
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
}

function ConfirmDeleteRegister(Idnotification) {
    var dialog = Metro.getPlugin('#DeleteRegisterApprovers', 'dialog');
    dialog.open();
    window.$IdNotification = Idnotification;
}

function ConfirmDeleteSecondRegister(Idnotification) {
    var dialog = Metro.getPlugin('#DeleteRegisterSecondApprovers', 'dialog');
    dialog.open();
    window.$SecondIdNotification = Idnotification;
}

function DeleteRegisterApprovers() {
    $.ajax({
        method: "POST",
        url: window.$DeleteFirstApproverRelation,
        data: { IdNotification: window.$IdNotification },
        success: function (result) {
            if (result = true) {
                Metro.toast.create("Record deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting record", null, null, "bg-red fg-white");
            }
        }
    });
}

function DeleteRegisterSecondApprovers() {
    $.ajax({
        method: "POST",
        url: window.$DeleteSecondApproverRelation,
        data: { IdNotification: window.$SecondIdNotification },
        success: function (result) {
            if (result = true) {
                Metro.toast.create("Record deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting record", null, null, "bg-red fg-white");
            }
        }
    });
}