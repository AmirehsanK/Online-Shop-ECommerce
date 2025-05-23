(function ($) {
    "use strict";
    let THEME = {};

    /*====== Example ======*/
    THEME.Example = function () {
        // Write your code here
    };
    /*====== end Example ======*/

    $(window).on("load", function () {
    });
    $(document).ready(function () {
        THEME.Example();
    });
})(jQuery);
//KamaDatePicker
$(document).ready(function () {

    $(".persianDate").each(function () {
        kamaDatepicker($(this), {
            twodigit: true,
            closeAfterSelect: true,
            nextButtonIcon: "fa fa-arrow-circle-right",
            previousButtonIcon: "fa fa-arrow-circle-left",
            forceFarsiDigits: true,
            markToday: true,
            markHolidays: true,
            highlightSelectedDay: true,
            gotoToday: true
        });
    })

});