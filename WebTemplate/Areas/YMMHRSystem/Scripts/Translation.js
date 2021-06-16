$(document).ready(function () {

    $('#dttTranslations').DataTable({
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

function TranslationDetailDialog(id) {

    $.ajax({
        method: "POST",
        url: window.$LoadTranslation,
        data: { translationId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#TranslationDetail', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddTranslation() {

    $('#SaveTranslation').attr('disabled', true);
    $("#SaveTranslation").removeClass('button my-control-colors');
    $("#SaveTranslation").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveTranslation,
        cache: false,
        data: $("#TranslationForm").serialize(),
        success: function (result) {
            if (result !== "false") {
                Metro.toast.create("Translation saved.", null, null, "bg-green fg-white");
                Metro.dialog.close('#TranslationDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error saving translation.", null, null, "bg-red fg-white");
            }

            $('#SaveTranslation').attr('disabled', false);
            $("#SaveTranslation").addClass('button my-control-colors');
            $("#SaveTranslation").addClass('button');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function ConfirmDeleteTranslation(translationId) {
    var dialog = Metro.getPlugin('#DeleteTranslation', 'dialog');
    dialog.open();
    window.$translationId = translationId;
}

function DeleteTranslation() {

    $.ajax({
        method: "POST",

        url: window.$DeleteTranslation,
        data: { translationId: window.$translationId },
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Translation deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error deleting translation", null, null, "bg-red fg-white");
            }

        }
    });

}

function OpenAttachmentDialog(flag) {
    isTranslated = flag;

    if ($("#Issue").val() === "" || $("#Deadline").val() === "" || $("#Description").val() === "") {
        Metro.toast.create("Please enter all fields before attaching files", null, null, "bg-red fg-white");
    } else {

        if ($("#TranslationId").val() == 0) {

            $('#SaveTranslation').attr('disabled', true);
            $("#SaveTranslation").removeClass('button my-control-colors');
            $("#SaveTranslation").addClass('button');
            $("#Preloader").css("visibility", "visible");

            $.ajax({
                method: "POST",
                url: window.$SaveTranslation,
                cache: false,
                data: $("#TranslationForm").serialize(),
                success: function (result) {
                    if (result !== "false") {
                        $("#TranslationId").val(result.split("|")[0]);
                        $("#Associate").val(result.split("|")[1]);
                        $("#Status").val(result.split("|")[2]);
                        $("#DateAdded").val(result.split("|")[3]);
                        Metro.toast.create("Translation saved.", null, null, "bg-green fg-white");
                    } else {
                        Metro.toast.create("Error saving translation.", null, null, "bg-red fg-white");
                    }

                    $('#SaveTranslation').attr('disabled', false);
                    $("#SaveTranslation").addClass('button my-control-colors');
                    $("#SaveTranslation").addClass('button');
                    $("#Preloader").css("visibility", "hidden");

                }
            });
        }

        $.ajax({
            method: "POST",
            url: window.$AttachmentDialog,
            data: { isTranslated: isTranslated },
            success: function (result) {
                var dialog = Metro.getPlugin('#AttachmentDialog', 'dialog');
                dialog.setContent(result);
                setTimeout(function () { dialog.open(); }, 100);
            }
        });
    }

}

function AddAttachment() {
    var formdata = new FormData($('#TranslationAttachmentForm').get(0));
    $.ajax({
        method: "POST",
        url: window.$AddAttachment,
        data: formdata,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result !== "false") {
                Metro.dialog.close('#AttachmentDialog');
                Metro.toast.create("File added", null, null, "bg-green fg-white");
                if (isTranslated) {
                    $("#TranslationTable").children().remove();
                    $("#TranslationTable").append(result);
                } else {
                    $("#AttachmentTable").children().remove();
                    $("#AttachmentTable").append(result);
                }

            } else {
                Metro.toast.create("Error: Translation is complete", null, null, "bg-red fg-white");
            }

        }
    });

}

function StartTranslation() {

    $('#StartTranslation').attr('disabled', true);
    $("#StartTranslation").removeClass('bg-blue fg-white');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$StartTranslation,
        cache: false,
        data: $("#TranslationForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Translation started.", null, null, "bg-green fg-white");
                Metro.dialog.close('#TranslationDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Translation could not be started.", null, null, "bg-red fg-white");
            }

            $('#StartTranslation').attr('disabled', false);
            $("#StartTranslation").addClass('bg-blue fg-white');
            $("#Preloader").css("visibility", "hidden");

        }
    });

}

function EndTranslation() {

    $('#EndTranslation').attr('disabled', true);
    $("#EndTranslation").removeClass('bg-red fg-white');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$EndTranslation,
        cache: false,
        data: $("#TranslationForm").serialize(),
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Translation ended.", null, null, "bg-green fg-white");
                Metro.dialog.close('#TranslationDetail');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Translation could not be ended.", null, null, "bg-red fg-white");
            }

            $('#EndTranslation').attr('disabled', false);
            $("#EndTranslation").addClass('bg-red fg-white');
            $("#Preloader").css("visibility", "hidden");
        }
    });
}






