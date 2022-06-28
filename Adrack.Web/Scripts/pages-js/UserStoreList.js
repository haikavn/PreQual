var UserStoreListParams = {
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
                    "/GetStoreUsers?d=" + $("#view-type").val(),
                    "ID, Username, Role, Status " + UserStoreListParams.str);
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
            "/GetStoreUsers?d=" + stid,
            "ID, Username, Role, Status " + UserStoreListParams.str);
    });

    $("#add_user").click(function () {
        alert('To add Store user please select buyer channel and add user from "Users" tab of channel properties.');
    });
    GenerateGridTable("users_grid",
        "/GetStoreUsers?d=" + $("#view-type").val(),
        "ID, Username, Role, Status " + UserStoreListParams.str);
});