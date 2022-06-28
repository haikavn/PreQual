var BuyerChannelListParams = {
    str: "",
    stid: 0,
    init: function (_str) {
        this.str = _str;
    }
};

function deleteBuyerChannel(id) {
    if (!confirm("Are you sure?")) return;

    $.ajax({
        cache: false,
        type: "POST",
        url: "/deletebuyerchannel",
        data: { "buyerchannelid": id },
        success: function (data) {
            if (data.result)
                GenerateGridTable("GridView1",
                    "/GetBuyerChannels?a=" + BuyerChannelListParams.stid,
                    "ID, Buyer Channel Name, Buyer Name, Campaign, Status " + BuyerChannelListParams.str);
            else
                alert(data.message);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

$(document).ready(function () {
    $("#add_new").click(function () {
        window.location = "/Management/BuyerChannel/Create";
    });

    $("input[name=statusradio]:radio").change(function () {
        BuyerChannelListParams.stid = $(this).attr("id");
        BuyerChannelListParams.stid = BuyerChannelListParams.stid.replace("status", "");
        GenerateGridTable("GridView1",
            "/GetBuyerChannels?a=" + BuyerChannelListParams.stid,
            "ID, Buyer Channel Name, Buyer Name, Campaign, Status " + BuyerChannelListParams.str);
    });

    GenerateGridTable("GridView1",
        "/GetBuyerChannels?a=1",
        "ID, Buyer Channel Name, Buyer Name, Campaign, Status " + BuyerChannelListParams.str);
});