$(document).ready(function () {
    $(".select").select2();
});

setTimeout(function () {
    $("#CardModel_CardNumber").validateCreditCard(function (result) {
        if (result.card_type != null) {
            $(".card-type").hide();
            $("#" + result.card_type.name).show();
        } else
            $(".card-type").hide();
    });
},
    500);