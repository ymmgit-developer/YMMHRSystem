$(document).ready(function () {

    $('#dttSindicates').DataTable({
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

function SindicateDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadSindicate,
        data: { sindicateId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#SindicateDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddSindicate() {

    $('#SaveSindicate').attr('disabled', true);
    $("#SaveSindicate").removeClass('button my-control-colors');
    $("#SaveSindicate").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveSindicate,
        cache: false,
        data: $("#SindicateForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Sindicate saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#SindicateDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveSindicate').attr('disabled', false);
            $("#SaveSindicate").addClass('button my-control-colors');
            $("#SaveSindicate").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteSindicate(sindicateId) {
    var dialog = Metro.getPlugin('#DeleteSindicate', 'dialog');
    setTimeout(function () { dialog.open(); }, 100);
    window.$sindicateId = sindicateId;
}

function DeleteSindicate() {

    $.ajax({
        method: "POST",

        url: window.$DeleteSindicate,
        data: { sindicateId: window.$sindicateId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Sindicate deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting sindicate", null, null, "bg-red fg-white");
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
        data: {sindicateId: id},
        success: function (result) {
            var dialog = Metro.getPlugin('#AttachmentListDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddAttachment() {
    var formdata = new FormData($('#SindicateAttachmentForm').get(0));
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

function OpenContractDialog() {

    $.ajax({
        method: "POST",
        url: window.$ContractDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#ContractDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function OpenContractListDialog() {

    $.ajax({
        method: "POST",
        url: window.$ContractListDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#ContractListDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddContract() {
    var formdata = new FormData($('#SindicateContractForm').get(0));
    $.ajax({
        method: "POST",
        url: window.$AddContract,
        data: formdata,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Contract added", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error: Please attach PDF file only", null, null, "bg-red fg-white");
            }

        }
    });

}





