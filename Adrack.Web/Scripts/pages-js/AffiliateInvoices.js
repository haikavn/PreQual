var AffiliateInvoicesParams = {
    timeZone: "",
    init: function (_timeZone) {
        this.timeZone = _timeZone;
    }
};

$(document).ready(function () {
    $(".select-search-buyers").select2();

    var dtNow = new Date(AffiliateInvoicesParams.timeZone);
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

    $("#AllAffiliatesList, .daterange, #status-filter").change(function () {
        var dates2 = $(".daterange").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }

        var dates2 = $(".daterange").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }
        var dataParam = "?dates=" +
            dates2[0].trim() +
            ":" +
            dates2[1].trim() +
            "&filteraffiliateid=" +
            $("#AllAffiliatesList").val() +
            "&statfilter=" +
            $("#status-filter").val();

        GenerateGridTableAffInvoice("GridView1",
            "/Accounting/GetAffiliateInvoices/" + dataParam,
            "ID, Invoice Number, Affiliate Name, From / To, Date Created, Total, Paid, Outstanding, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
    });

    $("#ShowAll").click(function () {
        $(".daterange").val("");
        $("#status-filter").val("-2");
        $("#AllAffiliatesList").val("0").trigger("change");

        GenerateGridTableAffInvoice("GridView1",
            "/Accounting/GetAffiliateInvoices/",
            "ID, Invoice Number, Affiliate Name, From / To, Date Created, Total, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
    });

    $("#status-filter").change(function () {
        GenerateGridTableAffInvoice("GridView1",
            "/Accounting/GetAffiliateInvoices/?statfilter=" + $("#status-filter").val(),
            "ID, Invoice Number, Affiliate Name, From / To, Date Created, Total, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
    });
});

var SelectedRowIndex = -1;

function GenerateGridTableAffInvoice(objID, dataPath, Fields, ActionsArr, Page, Pagesize, Params, FilterSort) {
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
                /* scrollY: contentHeight, */
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
            "&filteraffiliateid=" +
            $("#AllAffiliatesList").val() +
            "&statfilter=" +
            $("#status-filter").val();

        var data = "id=" + $(this).data("id");
        $.post("/Accounting/ApproveAffiliateInvoice", data).done(function (retData) {
            GenerateGridTableAffInvoice("GridView1",
                "/Accounting/GetAffiliateInvoices/" + dataParam,
                "ID, Invoice Number, Affiliate Name, From / To, Date Created, Total, Paid, Outstanding, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
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
            "&filteraffiliateid=" +
            $("#AllAffiliatesList").val() +
            "&statfilter=" +
            $("#status-filter").val();

        var data = "id=" + $(this).data("id") + "&status=0";
        $.post("/Accounting/AffiliateInvoiceChangeStatus", data).done(function (retData) {
            GenerateGridTableAffInvoice("GridView1",
                "/Accounting/GetAffiliateInvoices/" + dataParam,
                "ID, Invoice Number, Affiliate Name, From / To, Date Created, Total, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
        });
    });

    $(".BtnUnpaid").click(function () {
        if (confirm(
            "Are you sure?\nPlease confirm.\nThis invoice was indicated as a Paid and included in accounting reports.\nChanging of the status will impact current balances.") ==
            false) {
            return false;
        }

        var data = "id=" + $(this).data("id") + "&amount=" + 0;
        $.post("/Accounting/AddAffiliateInvoicePayment", data).done(function (retData) {
        });

        var dates2 = $(".daterange").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }
        var dataParam = "?dates=" +
            dates2[0].trim() +
            ":" +
            dates2[1].trim() +
            "&filteraffiliateid=" +
            $("#AllAffiliatesList").val() +
            "&statfilter=" +
            $("#status-filter").val();

        var data = "id=" + $(this).data("id") + "&status=1";
        $.post("/Accounting/AffiliateInvoiceChangeStatus", data).done(function (retData) {
            GenerateGridTableAffInvoice("GridView1",
                "/Accounting/GetAffiliateInvoices/" + dataParam,
                "ID, Invoice Number, Affiliate Name, From / To, Date Created, Total, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
        });
    });

    $(".BtnPaid").click(function () {
        var dates2 = $(".daterange").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }
        var dataParam = "?dates=" +
            dates2[0].trim() +
            ":" +
            dates2[1].trim() +
            "&filteraffiliateid=" +
            $("#AllAffiliatesList").val() +
            "&statfilter=" +
            $("#status-filter").val();

        var data = "id=" + $(this).data("id") + "&amount=" + $(this).data("total");
        $.post("/Accounting/AddAffiliateInvoicePayment", data).done(function (retData) {
            GenerateGridTableAffInvoice("GridView1",
                "/Accounting/GetAffiliateInvoices/" + dataParam,
                "ID, Invoice Number, Affiliate Name, From / To, Date Created, Total, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
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
            "&filteraffiliateid=" +
            $("#AllAffiliatesList").val() +
            "&statfilter=" +
            $("#status-filter").val();

        var data = "id=" + $(this).data("id");
        $.post("/Accounting/DisableAffiliateInvoice", data).done(function (retData) {
            GenerateGridTableAffInvoice("GridView1",
                "/Accounting/GetAffiliateInvoices/" + dataParam,
                "ID, Invoice Number, Affiliate Name, From / To, Date Created, Total, Paid, Outstanding, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
        });
    });

    $(".BtnDownload").click(function () {
        window.open("/Management/Accounting/PdfAffiliate/" + $(this).data("id"));
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
        "&filteraffiliateid=" +
        $("#AllAffiliatesList").val() +
        "&statfilter=" +
        $("#status-filter").val();
    GenerateGridTableAffInvoice("GridView1",
        "/Accounting/GetAffiliateInvoices/" + dataParam,
        "ID, Invoice Number, Affiliate Name, From / To, Date Created, Total, Paid, Outstanding, Actions, Status, <i class='glyphicon glyphicon-download-alt'></i>");
});