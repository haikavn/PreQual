var GenerateCustomInvoicesParams = {
    timeZone: "",
    init: function (_timeZone) {
        this.timeZone = _timeZone;
    }
};
$(function () {
    var dtNow = new Date(GenerateCustomInvoicesParams.timeZone);

    $(".daterange-buyer").daterangepicker({
        applyClass: "bg-slate-600",
        cancelClass: "btn-default",
        startDate: moment(dtNow),
        maxDate: moment(dtNow),
        singleDatePicker: true,
        locale: {
            format: "YYYY-MM-DD"
        }
    });

    $(".daterange-affiliate").daterangepicker({
        applyClass: "bg-slate-600",
        cancelClass: "btn-default",
        startDate: moment(dtNow),
        maxDate: moment(dtNow),
        singleDatePicker: true,
        locale: {
            format: "YYYY-MM-DD"
        }
    });

    $(".select-buyer").select2();
    $(".select-affiliate").select2();

    $("#Btn_GenerateBuyer").click(function () {
        if (!$("#select-buyer").val() || !$("#daterange-buyer").val()) {
            return;
        }
        $("#Loading").show();
        $("#invoice-message").hide();

        var date = $("#daterange-buyer").val();

        var data =
            "buyerid=" +
            $("#select-buyer").val() +
            "&dateto=" +
            date;

        $.post("/Management/Accounting/GenerateInvoiceAjax", data).done(function (retData) {
            $("#Loading").hide();
            if (retData > 0) {
                $("#generated-invoice-buyer").show();
                $("#InvoiceId").html(retData);
                $("#InvoiceUrl").attr("href", "/Management/Accounting/BuyerInvoiceItem/" + retData);
            } else {
                $("#invoice-message").show();
            }
        });
    });

    $("#Btn_GenerateAffiliate").click(function () {
        if (!$("#select-affiliate").val() || !$("#daterange-affiliate").val()) {
            return;
        }

        $("#Loading2").show();
        $("#invoice-message2").hide();

        var date = $("#daterange-affiliate").val();

        var data =
            "affiliateid=" +
            $("#select-affiliate").val() +
            "&dateto=" +
            date;

        $.post("/Management/Accounting/GenerateInvoiceAjax", data).done(function (retData) {
            $("#Loading2").hide();
            if (retData > 0) {
                $("#generated-invoice-affiliate").show();
                $("#InvoiceIdAff").html(retData);
                $("#InvoiceUrlAff").attr("href", "/Management/Accounting/AffiliateInvoiceItem/" + retData);
            } else {
                $("#invoice-message2").show();
            }
        });
    });

    $("#Btn_CreateBuyerBulkInvoice").click(function () {
        if (!$("#select-buyer").val() || !$("#daterange-buyer").val()) {
            alert("Plase Select Buyer and date");
            return;
        }
        var date = $("#daterange-buyer").val();

        var data =
            "buyerid=" +
            $("#select-buyer").val() +
            "&dateto=" +
            date;

        $.post("/Management/Accounting/CreateBulkInvoiceBuyer", data).done(function (retData) {
            if (retData != null) {
                document.location.href = "/Management/Accounting/BuyerInvoiceItem/" + retData;
            }
        });
    });

    $("#Btn_CreateAffiliateBulkInvoice").click(function () {
        if (!$("#select-affiliate").val() || !$("#daterange-affiliate").val()) {
            alert("Plase Select Affiliate and date");
            return;
        }

        var date = $("#daterange-affiliate").val();

        var data =
            "affiliateid=" +
            $("#select-affiliate").val() +
            "&dateto=" +
            date;

        $.post("/Management/Accounting/CreateBulkInvoiceAffiliate", data).done(function (retData) {
            if (retData != null) {
                document.location.href = "/Management/Accounting/AffiliateInvoiceItem/" + retData;
            }
        });
    });
});