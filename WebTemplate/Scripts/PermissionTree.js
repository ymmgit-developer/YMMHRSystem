$(document).ready(function () {
    //-------------- coloca los checks pertinentes a la lista Hidden
    // Cuando se clickea un abuelo o padre, confirmar los checks en hijos
    $("li > label.padre > input").change(function () {
        if ($(this).is(":checked")) {
            $(this).parent().parent().find(".taskCheckbox").each(function () {
                $(this).val("true");
            });
        } else {
            $(this).parent().parent().find(".taskCheckbox").each(function () {
                $(this).val("false");
            });
        }
    });
    // Cuando se clickea un hijo, confirmar los checks en los mismos
    $("li > label.task1 > input").change(function () {
        var hermano = $(this).parent().parent().prev(".taskList");
        if ($(this).is(":checked")) {
            $(this).parent().parent().siblings("input[id=" + hermano.attr("id") + "]").val("true");
        } else {
            $(this).parent().parent().siblings("input[id=" + hermano.attr("id") + "]").val("false");
        }
    });
    $("li > label.task2 > input").change(function () {
        var hermano = $(this).parent().parent().prev(".taskList");
        if ($(this).is(":checked")) {
            $(this).parent().parent().siblings("input[id=" + hermano.attr("id") + "]").val("true");
        } else {
            $(this).parent().parent().siblings("input[id=" + hermano.attr("id") + "]").val("false");
        }
    });
        //Revise el arbol por padres con un solo hijo, dar click al padre en caso que su hijo este checado.(Bug metro)
        $("li.tarea2 > label > input").each(function () {
            if ($(this).parent().parent().siblings("li").length === 0) {
                if ($(this).is(":checked")) {
                    $(this).parent().parent().parent().parent().children("label").children("input").click();
                }

            }
        });

        $("li.tarea1 > label > input").each(function () {
            if ($(this).parent().parent().siblings("li").length === 0) {
                if ($(this).is(":checked")) {
                    $(this).parent().parent().parent().parent().children("label").children("input").click();
                }
            }
        });

    $("li > label > input").change(function () {
        $changesMade = true;
    });

});