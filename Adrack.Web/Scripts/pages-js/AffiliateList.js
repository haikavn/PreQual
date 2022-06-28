var AffiliateListParams = {
    str: "",
    init: function (_str) {
        this.str = _str;
    }
};

function deleteAffiliate(id) {
    if (!confirm("Are you sure?")) return;

    $.ajax({
        cache: false,
        type: "POST",
        url: "/deleteaffiliate",
        data: { "affiliateid": id },
        success: function (data) {
            if (data.result)
                GenerateGridTable("affiliate_list",
                    "/GetAffiliates?d=" + $("#view-type").val(),
                    "ID, Name, Email, Channels, Manager, Registration Date, Registration IP, Status " +
                    AffiliateListParams.str);
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
            GenerateGridTable("affiliate_list",
                "/GetAffiliates?d=" + $("#view-type").val(),
                "ID, Name, Email, Channels, Manager, Registration Date, Registration IP, Status " +
                AffiliateListParams.str);
        });

    $("#add_new").click(function () {
        window.location = "/Management/Affiliate/Item";
    });

    $("input[name=statusradio]:radio").change(function () {
        stid = $(this).attr("id");
        stid = stid.replace("status", "");
        if (stid == 5) { //Applied
            stid = 2;
        }
        var posturl = "/GetAffiliates";
        if (stid != -1 && stid != 6) {
            posturl += "?st=" + stid;
        } else if (stid == 6) {
            posturl += "?d=1";
        }

        GenerateGridTable("affiliate_list",
            posturl,
            "ID, Name, Email, Channels, Manager, Registration Date, Registration IP, Status " +
            AffiliateListParams.str);
    });

    $("#add_new").click(function () {
        window.location = "/Management/Affiliate/Item";
    });

    $("#add_new").click(function () {
        window.location = "/Management/Affiliate/Item";
    });

    GenerateGridTable("affiliate_list",
        "/GetAffiliates?d=" + $("#view-type").val(),
        "ID, Name, Email, Channels, Manager, Registration Date, Registration IP, Status " + AffiliateListParams.str);
});