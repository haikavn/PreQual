$(document).ready(function () {
    $("#add_new").click(function () {
        window.location = "/Management/PaymentSource/Type";
    });

    GenerateGridTable("PaymentSourceTypes", "/GetPaymentSourceTypes", "ID, Name");
});