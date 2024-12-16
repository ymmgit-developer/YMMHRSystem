$(document).ready(function () {

    $('#dttCoffeeBreaks').DataTable({
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

var isTranslated;

function CoffeeBreakDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadCoffeeBreak,
        data: { coffeeBreakId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#CoffeeBreakDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddCoffeeBreak() {

    $('#SaveCoffeeBreak').attr('disabled', true);
    $("#SaveCoffeeBreak").removeClass('button my-control-colors');
    $("#SaveCoffeeBreak").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveCoffeeBreak,
        cache: false,
        data: $("#CoffeeBreakForm").serialize(),
        success: function (result) {
            console.warn(result);
            if (result !== "false") {
                Metro.toast.create("Coffee break saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#CoffeeBreakDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("No rooms available", null, null, "bg-red fg-white");
            }

            $('#SaveCoffeeBreak').attr('disabled', false);
            $("#SaveCoffeeBreak").addClass('button my-control-colors');
            $("#SaveCoffeeBreak").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteCoffeeBreak(coffeeBreakId) {
    var dialog = Metro.getPlugin('#DeleteCoffeeBreak', 'dialog');
    dialog.open();
    window.$coffeeBreakId = coffeeBreakId;
}

function DeleteCoffeeBreak() {

    $.ajax({
        method: "POST",

        url: window.$DeleteCoffeeBreak,
        data: { coffeeBreakId: window.$coffeeBreakId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Coffee break deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting coffeeBreak", null, null, "bg-red fg-white");
            }

        }
    });

}

function StartCoffeeBreak() {

    $('#StartCoffeeBreak').attr('disabled', true);
    $("#StartCoffeeBreak").removeClass('bg-blue fg-white');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$StartCoffeeBreak,
        cache: false,
        data: $("#CoffeeBreakForm").serialize(),
        success: function (result) {
            console.warn(result);
            if (result === "true") {
                Metro.toast.create("Coffee Break started.", null, null, "bg-green fg-white");
                Metro.dialog.close('#CoffeeBreakDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Coffee Break could not be started.", null, null, "bg-red fg-white");
            }

            $('#StartCoffeeBreak').attr('disabled', false);
            $("#StartCoffeeBreak").addClass('bg-blue fg-white');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function EndCoffeeBreak() {

    $('#EndCoffeeBreak').attr('disabled', true);
    $("#EndCoffeeBreak").removeClass('bg-red fg-white');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$EndCoffeeBreak,
        cache: false,
        data: $("#CoffeeBreakForm").serialize(),
        success: function (result) {
            console.warn(result);
            if (result === "true") {
                Metro.toast.create("Coffee Break ended.", null, null, "bg-green fg-white");
                Metro.dialog.close('#CoffeeBreakDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Coffee Break could not be ended.", null, null, "bg-red fg-white");
            }

            $('#EndCoffeeBreak').attr('disabled', false);
            $("#EndCoffeeBreak").addClass('bg-red fg-white');
            $("#Preloader").css("visibility", "hidden");
        }
    });
}






