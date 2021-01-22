$(document).ready(function () {

    $('#dttWorkerFiles').DataTable({
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4],
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4],
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4],
                }
            },
        ],  
        dom: 'Bfrtip',
        deferRender: true,
        responsive: true,
        compact: true,
        searching: true,
        info: true,
        paging: true,
        ordering: false,
        scrollY: "50vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#WorkerFileTable").show();
        }
    });

});

function SaveWorkerFile() {
    if ($("#Names").val() !== "" && $("#WorkerId").val() !== "" && $("#Email").val() !== "" && $("#Telephone").val() !== "" && $("#CURP").val() !== "" && $("#RFC").val() !== "" && $("#NSS").val() !== "") {
        $("#AdmissionDate").val($("#GetAdmissionDate").val());
        $("#DoB").val($("#GetDoB").val());

        var data = $('#FormDetail').find('select, input').serialize();

        var dialog = Metro.getPlugin('#preloaderWorkerFile', 'dialog');
        dialog.open();

        $.ajax({
            method: "POST",
            url: window.$SaveWorkerFile,
            cache: false,
            data: data,
            success: function (result) {
                if (result === "true") {
                    Metro.toast.create("Worker file saved.", null, null, "bg-green fg-white");
                } else {
                    Metro.dialog.open('#Error')
                }
                Metro.dialog.close('#preloaderWorkerFile');
            }
        });
    } else {
        Metro.toast.create("Please enter all fields.", null, null, "bg-red fg-white");
    }
}

function DismissWorkerFile(id) {

    $.ajax({
        method: "POST",
        url: window.$DismissWorkerFile,
        data: { workerFileId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#DismissalDialog', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function DismissWorker() {
    $("#Date").val($("#GetDate").val());
    var formdata = new FormData($('#WorkerDismissalForm').get(0));
    $.ajax({
        method: "POST",
        url: window.$DismissWorker,
        data: formdata,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Worker dismissed", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error: Please attach PDF file only", null, null, "bg-red fg-white");
            }

        }
    });

}

function AdmitWorkerFile(id) {

    $.ajax({
        method: "POST",
        url: window.$AdmitWorkerFile,
        data: { workerFileId: id },
        success: function (result) {
            var dialog = Metro.getPlugin('#AdmissionDialog', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function AdmitWorker() {

    $('#AdmitWorker').attr('disabled', true);
    $("#AdmitWorker").removeClass('button my-control-colors');
    $("#AdmitWorker").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $("#Admission").val($("#GetDate").val());
    var formdata = new FormData($('#WorkerAdmissionForm').get(0));
    $.ajax({
        method: "POST",
        url: window.$AdmitWorker,
        data: formdata,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Worker admitted", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error admitting worker", null, null, "bg-red fg-white");
            }
            $('AdmitWorker').attr('disabled', false);
            $("AdmitWorker").addClass('button my-control-colors');
            $("AdmitWorker").addClass('button');
            $("#Preloader").css("visibility", "hidden");
        }
    });

}

function OpenAttachmentDialog() {

    $.ajax({
        method: "POST",
        url: window.$AttachmentDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#AttachmentDialog', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function AddAttachment() {
    var formdata = new FormData($('#WorkerAttachmentForm').get(0));
    $.ajax({
        method: "POST",
        url: window.$AddAttachment,
        data: formdata,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Attachment added", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error: Please attach PDF file only", null, null, "bg-red fg-white");
            }

        }
    });

}

function OpenTrajectoryDialog() {

    $.ajax({
        method: "POST",
        url: window.$TrajectoryDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#TrajectoryDialog', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function AddTrajectory() {
    var formdata = new FormData($('#WorkerTrajectoryForm').get(0));
    $.ajax({
        method: "POST",
        url: window.$AddTrajectory,
        data: formdata,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Trajectory added", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error: Please attach PDF file only", null, null, "bg-red fg-white");
            }

        }
    });

}

function OpenWarningDialog() {

    $.ajax({
        method: "POST",
        url: window.$WarningDialog,
        success: function (result) {
            var dialog = Metro.getPlugin('#WarningDialog', 'dialog');
            dialog.setContent(result);
            dialog.open();
        }
    });
}

function AddWarning() {
    $("#Date").val($("#GetDate").val());
    var formdata = new FormData($('#WorkerWarningForm').get(0));
    $.ajax({
        method: "POST",
        url: window.$AddWarning,
        data: formdata,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result === "true") {
                Metro.toast.create("Warning added", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create("Error: Please attach PDF file only", null, null, "bg-red fg-white");
            }

        }
    });

}

function ChangePhoto() {

    var oFReader = new FileReader();
    //Revisar imagen si es mayor a 4MB
    if ($("#Url").get(0).files[0].size < 4000000) {
        oFReader.readAsDataURL($("#Url").get(0).files[0]);

        $('#Photo').css("opacity", "");
        oFReader.onload = function (e) {
            $('#Photo')
                .attr('src', e.target.result);
        };

        var data = new FormData();
        var files = $("#Url").get(0).files;
        if (files.length > 0) {
            data.append("Url", files[0]);
        }

        $.ajax({
            url: $SavePhoto,
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (result) {
                if (result !== "false") {
                    document.getElementById('WorkerPhoto').value = result;
                    Metro.toast.create("Worker photo changed", null, null, "bg-green fg-white");
                } else {
                    Metro.toast.create("Error changing photo", null, null, "bg-red fg-white");
                }

            }
        });
    } else {
        Metro.toast.create("Size surpasses 4 MB", null, null, "bg-red fg-white");
    }
};

function ImportWorkers() {
    Metro.dialog.open('#preloader');
    if (window.FormData !== undefined) {
        var fileUpload = $("#Url").get(0);
        if ($("#Url").get(0).files.length == 0) {
            Metro.dialog.close('#preloader');
            Metro.toast.create("Please load the template", null, null, "bg-red fg-white");
            return;
        }
        var files = fileUpload.files;
        var fileData = new FormData();
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }
        $.ajax({
            url: window.$ImportWorkers,
            type: "POST",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (result) {
                Metro.dialog.close('#preloader');
                if (result == false) {
                    Metro.dialog.open('#TemplateError')
                } else {
                    Metro.toast.create("Workers imported", null, null, "bg-green fg-white");
                    setTimeout(function () {
                        location.reload();
                    }, 1000);
                }
            },
            error: function (err) {
                Metro.dialog.close('#preloader');
                Metro.toast.create("Error, please contact IT administrator", null, null, "bg-red fg-white");
            }
        });
    } else {       
        Metro.toast.create("FormData no es soportado por el navegador.", null, null, "bg-red fg-white");
    }
}






