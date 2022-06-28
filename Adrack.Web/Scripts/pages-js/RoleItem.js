function GenerateUsersGridTable(ObjectId, PostURL) {
    var tbl_tr = "";
    var data = "roleid=" + "1";
    $.post(PostURL, data).done(function (retData) {
        $(ObjectId + " tbody").empty();

        if (retData.data.length == 0) {
            $(ObjectId + " tbody").append('<div class="h3" style="text-align:center">Nothing Found</div   >');
        }

        retData.data.forEach(function (item, i, arr) {
            if (item) {
                tbl_tr = "<tr>";

                item.forEach(function (item, i, arr) {
                    itemData = item == null ? "" : item;
                    tbl_tr += '<td style="text-align:center" width="8%">' + itemData + "</td>";
                });
                tbl_tr += "/<tr>";
                $(ObjectId + " tbody").append(tbl_tr);
            }
        });
    });
}

$(function () {
    $.fn.bootstrapSwitch.defaults.size = "mini";
    $(".switchery").bootstrapSwitch();

    $(".switchery").on("switchChange.bootstrapSwitch",
        function (event, state) {
            var currId = $(this).prop("id");

            if ($(this).prop("checked") == false) {
                $(".switchery3").each(function () {
                    if ($(this).attr("parentid") == currId || $(this).attr("parentid2") == currId) {
                        $(this).prop("checked", false);
                    }
                });

                $(".switchery2").each(function () {
                    if ($(this).attr("parentid") == currId || $(this).attr("parentid2") == currId) {
                        $(this).bootstrapSwitch("state", false, false);
                    }
                });

                $(".child_for_" + currId).slideUp(300);
                $(".child2_for_" + currId).slideUp(300);
            } else {
                $(".switchery3").each(function () {
                    if ($(this).attr("parentid") == currId || $(this).attr("parentid2") == currId) {
                        $(this).prop("checked", true);
                    }
                });

                $(".switchery2").each(function () {
                    if ($(this).attr("parentid") == currId || $(this).attr("parentid2") == currId) {
                        $(this).bootstrapSwitch("state", true, true);
                    }
                });

                $(".child2_for_" + currId).slideDown(300);
                $(".child_for_" + currId).slideDown(300);
            }
            event.preventDefault();
        });

    $(".select").select2();
});