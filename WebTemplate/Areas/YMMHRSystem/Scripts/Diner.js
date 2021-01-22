$(document).ready(function () {

    $('#dttDiner').DataTable({
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5],
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5],
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5],
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


    $('#dttExtraordinaryDiner').DataTable({
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5],
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5],
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5],
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

function DinerDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadDiner,
        data: { dinerId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#DinerDetail', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function AddDiner() {

    $('#SaveDiner').attr('disabled', true);
    $("#SaveDiner").removeClass('button my-control-colors');
    $("#SaveDiner").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveDiner,
        cache: false,
        data: $("#DinerForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Diner saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#DinerDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveDiner').attr('disabled', false);
            $("#SaveDiner").addClass('button my-control-colors');
            $("#SaveDiner").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteDiner(dinerId) {
    var dialog = Metro.getPlugin('#DeleteDiner', 'dialog');
    dialog.open();
    window.$dinerId = dinerId;
}

function DeleteDiner() {


    $.ajax({
        method: "POST",
        url: window.$DeleteDiner,
        data: { dinerId: window.$dinerId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Diner deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting diner", null, null, "bg-red fg-white");
            }

        }
    });

}


function ExtraordinaryDinerDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadExtraordinaryDiner,
        data: { extraordinaryDinerId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#ExtraordinaryDinerDetail', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function AddExtraordinaryDiner() {

    $('#SaveDiner').attr('disabled', true);
    $("#SaveDiner").removeClass('button my-control-colors');
    $("#SaveDiner").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $("#Type").val($("#SelectType option:selected").text());
    $("#Cost").val($("#SelectType option:selected").val());

    $.ajax({
        method: "POST",
        url: window.$SaveExtraordinaryDiner,
        cache: false,
        data: $("#ExtraDinerForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Diner saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#ExtraordinaryDinerDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveDiner').attr('disabled', false);
            $("#SaveDiner").addClass('button my-control-colors');
            $("#SaveDiner").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteExtraordinaryDiner(dinerId) {
    var dialog = Metro.getPlugin('#DeleteExtraordinaryDiner', 'dialog');
    dialog.open();
    window.$dinerId = dinerId;
}

function DeleteExtraordinaryDiner() {


    $.ajax({
        method: "POST",
        url: window.$DeleteExtraordinaryDiner,
        data: { extraordinaryDinerId: window.$dinerId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Diner deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting diner", null, null, "bg-red fg-white");
            }

        }
    });

}

function SelectAssociate(names, process) {
    $('#AssociateName').val(names);
    $('#Process').val(process);
    Metro.dialog.close('#AssociateImporter');
}







