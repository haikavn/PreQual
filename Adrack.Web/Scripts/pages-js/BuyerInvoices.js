var BuyerInvoiceParams = {
    timeZone: "",
    init: function (_timeZone) {
        this.timeZone = _timeZone;
    }
};

$(function () {
    $(".select-search-buyers").select2();

    var dtNow = new Date(BuyerInvoiceParams.timeZone);
    $(".daterange").daterangepicker({
        applyClass: "bg-slate-600",
        cancelClass: "btn-default",
        startDate: moment(dtNow).subtract(29, "days"),
        endDate: moment(dtNow),
        maxDate: moment(dtNow).subtract(-1, "days"),
        opens: "right",
        ranges: {
            'Today': [moment(dtNow), moment(dtNow)],
            'Yesterday': [moment(dtNow).subtract(1, "days"), moment(dtNow).subtract(1, "days")],
            'Last 7 Days': [moment(dtNow).subtract(6, "days"), moment(dtNow)],
            'Last 30 Days': [moment(dtNow).subtract(29, "days"), moment(dtNow)],
            'This Month': [moment(dtNow).startOf("month"), moment(dtNow).endOf("month")],
            'Last Month': [
                moment(dtNow).subtract(1, "month").startOf("month"), moment(dtNow).subtract(1, "month").endOf("month")
            ],
            'Last 12 Month': [moment(dtNow).subtract(12, "month"), moment(dtNow)]
        },
        locale: {
            format: "MM/DD/YYYY"
        }
    });

    $(".daterange").focusout(function () {
        $(".daterange").trigger("apply.daterangepicker");
    });

    $(".daterange").on("apply.daterangepicker",
        function (ev, picker) {
            return;
            var dates2 = $(".daterange").val().split("-");
            var d1 = new Date(dates2[0].replace(/ /g, ""));
            if (dates2[1] != undefined) {
                var d2 = new Date(dates2[1].replace(/ /g, ""));

                if (d2.toDateString() == d1.toDateString()) {
                    $(".daterange").val((d1.getMonth() + 1) + "/" + d1.getDate() + "/" + d1.getFullYear());
                }
            }
        });
    $(".daterange").focusout(function () {
        $(".daterange").trigger("apply.daterangepicker");
    });
    $(".daterange").trigger("apply.daterangepicker");

    /*
            $("#Btn_PayInvoice").click(function () {
                if (!GridSelectedID) {
                    alert("Plaese Select a Row");
                    return false;
                }

                if (parseInt($("#PaymentAmount").val()) > parseInt($("#distribution").text()) )
                {
                    alert("Inserted Amount bigger than Avaliable for Distribution");
                    return false;
                }

                if ( parseInt($("#PaymentAmount").val()) > parseInt($("#total-sum").text()) )
                {
                    alert("Inserted Amount bigger than Invoice Total");
                    return false;
                }

                var data = 'id=' + GridSelectedID + '&amount=' + $("#PaymentAmount").val();
                $.post("/Accounting/AddBuyerInvoicePayment", data).done(function (retData) {
                    GenerateGridTableBuyer("GridViewBuyersInvoices", "/Management/Accounting/GetBuyerInvoices", "ID, Num, Buyer, From / To, Created, Sum, Refunded, Adjust, Total, Paid, Outstanding, Distrib, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
                });
            });

            $("#InvoicePay").click(function () {
                var TotalSum = $("tbody .selected td:nth-child(9)").text().replace("$", "");
                var Status = $("tbody .selected td:nth-child(13)").text();

                if (Status == "Deleted" || Status == "NotApproved")
                {
                    alert("You Cannot Pay " + Status + " Invoice");
                    return false;
                }

                $("#total-sum").text(TotalSum);

                $("#PaymentAmount").val(TotalSum);

                var buyerHtml = $($("tbody .selected td:nth-child(5)").html());

                var data = 'BuyerId=' + buyerHtml.data("buyerid"); // BuyerId

                $.post("/Accounting/GetBuyerDistrib", data).done(function (retData) {
                    $("#distribution").text(retData);
                });
            });

            $("#InvoiceRefresh").click(function () {
                var dates2 = $('.daterange').val().split('-');
                if (dates2[1] == undefined) {
                    dates2[1] = dates2[0];
                }

                var dataParam = "?dates=" + dates2[0].trim() + ":" + dates2[1].trim() + "&filterbuyerid=" + $("#AllBuyersList").val();
                GenerateGridTableBuyer("GridViewBuyersInvoices", "/Management/Accounting/GetBuyerInvoices" + dataParam, "ID, Num, Buyer, From / To, Created, Total, Distrib, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
            });
    */
    $("#AllBuyersList, .daterange, #status-filter").change(function () {
        var dates2 = $(".daterange").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }

        var dataParam = "?dates=" +
            dates2[0].trim() +
            ":" +
            dates2[1].trim() +
            "&filterbuyerid=" +
            $("#AllBuyersList").val() +
            "&statfilter=" +
            $("#status-filter").val();
        GenerateGridTableBuyer("GridViewBuyersInvoices",
            "/Management/Accounting/GetBuyerInvoices" + dataParam,
            "ID, Num, Buyer, From / To, Created, Total, Distrib, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
    });

    $("#ShowAll").click(function () {
        $(".daterange").val("");
        $("#status-filter").val("-2");
        $("#AllBuyersList").val("0").trigger("change");

        GenerateGridTableBuyer("GridViewBuyersInvoices",
            "/Management/Accounting/GetBuyerInvoices",
            "ID, Num, Buyer, From / To, Created, Total, Distrib, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
    });
});

var SelectedRowIndex = -1;

function GenerateGridTableBuyer(objID, dataPath, Fields, ActionsArr, Page, Pagesize, Params, FilterSort) {
    var ScroolPos = $(document).scrollTop();

    if (FilterSort == undefined) {
        FilterSort = true;
    }

    $("#GridPageNumber").val(Page);

    Pagesize = Pagesize == null ? 25 : Pagesize;

    GridParams = { _objID: objID, _dataPath: dataPath, _Fields: Fields, _ActionsArr: ActionsArr };

    $(document).ready(function () {
        var fields = Fields.split(",");
        var tableStr = '<table id="tbl_' +
            objID +
            '" class="display table datatable-html dataTable"  role="grid" aria-describedby="DataTables_Table_0_info" cellspacing="0" width="100%"><thead><tr>';
        fields.forEach(function (item, i, arr) {
            tableStr += "<th>" + item.trim() + "</th>";
        });

        tableStr += "</tr></thead></table>";

        $("#" + objID).empty();
        $("#" + objID).append(tableStr);

        var contentHeight = $(window).height() -
            $("body > .navbar").outerHeight() -
            $("body > .navbar-fixed-top:not(.navbar)").outerHeight() -
            $("body > .navbar-fixed-bottom:not(.navbar)").outerHeight() -
            $("body > .navbar + .navbar").outerHeight() -
            $("body > .navbar + .navbar-collapse").outerHeight() -
            260;

        var data_table =
            $("#tbl_" + objID).dataTable({
                autoWidth: true,
                /*scrollY: contentHeight,*/
                "order": [[0, "desc"]],
                "searching": false,
                "ordering": FilterSort,
                "processing": true,
                "serverSide": false,
                "iDisplayLength": Pagesize,
                "paging": Page != null ? false : FilterSort,
                "info": Page != null ? false : FilterSort,

                ajax: {
                    url: dataPath,
                    data: {
                        "actions": JSON.stringify(ActionsArr),
                        "page": Page,
                        "pagesize": Pagesize,
                        "params": Params
                    },
                    processData: true,
                    dataType: "json",
                    type: "POST"
                },

                "fnDrawCallback": function (oSettings) {
                    var api = this.api();
                    var jsonD = api.ajax.json();

                    if (jsonD != undefined && jsonD.totalsSumStr != undefined) {
                        $("#InvoicedSum").html(jsonD.totalsSumStr[0]);
                        $("#ApprovedSum").html(jsonD.totalsSumStr[1]);
                        $("#PaidSum").html(jsonD.totalsSumStr[2]);

                        if (jsonD.totalsSumStr[2].indexOf("-") != -1) {
                            $("#PaidSum").addClass("text-danger");
                        }
                    }

                    SetButtonEvents();

                    if (SelectedRowIndex != -1) {
                        $(".datatable-html tbody tr:eq(" + SelectedRowIndex + ")").addClass("selected");
                    }
                    $(document).scrollTop(ScroolPos);
                }
            });

        if (Page != null) {
            GenerateGridPagination();
        }

        $(".datatable-html tbody").on("click",
            "tr",
            function () {
                $(".datatable-html tbody tr").removeClass("selected");
                $(this).toggleClass("selected");

                GridSelectedID = $(this).find("td:eq(0)").html();

                if (isNaN(parseInt(GridSelectedID))) {
                    GridSelectedID = $(this).find("td:eq(1)").html();
                }

                SelectedRowIndex = $(this).index();
            });
    });
}

function SetButtonEvents() {
    $(".BtnApprove").click(function () {
        var dates2 = $(".daterange").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }
        var dataParam = "?dates=" +
            dates2[0].trim() +
            ":" +
            dates2[1].trim() +
            "&filterbuyerid=" +
            $("#AllBuyersList").val() +
            "&statfilter=" +
            $("#status-filter").val();

        var data = "id=" + $(this).data("id");
        $.post("/Accounting/ApproveBuyerInvoice", data).done(function (retData) {
            GenerateGridTableBuyer("GridViewBuyersInvoices",
                "/Management/Accounting/GetBuyerInvoices" + dataParam,
                "ID, Num, Buyer, From / To, Created, Total, Distrib, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
        });
    });

    $(".BtnDisapprove").click(function () {
        if (confirm(
            "Are you sure?\n Please confirm.\n This document was indicated as an Approved and all data is included in accounting reports.\nChanging of the status will impact current balances.") ==
            false) {
            return false;
        }

        var dates2 = $(".daterange").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }
        var dataParam = "?dates=" +
            dates2[0].trim() +
            ":" +
            dates2[1].trim() +
            "&filterbuyerid=" +
            $("#AllBuyersList").val() +
            "&statfilter=" +
            $("#status-filter").val();

        var data = "id=" + $(this).data("id") + "&status=0";
        $.post("/Accounting/BuyerInvoiceChangeStatus", data).done(function (retData) {
            GenerateGridTableBuyer("GridViewBuyersInvoices",
                "/Management/Accounting/GetBuyerInvoices" + dataParam,
                "ID, Num, Buyer, From / To, Created, Total, Distrib, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
        });
    });

    $(".BtnUnpaid").click(function () {
        if (confirm(
            "Are you sure?\nPlease confirm.\nThis invoice was indicated as a Paid and included in accounting reports.\nChanging of the status will impact current balances.") ==
            false) {
            return false;
        }

        var data = "id=" + $(this).data("id") + "&amount=0";
        $.post("/Accounting/AddBuyerInvoicePayment", data).done(function (retData) {
        });

        var dates2 = $(".daterange").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }
        var dataParam = "?dates=" +
            dates2[0].trim() +
            ":" +
            dates2[1].trim() +
            "&filterbuyerid=" +
            $("#AllBuyersList").val() +
            "&statfilter=" +
            $("#status-filter").val();

        var data = "id=" + $(this).data("id") + "&status=1";
        $.post("/Accounting/BuyerInvoiceChangeStatus", data).done(function (retData) {
            GenerateGridTableBuyer("GridViewBuyersInvoices",
                "/Management/Accounting/GetBuyerInvoices" + dataParam,
                "ID, Num, Buyer, From / To, Created, Total, Distrib, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
        });
    });

    $(".BtnPaid").click(function () {
        var totalsSum = $(this).data("total");
        var distrib = $(this).data("distrib");
        var invid = $(this).data("id");

        if (parseFloat(distrib) < parseFloat(totalsSum)) {
            alert("Not enough money");
            return false;
        }

        var dates2 = $(".daterange").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }
        var dataParam = "?dates=" +
            dates2[0].trim() +
            ":" +
            dates2[1].trim() +
            "&filterbuyerid=" +
            $("#AllBuyersList").val() +
            "&statfilter=" +
            $("#status-filter").val();

        var data = "id=" + invid + "&amount=" + totalsSum;
        $.post("/Accounting/AddBuyerInvoicePayment", data).done(function (retData) {
            GenerateGridTableBuyer("GridViewBuyersInvoices",
                "/Management/Accounting/GetBuyerInvoices" + dataParam,
                "ID, Num, Buyer, From / To, Created, Total, Distrib, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
        });
    });

    $(".BtnDelete").click(function () {
        if (confirm(
            "Are you sure?\nPlease confirm. /nIf you delete this document all numbers will be included in the next billing") ==
            false) {
            return false;
        }

        var dates2 = $(".daterange").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }
        var dataParam = "?dates=" +
            dates2[0].trim() +
            ":" +
            dates2[1].trim() +
            "&filterbuyerid=" +
            $("#AllBuyersList").val() +
            "&statfilter=" +
            $("#status-filter").val();

        var data = "id=" + $(this).data("id");
        $.post("/Accounting/DisableBuyerInvoice", data).done(function (retData) {
            GenerateGridTableBuyer("GridViewBuyersInvoices",
                "/Management/Accounting/GetBuyerInvoices" + dataParam,
                "ID, Num, Buyer, From / To, Created, Total, Distrib, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
        });
    });

    $(".BtnDownload").click(function () {
        window.open("/Management/Accounting/Pdf/" + $(this).data("id"));
    });
}

$(document).ready(function () {
    var dates2 = $(".daterange").val().split("-");
    if (dates2[1] == undefined) {
        dates2[1] = dates2[0];
    }
    var dataParam = "?dates=" +
        dates2[0].trim() +
        ":" +
        dates2[1].trim() +
        "&filterbuyerid=" +
        $("#AllBuyersList").val() +
        "&statfilter=" +
        $("#status-filter").val();

    GenerateGridTableBuyer("GridViewBuyersInvoices",
        "/Management/Accounting/GetBuyerInvoices" + dataParam,
        "ID, Num, Buyer, From / To, Created, Total, Distrib, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
});