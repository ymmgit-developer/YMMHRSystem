$(document).ready(function () {

    $('#dttRooms').DataTable({
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

function RoomDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadRoom,
        data: { roomId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#RoomDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddRoom() {

    $('#SaveRoom').attr('disabled', true);
    $("#SaveRoom").removeClass('button my-control-colors');
    $("#SaveRoom").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveRoom,
        cache: false,
        data: $("#RoomForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Room saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#RoomDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.dialog.open('#Error')
            }

            $('#SaveRoom').attr('disabled', false);
            $("#SaveRoom").addClass('button my-control-colors');
            $("#SaveRoom").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteRoom(roomId) {
    var dialog = Metro.getPlugin('#DeleteRoom', 'dialog');
    dialog.open();
    window.$roomId = roomId;
}

function DeleteRoom() {

    $.ajax({
        method: "POST",
        url: window.$DeleteRoom,
        data: { roomId: window.$roomId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Room deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting room", null, null, "bg-red fg-white");
            }

        }
    });

}





