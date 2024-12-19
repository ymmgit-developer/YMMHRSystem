$(document).ready(function () {
    $('#dttVacationAuthorization').DataTable({
        // Configure export buttons
        buttons: [
            {
                extend: 'copyHtml5', // Copy to clipboard
                exportOptions: {
                    columns: [0, 1, 2],  // Specify columns to export
                }
            },
            {
                extend: 'excelHtml5', // Export to Excel
                exportOptions: {
                    columns: [0, 1, 2],
                }
            },
            {
                extend: 'print', // Print the table
                exportOptions: {
                    columns: [0, 1, 2],
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

function AuthorizationVacationAddDialog()
{
    $.ajax({
        method: "POST",
        url: window.$AddRecord,
        success: function (result) {
            var dialog = Metro.getPlugin('#AuthorizationRecordAdd', 'dialog');
            dialog.setContent(result);
            setTimeout(function () { dialog.open(); }, 100);
        }
    });  
}

function SaveAuthorizationVacation()
{
    $('#SaveAuthorizationVacation').attr('disabled', true);
    $("#SaveAuthorizationVacation").removeClass('button my-control-colors');
    $("#SaveAuthorizationVacation").addClass('button');
    $("#Preloader").css("visibility", "visible");

    $.ajax({
        method: "POST",
        url: window.$SaveRecord,
        cache: false,
        data: $("#AuthorizationVacationForm").serialize(),
        success: function (result) {
            if (result.success) {
                Metro.toast.create("Record saved.", null, null, "bg-green fg-white");
                setTimeout(function () { location.reload(); }, 1000);
                var dialog = Metro.getPlugin('#AuthorizationRecordAdd', 'dialog');
                dialog.close();
                $("#Preloader").css("visibility", "hidden");
            }
            else
            {
                Metro.toast.create(result.message, null, null, "bg-red fg-white");
                $("#Preloader").css("visibility", "hidden");
            }
        }
    });
}

function ConfirmDeleteVacationsAuthorization(VacationAuthorizationsId)
{
    var dialog = Metro.getPlugin('#DeleteAuthorizationRecord', 'dialog');
    dialog.open();
    window.$VacationAuthorizationsId = VacationAuthorizationsId;
}
function DeleteAuthorizationRecord()
{
    $.ajax({
        method: "POST",
        url: window.$DeleteRecord,
        data: { vacationAuthorizationsId: window.$VacationAuthorizationsId },
        success: function (result) {
            if (result.success) {
                Metro.toast.create("Room deleted.", null, null, "bg-green fg-white");
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                Metro.toast.create(result.message, null, null, "bg-red fg-white");
            }

        }
    });
}

function SelectUsers(names, process, id) {
    console.warn("Table Users: " + tableUsers.value);
    var same;
    $("#dttUserList td").each(function () {
        if ($(this).text() === names) {
            same = "true";
        }
    });
    if (same === "true") {
        Metro.toast.create("User already added.", null, null, "bg-red fg-white");
    } else {
        tableUsers.row($('#' + id)).remove().draw();

        var node = table_User.row.add([names, process, '<a class="button small bg-red fg-white" style="cursor: pointer;" onclick="RemoveUser(\'' + names + '\')"><span class="mif-bin"></span></a>']).draw(false).node();
        $(node).css('text-align', 'center');
        $("#dttUserList").find("tr").last().append("<input type='hidden' name='UserList[" + ($("#dttUserList").find("tr").length - 2) + "].Names' value='" + names + "'>");
        $("#dttUserList").find("tr").last().append("<input type='hidden' name='UserList[" + ($("#dttUserList").find("tr").length - 2) + "].Process' value='" + process + "'>");
        $("#dttUserList").find("tr").last().append("<input type='hidden' name='UserList[" + ($("#dttUserList").find("tr").length - 2) + "].WorkerId' value='" + id + "'>");
        Metro.toast.create("User added.", null, null, "bg-green fg-white");
    }
}

function SelectAssociates(names, process, id) {
    console.warn("Table Associates: " + tableAssociates.value);
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

        var node = table_Associate.row.add([names, process, '<a class="button small bg-red fg-white" style="cursor: pointer;" onclick="RemoveAssociate(\'' + names + '\')"><span class="mif-bin"></span></a>']).draw(false).node();
        $(node).css('text-align', 'center');
        $("#dttAssociateList").find("tr").last().append("<input type='hidden' name='WorkerList[" + ($("#dttAssociateList").find("tr").length - 2) + "].Names' value='" + names + "'>");
        $("#dttAssociateList").find("tr").last().append("<input type='hidden' name='WorkerList[" + ($("#dttAssociateList").find("tr").length - 2) + "].Process' value='" + process + "'>");
        $("#dttAssociateList").find("tr").last().append("<input type='hidden' name='WorkerList[" + ($("#dttAssociateList").find("tr").length - 2) + "].WorkerFileId' value='" + id + "'>");
        Metro.toast.create("Associate added.", null, null, "bg-green fg-white");
    }
}
