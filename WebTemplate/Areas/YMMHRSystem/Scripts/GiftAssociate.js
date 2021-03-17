$(document).ready(function () {

    $('#dttGiftAssociates').DataTable({
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
            $("#GiftAssociateTable").show();
        }
    });

});

function ConfirmDeleteGiftAssociate(giftAssociateId) {
    var dialog = Metro.getPlugin('#DeleteGiftAssociate', 'dialog');
    dialog.open();
    window.$GiftAssociateId = giftAssociateId;
}

function DeleteGiftAssociate() {

    $.ajax({
        method: "POST",
        url: window.$DeleteGiftAssociate,
        data: { giftAssociateId: window.$GiftAssociateId },
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

function OpenGiftDialog(id, associate) {

    $.ajax({
        method: "POST",
        url: window.$OpenGiftDialog,
        data: { giftAssociateId: id, associate: associate },
        success: function (result) {
            var dialog = Metro.getPlugin('#GiftDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function OpenGiftLogDialog(associate) {

    $.ajax({
        method: "POST",
        url: window.$OpenGiftLogDialog,
        data: { associate: associate },
        success: function (result) {
            var dialog = Metro.getPlugin('#GiftLogDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function SaveAssociateGift() {
    $("#Gift").val($("#GiftId option:selected").text());
    $.ajax({
        method: "POST",
        url: window.$SaveAssociateGift,
        data: $('#GiftForm').serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Gift saved", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create(result, null, null, "bg-red fg-white");
            }

        }
    });

}









