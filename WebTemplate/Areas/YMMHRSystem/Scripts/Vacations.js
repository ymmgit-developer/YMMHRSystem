$(document).ready(function () {
    $('#dttWorkerVacations').DataTable({
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
                            .appendTo($(column.header()).empty()) // Reemplazar contenido del encabezado
                            .on('change', function () {
                                // Filtrar según la selección del usuario
                                let val = $.fn.dataTable.util.escapeRegex($(this).val());
                                column
                                    .search(val ? '^' + val + '$' : '', true, false)
                                    .draw();
                            });

                        // Poblar el <select> con opciones únicas, limpiando etiquetas HTML
                        column
                            .nodes()
                            .to$() // Convertir los nodos a objetos jQuery
                            .map(function () {
                                return $(this).text().trim(); // Extraer el texto limpio
                            })
                            .get() // Convertir a un array estándar
                            .filter((v, i, self) => self.indexOf(v) === i) // Eliminar duplicados
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
    $.ajax({
        method: "POST",
        url: window.$RecordVacationsView,
        data: { WorkerFileId: workerFileId },
        success: function (result) {
            var dialog = Metro.getPlugin("#VacationDetailsDialog", "dialog");
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });
}


