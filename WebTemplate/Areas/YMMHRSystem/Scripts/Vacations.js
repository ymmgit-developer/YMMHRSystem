$(document).ready(function () {
    $('#dttWorkerVacations').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copyHtml5',
                text: 'Copy',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4], // Especificar columnas a exportar
                    format: {
                        header: function (data, columnIdx) {
                            // Personalizar encabezados según el índice de la columna
                            switch (columnIdx) {
                                case 3: return 'Process';
                                case 4: return 'Status of last request';
                                default: return data; // Retornar el encabezado original para otras columnas
                            }
                        }
                    }
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4],
                    format: {
                        header: function (data, columnIdx) {
                            switch (columnIdx) {
                                case 3: return 'Process';
                                case 4: return 'Status of last request';
                                default: return data;
                            }
                        }
                    }
                }
            },
            {
                extend: 'print',
                text: 'Print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4],
                    format: {
                        header: function (data, columnIdx) {
                            switch (columnIdx) {
                                case 3: return 'Process';
                                case 4: return 'Status of last request';
                                default: return data;
                            }
                        }
                    }
                }
            }
        ],
        responsive: true,
        searching: true,
        info: true,
        paging: true,
        ordering: false,
        scrollY: "50vh",
        scrollCollapse: true,
        initComplete: function () {
            $("#VacationTable").show();

            // Índices de las columnas que tendrán filtro (Process y Vacation Request)
            [3, 4].forEach(function (index) {
                this.api()
                    .columns(index)
                    .every(function () {
                        let column = this;

                        // Crear el elemento <select>
                        let select = $('<select><option value="">All</option></select>')
                            .appendTo($(column.header()).empty())
                            .on('change', function () {
                                let val = $.fn.dataTable.util.escapeRegex($(this).val());
                                column
                                    .search(val ? '^' + val + '$' : '', true, false)
                                    .draw();
                            });

                        // Poblar el <select> con opciones únicas
                        column
                            .nodes()
                            .to$()
                            .map(function () {
                                return $(this).text().trim();
                            })
                            .get()
                            .filter((v, i, self) => self.indexOf(v) === i)
                            .sort()
                            .forEach(function (d) {
                                select.append('<option value="' + d + '">' + d + '</option>');
                            });
                    });
            }, this);
        }
    });
});



function VacationsRequestDialog(workerFileId)
{
    $.ajax({
        method: "POST",
        url: window.$AddRecordVacations,
        data: { WorkerFileId: workerFileId },
        success: function (result) {
            var dialog = Metro.getPlugin("#VacationAddDialog", "dialog");
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}

function AddVacations()
{
    $("#Preloader").css("visibility", "visible");
    $.ajax({
        method: "POST",
        url: window.$SaveRecordVacations,
        cache: false,
        data: $("#VacationForm").serialize(),
        success: function (result) {
            if (result) {
                Metro.toast.create("Successful registration.", null, null, "bg-green fg-white");
                Metro.dialog.close('#VacationAddDialog');
                setTimeout(function () {
                    location.reload();
                }, 1500);
            }
            else {
                $("#Error .dialog-content").html("<p>" + result.message + "</p>");
                Metro.dialog.open('#Error');
            }
        }
    });
}

function UpdateVacations() {
    $("#Preloader").css("visibility", "visible");
    $.ajax({
        method: "POST",
        url: window.$UpdateRecordVacations,
        cache: false,
        data: $("#VacationForm").serialize(),
        success: function (result) {
            if (result) {
                Metro.toast.create("Successful registration.", null, null, "bg-green fg-white");
                Metro.dialog.close('#VacationEditDialog');
                setTimeout(function () {
                    location.reload();
                }, 1500);
            }
            else {
                $("#Error .dialog-content").html("<p>" + result.message + "</p>");
                Metro.dialog.open('#Error');
            }
        }
    });
}

function RecordsVacationsDetailDialog(workerFileId)
{
    console.log(window.$RecordVacationsView);
    console.log(workerFileId)
    $.ajax({
        method: "POST",
        url: window.$RecordVacationsView,
        data: { WorkerFileId: workerFileId },
        success: function (result) {
            console.log(result);
            var dialog = Metro.getPlugin("#VacationDetailsDialog", "dialog");
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        },
        error: function (result)
        {
            console.error(result);
        }
    });
}

function RecordApproveVacationsBoss(VacationId)
{
    var dialog = Metro.getPlugin('#VacationApprovalConfirmationDialog', 'dialog');
    dialog.open();
    window.$VacationBossAuthorizations = VacationId;
}

function BossApprovingVacations() {
    $.ajax({
        method: "POST",
        url: window.$ApproveBossRecord,
        data: { VacationId: window.$VacationBossAuthorizations },
        success: function (result) {
            if (result.success) {
                Metro.toast.create("Vacation approved!", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create(result.message, null, null, "bg-red fg-white");
            }

        }
    });
}

function RecordApproveVacationsHR(VacationId)
{
    var dialog = Metro.getPlugin('#VacationApprovalHRConfirmationDialog', 'dialog');
    dialog.open();
    window.$VacationHRAuthorizations = VacationId;
}

function HrApprovingVacations()
{
    $.ajax({
        method: "POST",
        url: window.$ApproveHrRecord,
        data: { VacationId: window.$VacationHRAuthorizations },
        success: function (result) {
            if (result.success) {
                Metro.toast.create("Vacation approved!", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create(result.message, null, null, "bg-red fg-white");
            }

        }
    });
}

function ConfirmCancelRecordVacations(VacationId)
{
    var dialog = Metro.getPlugin('#VacationCancelDialog', 'dialog');
    dialog.open();
    window.$VacationCancellation = VacationId;
}

function CancelRecordVacations()
{
    $.ajax({
        method: "POST",
        url: window.$CancelRecord,
        data: { VacationId: window.$VacationCancellation },
        success: function (result) {
            if (result.success) {
                Metro.toast.create("Vacation canceled.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create(result.message, null, null, "bg-red fg-white");
            }

        }
    });
}

function ConfirmDRevertRecordVacation(VacationId)
{
    var dialog = Metro.getPlugin('#VacationRevertDialog', 'dialog');
    dialog.open();
    window.$VacationRevert = VacationId;
}

function RevertRecordVacation()
{
    $.ajax({
        method: "POST",
        url: window.$RevertRecord,
        data: { VacationId: window.$VacationRevert },
        success: function (result) {
            if (result.success) {
                Metro.toast.create("Vacation reverted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create(result.message, null, null, "bg-red fg-white");
            }

        }
    });
}

