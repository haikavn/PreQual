var BulkFilterChangesParams = {
    BaseUrl: "",
    init: function (_BaseUrl) {
        this.BaseUrl = _BaseUrl;
    }
};

$(document).ready(function () {
    $("#apply-btn").on("click",
        function () {
            var items = [];

            $("#fields tbody tr").each(function () {
                var field = $(this).find(".campaign-field").first();
                var condition = $(this).find(".filter-condition").first();
                var value = $(this).find(".filter-value").first();

                items.push({ field: field.data("id"), condition: condition.val(), value: value.val() });
            });

            var itemsJson = JSON.stringify(items);

            var buyerChannels = $("#buyerChannels").val();
            var buyerChannelIds = "0";
            if (buyerChannels != undefined) {
                if (buyerChannels.length > 0)
                    buyerChannelIds += ",";
                for (var i = 0; i < buyerChannels.length; i++) {
                    buyerChannelIds += buyerChannels[i];
                    if (i < buyerChannels.length - 1)
                        buyerChannelIds += ",";
                }
            }

            $.ajax({
                cache: false,
                async: false,
                type: "POST",
                url: "/management/buyer/ApplyBulkFilters",
                data: { filters: itemsJson, buyerChannelIds: buyerChannelIds },
                success: function (data) {
                    $("#results").html("");

                    for (var i = 0; i < data.length; i++) {
                        $("#results").append("<p>" + data[i].message + "</p>");
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        });

     $(".multiselect").multiselect({
        enableCaseInsensitiveFiltering: true,
        numberDisplayed: 1
    });

    //initReportBuyersByCampaigns();
    $("#campaigns").on("change",
        function () {
            $.ajax({
                cache: false,
                async: false,
                type: "POST",
                url: BulkFilterChangesParams.BaseUrl + "/management/buyerchannel/GetBuyerChannelsByCampaign",
                data: { id: $(this).val(), mode: 1 },
                success: function (data) {
                    var $dropdown = $("#buyerChannels");
                    $dropdown.find("option").remove();
                    $(".multiselect").multiselect("destroy").multiselect({
                        enableCaseInsensitiveFiltering: true,
                        numberDisplayed: 1
                    });
                    $.each(data.data,
                        function () {
                            $dropdown.append($("<option />").val(this[0]).text(this[1]));
                        });
                    $(".multiselect").multiselect("destroy").multiselect({
                        enableCaseInsensitiveFiltering: true,
                        numberDisplayed: 1
                    });
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });

            $.ajax({
                cache: false,
                async: false,
                type: "POST",
                url: BulkFilterChangesParams.BaseUrl + "/management/campaign/LoadCampaignTemplateList",
                data: { campaignid: $(this).val(), dbonly: false, filterable: true },
                success: function (data) {
                    $("#fields tbody").html("");
                    $.each(data,
                        function (index, item) {
                            var condition = '<select class="form-control filter-condition">';
                            condition += '<option value="5">EQUAL</option>';
                            condition += '<option value="6">NOT EQUAL</option>';
                            condition += '<optgroup label="String">';
                            condition += '<option value="1">CONTAINS</option>';
                            condition += '<option value="2">DOES NOT CONTAIN</option>';
                            condition += '<option value="3">STARTS WITH</option>';
                            condition += '<option value="4">ENDS WITH</option>';
                            condition += "</optgroup>";
                            condition += '<optgroup label="Number, DateTime">';
                            condition += '<option value="7">GREATER</option>';
                            condition += '<option value="8">GREATER EQUAL</option>';
                            condition += '<option value="9">LESS</option>';
                            condition += '<option value="10">LESS EQUAL</option>';
                            condition += '<option value="11">RANGE</option>';
                            condition += "</optgroup>";
                            condition += '<option value="12">NO SAME DIGITS</option>';
                            condition += "</select>";

                            var filterValue = '<input type="text" class="form-control filter-value" />';

                            $("#fields tbody").append('<tr><td width="20%"><span class="campaign-field" data-id="' +
                                item.id +
                                '" data-index="' +
                                item.index +
                                '">' +
                                this.name +
                                '</span></td><td width="30%">' +
                                condition +
                                '</td><td width="80%">' +
                                filterValue +
                                "</td></tr>");
                        });
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        });
});