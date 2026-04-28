let redeemDT = null;
let resetWorkerIds = [];

$(document).ready(function () {
    $('#dttWorkerFiles').DataTable({
        dom: "<'dt-top'<'dt-left'B><'dt-center'l><'dt-right'f>>" +
            "<'dt-body'tr>" +
            "<'dt-bottom'<'dt-info'i><'dt-paging'p>>",
        buttons: [
            {
                extend: 'copyHtml5',
                text: 'Copy',
                exportOptions: {
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                exportOptions: {
                }
            },
            {
                extend: 'print',
                text: 'Print',
                exportOptions: {
                }
            }
        ],
        responsive: true,
        lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        pageLength: 50, // por defecto
        searching: true,
        info: true,
        paging: true,
        ordering: false,
        scrollY: "50vh",
        scrollCollapse: true,

        // Para que no intente ordenar/filtrar por la columna de checkbox
        columnDefs: [
            { targets: 0, searchable: false, orderable: false, width: "35px" },
        ],

        initComplete: function () {
            $("#WorkerRewardTable").show();
            const api = this.api();

            // 1) Checkboxes en filas
            api.rows().every(function () {
                const rowNode = this.node();
                const workerFileId = $(rowNode).data('workerfileid');
                const fallbackWorkerId = $(rowNode).find('td:eq(1)').text().trim();
                const val = workerFileId ? workerFileId : fallbackWorkerId;
                const checkboxHtml = '<input type="checkbox" class="rw-select" value="' + val + '" />';
                $(rowNode).find('td:eq(0)').html(checkboxHtml);
            });

            // 2) Filtro MULTISELECT para Process (Columna 3)
            api.columns(3).every(function () {
                const column = this;
                const headerText = $(column.header()).text().trim();

                const select = $('<select class="rw-filter-multi" multiple="multiple" style="width:100%"></select>')
                    .appendTo($(column.header()).empty())
                    .on('change', function () {
                        const data = $(this).val();
                        if (data && data.length > 0) {
                            // Solución al filtrado: Agregamos \s* para ignorar espacios invisibles antes o después
                            const searchVal = '(' + data.map(function (v) {
                                return '^\\s*' + $.fn.dataTable.util.escapeRegex(v) + '\\s*$';
                            }).join('|') + ')';

                            column.search(searchVal, true, false).draw();
                        } else {
                            column.search('', false, false).draw();
                        }
                    });

                // Para limpiar los textos
                const unique = new Set();
                column.nodes().to$().each(function () {
                    const t = $(this).text().trim();
                    if (t) unique.add(t);
                });

                Array.from(unique).sort().forEach(function (d) {
                    select.append('<option value="' + d + '">' + d + '</option>');
                });

                // Inicializar Select2
                select.select2({
                    placeholder: headerText,
                    allowClear: true,
                    closeOnSelect: false
                });
            });

            // 3) Filtro MULTISELECT para Job Title (Columna 4)
            api.columns(4).every(function () {
                const column = this;
                const headerText = $(column.header()).text().trim();

                const select = $('<select class="rw-filter-multi" multiple="multiple" style="width:100%"></select>')
                    .appendTo($(column.header()).empty())
                    .on('change', function () {
                        const data = $(this).val();
                        if (data && data.length > 0) {
                            const searchVal = '(' + data.map(function (v) {
                                return '^\\s*' + $.fn.dataTable.util.escapeRegex(v) + '\\s*$';
                            }).join('|') + ')';

                            column.search(searchVal, true, false).draw();
                        } else {
                            column.search('', false, false).draw();
                        }
                    });

                const unique = new Set();
                column.nodes().to$().each(function () {
                    const t = $(this).text().trim();
                    if (t) unique.add(t);
                });

                Array.from(unique).sort().forEach(function (d) {
                    select.append('<option value="' + d + '">' + d + '</option>');
                });

                // Inicializar Select2
                select.select2({
                    placeholder: headerText,
                    allowClear: true,
                    closeOnSelect: false,
                    width: 'resolve'
                });
            });

            // 4) Filtro MULTISELECT para Group (Columna 5)
            api.columns(5).every(function () {
                const column = this;
                const headerText = $(column.header()).text().trim();

                const select = $('<select class="rw-filter-multi" multiple="multiple" style="width:100%"></select>')
                    .appendTo($(column.header()).empty())
                    .on('change', function () {
                        const data = $(this).val();

                        if (data && data.length > 0) {

                            // Soporta "sin grupo" (vacío) y grupos normales
                            const parts = data.map(function (v) {
                                if (v === "__EMPTY__") return '^\\s*$';     // celdas vacías
                                return '^\\s*' + $.fn.dataTable.util.escapeRegex(v) + '\\s*$';
                            });

                            const searchVal = '(' + parts.join('|') + ')';
                            column.search(searchVal, true, false).draw(); // regex=true, smart=false
                        } else {
                            column.search('', false, false).draw();
                        }
                    });

                // Poblar opciones únicas
                const unique = new Set();
                column.nodes().to$().each(function () {
                    const t = $(this).text().trim();
                    if (t) unique.add(t);
                });

                // Opción "No group" (para tus spans vacíos)
                select.append('<option value="__EMPTY__">(No group)</option>');

                // Grupos válidos (A, B, C)
                Array.from(unique).sort().forEach(function (d) {
                    select.append('<option value="' + d + '">' + d + '</option>');
                });

                // Inicializar Select2
                select.select2({
                    placeholder: headerText,
                    allowClear: true,
                    closeOnSelect: false,
                    width: 'resolve'
                });
            });

            const th0 = $(api.column(0).header());
            if (th0.find('#rw-select-all').length === 0) {
                th0.html('<input type="checkbox" id="rw-select-all" />');
            }
        }
    });

    // Select all / deselect all
    $(document).on('change', '#rw-select-all', function () {
        const checked = $(this).is(':checked');
        $('#dttWorkerFiles tbody .rw-select').prop('checked', checked);
    });

    // Si el usuario desmarca una fila, desmarca el select-all
    $(document).on('change', '#dttWorkerFiles tbody .rw-select', function () {
        if (!$(this).is(':checked')) {
            $('#rw-select-all').prop('checked', false);
        }
    });

    // Helper para obtener seleccionados
    window.getSelectedWorkers = function () {
        return $('#dttWorkerFiles tbody .rw-select:checked')
            .map(function () { return $(this).val(); })
            .get();
    };
});

function updateSelectedInfo(api) {
    const selectedCount = $('#dttWorkerFiles tbody .rw-select:checked').length;

    // Fuerza refresco del info para que exista el nodo
    api.draw(false);

    // Agrega/actualiza el texto "Selected: N" junto al info nativo
    const info = $('#dttWorkerFiles_info');
    const badgeId = 'rw-selected-badge';

    if ($('#' + badgeId).length === 0) {
        info.append(' <span id="' + badgeId + '"></span>');
    }

    $('#' + badgeId).text(' | Selected: ' + selectedCount);
}

function RewardScoreAddDialog() {
    // Obtener los WorkerIds seleccionados
    let selectedWorkerIds = $('#dttWorkerFiles tbody .rw-select:checked')
        .map(function () { return $(this).val(); })
        .get();
    if (selectedWorkerIds.length > 0) // Valida si hay trabajadores seleccionados
    {
        // Se envian al backend, si resulta exitoso abre el dialog
        $.ajax({
            method: "POST",
            url: window.$AddRecordRewards,
            data: { SelectedWorkerIds: selectedWorkerIds },
            success: function (result) {
                var dialog = Metro.getPlugin("#ScoreAddDialog", "dialog");
                dialog.setContent(result);
                setTimeout(function () {
                    dialog.open(),
                        calcImpTotalByCategory();
                }, 100);
            },
            error: function (result) {
                Metro.toast.create('An error has occurred.', null, null, "bg-red fg-white");
                console.error(result);
            }
        });
    }
    else
    {
        Metro.toast.create('You must select at least one worker.', null, null, "bg-orange fg-white");
    }
}

// Total puntos (solo UI)
$(document).on('change', '.imp-check', function () {
    calcImpTotalByCategory();
});

function calcImpTotalByCategory() {
    let ptsA = 0, ptsB = 0, ptsC = 0;

    $('.imp-check:checked').each(function () {
        const score = parseInt($(this).data('score') || 0, 10);
        const wt = (($(this).data('workertype') || '') + '').toUpperCase(); // A/B/C

        if (wt === 'A') ptsA += score;
        else if (wt === 'B') ptsB += score;
        else if (wt === 'C') ptsC += score;
    });

    $('#ptsA').text(ptsA);
    $('#ptsB').text(ptsB);
    $('#ptsC').text(ptsC);
}

// Envio al backend
function SubmitAddRewards() {

    // Workers por tipo (desde hidden)
    const trustedWorkerIds = ($('#trustedWorkerIds').val() || '')
        .split(',').map(x => x.trim()).filter(Boolean);

    const associateWorkerIds = ($('#associateWorkerIds').val() || '')
        .split(',').map(x => x.trim()).filter(Boolean);

    // Improvements por categoría (A/B/C)
    const improvementIdsAssociates = [];
    const improvementIdsTrusted = [];
    const improvementIdsContingencies = [];

    $('.imp-check:checked').each(function () {
        const id = parseInt(this.value, 10);
        const wt = (($(this).data('workertype') || '') + '').toUpperCase(); // A/B/C

        if (wt === 'A') improvementIdsAssociates.push(id);
        else if (wt === 'B') improvementIdsTrusted.push(id);
        else if (wt === 'C') improvementIdsContingencies.push(id);
    });

    if (
        improvementIdsAssociates.length === 0 &&
        improvementIdsTrusted.length === 0 &&
        improvementIdsContingencies.length === 0
    ) {
        Metro.toast.create('Select at least one improvement.', null, null, "bg-orange fg-white");
        return;
    }

    console.log(associateWorkerIds)

    // Validaciones útiles
    if (associateWorkerIds.length === 0 && improvementIdsAssociates.length > 0) {
        Metro.toast.create('There are no Associates for category A improvements.', null, null, "bg-orange fg-white");
        return;
    }
    if (trustedWorkerIds.length === 0 && improvementIdsTrusted.length > 0) {
        Metro.toast.create('There are no Trusted Personnel for category B improvements.', null, null, "bg-orange fg-white");
        return;
    }

    $('#Preloader').css('visibility', 'visible');

    $.ajax({
        method: "POST",
        url: window.$SaveRegisterRewards,
        traditional: true,
        data: {
            TrustedWorkerIds: trustedWorkerIds,
            AssociateWorkerIds: associateWorkerIds,
            ImprovementIdsTrusted: improvementIdsTrusted,
            ImprovementIdsAssociates: improvementIdsAssociates,
            ImprovementIdsContingencies: improvementIdsContingencies
        },
        success: function (res) {
            $('#Preloader').css('visibility', 'hidden');

            if (res && res.success) {
                Metro.toast.create("Scores applied successfully.", null, null, "bg-green fg-white");
                Metro.getPlugin('#ScoreAddDialog', 'dialog').close();
            } else {
                Metro.toast.create('Could not apply scores.', null, null, "bg-red fg-white");
                console.error(res.message);
            }
        },
        error: function (xhr) {
            $('#Preloader').css('visibility', 'hidden');
            console.error(xhr);
            Metro.toast.create('Error applying scores.', null, null, "bg-red fg-white");
        }
    });
}

function RewardRedeemDialog() {
    // Obtener los WorkerIds seleccionados
    let selectedWorkerIds = $('#dttWorkerFiles tbody .rw-select:checked')
        .map(function () { return $(this).val(); })
        .get();
    if (selectedWorkerIds.length > 0 && selectedWorkerIds.length < 2 ) // Valida si hay trabajadores seleccionados
    {
        const workerId = selectedWorkerIds[0];
        $.ajax({
            method: "POST",
            url: window.$AddRedeemRewards,
            data: { WorkerId: workerId },
            success: function (result) {
                var dialog = Metro.getPlugin("#RedeemRewardsDialog", "dialog");
                dialog.setContent(result);
                setTimeout(function () {
                    dialog.open();
                }, 100);
            },
            error: function (result) {
                Metro.toast.create('An error has occurred.', null, null, "bg-red fg-white");
                console.error(result);
            }
        });
    }
    else
    {
        Metro.toast.create('You must select exactly 1 worker.', null, null, "bg-orange fg-white");
    }
}

function RewardsHistoryDialog(workerId) {
    console.log('RewardsHistoryDialog:', workerId);

    $.ajax({
        method: "POST",
        url: window.$ViewHistoryRecordRewards,
        data: { WorkerId: workerId },
        success: function (result) {
            var dialog = Metro.getPlugin("#ScoreHistoryViewDialog", "dialog");
            dialog.setContent(result);
            setTimeout(function () {
                dialog.open();
                initRewardsHistoryTable(); // inicializa DataTables
            }, 100);
        },
        error: function (xhr) {
            console.error(xhr);
            Metro.toast.create('Could not load history.', null, null, "bg-red fg-white");
        }
    });
}

// DataTable del historial
function initRewardsHistoryTable() {
    if ($.fn.DataTable.isDataTable('#dttRewardsHistoryWorker')) {
        $('#dttRewardsHistoryWorker').DataTable().destroy();
    }

    $('#dttRewardsHistoryWorker').DataTable({
        dom: 'lfrtip',
        lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        pageLength: 10,
        responsive: true,
        searching: true,
        info: true,
        paging: true,
        ordering: false,
        scrollY: "50vh",
        scrollCollapse: true
    });
}

function RewardRedeemDialog() {
    /* Obtener los workers seleccionados */
    const selectedWorkerIds = $('#dttWorkerFiles tbody .rw-select:checked')
        .map(function () { return $(this).val(); })
        .get();

    if (selectedWorkerIds.length !== 1) {
        Metro.toast.create('You must select exactly 1 worker.', null, null, "bg-orange fg-white");
        return;
    }

    const workerId = selectedWorkerIds[0];

    $.ajax({
        method: "POST",
        url: window.$AddRedeemRewards,
        data: { WorkerId: workerId },
        success: function (result) {
            var dialog = Metro.getPlugin("#RedeemRewardsDialog", "dialog");
            dialog.setContent(result);

            setTimeout(function () {
                dialog.open();
                initRedeemItemsTable();
            }, 100);
        },
        error: function (xhr) {
            Metro.toast.create('An error has occurred.', null, null, "bg-red fg-white");
            console.error(xhr);
        }
    });
}

function initRedeemItemsTable() {
    const $table = $('#dttRedeemItems');

    // 1) Destruir instancia previa
    if ($.fn.DataTable.isDataTable($table)) {
        $table.DataTable().destroy();
        $table.find('tbody').off('.redeem');
    }

    // 2) Inicializar DataTable SIN la extensión Select
    redeemDT = $table.DataTable({
        dom: 'frt', /* lfrtip */
        paging: false,
        pageLength: 100,
        searching: true,
        ordering: false,
        scrollY: "40vh",
        scrollCollapse: true,
        scrollX: false,
        autoWidth: false,
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                orderable: false,
                searchable: false,
                width: "35px",
                className: 'text-center'
            },
            {
                targets: 1,
                orderable: false,
                searchable: false,
                width: "90px"
            }
        ],
        language: {
            search: "Search:",
            searchPlaceholder: "Item name..."
        }
    });

    // 3) Evento: Checkbox individual (Habilitar/Deshabilitar Qty)
    $(document).off('change', '#dttRedeemItems .redeem-select');
    $(document).on('change', '#dttRedeemItems .redeem-select', function () {
        // Buscamos la fila (tr) más cercana al checkbox actual
        const $row = $(this).closest('tr');
        // Buscamos el input de cantidad dentro de ESA fila
        const $qtyInput = $row.find('.redeem-qty');
        const isChecked = $(this).is(':checked');

        // Habilitar si está checkeado, deshabilitar si no
        $qtyInput.prop('disabled', !isChecked);

        if (isChecked) {
            // Si se activa, aseguramos que tenga al menos 1
            if (parseInt($qtyInput.val()) < 1 || isNaN(parseInt($qtyInput.val()))) {
                $qtyInput.val(1);
            }
        } else {
            // Opcional: podrías limpiar el valor si se desmarca
            // $qtyInput.val(1); 
        }

        // Actualizar estados globales
        updateSelectAllState();
        calcRedeemTotals();
    });

    // 4) Evento: Checkbox individual
    $(document).off('input.redeem change.redeem', '#dttRedeemItems .redeem-qty');
    $(document).on('input.redeem change.redeem', '#dttRedeemItems .redeem-qty', function () {
        const $row = $(this).closest('tr');
        const stock = parseInt($row.data('stock') || 0, 10);
        let qty = parseInt($(this).val() || 1, 10);

        // Validar límites
        if (qty < 1) qty = 1;
        if (stock > 0 && qty > stock) qty = stock;

        $(this).val(qty);

        // Recalcular totales
        calcRedeemTotals();
    });

    // 5) Evento: Cambio en cantidad
    $(document).off('change.redeem', '#selectAllItems');
    $(document).on('change.redeem', '#selectAllItems', function () {
        const isChecked = $(this).prop('checked');

        $('#dttRedeemItems tbody .redeem-select').each(function () {
            $(this).prop('checked', isChecked);
            const $row = $(this).closest('tr');
            const $qty = $row.find('.redeem-qty');
            $qty.prop('disabled', !isChecked);
            if (isChecked) $qty.val(1);
        });

        calcRedeemTotals();
    });

    // Helper: Actualizar estado del checkbox "Seleccionar todo"
    function updateSelectAllState() {
        const $checkboxes = $('#dttRedeemItems tbody .redeem-select');
        const total = $checkboxes.length;
        const checked = $checkboxes.filter(':checked').length;
        const $selectAll = $('#selectAllItems');

        $selectAll.prop('checked', total === checked && total > 0);
        $selectAll.prop('indeterminate', checked > 0 && checked < total);
    }

    // Ajustar scroll al abrir el dialog (Metro UI)
    setTimeout(function () {
        $table.DataTable().columns.adjust().draw();
    }, 100);

    // Inicializar
    calcRedeemTotals();
}

function calcRedeemTotals() {
    const available = parseInt($('#RedeemAvailablePoints').val() || '0', 10);
    let total = 0;

    // 1. Calcular el total recorriendo solo filas seleccionadas
    $('#dttRedeemItems tbody .redeem-select:checked').each(function () {
        const $row = $(this).closest('tr');
        const cost = parseInt($row.data('cost') || 0, 10);
        const qty = parseInt($row.find('.redeem-qty').val() || 0, 10);

        total += (cost * qty);
    });

    const remaining = available - total;

    // 2. Actualizar etiquetas de la interfaz
    $('#RedeemTotal').text(total + ' pts');
    $('#RedeemRemaining').text(remaining + ' pts');

    // 3. Validación lógica del botón de envío
    const $btnSave = $('#BtnConfirmRedeem');
    const $lblRemaining = $('#RedeemRemaining');

    if (remaining < 0) {
        // CASO: Saldo insuficiente
        $lblRemaining.removeClass('bg-green').addClass('bg-red');

        $btnSave.addClass('disabled').prop('disabled', true);
        //console.log("Deshabilitar envío: Saldo insuficiente");

    } else if (total > 0) {
        // CASO: Saldo suficiente y al menos un artículo seleccionado
        $lblRemaining.removeClass('bg-red').addClass('bg-green');

        $btnSave.removeClass('disabled').prop('disabled', false);
        //console.log("Habilitar envío: Todo correcto");

    } else {
        // CASO: Saldo suficiente pero total es 0 (nada seleccionado)
        $lblRemaining.removeClass('bg-red').addClass('bg-green');

        $btnSave.addClass('disabled').prop('disabled', true);
        //console.log("Deshabilitar envío: Ningún ítem seleccionado");
    }
}

function SubmitRedeemRewards() {
    const workerId = ($('#RedeemWorkerId').val() || '').trim();
    const available = parseInt($('#RedeemAvailablePoints').val() || '0', 10);

    if (!workerId) {
        Metro.toast.create('WorkerId not found.', null, null, "bg-red fg-white");
        return;
    }

    const itemIds = [];
    const quantities = [];

    // Recolectar seleccionados
    $('#dttRedeemItems tbody .redeem-select:checked').each(function () {
        const $row = $(this).closest('tr');
        const itemId = parseInt($row.data('itemid') || $(this).data('itemid') || 0, 10);
        const qty = parseInt($row.find('.redeem-qty').val() || 0, 10);

        if (itemId > 0 && qty > 0) {
            itemIds.push(itemId);
            quantities.push(qty);
        }
    });

    if (itemIds.length === 0) {
        Metro.toast.create('Select at least one item.', null, null, "bg-orange fg-white");
        return;
    }

    // Validación rápida de UI (no es de seguridad, solo UX)
    const remainingText = ($('#RedeemRemaining').text() || '0').replace('pts', '').trim();
    const remaining = parseInt(remainingText || '0', 10);
    if (remaining < 0) {
        Metro.toast.create('Not enough points.', null, null, "bg-red fg-white");
        return;
    }

    // UI
    $('#BtnConfirmRedeem').prop('disabled', true).addClass('disabled');

    $.ajax({
        method: "POST",
        url: window.$SaveRedeemRewards,
        traditional: true, // <- CLAVE para arrays en MVC clásico
        data: {
            WorkerId: workerId,
            ItemIds: itemIds,
            Quantities: quantities
        },
        success: function (res) {
            $('#BtnConfirmRedeem').prop('disabled', false).removeClass('disabled');

            if (res && res.success) {
                Metro.toast.create(res.message || 'Redeem successful.', null, null, "bg-green fg-white");
                Metro.getPlugin("#RedeemRewardsDialog", "dialog").close();
                setTimeout(() => {
                    location.reload();
                }, 2000);
            } else {
                Metro.toast.create(res.message || 'Redeem failed.', null, null, "bg-red fg-white");
            }
        },
        error: function (xhr) {
            $('#BtnConfirmRedeem').prop('disabled', false).removeClass('disabled');
            console.error(xhr);
            Metro.toast.create('An error has occurred.', null, null, "bg-red fg-white");
        }
    });
}

function ResetPoints() {
    resetWorkerIds = $('#dttWorkerFiles tbody .rw-select:checked')
        .map(function () { return $(this).val(); })
        .get();

    if (resetWorkerIds.length === 0) {
        Metro.toast.create('You must select at least 1 worker.', null, null, "bg-orange fg-white");
        return;
    }

    var dialog = Metro.getPlugin("#ResetPointsDialog", "dialog");
    dialog.open();
}


function ResetPointsConfirm() {

    if (!resetWorkerIds || resetWorkerIds.length === 0) {
        Metro.toast.create('No workers selected.', null, null, "bg-orange fg-white");
        return;
    }

    $.ajax({
        method: "POST",
        url: window.$ResetPoints,
        traditional: true,
        data: { WorkerIds: resetWorkerIds },
        success: function (res) {
            if (res && res.success) {
                Metro.toast.create(res.message || 'Points reset successfully.', null, null, "bg-green fg-white");
                Metro.dialog.close('#ResetPointsDialog');

                setTimeout(() => {
                    location.reload();
                }, 2000);

            } else {
                Metro.toast.create(res.message || 'Could not reset points.', null, null, "bg-red fg-white");
            }
        },
        error: function (xhr) {
            console.error(xhr);
            Metro.toast.create('An error has occurred.', null, null, "bg-red fg-white");
        }
    });
}

// Rewards Inventory
$(document).ready(function () {

    // Solo inicializar si existe la tabla en esta vista
    if ($('#dttrewardsItems').length === 0) return;

    // Evitar reinicialización si navegas partials / ajax
    if ($.fn.DataTable.isDataTable('#dttrewardsItems')) {
        $('#dttrewardsItems').DataTable().destroy();
    }

    $('#dttrewardsItems').DataTable({
        dom: "<'dt-top'<'dt-left'B><'dt-center'l><'dt-right'f>>" +
            "<'dt-body'tr>" +
            "<'dt-bottom'<'dt-info'i><'dt-paging'p>>",
        buttons: [
            {
                extend: 'copyHtml5',
                text: 'Copy',
                exportOptions: { columns: [0, 1, 2, 3, 4] } // sin Actions
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                exportOptions: { columns: [0, 1, 2, 3, 4] }
            }
        ],
        responsive: true,
        lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        pageLength: 50,
        searching: true,
        info: true,
        paging: true,
        ordering: false,
        scrollY: "55vh",
        scrollCollapse: true,
        columnDefs: [
            { targets: 5, searchable: false, orderable: false } // Actions
        ],
        initComplete: function () {
            $("#RewardsItemsTable").show();
        }
    });
});

function RewardInventoryAddDialog() {
    $.ajax({
        method: "GET",
        url: window.$AddItemDialog,
        success: function (html) {
            var dialog = Metro.getPlugin("#ItemAddDialog", "dialog");
            dialog.setContent(html);
            setTimeout(function () { dialog.open(); }, 100);
        },
        error: function (xhr) {
            console.error(xhr);
            Metro.toast.create('Could not open dialog.', null, null, "bg-red fg-white");
        }
    });
}

function SubmitAddItem() {
    const error = (typeof validateItemForm === "function") ? validateItemForm() : "";
    if (error) {
        $('#ItemAddValidation').text(error).show();
        Metro.toast.create(error, null, null, "bg-orange fg-white");
        return;
    }

    // Cost: parseFloat + fallback seguro
    let costVal = ($('#Cost').val() || '').toString().trim();
    // Normaliza coma por punto por si el usuario escribe "10,50"
    costVal = costVal.replace(',', '.');
    let cost = parseFloat(costVal);
    if (isNaN(cost) || cost < 0) cost = 0;

    cost = parseFloat(cost.toFixed(2));

    const payload = {
        ItemId: 0,
        ItemName: ($('#ItemName').val() || '').trim(),
        ValuePoints: parseInt($('#ValuePoints').val() || '0', 10),
        Stock: parseInt($('#Stock').val() || '0', 10),
        Cost: cost,
        StockMin: parseInt($('#StockMin').val() || '0', 10),
        StockMax: parseInt($('#StockMax').val() || '0', 10),
        IsActive: $('#IsActive').is(':checked')
    };

    $('#Preloader').css('visibility', 'visible');
    $('#SaveRegisterRewards').prop('disabled', true).addClass('disabled');

    $.ajax({
        method: "POST",
        url: window.$SaveItem,
        data: payload, // MVC clásico (form-urlencoded)
        success: function (res) {
            $('#Preloader').css('visibility', 'hidden');
            $('#SaveRegisterRewards').prop('disabled', false).removeClass('disabled');

            if (res && res.success) {
                Metro.toast.create('Item saved successfully.', null, null, "bg-green fg-white");
                Metro.getPlugin("#ItemAddDialog", "dialog").close();
                setTimeout(() => {
                    location.reload();
                }, 2000);
            } else {
                Metro.toast.create(res.message || 'Could not save item.', null, null, "bg-red fg-white");
            }
        },
        error: function (xhr) {
            $('#Preloader').css('visibility', 'hidden');
            $('#SaveRegisterRewards').prop('disabled', false).removeClass('disabled');

            console.error(xhr);
            Metro.toast.create('An error has occurred.', null, null, "bg-red fg-white");
        }
    });
}

function DetailsRewardsInvDialog(itemId)
{
    console.log('Detalles de: ', itemId);
    $.ajax({
        method: "POST",
        url: window.$EditItemDialog,
        traditional: true,
        data: { ItemId: itemId },
        success: function (html) {
            var dialog = Metro.getPlugin("#ItemEditDialog", "dialog");
            dialog.setContent(html);
            setTimeout(function () { dialog.open(); }, 100);
        },
        error: function (xhr) {
            console.error(xhr);
            Metro.toast.create('Could not open dialog.', null, null, "bg-red fg-white");
        }
    });
}

function SubmitEditItem() {

    // Validación de negocio (la que ya tienes en el dialog)
    if (typeof validateRewardsItemBusinessRules_Edit === "function") {
        const msg = validateRewardsItemBusinessRules_Edit();
        if (msg) {
            $('#ItemEditValidation').text(msg).show();
            Metro.toast.create(msg, null, null, "bg-orange fg-white");
            return;
        }
    }

    // Cost: parseFloat + fallback seguro
    let costVal = ($('#Cost').val() || '').toString().trim();
    // Normaliza coma por punto por si el usuario escribe "10,50"
    costVal = costVal.replace(',', '.');
    let cost = parseFloat(costVal);
    if (isNaN(cost) || cost < 0) cost = 0;

    cost = parseFloat(cost.toFixed(2));

    const payload = {
        ItemId: parseInt($('#ItemId').val() || '0', 10),
        ItemName: ($('#ItemName').val() || '').trim(),
        ValuePoints: parseInt($('#ValuePoints').val() || '0', 10),
        Stock: parseInt($('#Stock').val() || '0', 10),
        Cost: cost,
        StockMin: parseInt($('#StockMin').val() || '0', 10),
        StockMax: parseInt($('#StockMax').val() || '0', 10),
        IsActive: $('#IsActive').is(':checked')
    };

    if (!payload.ItemId || payload.ItemId <= 0) {
        Metro.toast.create('Invalid ItemId.', null, null, "bg-red fg-white");
        return;
    }

    $('#Preloader').css('visibility', 'visible');
    $('#SaveRegisterRewardsItem').prop('disabled', true).addClass('disabled');

    $.ajax({
        method: "POST",
        url: window.$SaveEditItem,
        data: payload,
        success: function (res) {
            $('#Preloader').css('visibility', 'hidden');
            $('#SaveRegisterRewardsItem').prop('disabled', false).removeClass('disabled');

            if (res && res.success) {
                Metro.toast.create(res.message || 'Item updated.', null, null, "bg-green fg-white");
                Metro.getPlugin("#ItemEditDialog", "dialog").close();
                location.reload();
            } else {
                Metro.toast.create(res.message || 'Could not update item.', null, null, "bg-red fg-white");
            }
        },
        error: function (xhr) {
            $('#Preloader').css('visibility', 'hidden');
            $('#SaveRegisterRewardsItem').prop('disabled', false).removeClass('disabled');
            console.error(xhr);
            Metro.toast.create('An error has occurred.', null, null, "bg-red fg-white");
        }
    });
}

function DeleteRewardsInvDialog(ItemId) {
    if (!ItemId || ItemId <= 0) {
        Metro.toast.create('Invalid ItemId.', null, null, "bg-red fg-white");
        return;
    }

    // Confirmación simple (nativa del navegador)
    // Si prefieres Metro dialog, te lo armo también.
    if (!confirm("Are you sure you want to delete this item?")) return;

    $.ajax({
        method: "POST",
        url: window.$DeleteItem,
        data: { ItemId: ItemId },
        success: function (res) {
            if (res && res.success) {
                Metro.toast.create(res.message || 'Item deleted successfully.', null, null, "bg-green fg-white");
                location.reload(); // simple refresh
            } else {
                Metro.toast.create(res.message || 'Could not delete item.', null, null, "bg-red fg-white");
            }
        },
        error: function (xhr) {
            console.error(xhr);
            Metro.toast.create('An error has occurred.', null, null, "bg-red fg-white");
        }
    });
}