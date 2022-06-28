var AffiliatesBalanceParams = {
    timeZone: "",
    init: function (_timeZone) {
        this.timeZone = _timeZone;
    }
};

function GenerateGridBalance(objID, dataPath, Fields, Page, Pagesize, Params, FilterSort) {
    if (FilterSort == undefined) {
        FilterSort = true;
    }

    Pagesize = Pagesize == null ? 1000 : Pagesize;

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
                "order": [],
                "searching": false,
                "ordering": FilterSort,
                "processing": true,
                "serverSide": false,
                "iDisplayLength": Pagesize,
                "paging": false,
                "info": false,
                "bAutoWidth": false,

                ajax: {
                    url: dataPath,
                    data: { "page": Page, "pagesize": Pagesize, "params": Params },
                    processData: true,
                    dataType: "json",
                    type: "POST"
                },

                "fnDrawCallback": function (oSettings) {
                    var api = this.api();
                    var jsonD = api.ajax.json();

                    if (jsonD != undefined && jsonD.recordsSum != undefined) {
                        var api = this.api();
                        var jsonD = api.ajax.json();

                        if (jsonD != undefined && jsonD.recordsSum != undefined) {
                            footerStr = "";
                            fields.forEach(function (item, i, arr) {
                                footerStr += '<td style="font-weight:bold; text-align: right; width:14.2% "><p>' +
                                    jsonD.totalsSumStr[i] +
                                    "</p></td>";
                            });

                            $(".footer-totals").remove();
                            $("#tbl_" + objID)
                                .after('<table class="footer-totals"><tfoot>' + footerStr + "</tfoot></table>");
                        }

                        $(".datatable-html tr").each(function () {
                            $(this).find("td:eq(1)").addClass("cell-color1");
                            $(this).find("td:eq(2)").addClass("cell-color2");
                            $(this).find("td:eq(3)").addClass("cell-color2");
                            $(this).find("td:eq(4)").addClass("cell-color2");
                            $(this).find("td:eq(5)").addClass("cell-color2");
                            $(this).find("td:eq(6)").addClass("cell-color3");
                        });
                    }
                }
            });

        $(".datatable-html tbody").on("click",
            "tr",
            function () {
                $(".datatable-html tbody tr").removeClass("selected");
                $(this).toggleClass("selected");

                GridSelectedID = $(this).find("td:eq(0)").html();

                if (isNaN(parseInt(GridSelectedID))) {
                    GridSelectedID = $(this).find("td:eq(1)").html();
                }
            });
    });
}

$(function () {
    $(".datatable-sorting1").DataTable({
        order: [[3, "desc"]],
        bProcessing: false,
        serverSide: false,
        bPaginate: false, //hide pagination
        bFilter: false, //hide Search bar
        bInfo: false
    });

    var dtNow = new Date(AffiliatesBalanceParams.timeZone);
    $(".daterange").daterangepicker({
        applyClass: "bg-slate-600",
        cancelClass: "btn-default",
        startDate: moment(dtNow),
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

    $(".select-all-buyers").select2();

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

    $("#AllBuyersList").change(function () {
        var dates2 = $(".daterange").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }

        var dataParam = "dates=" + dates2[0].trim() + ":" + dates2[1].trim() + "&buyerid=" + $("#AllBuyersList").val();

        GenerateGridBalance("GridView_AffiliatesBalances",
            "/Management/Accounting/GetAffiliatesBalanceAjax/?" + dataParam,
            "Affiliate Name, Initial Balance, Sold Sum, Payment Notices, Payments, Balance, Final Balance");
    });

    $("#BalanceRefresh").click(function () {
        var dates2 = $(".daterange").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }

        var dataParam = "dates=" + dates2[0].trim() + ":" + dates2[1].trim() + "&buyerid=" + $("#AllBuyersList").val();
        GenerateGridBalance("GridView_AffiliatesBalances",
            "/Management/Accounting/GetAffiliatesBalanceAjax/?" + dataParam,
            "Affiliate Name, Initial Balance, Sold Sum, Payment Notices, Payments, Balance, Final Balance");
    });

    $("#ShowAll").click(function () {
        $(".daterange").val("");
        var dates2 = $(".daterange").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }

        var dataParam = "dates=" + dates2[0].trim() + ":" + dates2[1].trim() + "&buyerid=" + $("#AllBuyersList").val();
        GenerateGridBalance("GridView_AffiliatesBalances",
            "/Management/Accounting/GetAffiliatesBalanceAjax/?" + dataParam,
            "Affiliate Name, Initial Balance, Sold Sum, Payment Notices, Payments, Balance, Final Balance");
    });

    var dates2 = $(".daterange").val().split("-");
    if (dates2[1] == undefined) {
        dates2[1] = dates2[0];
    }

    var dataParam = "dates=" + dates2[0].trim() + ":" + dates2[1].trim() + "&buyerid=" + $("#AllBuyersList").val();
    GenerateGridBalance("GridView_AffiliatesBalances",
        "/Management/Accounting/GetAffiliatesBalanceAjax/?" + dataParam,
        "Affiliate Name, Initial Balance, Sold Sum, Payment Notices, Payments, Balance, Final Balance");
});