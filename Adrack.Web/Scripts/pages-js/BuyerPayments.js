var BuyerPaymentsParams = {
    timeZone: "",
    init: function (_timeZone) {
        this.timeZone = _timeZone;
    }
};

var actionsArr = [
    {
        Name: "Edit",
        Url: "",
        Class: "ActionEditBtn",
        Modal: "modal_form_edit_payment",
        IconClass: "glyphicon glyphicon-edit green",
        Confirm: 0
    },
    { Name: "Detete", Url: "/Accounting/DeleteBuyerPayment", IconClass: "glyphicon glyphicon-remove red", Confirm: 1 }
];

$(function () {
    var dtNow = new Date(BuyerPaymentsParams.timeZone);
    $(".daterange-single").daterangepicker({
        singleDatePicker: true,
        startDate: moment(dtNow),
        maxDate: moment(dtNow).subtract(-1, "days"),
        locale: {
            format: "MM/DD/YYYY"
        }
    });

    $(".select-search").select2();

    $("#AddPaymentBtn").click(function () {
        $("#PaymentDate").val("");
        $("#PaymentAffiliate").val("");
        $("#PaymentAmount").val("");
        $("#PaymentMethodSelect").val("");
        $("#PaymentNote").val("");
    });

    $("#Btn_AddPayment").click(function () {
        var data =
            "date=" +
            $("#PaymentDate").val() +
            "&affiliateid=" +
            $("#PaymentAffiliate").val() +
            "&amount=" +
            $("#PaymentAmount").val() +
            "&method=" +
            $("#PaymentMethodSelect").val() +
            "&note=" +
            encodeURI($("#PaymentNote").val());

        $.post("/Management/Accounting/AddBuyerPayment", data).done(function (retData) {
            GenerateGridTable("GridViewPayments",
                "/Management/Accounting/GetBuyerPayments",
                "ID, Payment Date, Buyer ID # Name, Amount, Payment Method, Note, Created, User ID # Name, Actions");
        });
    });

    GenerateGridTable("GridViewPayments",
        "/Management/Accounting/GetBuyerPayments",
        "ID, Payment Date, Buyer ID # Name, Amount, Payment Method, Note, Created, User ID # Name, Actions",
        actionsArr);
});