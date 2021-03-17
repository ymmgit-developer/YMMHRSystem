function SaveExtraDinerContacts() {

    var dialog = Metro.getPlugin('#preloader', 'dialog');
    dialog.open();
    var contacts = $("#ExtraDinerContacts").val();

    $.ajax({
        method: "POST",
        url: window.$SaveExtraDinerContacts,
        cache: false,
        data: { contacts: contacts },
        success: function (result) {
            dialog.close();
            if (result !== "false") {
                Metro.toast.create("Contacts saved.", null, null, "bg-green fg-white");
            } else {
                Metro.toast.create("Please enter correct email addresses / Please do not repeat a contact.", null, null, "bg-red fg-white");
            }
        }
    });

}

function SaveExtraTransportContacts() {

    var dialog = Metro.getPlugin('#preloader', 'dialog');
    dialog.open();
    var contacts = $("#ExtraTransportContacts").val();

    $.ajax({
        method: "POST",
        url: window.$SaveExtraTransportContacts,
        cache: false,
        data: { contacts: contacts },
        success: function (result) {
            dialog.close();
            if (result !== "false") {
                Metro.toast.create("Contacts saved.", null, null, "bg-green fg-white");
            } else {
                Metro.toast.create("Please enter correct email addresses / Please do not repeat a contact.", null, null, "bg-red fg-white");
            }
        }
    });

}

function SaveLegalRequirementContacts() {

    var dialog = Metro.getPlugin('#preloader', 'dialog');
    dialog.open();
    var contacts = $("#LegalRequirementContacts").val();

    $.ajax({
        method: "POST",
        url: window.$SaveLegalRequirementContacts,
        cache: false,
        data: { contacts: contacts },
        success: function (result) {
            dialog.close();
            if (result !== "false") {
                Metro.toast.create("Contacts saved.", null, null, "bg-green fg-white");
            } else {
                Metro.toast.create("Please enter correct email addresses / Please do not repeat a contact.", null, null, "bg-red fg-white");
            }
        }
    });

}

function SaveVehicleContacts() {

    var dialog = Metro.getPlugin('#preloader', 'dialog');
    dialog.open();
    var contacts = $("#VehicleContacts").val();

    $.ajax({
        method: "POST",
        url: window.$SaveVehicleContacts,
        cache: false,
        data: { contacts: contacts },
        success: function (result) {
            dialog.close();
            if (result !== "false") {
                Metro.toast.create("Contacts saved.", null, null, "bg-green fg-white");
            } else {
                Metro.toast.create("Please enter correct email addresses / Please do not repeat a contact.", null, null, "bg-red fg-white");
            }
        }
    });

}

function SaveUtilityCarContacts() {

    var dialog = Metro.getPlugin('#preloader', 'dialog');
    dialog.open();
    var contacts = $("#UtilityCarContacts").val();

    $.ajax({
        method: "POST",
        url: window.$SaveUtilityCarContacts,
        cache: false,
        data: { contacts: contacts },
        success: function (result) {
            dialog.close();
            if (result !== "false") {
                Metro.toast.create("Contacts saved.", null, null, "bg-green fg-white");
            } else {
                Metro.toast.create("Please enter correct email addresses / Please do not repeat a contact.", null, null, "bg-red fg-white");
            }
        }
    });

}

function SaveTranslationContacts() {

    var dialog = Metro.getPlugin('#preloader', 'dialog');
    dialog.open();
    var contacts = $("#TranslationContacts").val();

    $.ajax({
        method: "POST",
        url: window.$SaveTranslationContacts,
        cache: false,
        data: { contacts: contacts },
        success: function (result) {
            dialog.close();
            if (result !== "false") {
                Metro.toast.create("Contacts saved.", null, null, "bg-green fg-white");
            } else {
                Metro.toast.create("Please enter correct email addresses / Please do not repeat a contact.", null, null, "bg-red fg-white");
            }
        }
    });

}

function SaveCoffeeBreakContacts() {

    var dialog = Metro.getPlugin('#preloader', 'dialog');
    dialog.open();
    var contacts = $("#CoffeeBreakContacts").val();

    $.ajax({
        method: "POST",
        url: window.$SaveCoffeeBreakContacts,
        cache: false,
        data: { contacts: contacts },
        success: function (result) {
            dialog.close();
            if (result !== "false") {
                Metro.toast.create("Contacts saved.", null, null, "bg-green fg-white");
            } else {
                Metro.toast.create("Please enter correct email addresses / Please do not repeat a contact.", null, null, "bg-red fg-white");
            }
        }
    });

}

function SaveBusinessTripContacts() {

    var dialog = Metro.getPlugin('#preloader', 'dialog');
    dialog.open();
    var contacts = $("#BusinessTripContacts").val();

    $.ajax({
        method: "POST",
        url: window.$SaveBusinessTripContacts,
        cache: false,
        data: { contacts: contacts },
        success: function (result) {
            dialog.close();
            if (result !== "false") {
                Metro.toast.create("Contacts saved.", null, null, "bg-green fg-white");
            } else {
                Metro.toast.create("Please enter correct email addresses / Please do not repeat a contact.", null, null, "bg-red fg-white");
            }
        }
    });

}

function SaveLegalAffairContacts() {

    var dialog = Metro.getPlugin('#preloader', 'dialog');
    dialog.open();
    var contacts = $("#LegalAffairContacts").val();

    $.ajax({
        method: "POST",
        url: window.$SaveLegalAffairContacts,
        cache: false,
        data: { contacts: contacts },
        success: function (result) {
            dialog.close();
            if (result !== "false") {
                Metro.toast.create("Contacts saved.", null, null, "bg-green fg-white");
            } else {
                Metro.toast.create("Please enter correct email addresses / Please do not repeat a contact.", null, null, "bg-red fg-white");
            }
        }
    });

}

function SaveGiftContacts() {

    var dialog = Metro.getPlugin('#preloader', 'dialog');
    dialog.open();
    var contacts = $("#GiftContacts").val();

    $.ajax({
        method: "POST",
        url: window.$SaveGiftContacts,
        cache: false,
        data: { contacts: contacts },
        success: function (result) {
            dialog.close();
            if (result !== "false") {
                Metro.toast.create("Contacts saved.", null, null, "bg-green fg-white");
            } else {
                Metro.toast.create("Please enter correct email addresses / Please do not repeat a contact.", null, null, "bg-red fg-white");
            }
        }
    });

}
