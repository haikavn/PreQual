var UserAffiliateListParams = {
    str: "",
    init: function (_str) {
        this.str = _str;
    }
};

function deleteUser(id) {
    if (!confirm("Are you sure?")) return;

    $.ajax({
        cache: false,
        type: "POST",
        url: "/deleteuser",
        data: { "userid": id },
        success: function (data) {
            if (data.result)
                GenerateGridTable("users_grid",
                    "/GetAffiliateUsers?d=" + $("#view-type").val(),
                    "ID, Username, Role, Status " + UserAffiliateListParams.str);
            else
                alert(data.message);
        },
        error: function (xhr, ajaxOptions, thrownError) {
        }
    });
}

$(document).ready(function () {
    $("input[name=statusradio]:radio").change(function () {
        stid = $(this).attr("id");
        stid = stid.replace("status", "");
        GenerateGridTable("users_grid",
            "/GetAffiliateUsers?d=" + stid,
            "ID, Username, Role, Status " + UserAffiliateListParams.str);
    });

    $("#add_user").click(function () {
        window.location = "/Management/User/Affiliate";
    });

    GenerateGridTable("users_grid",
        "/GetAffiliateUsers?d=" + $("#view-type").val(),
        "ID, Username, Role, Status " + UserAffiliateListParams.str);
});