$(document).ready(function () {
    $('#dttTypes').DataTable({
        // Configure export buttons
        buttons: [
            {
                extend: 'copyHtml5', // Copy to clipboard
                exportOptions: {
                    columns: [0, 1, 2, 3],  // Specify columns to export
                }
            },
            {
                extend: 'excelHtml5', // Export to Excel
                exportOptions: {
                    columns: [0, 1, 2, 3],
                }
            },
            {
                extend: 'print', // Print the table
                exportOptions: {
                    columns: [0, 1, 2, 3],
                }
            },
        ],
        // Configure table's DOM structure
        dom: 'Bfrtip',
        // Enable responsive behavior
        responsive: true,
        // Enable searching within the table
        searching: true,
        // Show information about the table (e.g., number of entries)
        info: true,
        // Enable pagination
        paging: true,
        // Disable default sorting
        ordering: false,
        // Set vertical scrolling height
        scrollY: "50vh",
        // Collapse the table when scrolled to the bottom
        scrollCollapse: true,
    });
});

function TypeDetailDialog(id) {
    window.$LoadType = "/YMMHRSystem/Type/LoadType";
    console.warn(window.$LoadType);
    $.ajax({
        method: "POST",
        url: window.$LoadType,
        data: { typeId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#TypeDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddType() {

    $('#SaveType').attr('disabled', true);
    $("#SaveType").removeClass('button my-control-colors');
    $("#SaveType").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveType,
        cache: false,
        data: $("#TypeForm").serialize(),
        success: function (result) {
            console.warn(result);
            if (result == true) {
                Metro.toast.create("Type saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#TypeDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveType').attr('disabled', false);
            $("#SaveType").addClass('button my-control-colors');
            $("#SaveType").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function DeleteType() {

    $.ajax({
        method: "POST",
        url: window.$DeleteType,
        data: { typeId: window.$typeId },
        success: function (result) {
            if (result = true) {
                Metro.toast.create("Input type deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting input type", null, null, "bg-red fg-white");
            }

        }
    });

}

function ConfirmDeleteType(typeId) {
    var dialog = Metro.getPlugin('#DeleteType', 'dialog');
    dialog.open();
    window.$typeId = typeId;
}