var LeadViewParams = {
    columnsVisibility: "",
    TimeZoneStr: "",

    init: function (_columnsVisibility, _timeZoneStr) {
        this.TimeZoneStr = _timeZoneStr,
            this.columnsVisibility = _columnsVisibility;
    }
};

var spinnerObj = null;
var _TimeZoneNow = "";

function GeneratePagination(totalRecords) {
    $("#GridViewLeads_paginate").html("");
    var pSize = $("#PageSizeSelect").val();

    var totalPages = totalRecords / pSize + 1;
    if (totalPages > 50) totalPages = 50;

    console.log(totalRecords, totalPages);

    var str = '<div class="paginginner">';
    for (i = 1; i <= totalPages; i++) {
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

    $(".paginginner").css("width", (36 + 2) * totalPages + "px");

    $(function () {
        $(".paginate_button").click(function () {
            $(".paginate_button").removeClass("current");
            $(this).addClass("current");
            // $("#PageNumberSpan").html($(this).attr("data-dt-idx"));
            $("#GridPageNumber").val($(this).attr("data-dt-idx"));
            GenerateGridLeads($(this).attr("data-dt-idx"), $("#PageSizeSelect").val());
        });

        $("#NextPage").click(function () {
            $("#GridPageNumber").val($("#GridPageNumber").val() + 1);
        });
    });
}

function GenerateGridLeads(PageNumber, PageSize) {
    $(window).scrollTop(0);

    if (PageSize == undefined)
        PageSize = 50;

    if (PageNumber == undefined)
        PageNumber = 1;

    var dates2 = $("#filter-created").val().split("-");
    if (dates2[1] == undefined) {
        dates2[1] = dates2[0];
    }

    var a = $("#filter-affiliate-channel-id").val();
    var affiliateChannelIds = '';
    if (a != undefined) {
        for (var i = 0; i < a.length; i++) {
            affiliateChannelIds += a[i];
            if (i < a.length - 1)
                affiliateChannelIds += ",";
        }
    }

    a = $("#filter-affiliate").val();
    var affiliateIds = '';
    if (a != undefined) {
        for (var i = 0; i < a.length; i++) {
            affiliateIds += a[i];
            if (i < a.length - 1)
                affiliateIds += ",";
        }
    }

    a = $("#filter-buyer-id").val();
    var buyerIds = '';
    if (a != undefined) {
        for (var i = 0; i < a.length; i++) {
            buyerIds += a[i];
            if (i < a.length - 1)
                buyerIds += ",";
        }
    }

    a = $("#filter-buyer-channel-id").val();
    var buyerChannelIds = '';
    if (a != undefined) {
        for (var i = 0; i < a.length; i++) {
            buyerChannelIds += a[i];
            if (i < a.length - 1)
                buyerChannelIds += ",";
        }
    }

    a = $("#filter-campaign-id").val();
    var campaignIds = '';
    if (a != undefined) {
        for (var i = 0; i < a.length; i++) {
            campaignIds += a[i];
            if (i < a.length - 1)
                campaignIds += ",";
        }
    }

    var tbl_tr = "";
    var data = "dates=" +
        dates2[0].trim() +
        ":" +
        dates2[1].trim() +
        "&leadid=" +
        $("#filter-id").val() +
        "&email=" +
        $("#filter-email").val() +
        "&firstname=" +
        $("#filter-firstname").val() +
        "&lastname=" +
        $("#filter-lastname").val() +
        "&affiliate=" +
        affiliateIds +
        "&affiliatechannel=" +
        affiliateChannelIds +
        "&affiliatesubid=" +
        "&buyer=" +
        buyerIds +
        "&buyerchannel=" +
        buyerChannelIds +
        "&campaign=" +
        campaignIds +
        "&status=" +
        $("#filter-status").val() +
        "&state=" +
        $("#filter-state").val() +
        "&ip=" +
        $("#filter-ip").val() +
        "&pagesize=" +
        PageSize +
        "&page=" +
        PageNumber;

    $.post("/Management/Lead/GetLeadsAjax", data).done(function (retData) {
        $("#totalRecords").html(retData.recordsTotal);

        $("#DataTables_Leads tbody").empty();

        if (retData.data.length == 0) {
            $("#DataTables_Leads tbody").append('<div class="h3" style="text-align:center">Nothing Found</div   >');
        }

        _TimeZoneNow = retData.TimeZoneNowStr;

        retData.data.forEach(function (item, i, arr) {
            if (item) {
                var style = "";

                if (i % 2 == 0)
                    style = 'style="background-color: #e2e2e2"';

                tbl_tr = "<tr " + style + ">";

                var tdNumber = 1;
                item.forEach(function (item, i, arr) {
                    if (!LeadViewParams.columnsVisibility[i]) return;

                    var thWidth = $("#DataTables_Leads thead tr th:nth-child(" + tdNumber + ")").attr("width");

                    itemData = item == null ? "" : item;
                    tbl_tr += '<td style="text-align:center" width="' + thWidth + '">' + itemData + "</td>";

                    tdNumber++;
                });
                tbl_tr += "</tr>";
                $("#DataTables_Leads tbody").append(tbl_tr);
            }
        });

        GeneratePagination(retData.recordsTotal);

        $(".addnotebtn").click(function () {
            var thisObj = $(this);
            var currId = $(this).attr("id").replace("l", "");
            $("#addnoteleadid").html(currId);

            $("#LeadNoteTitle").val(thisObj.data("titleid"));

            $("#LeadNoteText").val("");
            $("#LeadNoteAuthor").val("");

            $("#LeadAllNotes tbody").html("");

            var data = "leadid=" + currId;
            $.post("/Management/Lead/GetLeadNotesAjax/", data).done(function (retData) {
                retData.data.forEach(function (item, i, arr) {
                    if (item) {
                        tbl_tr = "<tr>";
                        item.forEach(function (item, i, arr) {
                            itemData = item == null ? "" : item;
                            tbl_tr += '<td style="text-align:left">' + itemData + "</td>";
                        });
                        tbl_tr += "/<tr>";
                        $("#LeadAllNotes tbody").append(tbl_tr);
                    }
                });
            });

            $(".btn-addnote").trigger("click");
        });

        $(".idhref").click(function () {
            var thisObj = $(this);
            GridSelectedID = $(this).attr("id");
            // GridSelectedID = $(this).find('td:eq(0)').html();

            $("#DataTables_Leads tbody tr").removeClass("selected");
            $(this).toggleClass("selected");
            $(".tab2tab").removeClass("active");
            $(".tab3tab").removeClass("active");
            $(".tab4tab").removeClass("active");
            $(".tab5tab").removeClass("active");
            $(".tab6tab").removeClass("active");
            $(".tab1tab").addClass("active");
            $(".tab-pane2").removeClass("active");
            $(".tab-pane3").removeClass("active");
            $(".tab-pane4").removeClass("active");
            $(".tab-pane5").removeClass("active");
            $(".tab-pane6").removeClass("active");
            $(".tab-pane1").addClass("active");

            $(".btn-info2").trigger("click");

            $("#LeadGeneralInfoContainer").html("");

            var data = "leadid=" + GridSelectedID;

            $("#left-icon-tab1").html('<img src="/Content/img/ajax-loader.gif" />');

            $.post("/Management/Lead/Item/" + GridSelectedID, data).done(function (retData) {
                $("#left-icon-tab1").html(retData);
                $("#left-icon-tab2").html($("#tab2content").html());
                $("#left-icon-tab3").html($("#tab3content").html());
                $("#left-icon-tab4").html($("#tab4content").html());
                $("#left-icon-tab5").html($("#tab5content").html());
                $("#left-icon-tab6").html($("#tab6content").html());

                $("#LeadGeneralInfo").hide();
                $("#LeadGeneralInfoContainer").html($("#LeadGeneralInfo").html());

                $("#tab2content").hide();
                $("#tab3content").hide();
                $("#tab4content").hide();
                $("#tab5content").hide();
                $("#tab6content").hide();

                $("#ResponseBuyerList > option").each(function () {
                    $("#RefundLeadBuyer").html("");
                    $("#RefundLeadBuyer")
                        .append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option>");
                });

                if ($(".for-popup-header").data("id") == "1") //Sold
                {
                    $("#Btn_AddRefund").show();
                } else {
                    $("#Btn_AddRefund").hide();
                }

                $("#dublicate_count").html("(" + $("#dublicate_count_view").html() + ")");

                if (thisObj.hasClass("monitor-ico")) {
                    $(".tab3tab a").trigger("click");
                }
                if (thisObj.hasClass("redirect-ico")) {
                    $(".tab5tab a").trigger("click");
                }
            });
        });

        /*
        $("#refresh-play").hide();
        $("#refresh-stop").show();
        */

        if (spinnerObj) {
            spinnerObj.stop();
        }
    });
}

$(function() {

    $(".multiselect").multiselect({
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        maxHeight: 150,
        buttonWidth: 150,
        numberDisplayed: 2,
        nSelectedText: 'selected',
        nonSelectedText: 'None selected',
        inheritClass: true,
        buttonText: function(options, select) {
            var numberOfOptions = $(this).children('option').length;
            if (options.length === 0) {
                return this.nonSelectedText + ' <b class="caret"></b>';
            }
            else {
                return options.length + ' ' + this.nSelectedText;
            }
        }
    });

    $(".lead-column").each(function () {
        for (var i = 1; i <= LeadViewParams.columnsVisibility.length; i++) {
            if (parseInt($(this).val()) == i) {
                $(this).prop("checked", LeadViewParams.columnsVisibility[i - 1]);

                if (LeadViewParams.columnsVisibility[i - 1]) {
                    $("#DataTables_Leads tbody tr td:nth-child(" + $(this).val() + ")").show();
                    $("#DataTables_Leads thead tr th:nth-child(" + $(this).val() + ")").show();
                } else {
                    $("#DataTables_Leads tbody tr td:nth-child(" + $(this).val() + ")").hide();
                    $("#DataTables_Leads thead tr th:nth-child(" + $(this).val() + ")").hide();
                }

                break;
            }
        }
    });

    /*
    $(".lead-column").on("change",
        function() {
            LeadViewParams.columnsVisibility[parseInt($(this).val()) - 1] = $(this).is(":checked");

            var columns = "";

            for (var i = 0; i < LeadViewParams.columnsVisibility.length; i++) {
                columns += LeadViewParams.columnsVisibility[i];
                if (i < LeadViewParams.columnsVisibility.length - 1)
                    columns += ",";
            }

            GenerateGridLeads();

            $.post("/Management/Lead/SaveColumnsVisibility/", "columns=" + columns).done(function(retData) {
            });
            return false;
    });
*/

    //$(".select-search-affiliate").select2();

    $(".btn-ladda-spinner").click(function (e) {
        e.preventDefault();
        spinnerObj = Ladda.create(this);
        spinnerObj.start();
        return false;
    });

    var dtNow = new Date(LeadViewParams.TimeZoneStr);
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
        $(this).trigger("apply.daterangepicker");
    });

    $(".daterange").on("apply.daterangepicker",
        function (ev, picker) {
            return;
            var dates2 = $(this).val().split("-");
            var d1 = new Date(dates2[0].replace(/ /g, ""));
            if (dates2[1] != undefined) {
                var d2 = new Date(dates2[1].replace(/ /g, ""));

                if (d2.toDateString() == d1.toDateString()) {
                    $(this).val((d1.getMonth()) + "/" + d1.getDate() + "/" + d1.getFullYear());
                }
            }
        });

    $(".daterange").trigger("apply.daterangepicker");

    GenerateGridLeads();

    $("#LeadRefresh2").click(function () {
        $(".paginate_button").removeClass("current");
        $(".first-page").addClass("current");
        GenerateGridLeads();
    });

    $("#ClearFilters").click(function () {
        $(".paginate_button").removeClass("current");
        $(".first-page").addClass("current");

        $(".daterange").val(_TimeZoneNow);

        $("#filter-id").val("");
        $("#filter-email").val("");
        $("#filter-firstname").val("");
        $("#filter-lastname").val("");
        $("#filter-affiliate").val("0").trigger("change");
        $("#filter-affiliate-channel-id").val("0").trigger("change");
        $("#filter-affiliate-sub-id").val("");
        $("#filter-buyer-id").val("0").trigger("change");
        $("#filter-buyer-channel-id").val("0").trigger("change");
        $("#filter-ip").val("");
        $("#filter-state").val("");
        $("#filter-campaign-id").val("0").trigger("change");
        $("#filter-status").val("-1").change();

        GenerateGridLeads();
    });

    $(".table-options-button").click(function () {
        if ($(".table-options-list").is(":visible")) {
            $(".table-options-list").hide();
        } else {
            $(".table-options-list").show();
        }
        return false;
    });

    $("#table-options-list-btn").click(function () {
        // return false;
        $(".table-options-list").hide();
    });

    $("body").click(function () {
        // $(".table-options-list").hide();
    });

    $(".table-options-list input").click(function () {
        if ($(this).is(":checked")) {
            $("#DataTables_Leads tbody tr td:nth-child(" + $(this).val() + ")").show();
            $("#DataTables_Leads thead tr th:nth-child(" + $(this).val() + ")").show();
        } else {
            $("#DataTables_Leads tbody tr td:nth-child(" + $(this).val() + ")").hide();
            $("#DataTables_Leads thead tr th:nth-child(" + $(this).val() + ")").hide();
        }

        LeadViewParams.columnsVisibility[parseInt($(this).val()) - 1] = $(this).is(":checked");

        var columns = "";

        for (var i = 0; i < LeadViewParams.columnsVisibility.length; i++) {
            columns += LeadViewParams.columnsVisibility[i];
            if (i < LeadViewParams.columnsVisibility.length - 1)
                columns += ",";
        }

        GenerateGridLeads();

        $.post("/Management/Lead/SaveColumnsVisibility/", "columns=" + columns).done(function (retData) {
        });
        // return false;

        // return false;
    });

    $("body").click(function () {
        // $(".table-options-list").hide();
    });

    $("#Btn_AddRefund").click(function () {
        if (!$("#RefundNoteContainer").is(":visible")) {
            $("#RefundNoteContainer").show(800);
            $("#RefundNote").show();
        } else {
            if ($("#RefundNote").val().trim().length <= 3) {
                alert("Please Leave Reason");
                return false;
            }
            var data =
                "leadid=" +
                $("#CurrentLeadId").text() +
                "&buyerid=" +
                $("#RefundLeadBuyer").val() +
                "&note=" +
                encodeURI($("#RefundNote").val());

            $("#RefundNote").val("");
            $("#RefundNoteContainer").hide(800);

            $.post("/Management/Accounting/AddRefundLeads", data).done(function (retData) {
                alert("Refund Request Recived");
            });
        }
    });

    $("#PageSizeSelect").change(function () {
        GenerateGridLeads(1, $(this).val());
    });

    $("#DataTables_Leads tbody").on("mousedown",
        "tr",
        function (event) {
            GridSelectedID = $(this).find("td:eq(0)").html();

            $("#DataTables_Leads tbody tr").removeClass("selected");
            $(this).toggleClass("selected");
        });

    $("#Btn_AddLeadNote").click(function () {
        var data =
            "leadid=" +
            $("#addnoteleadid").html() +
            "&titleid=" +
            $("#LeadNoteTitle").val() +
            "&note=" +
            encodeURI($("#LeadNoteText").val()) +
            "&author=" +
            $("#LeadNoteAuthor").val();

        $.post("/Management/Lead/AddLeadNote", data).done(function (retData) {
            GenerateGridLeads();
        });
    });

    $("#Btn_SaveNoteTitle").click(function () {
        var data = "notes=";

        $(".notes-container").find("input").each(function () {
            data += $(this).data("id") + ":" + $(this).val() + ";";
        });

        $.post("/Management/Lead/SaveNotes", data).done(function (retData) {
        });
    });

    $("#Btn_export-excel").click(function () {
        if ($("#excel-password").val().length < 1) {
            alert("Plase enter your password!");
            return false;
        }

        $("#export-to-excel").hide();
        $("#download-excel-loader").show();

        var dates2 = $("#filter-created").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }

        var data = "dates=" +
            dates2[0].trim() +
            ":" +
            dates2[1].trim() +
            "&leadid=" +
            $("#filter-id").val() +
            "&email=" +
            $("#filter-email").val() +
            "&affiliate=" +
            $("#filter-affiliate").val() +
            "&affiliatechannel=" +
            $("#filter-affiliate-channel-id").val() +
            "&affiliatesubid=" + //$("#filter-affiliate-sub-id").val() +
            "&buyer=" +
            $("#filter-buyer-id").val() +
            "&buyerchannel=" +
            $("#filter-buyer-channel-id").val() +
            "&campaign=" +
            $("#filter-campaign-id").val() +
            "&status=" +
            $("#filter-status").val() +
            "&state=" +
            $("#filter-state").val() +
            "&ip=" +
            $("#filter-ip").val() +
            "&pagesize=" +
            100 +
            "&page=" +
            1 +
            "&pass=" +
            $("#excel-password").val();

        $.post("/Management/Lead/GenerateCSVFileAjax", data).done(function (retData) {
            if (retData == "0") {
                alert("Incorrect Password");
                $("#export-to-excel").show();
                $("#download-excel-loader").hide();
                $("#excel-password").val("");
                return false;
            }
            $("#export-to-excel-download").attr("href", "/Downloads/" + retData);
            $("#export-to-excel-download").get(0).click();
            $("#export-to-excel").show();
            $("#download-excel-loader").hide();
            $("#excel-password").val("");
        });
        /*
                    $.post("/Management/Lead/GenerateExcelFileAjax", data).done(function (retData) {
                        $('#export-to-excel-download').attr("href", "/Downloads/" + retData);
                        $('#export-to-excel-download').get(0).click();
                    });
        */
    });
});