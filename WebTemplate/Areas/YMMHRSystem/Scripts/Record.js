function ExportWorkerRecord() {
    Metro.dialog.open('#preloader');
    var status = $("#Status option:selected").val();
    window.location = $ExportWorkerRecord + "?status=" + status;
}

function ExportTranslations() {
    var startDate = $("#StartDate2").val();
    var endDate = $("#EndDate2").val();
    window.location = $ExportTranslations + "?startDate=" + startDate + "&endDate=" + endDate;
}

function ExportDiners() {
    var startDate = $("#StartDate3").val();
    var endDate = $("#EndDate3").val();
    window.location = $ExportDiners + "?startDate=" + startDate + "&endDate=" + endDate;
}

function ExportExtraDiners() {
    var startDate = $("#StartDate4").val();
    var endDate = $("#EndDate4").val();
    window.location = $ExportExtraDiners + "?startDate=" + startDate + "&endDate=" + endDate;
}

function ExportTransports() {
    var startDate = $("#StartDate5").val();
    var endDate = $("#EndDate5").val();
    window.location = $ExportTransports + "?startDate=" + startDate + "&endDate=" + endDate;
}

function ExportExtraTransports() {
    var startDate = $("#StartDate6").val();
    var endDate = $("#EndDate6").val();
    window.location = $ExportExtraTransports + "?startDate=" + startDate + "&endDate=" + endDate;
}

function ExportLegalRequirements() {
    var startDate = $("#StartDate7").val();
    var endDate = $("#EndDate7").val();
    window.location = $ExportLegalRequirements + "?startDate=" + startDate + "&endDate=" + endDate;
}


