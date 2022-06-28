var BuyerListParams = {
    str: "",
    init: function (_str) {
        this.str = _str;
    }
};

function deleteBuyer(id) {
    if (!confirm("Are you sure?")) return;

    $.ajax({
        cache: false,
        type: "POST",
        url: "/deletebuyer",
        data: { "buyerid": id },
        success: function (data) {
            if (data.result)
                GenerateGridTable("GridView1",
                    "/GetBuyers?d=" + $("#view-type").val(),
                    "ID, Name, Type, Country, City, Address, Email, Zip, Phone, Manager, Status " +
                    BuyerListParams.str);
            else
                alert(data.message);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

$(document).ready(function () {
    $("#campaigns").change(function () {
        GenerateGridTable("GridView1",
            "/GetBuyers?d=0&campaignid=" + $('#campaigns').val(),
            "ID, Name, Type, Country, City, Address, Email, Zip, Phone, Manager, Status " + BuyerListParams.str);
    });

    $("#view-type").on("change",
        function () {
            GenerateGridTable("GridView1",
                "/GetBuyers?d=" + $("#view-type").val() + '&campaignid=' + $('#campaigns').val(),
                "ID, Name, Type, Country, City, Address, Email, Zip, Phone, Manager, Status " + BuyerListParams.str);
        });

    $("input[name=statusradio]:radio").change(function () {
        stid = $(this).attr("id");
        stid = stid.replace("status", "");
        if (parseInt(stid) >= 0) {
            GenerateGridTable("GridView1",
                "/GetBuyers?d=" + stid + '&campaignid=' + $('#campaigns').val(),
                "ID, Name, Type, Country, City, Address, Email, Zip, Phone, Manager, Status " + BuyerListParams.str);
        }
        else {
            stid = -parseInt(stid);

            GenerateGridTable("GridView1",
                "/GetBuyers?a=" + stid + '&campaignid=' + $('#campaigns').val(),
                "ID, Name, Type, Country, City, Address, Email, Zip, Phone, Manager, Status " + BuyerListParams.str);
        }
    });

    $("#add_new").click(function () {
        window.location = "/Management/Buyer/Create";
    });

    GenerateGridTable("GridView1",
        "/GetBuyers?d=0&campaignid=" + $('#campaigns').val(),
        "ID, Name, Type, Country, City, Address, Email, Zip, Phone, Manager, Status " + BuyerListParams.str);
});