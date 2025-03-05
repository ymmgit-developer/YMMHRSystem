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
        initComplete: function () {
            $("#DinerTable").show();
        }
    });
});


$(document).ready(function () {
    $('#dttExtraordinaryDiner').DataTable({
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6],
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6],
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6],
                }
            },
        ],
        dom: '<"row"<"col-sm-6"B><"col-sm-6"f>>rtip',
        responsive: true,
        searching: true,
        info: true,
        paging: true,
        ordering: false,
        scrollY: "70vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#ExtraordinaryDinerTable").show();
        }
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
            setTimeout(function () { dialog.open(); }, 100);
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

function ExtraordinaryDinerAddDialog() {

    $.ajax({
        method: "POST",
        url: window.$AddExtraordinaryDiner,
        success: function (result) {
            var dialog = Metro.getPlugin('#ExtraordinaryDinerAddDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function GuestExtraordinaryDinerAddDialog() {

    $.ajax({
        method: "POST",
        url: window.$AddGuestExtraordinaryDiner,
        success: function (result) {
            console.log(result);
            var dialog = Metro.getPlugin('#GuestExtraordinaryDinerAddDialog', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
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
            setTimeout(function () { dialog.open(); }, 100);
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

    console.log($("#Contacts").val());

    if (validarCorreos($("#Contacts").val()))
    {
        $.ajax({
            method: "POST",
            url: window.$SaveExtraordinaryDiner,
            cache: false,
            data: $("#ExtraDinerForm").serialize(),
            success: function (result) {
                console.log("Result: " + result.success)
                if (result.success) {
                    Metro.toast.create("Diner saved.", null, null, "bg-green fg-white");
                    Metro.dialog.close('#ExtraordinaryDinerDetail');
                    setTimeout(function () {
                        location.reload();
                    }, 1000);
                } else {
                    // Actualiza el contenido del diálogo con el mensaje específico del error
                    $("#Error .dialog-content").html("<p>" + result.message + "</p>");
                    Metro.dialog.open('#Error');
                }

                $('#SaveDiner').attr('disabled', false);
                $("#SaveDiner").addClass('button my-control-colors');
                $("#SaveDiner").addClass('button');
                $("#Preloader").css("visibility", "hidden");
            }
        });      
    }
    else
    {
        alert("Correo inválido encontrado: " + validarCorreos($("#Contacts").val()).correoInvalido);
    }
}


function AddGuestExtraordinaryDiner() {

    $('#SaveDiner').attr('disabled', true);
    $("#SaveDiner").removeClass('button my-control-colors');
    $("#SaveDiner").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $("#Type").val($("#SelectType option:selected").text());
    $("#Cost").val($("#SelectType option:selected").val());

    $.ajax({
        method: "POST",
        url: window.$SaveGuestExtraordinaryDiner,
        cache: false,
        data: $("#ExtraDinerForm").serialize(),
        success: function (result) {
            console.log(result);
            console.log(result.message);
            if (result == true) {
                Metro.toast.create("Diner saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#ExtraordinaryDinerDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                $("#Error .dialog-content").html("<p>" + result.message + "</p>");
                Metro.dialog.open('#Error');
            }

            $('#SaveDiner').attr('disabled', false);
            $("#SaveDiner").addClass('button my-control-colors');
            $("#SaveDiner").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmCancelExtraordinaryDiner(dinerId) {
    var dialog = Metro.getPlugin('#CancelExtraordinaryDiner', 'dialog');
    dialog.open();
    window.$dinerId = dinerId;
}

function CancelExtraordinaryDiner() {


    $.ajax({
        method: "POST",
        url: window.$CancelExtraordinaryDiner,
        data: { extraordinaryDinerId: window.$dinerId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Diner cancelled.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error cancelling diner", null, null, "bg-red fg-white");
            }

        }
    });

}

function SelectAssociate(names, process) {
    $('#AssociateName').val(names);
    $('#Process').val(process);
    Metro.dialog.close('#AssociateImporter');
}

function SelectAssociates(names, process, id) {

    var same;
    $("#dttAssociateList td").each(function () {
        if ($(this).text() === names) {
            same = "true";
        }
    });
    if (same === "true") {
        Metro.toast.create("Associate already added.", null, null, "bg-red fg-white");
    } else {
        tableAssociates.row($('#' + id)).remove().draw();

        var node = table.row.add([names, process, '<a class="button small bg-red fg-white" style="cursor: pointer;" onclick="RemoveAssociate(\'' + names + '\')"><span class="mif-bin"></span></a>']).draw(false).node();
        $(node).css('text-align', 'center');
        $("#dttAssociateList").find("tr").last().append("<input type='hidden' name='WorkerList[" + ($("#dttAssociateList").find("tr").length - 2) + "].Names' value='" + names + "'>");
        $("#dttAssociateList").find("tr").last().append("<input type='hidden' name='WorkerList[" + ($("#dttAssociateList").find("tr").length - 2) + "].Process' value='" + process + "'>");
        Metro.toast.create("Associate added.", null, null, "bg-green fg-white");
    }
}

function validarCorreos(contacts) {
    // Expresión regular para validar el formato nombre.apellido@motherson.com
    const regex = /^[a-zA-Z]+\.[a-zA-Z]+@motherson\.com$/;

    // Dividir la lista de correos por comas
    const correos = contacts.split(',');

    // Recorrer cada correo en la lista
    for (let correo of correos) {
        // Eliminar espacios en blanco alrededor del correo
        const correoTrimmed = correo.trim();

        // Verificar si el correo NO coincide con la expresión regular
        if (!regex.test(correoTrimmed)) {
            // Enviar una alerta indicando el correo inválido
            alert(`Correo inválido encontrado: ${correoTrimmed}`);
            // Retornar false y el correo inválido
            return { valido: false, correoInvalido: correoTrimmed };
        }
    }

    // Si todos los correos son válidos, retornar true
    return { valido: true };
}





