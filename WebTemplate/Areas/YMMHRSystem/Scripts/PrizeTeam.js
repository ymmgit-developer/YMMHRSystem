$(document).ready(function () {

    $('#dttPrizeTeam').DataTable({
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
            $("#PrizeTeamTable").show();
        }
    });

});

function OpenPrizeTeamDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$PrizeTeamDialog,
        data: { prizeTeamId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#NewTeamDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SavePrizeTeam() {

    $.ajax({
        method: "POST",
        url: window.$SavePrizeTeam,
        data: $('#TeamForm').serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Team added", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else if (result === "false") {
                Metro.toast.create("Error adding team", null, null, "bg-red fg-white");
            } else {
                window.location.href = result.redirectToUrl;
            }

        }
    });

}

function ConfirmDeletePrizeTeam(prizeTeamId) {
    var dialog = Metro.getPlugin('#DeletePrizeTeam', 'dialog');
    setTimeout(function () { dialog.open(); }, 100);
    window.$prizeTeamId = prizeTeamId;
}

function DeletePrizeTeam() {

    $.ajax({
        method: "POST",
        url: window.$DeletePrizeTeam,
        data: { prizeTeamId: window.$prizeTeamId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Team deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting Team", null, null, "bg-red fg-white");
            }

        }
    });

}

function OpenPrizeTeamProductivityDialog() {

    $.ajax({
        method: "POST",
        url: window.$PrizeTeamProductivityDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#ProductivityDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SavePrizeTeamProductivity() {
    $.ajax({
        method: "POST",
        url: window.$SavePrizeTeamProductivity,
        data: $('#ProductivityForm').serialize(),
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

function OpenPrizeTeamScrapDialog() {

    $.ajax({
        method: "POST",
        url: window.$PrizeTeamScrapDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#ScrapDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SavePrizeTeamScrap() {

    $.ajax({
        method: "POST",
        url: window.$SavePrizeTeamScrap,
        data: $('#ScrapForm').serialize(),
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

function OpenPrizeTeamDockAuditDialog() {

    $.ajax({
        method: "POST",
        url: window.$PrizeTeamDockAuditDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#DockAuditDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SavePrizeTeamDockAudit() {

    $.ajax({
        method: "POST",
        url: window.$SavePrizeTeamDockAudit,
        data: $('#DockAuditForm').serialize(),
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

function OpenPrizeTeamSecurityCrossDialog() {

    $.ajax({
        method: "POST",
        url: window.$PrizeTeamSecurityCrossDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#SecurityCrossDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SavePrizeTeamSecurityCross() {

    $.ajax({
        method: "POST",
        url: window.$SavePrizeTeamSecurityCross,
        data: $('#SecurityCrossForm').serialize(),
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

function OpenPrizeTeamAttendanceDialog() {

    $.ajax({
        method: "POST",
        url: window.$PrizeTeamAttendanceDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#AttendanceDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SavePrizeTeamAttendance() {

    $.ajax({
        method: "POST",
        url: window.$SavePrizeTeamAttendance,
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

function PrizeTeamPointLogDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$PrizeTeamPointLogDialog,
        data: { prizeTeamId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#PrizeTeamPointLogDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SelectAssociate(names, process) {
    $('#Associate').val(names);
    Metro.dialog.close('#AssociateImporter');
}

function SavePrizeTeamAssociate() {

    $.ajax({
        method: "POST",
        url: window.$SavePrizeTeamAssociate,
        data: $('#TeamAssociateForm').serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Associate added", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error adding associate", null, null, "bg-red fg-white");
            }

        }
    });

}

function OpenNewTeamAssociateDialog() {

    $.ajax({
        method: "POST",
        url: window.$NewTeamAssociateDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#NewTeamAssociateDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

