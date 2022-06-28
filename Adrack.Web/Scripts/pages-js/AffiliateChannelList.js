var AffiliateChannelListParams = {
    str: "",
    stid: 0,
    init: function (_str) {
        this.str = _str;
    }
};

function deleteAffiliateChannel(id) {
    if (!confirm("Are you sure?")) return;

    $.ajax({
        cache: false,
        type: "POST",
        url: "/deleteaffiliatechannel",
        data: { "affiliatechannelid": id },
        success: function (data) {
            if (data.result)
                GenerateGridTable("affiliate_channels",
                    "/GetAffiliateChannels?d=" + AffiliateChannelListParams.stid,
                    "ID, Key, Affiliate Channel Name, Affiliate Name, Campaign, Status " + AffiliateChannelListParams.str);
            else
                alert(data.message);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

$(document).ready(function () {
    $("#view-type").on("change",
        function () {
            GenerateGridTable("affiliate_channels",
                "/GetAffiliateChannels?d=" + AffiliateChannelListParams.stid,
                "ID, Key, Affiliate Channel Name, Affiliate Name, Campaign, Status " + AffiliateChannelListParams.str);
        });

    $("input[name=statusradio]:radio").change(function () {
        AffiliateChannelListParams.stid = $(this).attr("id");
        AffiliateChannelListParams.stid = AffiliateChannelListParams.stid.replace("status", "");

        GenerateGridTable("affiliate_channels",
            "/GetAffiliateChannels?d=" + AffiliateChannelListParams.stid,
            "ID, Key, Affiliate Channel Name, Affiliate Name, Campaign, Status " + AffiliateChannelListParams.str);
    });

    $("#add_new").click(function () {
        window.location = "/Management/AffiliateChannel/Create";
    });
    GenerateGridTable("affiliate_channels",
        "/GetAffiliateChannels?d=0",
        "ID, Key, Affiliate Channel Name, Affiliate Name, Campaign, Status " + AffiliateChannelListParams.str);
});