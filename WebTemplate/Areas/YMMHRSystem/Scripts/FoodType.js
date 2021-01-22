$(document).ready(function () {

    $('#dttFoodTypes').DataTable({
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

function FoodTypeDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadFoodType,
        data: { foodTypeId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#FoodTypeDetail', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function AddFoodType() {

    $('#SaveFoodType').attr('disabled', true);
    $("#SaveFoodType").removeClass('button my-control-colors');
    $("#SaveFoodType").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveFoodType,
        cache: false,
        data: $("#FoodTypeForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Food type saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#FoodTypeDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveFoodType').attr('disabled', false);
            $("#SaveFoodType").addClass('button my-control-colors');
            $("#SaveFoodType").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteFoodType(foodTypeId) {
    var dialog = Metro.getPlugin('#DeleteFoodType', 'dialog');
    dialog.open();
    window.$foodTypeId = foodTypeId;
}

function DeleteFoodType() {

    $.ajax({
        method: "POST",
        url: window.$DeleteFoodType,
        data: { foodTypeId: window.$foodTypeId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Food type deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting food type", null, null, "bg-red fg-white");
            }

        }
    });

}





