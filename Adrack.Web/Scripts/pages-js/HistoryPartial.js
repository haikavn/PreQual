var HistoryPartialParams = {
    UserId: "",
    Entity: "",
    EntityId: "",
    TimeZoneStr: "",
    init: function (_UserId, _Entity, _EntityId, _TimeZoneStr) {
        this.UserId = _UserId;
        this.Entity = _Entity;
        this.EntityId = _EntityId;
        this.TimeZoneStr = _TimeZoneStr;
    }
};

var _TimeZoneNow = "";

function GeneratePagination(totalRecords) {
    $("#GridViewLeads_paginate").html("");
    var str = '<div class="paginginner">';
    for (i = 1; i <= totalRecords / 100 + 1; i++) {
        str += '<a class="paginate_button ' +
            (i == $("#GridPageNumber").val() ? "first-page current" : "") +
            '" aria-controls="tbl_GridViewLeads" data-dt-idx="' +
            i +
            '" tabindex="0">' +
            i +
            "</a>";
    }
    str += "</div>";
    $("#GridViewLeads_paginate").html(str);

    $(".paginginner").css("width", (36 + 2) * (totalRecords / 100 + 1) + "px");

    $(function () {
        $(".paginate_button").click(function () {
            $(".paginate_button").removeClass("current");
            $(this).addClass("current");
            $("#PageNumberSpan").html($(this).attr("data-dt-idx"));
            $("#GridPageNumber").val($(this).attr("data-dt-idx"));
            GenerateGrid($(this).attr("data-dt-idx"));
        });

        $("#NextPage").click(function () {
            $("#GridPageNumber").val($("#GridPageNumber").val() + 1);
        });
    });
}

function GenerateGrid(PageNumber) {
    $("#context-menu").hide();

    if (PageNumber == undefined)
        PageNumber = 1;
    var tbl_tr = "";

    var dates2 = $(".daterange").val().split("-");
    if (dates2[1] == undefined) {
        dates2[1] = dates2[0];
    }

    var data = "dates=" +
        dates2[0] +
        ":" +
        dates2[1] +
        "&userid=" +
        HistoryPartialParams.UserId +
        "&entity=" +
        HistoryPartialParams.Entity +
        "&entityid=" +
        HistoryPartialParams.EntityId +
        "&simplemode=true" +
        "&page=" +
        PageNumber;

    $.post("/Management/History/GetHistoryAjax", data).done(function (retData) {
        $("#totalRecords").html(retData.recordsTotal);

        $("#DataTables_History tbody").empty();

        if (retData.data.length == 0) {
            $("#DataTables_History tbody").append('<div class="h3" style="text-align:center">Nothing Found</div   >');
        }

        _TimeZoneNow = retData.TimeZoneNowStr;

        retData.data.forEach(function (item, i, arr) {
            if (item) {
                tbl_tr = "<tr>";

                item.forEach(function (item, i, arr) {
                    itemData = item == null ? "" : item;
                    if (i == 3 || i == 7) {
                        tbl_tr += '<td width="20%">' + itemData + "</td>";
                    } else {
                        tbl_tr += '<td width="12%">' + itemData + "</td>";
                    }
                });
                tbl_tr += "</tr>";
                $("#DataTables_History tbody").append(tbl_tr);
            }
        });

        GeneratePagination(retData.recordsTotal);
    });
}

$(function () {
    var dtNow = new Date(HistoryPartialParams.TimeZoneStr);
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
        // $('.daterange').trigger('apply.daterangepicker');
    });

    $(".daterange").on("apply.daterangepicker",
        function (ev, picker) {
            $(".paginate_button").removeClass("current");
            $(".first-page").addClass("current");
            GenerateGrid();
        });

    $("#HistoryRefresh").click(function () {
        $(".paginate_button").removeClass("current");
        $(".first-page").addClass("current");
        GenerateGrid();
    });

    $("#ClearFilters").click(function () {
        $(".paginate_button").removeClass("current");
        $(".first-page").addClass("current");

        var today = new Date();
        var dateFrom = new Date();
        dateFrom.setDate(today.getDate() - 30);

        var dd = dateFrom.getDate();
        var mm = dateFrom.getMonth() + 1; //January is 0!
        var yyyy = dateFrom.getFullYear();

        if (dd < 10) {
            dd = "0" + dd;
        }

        if (mm < 10) {
            mm = "0" + mm;
        }

        dateFromStr = yyyy + "-" + mm + "-" + dd;

        var dateTo = new Date();
        dateTo.setDate(today.getDate() + 1);

        dd = dateTo.getDate();
        mm = dateTo.getMonth() + 1; //January is 0!
        yyyy = dateTo.getFullYear();

        if (dd < 10) {
            dd = "0" + dd;
        }

        if (mm < 10) {
            mm = "0" + mm;
        }

        dateToStr = yyyy + "-" + mm + "-" + dd;

        $(".daterange-from-h").val(dateFromStr);
        $(".daterange-to-h").val(dateToStr);

        $("#filter-id").val("");
        $("#filter-email").val("");
        $("#filter-affiliate").val("");
        $("#filter-affiliate-channel-id").val("");
        $("#filter-buyer-id").val("");
        $("#filter-buyer-channel-id").val("");
        $("#filter-campaign-id").val("");
        $("#filter-ip").val("");
        $("#filter-state").val("");
        $("#filter-affiliate").val("");
        $("#filter-Affiliate-channel-id").val("");
        $("#filter-buyer-id").val("");
        $("#filter-buyer-channel-id").val("");
        $("#filter-campaign-id").val("");
        $("#filter-status").val("-1").change();

        GenerateGrid();
    });

    $("#ClearFilters").trigger("click");

    $("#DataTables_History tbody").on("mousedown",
        "tr",
        function (event) {
            GridSelectedID = $(this).find("td:eq(0)").text();
            $("#DataTables_History tbody tr").removeClass("selected");
            $(this).toggleClass("selected");
            $(".btn-row-click").trigger("click");

            var data = "id=" + GridSelectedID;

            $("#history-details").hide();

            $.post("/Management/History/GetHistoryByIdAjax", data).done(function (retData) {
                $("#table-details tbody").empty();

                $("#tr-data").html(retData.basicData[0][1]);
                $("#tr-action").html(retData.basicData[0][2]);
                $("#tr-enitity").html(retData.basicData[0][4]);
                $("#tr-note").html(retData.basicData[0][5]);
                $("#tr-user").html(retData.basicData[0][7]);

                if (retData.data.length > 0) {
                    $("#history-details").show();
                }

                retData.data.forEach(function (item, i, arr) {
                    if (item) {
                        tbl_tr = "<tr>";

                        item.forEach(function (item, i, arr) {
                            itemData = item == null ? "" : item;
                            tbl_tr += '<td width="12%">' + itemData + "</td>";
                        });
                        tbl_tr += "/<tr>";
                        $("#table-details tbody").append(tbl_tr);
                    }
                });
            });
        });
});