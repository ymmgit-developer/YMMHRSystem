$(document).ready(function () {

    $('#dttLegalRequirements').DataTable({
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

function LegalRequirementDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadLegalRequirement,
        data: { legalRequirementId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#LegalRequirementDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddLegalRequirement() {

    $('#SaveLegalRequirement').attr('disabled', true);
    $("#SaveLegalRequirement").removeClass('button my-control-colors');
    $("#SaveLegalRequirement").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveLegalRequirement,
        cache: false,
        data: $("#LegalRequirementForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Legal requirement saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#LegalRequirementDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveLegalRequirement').attr('disabled', false);
            $("#SaveLegalRequirement").addClass('button my-control-colors');
            $("#SaveLegalRequirement").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteLegalRequirement(legalRequirementId) {
    var dialog = Metro.getPlugin('#DeleteLegalRequirement', 'dialog');
    dialog.open();
    window.$legalRequirementId = legalRequirementId;
}

function DeleteLegalRequirement() {

    $.ajax({
        method: "POST",

        url: window.$DeleteLegalRequirement,
        data: { legalRequirementId: window.$legalRequirementId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("LegalRequirement deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting legalRequirement", null, null, "bg-red fg-white");
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

function OpenAttachmentListDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$AttachmentListDialog,
        data: { legalRequirementId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#AttachmentListDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddAttachment() {
    var formdata = new FormData($('#LegalRequirementAttachmentForm').get(0));
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





