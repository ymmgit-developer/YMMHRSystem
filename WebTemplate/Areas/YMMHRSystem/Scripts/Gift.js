$(document).ready(function () {

    $('#dttGifts').DataTable({
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

function GiftDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadGift,
        data: { giftId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#GiftDetail', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function GiftLogDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadGiftLog,
        data: { giftId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#GiftLogDialog', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function AddGift() {

    $('#SaveGift').attr('disabled', true);
    $("#SaveGift").removeClass('button my-control-colors');
    $("#SaveGift").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveGift,
        cache: false,
        data: $("#GiftForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Gift saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#GiftDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveGift').attr('disabled', false);
            $("#SaveGift").addClass('button my-control-colors');
            $("#SaveGift").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });
}

function MovementDialog(giftId) {
    var dialog = Metro.getPlugin('#MovementDialog', 'dialog');
    dialog.open();
    window.$giftId = giftId;
}

function GiftMovement() {

    Metro.dialog.open('#preloader');
    var quantity = $("#MovementQuantity").val();
    var type = $("#MovementType option:selected").val();

    $.ajax({
        method: "POST",
        url: window.$GiftMovement,
        data: { giftId: window.$giftId, quantity: quantity, movementType: type },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Gift updated.", null, null, "bg-green fg-white");
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

function ConfirmDeleteGift(giftId, quantity) {
    if (quantity == 0) {
        var dialog = Metro.getPlugin('#DeleteGift', 'dialog');
        dialog.open();
        window.$giftId = giftId;
    } else {
        Metro.toast.create("Please reduce gift quantity to 0", null, null, "bg-red fg-white");
    }
}

function DeleteGift() {

    $.ajax({
        method: "POST",
        url: window.$DeleteGift,
        data: { giftId: window.$giftId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Gift deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting gift", null, null, "bg-red fg-white");
            }

        }
    });

}





