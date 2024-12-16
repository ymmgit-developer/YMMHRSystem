$(document).ready(function () {
    $('#dttMotives').DataTable({
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

function MotiveDetailDialog(id) {
    console.warn(id);
    $.ajax({
        method: "POST",
        url: window.$LoadMotive,
        data: { motiveId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#MotiveDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddMotive() {

    $('#SaveMotive').attr('disabled', true);
    $("#SaveMotive").removeClass('button my-control-colors');
    $("#SaveMotive").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveMotive,
        cache: false,
        data: $("#MotiveForm").serialize(),
        success: function (result) {
            if (result == true) {
                Metro.toast.create("Motive saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#MotiveDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveMotive').attr('disabled', false);
            $("#SaveMotive").addClass('button my-control-colors');
            $("#SaveMotive").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });
}

function DeleteMotive() {

    $.ajax({
        method: "POST",
        url: window.$DeleteMotive,
        data: { motiveId: window.$motiveId },
        success: function (result) {
            if (result = true) {
                Metro.toast.create("Input motive deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting input motive", null, null, "bg-red fg-white");
            }
        }
    });
}

function ConfirmDeleteMotive(motiveId) {
    var dialog = Metro.getPlugin('#DeleteMotive', 'dialog');
    dialog.open();
    window.$motiveId = motiveId;
}