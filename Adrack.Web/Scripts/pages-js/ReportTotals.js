var ReportTotalstParams = {
    parentId: "",
    url: "",
    init: function (_parentId, _url) {
        this.parentId = _parentId,
            this.url = _url;
    }
};

function UpdateReportTotals() {
    GenerateGridTable2();
}

$(document).ready(function () {
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

function GenerateGridTable2() {
    $(document).ready(function () {
        $.post(ReportTotalstParams.url + ReportTotalstParams.parentId, "").done(function (retData) {
            $("#totalRecords").html(retData.recordsTotal);
            $("#DataTables_Totals tbody").empty();

            if (retData.data.length == 0) {
                $("#DataTables_Leads tbody").append('<div class="h3" style="text-align:center">Nothing Found</div   >');
            }

            retData.data.forEach(function (item, i, arr) {
                if (item) {
                    tbl_tr = '<tr id="row_' + i + '">';

                    var tdNumber = 1;
                    item.forEach(function (item, i, arr) {
                        var thWidth = $("#DataTables_Totals thead tr th:nth-child(" + tdNumber + ")").attr("width");

                        itemData = item == null ? "" : item;
                        tbl_tr += '<td style="text-align:center" width="' + thWidth + '">' + itemData + "</td>";

                        tdNumber++;
                    });
                    tbl_tr += "/<tr>";
                    $("#DataTables_Totals tbody").append(tbl_tr);
                }
            });

            $(".DayTypeSelector").change(function () {
                $(".DayTypeSelector").val($(this).val());

                if ($(this).val() == 2) {
                    $("#row_0").hide();
                    $("#row_2").hide();

                    $("#row_1").show();
                    $("#row_3").show();
                } else {
                    $("#row_1").hide();
                    $("#row_3").hide();

                    $("#row_2").show();
                    $("#row_0").show();
                }
            });
        });
    });
}

function GenerateGridHistory(PageNumber) {
    $("#refresh-play").show();
    $("#refresh-stop").hide();

    $("#context-menu").hide();

    if (PageNumber == undefined)
        PageNumber = 1;
    var tbl_tr = "";

    $.post("/Management/History/GetHistoryAjax/?pageSize=5", "").done(function (retData) {
        $("#totalRecords").html(retData.recordsTotal);

        $("#DataTables_Leads tbody").empty();

        if (retData.data.length == 0) {
            $("#DataTables_Leads tbody").append('<div class="h3" style="text-align:center">Nothing Found</div   >');
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
                tbl_tr += "/<tr>";
                $("#DataTables_History tbody").append(tbl_tr);
            }
        });
    });
}

$(document).ready(function () {
    GenerateGridTable2();
});

GenerateGridHistory(1);