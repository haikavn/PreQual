var CampaignListParam = {
    str: "",
    stid: 0,
    init: function (_str) {
        this.str = _str;
    }
};

function deleteCampaign(id) {
    if (!confirm("Are you sure?")) return;

    $.ajax({
        cache: false,
        type: "POST",
        url: "/deletecampaign",
        data: { "campaignid": id },
        success: function (data) {
            if (data.result)
                GenerateGridTable("GridView1",
                    "/GetCampaigns?d=" + CampaignListParam.stid,
                    "ID, Name, Vertical, Received, Posted, Sold, Revenue, Cost, Profit, Status " +
                    CampaignListParam.str);
            else
                alert(data.message);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

function deleteFilterSet(id, campaignid) {
    if (!confirm("Are you sure?")) return;

    $.ajax({
        cache: false,
        type: "POST",
        url: "/deletefilterset",
        data: { id: id },
        success: function (data) {
            if (data.result)
                GenerateGridTable("filtersGridView", "/GetFilters", "ID, Name, Campaign, Action", null, null, null, campaignid);
            else
                alert(data.message);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

$(document).ready(function () {
    $("input[name=statusradio]:radio").change(function () {
        CampaignListParam.stid = $(this).attr("id");
        CampaignListParam.stid = CampaignListParam.stid.replace("status", "");
        GenerateGridTable("GridView1",
            "/GetCampaigns?d=" + CampaignListParam.stid,
            "ID, Name, Vertical, Received, Posted, Sold, Revenue, Cost, Profit, Status " + CampaignListParam.str);
    });

    $("#add_new").click(function () {
        window.location = "/Management/Campaign/Create";
    });

    $(".select-search").select2();

    $("#IdCampaigns").change(function () {
        UpdateReportByMinutes($(this).val());
    });

    GenerateGridTable("GridView1",
        "/GetCampaigns?d=0",
        "ID, Name, Vertical, Received, Posted, Sold, Revenue, Cost, Profit, Status " + CampaignListParam.str);
});