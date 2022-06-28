var ReportModule = {
    showLeads: function (e) {
        $('.btn-leads').trigger('click');

        var dates2 = $(".daterange-single").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }

        GenerateGridLeads(1, 50, dates2[0], dates2[1], $(e).data('query'));
    },
    loadBuyerReport: function (report, csv, baseUrl) {
        var self = this;

        if (report == undefined) return;
        if (csv == undefined) csv = false;

        var a = $("#buyers").val();

        var buyerIds = buyerid;

        if (a != undefined) {
            if (a.length > 0)
                buyerIds += ",";
            for (var i = 0; i < a.length; i++) {
                buyerIds += a[i];
                if (i < a.length - 1)
                    buyerIds += ",";
            }
        }

        a = $("#buyerChannels").val();

        var buyerChannelIds = "";

        if (a != undefined) {
            for (var i = 0; i < a.length; i++) {
                buyerChannelIds += a[i];
                if (i < a.length - 1)
                    buyerChannelIds += ",";
            }
        }

        a = $("#affiliateChannels").val();

        var affiliateChannelIds = "";

        if (a != undefined) {
            for (var i = 0; i < a.length; i++) {
                affiliateChannelIds += a[i];
                if (i < a.length - 1)
                    affiliateChannelIds += ",";
            }
        }

        a = $("#campaigns").val();

        var campaignIds = "";

        if (a != undefined) {
            for (var i = 0; i < a.length; i++) {
                campaignIds += a[i];
                if (i < a.length - 1)
                    campaignIds += ",";
            }
        }

        a = $("#states").val();

        var sids = "";

        if (a != undefined) {
            for (var i = 0; i < a.length; i++) {
                sids += "'" + a[i] + "'";
                if (i < a.length - 1)
                    sids += ",";
            }
        }

        var dates2 = $(".daterange-single").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }

        var type = $("#leadNotesReportType").val();

        var fromPrice = $("#fromPrice").val();
        var toPrice = $("#toPrice").val();

        if (!csv) {
            ShowLoader();
            $.ajax({
                cache: false,
                async: true,
                type: "POST",
                url: baseUrl + "/management/report/Get" + report,
                data: {
                    startDate: dates2[0],
                    endDate: dates2[1],
                    buyerIds: buyerIds,
                    buyerChannelIds: buyerChannelIds,
                    affiliateIds: affiliateChannelIds,
                    campaignIds: campaignIds,
                    sids: sids,
                    type: type,
                    parentType: 2,
                    date1: $("#date1").val(),
                    date2: $("#date2").val(),
                    date3: $("#date3").val(),
                    fromPrice: fromPrice,
                    toPrice: toPrice
                },
                success: function (data) {
                    HideLoader();
                    ReportCommon.loadTree(report, data);
                    $(".sortable-column-default").data('order', 'desc');
                    $(".sortable-column-default").trigger('click');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    HideLoader();
                }
            });
        } else {
            location.href = "/management/report/Get" +
                report +
                "?startDate=" +
                dates2[0] +
                "&endDate=" +
                dates2[1] +
                "&buyerIds=" +
                buyerIds +
                "&buyerChannelIds=" +
                buyerChannelIds +
                "&affiliateIds=" +
                affiliateChannelIds +
                "&campaignIds=" +
                campaignIds +
                "&sids=" +
                sids +
                "&type=" +
                type +
                "&parentType=2" +
                "&date1=" +
                $("#date1").val() +
                "&date2=" +
                $("#date2").val() +
                "&date3=" +
                $("#date3").val() +
                "&fromPrice=" +
                fromPrice +
                "&toPrice=" +
                toPrice +
                "&csv=true";
        }
    },

    loadAffiliateReport: function (report, csv, baseUrl) {
        var self = this;

        if (report == undefined) return;
        if (csv == undefined) csv = false;

        var a = $("#affiliates").val();

        var affiliateIds = "";

        if (a != undefined) {
            for (var i = 0; i < a.length; i++) {
                affiliateIds += a[i];
                if (i < a.length - 1)
                    affiliateIds += ",";
            }
        }

        a = $("#affiliateChannels").val();

        var affiliateChannelIds = "";

        if (a != undefined) {
            for (var i = 0; i < a.length; i++) {
                affiliateChannelIds += a[i];
                if (i < a.length - 1)
                    affiliateChannelIds += ",";
            }
        }

        a = $("#states").val();

        var sids = "";

        if (a != undefined) {
            for (var i = 0; i < a.length; i++) {
                sids += "'" + a[i] + "'";
                if (i < a.length - 1)
                    sids += ",";
            }
        }

        var dates2 = $(".daterange-single").val().split("-");
        if (dates2[1] == undefined) {
            dates2[1] = dates2[0];
        }

        if (!csv) {
            ShowLoader();
            $.ajax({
                cache: false,
                async: false,
                type: "POST",
                url: baseUrl + "/management/report/Get" + report,
                data: {
                    startDate: dates2[0],
                    endDate: dates2[1],
                    affiliateid: affiliateIds,
                    affiliateChannelIds: affiliateChannelIds,
                    parentType: 1,
                    csv: false,
                    sids: sids
                },
                success: function (data) {
                    HideLoader();
                    ReportCommon.loadTree(report, data);
                    $(".sortable-column-default").data('order', 'desc');
                    $(".sortable-column-default").trigger('click');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    HideLoader();
                }
            });
        } else {
            location.href = baseUrl +
                "/management/report/Get" +
                report +
                "?startDate=" +
                dates2[0] +
                "&endDate=" +
                dates2[1] +
                "&affiliateid=" +
                affiliateid +
                "&affiliateChannelIds=" +
                affiliateChannelIds +
                "&parentType=1&csv=true" + 
                "&sids=" +
                sids;
        }
    },

    ReportAffiliatesByAffiliateChannels: function ($tdList, node, data) {
        $tdList.eq(1).addClass("text-center").text(node.data.TotalLeads);
        $tdList.eq(2).addClass("text-center").text(node.data.SoldLeads);

        $tdList.eq(3).addClass("text-center").text(node.data.AcceptRate + '%');
        $tdList.eq(4).addClass("text-center").text(node.data.Redirected + ' (' + node.data.RedirectedRate + '%)');
        $tdList.eq(5).addClass("text-center").text('$' + node.data.Profit);
        $tdList.eq(6).addClass("text-center").text(node.data.EPL);
        $tdList.eq(7).addClass("text-center").text(node.data.EPA);
    },

    ReportAffiliatesByCampaigns: function ($tdList, node, data) {
        $tdList.eq(1).addClass("text-center").text(node.data.TotalLeads);
        $tdList.eq(2).addClass("text-center").text(node.data.SoldLeads);

        $tdList.eq(3).addClass("text-center").text(node.data.AcceptRate + '%');
        $tdList.eq(4).addClass("text-center").text(node.data.Redirected + ' (' + node.data.RedirectedRate + '%)');
        $tdList.eq(5).addClass("text-center").text('$' + node.data.Profit);
        $tdList.eq(6).addClass("text-center").text(node.data.EPL);
        $tdList.eq(7).addClass("text-center").text(node.data.EPA);
    },

    ReportAffiliatesByEpl: function ($tdList, node, data) {
        $tdList.eq(1).addClass("text-center").text(node.data.AcceptRate + '%');
        $tdList.eq(2).addClass("text-center").text(node.data.EPL);
        $tdList.eq(3).addClass("text-center").text(node.data.Redirected + ' (' + node.data.RedirectedRate + '%)');
        $tdList.eq(4).addClass("text-center").text(node.data.EPA);
    },

    ReportAffiliatesByStates: function ($tdList, node, data) {
        $tdList.eq(1).addClass("text-center").text(node.data.TotalLeads);
        $tdList.eq(2).addClass("text-center").text(node.data.SoldLeads);
        $tdList.eq(3).addClass("text-center").text(node.data.AcceptRate + '%');
        $tdList.eq(4).addClass("text-center").text(node.data.Redirected + ' (' + node.data.RedirectedRate + '%)');
        $tdList.eq(5).addClass("text-center").text('$' + node.data.Profit);
        $tdList.eq(6).addClass("text-center").text(node.data.EPL);
        $tdList.eq(7).addClass("text-center").text(node.data.EPA);
    },

    ReportClickMain: function ($tdList, node, data) {

        $tdList.eq(1).addClass('text-center').text(node.data.Hits + '/' + node.data.UniqueClicks);
        $tdList.eq(2).addClass('text-center').text(node.data.TotalLeads);
        $tdList.eq(3).addClass('text-center').text(node.data.CTA);
        $tdList.eq(4).addClass('text-center').text(node.data.SoldLeads);
        $tdList.eq(5).addClass('text-center').text(node.data.AcceptRate + '%');
        $tdList.eq(6).addClass('text-center').text(node.data.Redirected + ' (' + node.data.RedirectedRate + '%)');
        $tdList.eq(7).addClass('text-center').text('$' + node.data.Profit);
        $tdList.eq(8).addClass('text-center').text(node.data.EPL);
        $tdList.eq(9).addClass('text-center').text(node.data.EPA);
        $tdList.eq(10).addClass('text-center').text(node.data.EPC);
    },

    BuyerReportByBuyer: function ($tdList, node, data) {
        $tdList.eq(1).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="buyer=' + node.data.BuyerId + '">' + node.data.TotalLeads + '</a>');
        $tdList.eq(2).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="buyer=' + node.data.BuyerId + '&status=1">' + node.data.SoldLeads + '</a>');
        $tdList.eq(3).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="buyer=' + node.data.BuyerId + '&status=3">' + node.data.RejectedLeads + '</a>');
        $tdList.eq(4).addClass("text-center").text('$' + node.data.Cost);
        $tdList.eq(5).addClass("text-center").text('$' + node.data.Profit);
        $tdList.eq(6).addClass("text-center").text('$' + node.data.AveragePrice);
        $tdList.eq(7).addClass("text-center").text(node.data.AcceptRate + '%');
        $tdList.eq(8).addClass("text-center").text(node.data.Redirected + ' (' + node.data.RedirectedRate + '%)');
        $tdList.eq(9).addClass("text-center").text(node.data.LastSoldDate);
    },

    BuyerReportByBuyerChannel: function ($tdList, node, data) {
        if (node.title != 'Total')
            $tdList.eq(1).addClass("text-center").html((!node.data.CapHit ? '<span style="color:green"><b>No</b></span>' : '<span style="color:red"><b>Yes</b></span>'));
        else
            $tdList.eq(1).addClass("text-center").html('');
        $tdList.eq(2).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="buyerchannel=' + node.data.BuyerChannelId + '">' + node.data.TotalLeads + '</a>');
        $tdList.eq(3).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="buyerchannel=' + node.data.BuyerChannelId + '&status=1">' + node.data.SoldLeads + '</a>');
        $tdList.eq(4).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="buyerchannel=' + node.data.BuyerChannelId + '&status=3">' + node.data.RejectedLeads + '</a>');

        if (data.AlwaysSoldOption == 1) {
            $tdList.eq(5).addClass("text-center").text(node.data.LoanedLeads);
            $tdList.eq(6).addClass("text-center").text('$' + node.data.Cost);
            $tdList.eq(7).addClass("text-center").text('$' + node.data.Profit);
            $tdList.eq(8).addClass("text-center").text('$' + node.data.AveragePrice);
            $tdList.eq(9).addClass("text-center").text(node.data.AcceptRate + '%');
            $tdList.eq(10).addClass("text-center").text(node.data.Redirected + ' (' + node.data.RedirectedRate + '%)');
        } else {
            $tdList.eq(5).addClass("text-center").text('$' + node.data.Cost);
            $tdList.eq(6).addClass("text-center").text('$' + node.data.Profit);
            $tdList.eq(7).addClass("text-center").text('$' + node.data.AveragePrice);
            $tdList.eq(8).addClass("text-center").text(node.data.AcceptRate + '%');
            $tdList.eq(9).addClass("text-center").text(node.data.Redirected + ' (' + node.data.RedirectedRate + '%)');
        }
    },

    ReportBuyersByAffiliateChannels: function ($tdList, node, data) {
        $tdList.eq(1).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="affiliatechannel=' + node.data.AffiliateChannelId + '">' + node.data.TotalLeads + '</a>');
        $tdList.eq(2).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="affiliatechannel=' + node.data.AffiliateChannelId + '&status=1">' + node.data.SoldLeads + '</a>');
        $tdList.eq(3).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="affiliatechannel=' + node.data.AffiliateChannelId + '&status=3">' + node.data.RejectedLeads + '</a>');
        $tdList.eq(4).addClass("text-center").text('$' + node.data.Debet);
        $tdList.eq(5).addClass("text-center").text('$' + node.data.Profit);
        $tdList.eq(6).addClass("text-center").text('$' + node.data.AveragePrice);
        $tdList.eq(7).addClass("text-center").text(node.data.AcceptRate + '%');
        $tdList.eq(8).addClass("text-center").text(node.data.Redirected + ' (' + node.data.RedirectedRate + '%)');
    },

    ReportBuyersByCampaigns: function ($tdList, node, data) {
        //$tdList.eq(0).html(node.title);
        $tdList.eq(1).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="campaign=' + node.data.CampaignId + '">' + node.data.TotalLeads + '</a>');
        $tdList.eq(2).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="campaign=' + node.data.CampaignId + '&status=1">' + node.data.SoldLeads + '</a>');
        $tdList.eq(3).addClass("text-center").text('$' + node.data.Debit);
        $tdList.eq(4).addClass("text-center").text('$' + node.data.Credit);
        $tdList.eq(5).addClass("text-center").text(node.data.Redirected + ' (' + node.data.RedirectedRate + '%)');
    },

    ReportBuyersByDates: function ($tdList, node, data) {
        $tdList.eq(1).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="datetime=' + node.title + '">' + node.data.TotalLeads + '</a>');
        $tdList.eq(2).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="datetime=' + node.title + '&status=1">' + node.data.SoldLeads + '</a>');
        $tdList.eq(3).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="datetime=' + node.title + '&status=3">' + node.data.RejectedLeads + '</a>');
        if (data.AlwaysSoldOption == 1) {
            $tdList.eq(4).addClass("text-center").text(node.data.LoanedLeads);
            $tdList.eq(5).addClass("text-center").text('$' + node.data.Debit);
            $tdList.eq(6).addClass("text-center").text('$' + node.data.Profit);
            $tdList.eq(7).addClass("text-center").text('$' + node.data.AveragePrice);
            $tdList.eq(8).addClass("text-center").text(node.data.AcceptRate + '%');
            $tdList.eq(9).addClass("text-center").text(node.data.Redirected + ' (' + node.data.RedirecedtRate + '%)');
        } else {
            $tdList.eq(4).addClass("text-center").text('$' + node.data.Debit);
            $tdList.eq(5).addClass("text-center").text('$' + node.data.Profit);
            $tdList.eq(6).addClass("text-center").text('$' + node.data.AveragePrice);
            $tdList.eq(7).addClass("text-center").text(node.data.AcceptRate + '%');
            $tdList.eq(8).addClass("text-center").text(node.data.Redirected + ' (' + node.data.RedirectedRate + '%)');
        }
    },

    ReportBuyersByLeadNotes: function ($tdList, node, data) {
        //$tdList.eq(0).html(node.title);
        $tdList.eq(0).css("text-align", "left");

        $tdList.eq(0).html(node.title);
        $tdList.eq(1).addClass("text-center").text(node.data.Quantity1);
        $tdList.eq(2).addClass("text-center").text(node.data.Quantity2);
        $tdList.eq(3).addClass("text-center").text(node.data.Quantity3);
        $tdList.eq(4).addClass("text-center").text(node.data.Quantity4);
        $tdList.eq(5).addClass("text-center").text(node.data.Quantity5);
        $tdList.eq(6).addClass("text-center").text(node.data.Quantity6);
        $tdList.eq(7).addClass("text-center").text(node.data.Quantity7);
        $tdList.eq(8).addClass("text-center").text(node.data.Quantity8);
        $tdList.eq(9).addClass("text-center").text(node.data.Quantity9);
        $tdList.eq(10).addClass("text-center").text(node.data.Quantity10);
        $tdList.eq(11).addClass("text-center").text(node.data.Quantity11);
        $tdList.eq(12).addClass("text-center").text(node.data.Quantity12);
        $tdList.eq(13).addClass("text-center").text(node.data.Quantity13);
        $tdList.eq(14).addClass("text-center").text(node.data.Quantity14);
        $tdList.eq(15).addClass("text-center").text(node.data.Quantity15);
    },

    ReportBuyersByPrices: function ($tdList, node, data) {
        //$tdList.eq(0).html(node.title);
        $tdList.eq(1).addClass("text-center").text(node.data.Quantity);
        $tdList.eq(2).addClass("text-center").text(node.data.UQuantity);
    },

    ReportBuyersByPrices: function ($tdList, node, data) {
        //$tdList.eq(0).html(node.title);
        $tdList.eq(1).addClass("text-center").text(node.data.Quantity);
        $tdList.eq(2).addClass("text-center").text(node.data.UQuantity);
    },

    ReportBuyersByStates: function ($tdList, node, data) {
        //$tdList.eq(0).html(node.title);
        $tdList.eq(1).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="state=' + node.title + '">' + node.data.TotalLeads + '</a>');
        $tdList.eq(2).addClass("text-center").html('<a href="#" onclick="ReportModule.showLeads(this)" data-query="state=' + node.title + '&status=1">' + node.data.SoldLeads + '</a>');
        $tdList.eq(3).addClass("text-center").text(node.data.RejectedLeads);
        $tdList.eq(4).addClass("text-center").text('$' + node.data.Debit);
        $tdList.eq(5).addClass("text-center").text(node.data.AcceptRate + '%');
        $tdList.eq(6).addClass("text-center").text(node.data.Redirected + ' (' + node.data.RedirectedRate + '%)');
    },

    ReportBuyersByPrices: function ($tdList, node, data) {
        //$tdList.eq(0).html(node.title);
        $tdList.eq(1).addClass("text-center").text(node.data.Quantity);
        $tdList.eq(2).addClass("text-center").text(node.data.UQuantity);
    },

    ReportBuyersByTrafficEstimator: function ($tdList, node, data) {
        //$tdList.eq(0).html(node.title);
        $tdList.eq(1).addClass("text-center").text(node.data.Quantity);
        $tdList.eq(2).addClass("text-center").text(node.data.UQuantity);
    }
};

var ReportCommon = {
    type: "",
    baseUrl: "",

    multiselect_selected: function ($el) {
        var ret = true;
        $("option", $el).each(function (element) {
            if (!!!$(this).prop("selected")) {
                ret = false;
            }
        });
        return ret;
    },

    multiselect_selectAll: function ($el) {
        $("option", $el).each(function (element) {
            $el.multiselect("select", $(this).val());
        });
    },

    multiselect_deselectAll: function ($el) {
        $("option", $el).each(function (element) {
            $el.multiselect("deselect", $(this).val());
        });
    },

    multiselect_toggle: function ($el, $btn) {
        if (this.multiselect_selected($el)) {
            this.multiselect_deselectAll($el);
            $btn.text("Select All");
        } else {
            this.multiselect_selectAll($el);
            $btn.text("Deselect All");
        }
    },

    loadReport: function (report, csv) {
        switch (this.type) {
            case "buyer":
                ReportModule.loadBuyerReport(report, csv, this.baseUrl);
                break;
            case "affiliate":
                ReportModule.loadAffiliateReport(report, csv, this.baseUrl);
                break;
        }
    },

    loadTree: function (report, source) {
        if ($("#" + report).length == 0) return;
        var tree = $("#" + report).fancytree("getTree");
        tree.reload(source);
    },

    ready: function (timeZone, _type, _baseUrl) {
        var self = this;

        this.type = _type;
        this.baseUrl = _baseUrl;

        $(".sortable-column").on("click",
            function (e) {
                e.preventDefault();

                var col = $(this).data("col");
                var order = $(this).data("order");

                var cmp = function (a, b) {
                    if (a.title == "Total" || b.title == "Total")
                        return 0;

                    console.log(a, b);

                    var x = 0;
                    var y = 0;

                    if (col == "title") {
                        x = a.title;
                        y = b.title;
                    } else {
                        x = a.data[col];
                        y = b.data[col];

                        if (!isNaN(parseFloat(x)) && isFinite(x)) {
                            x = parseFloat(x);
                        }

                        if (!isNaN(parseFloat(y)) && isFinite(y)) {
                            y = parseFloat(y);
                        }

                        console.log(x, y);
                    }

                    if (order == "asc")
                        return x === y ? 0 : (x > y ? 1 : -1);
                    else if (order == "desc")
                        return x === y ? 0 : (x < y ? 1 : -1);
                };

                $("#" + $(this).data("report")).fancytree("getRootNode").sortChildren(cmp, false);

                $(this).data("order", (order == "asc" ? "desc" : "asc"));
            });

        $(".nav-tabs li").on("click",
            function () {
                var href = $(this).children("a").attr("href");
                self.loadReport($("#" + href.substring(1, href.length)).data("report"));
            });

        if ($(".multiselect").length > 0) {
            $(".multiselect").multiselect({
                enableCaseInsensitiveFiltering: true,
                onChange: function () {
                    //self.loadReport($('#buyer-reports-tab .tab-pane.active').data('report'));
                }
            });

            $(".multiselect-toggle-selection-button").click(function (e) {
                e.preventDefault();
                self.multiselect_toggle($("#" + $(this).data("selector")), $(this));
                //$.uniform.update();
            });
        }

        var dtNow = new Date(timeZone);
        $(".daterange-single").daterangepicker({
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
                    moment(dtNow).subtract(1, "month").startOf("month"),
                    moment(dtNow).subtract(1, "month").endOf("month")
                ],
                'Last 12 Month': [moment(dtNow).subtract(12, "month"), moment(dtNow)]
            },
            locale: {
                format: "MM/DD/YYYY"
            }
        });

        $("#daterange-single").on("apply.daterangepicker",
            function (ev, picker) {
                //do something, like clearing an input
                self.loadReport($("#reports-tab .tab-pane.active").data("report"));
            });

        $("#report-refresh").click(function () {
            self.loadReport($("#reports-tab .tab-pane.active").data("report"));
        });

        $("#report-download").click(function () {
            self.loadReport($("#reports-tab .tab-pane.active").data("report"), true);
        });
    },

    initReport: function (name, extra) {
        if (extra == undefined) {
            extra = null;
        }
        $("#" + name).fancytree({
            extensions: ["table", "dnd"],
            checkbox: false,
            table: {
                indentation: 20, // indent 20px per node level
                nodeColumnIdx: 0 // render the node title into the 2nd column
            },
            source: [],
            lazyLoad: function (event, data) {
                data.result = { url: "ajax-sub2.json" };
            },
            renderColumns: function (event, data) {
                var node = data.node,
                    $tdList = $(node.tr).find(">td");

                $tdList.eq(0).css("text-align", "left");

                if (!node.folder) {
                }

                ReportModule[name]($tdList, node, extra);

                if (node.title == 'Total') {
                    for (var i = 0; i < $tdList.length; i++) {
                        $tdList.eq(i).html('<b>' + $tdList.eq(i).text() + '</b>');
                    }
                }
            },
            activate: function (event, data) {
            },
            select: function (event, data) {
            },
            dnd: {
                preventVoidMoves: true, // Prevent dropping nodes 'before self', etc.
                preventRecursiveMoves: true, // Prevent dropping nodes on own descendants
                autoExpandMS: 400,
                draggable: {
                    //zIndex: 1000,
                    // appendTo: "body",
                    // helper: "clone",
                    scroll: false,
                    revert: "invalid"
                },
                dragStart: function (node, data) {
                    if (data.originalEvent.shiftKey) {
                        console.log("dragStart with SHIFT");
                    }
                    // allow dragging `node`:
                    return true;
                },
                dragEnter: function (node, data) {
                    // Prevent dropping a parent below another parent (only sort
                    // nodes under the same parent)
                    /* 					if(node.parent !== data.otherNode.parent){
                                            return false;
                                        }
                                        // Don't allow dropping *over* a node (would create a child)
                                        return ["before", "after"];
                    */
                    return true;
                },
                dragDrop: function (node, data) {
                    if (!data.otherNode) {
                        // It's a non-tree draggable
                        var title = $(data.draggable.element).text() + " (" + (count)++ + ")";
                        node.addNode({ title: title }, data.hitMode);
                        alert("dd");
                        return;
                    }
                    data.otherNode.moveTo(node, data.hitMode);
                    $(node.tr).data("folder", true);
                }
            }
        });
    }
};