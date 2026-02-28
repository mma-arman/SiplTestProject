$(document).ready(function () {
        $(".EnterPressNext").on("keydown", "input,select,textarea,button", function (e) {
            if (e.which == 13) {
                e.preventDefault();
                var inputs = $(this).closest(".EnterPressNext")
                    .find("input,select,textarea,button")
                    .filter(":visible:not([readonly]):not([disabled])");

                var index = inputs.index(this);

                if ($(this).hasClass("execute")) {
                    $(this).trigger("click"); 
                    return;
                }
                if (index >= 0 && index + 1 < inputs.length) {
                    inputs.eq(index + 1).focus();
                }
            }
        });
});


//For Auto Complete InputTextBox
function AutoComplete(ID, Type,typeSearch) {

    typeSearch = $("#" + typeSearch).val();
    $("#" + ID).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/AutoComplete/Autocomplete",
                type: "POST",
                dataType: "json",
                data: {
                    Search: request.term,
                    Type: Type,
                    typeSearch: typeSearch
                },
                success: function (data) {
                    console.log(data);
                    response(data);
                },
                error: function () {
                    alert("error");
                    response([]);
                }
            });

        },
        minLength: 0
    });
}








