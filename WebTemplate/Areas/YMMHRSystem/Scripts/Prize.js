$(document).ready(function () {

    $('#dttPrizes').DataTable({
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

function PrizeDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadPrize,
        data: { prizeId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#PrizeDetail', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function PrizeLogDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadPrizeLog,
        data: { prizeId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#PrizeLogDialog', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function SavePrize() {

    $('#SavePrize').attr('disabled', true);
    $("#SavePrize").removeClass('button my-control-colors');
    $("#SavePrize").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SavePrize,
        cache: false,
        data: $("#PrizeForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Prize saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#PrizeDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SavePrize').attr('disabled', false);
            $("#SavePrize").addClass('button my-control-colors');
            $("#SavePrize").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });
}

function MovementDialog(prizeId) {
    var dialog = Metro.getPlugin('#MovementDialog', 'dialog');
    dialog.open();
    window.$prizeId = prizeId;
}

function PrizeMovement() {

    Metro.dialog.open('#preloader');
    var quantity = $("#MovementQuantity").val();
    var type = $("#MovementType option:selected").val();

    $.ajax({
        method: "POST",
        url: window.$PrizeMovement,
        data: { prizeId: window.$prizeId, quantity: quantity, movementType: type },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Prize updated.", null, null, "bg-green fg-white");
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

function ConfirmDeletePrize(prizeId, quantity) {
    if (quantity == 0) {
        var dialog = Metro.getPlugin('#DeletePrize', 'dialog');
        dialog.open();
        window.$prizeId = prizeId;
    } else {
        Metro.toast.create("Please reduce prize quantity to 0", null, null, "bg-red fg-white");
    }
}

function DeletePrize() {

    $.ajax({
        method: "POST",
        url: window.$DeletePrize,
        data: { prizeId: window.$prizeId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Prize deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting prize", null, null, "bg-red fg-white");
            }

        }
    });

}





