var SupportTicketsParams = {
    str: "",
    init: function (_str) {
        this.str = _str;
    }
};

$(document).ready(function () {
    $("#AddTicketBtn").click(function () {
        $("#TicketSubject").val("");
        $("#TicketMessage").val("");
    });

    $("input[name=statusradio]:radio").change(function () {
        if ($(this).data("id") == 1) {
            GenerateGridTable("GridViewSupportTickets",
                "/Management/Support/GetSupportTickets?t=1&buyerid=" + SupportTicketsParams.str,
                "ID, Subject, Reporter, Assigned To, Priority, Status, Date/Time");
        }
        else {
            GenerateGridTable("GridViewSupportTickets",
                "/Management/Support/GetSupportTickets?t=0&buyerid=" + SupportTicketsParams.str,
                "ID, Subject, Reporter, Assigned To, Priority, Status, Date/Time");
        }
    });

    $("#Btn_AddPayment").click(function () {
        if ($.trim($("#TicketSubject").val()) == "" || $.trim($("#TicketMessage").val()) == "") {
            return;
        }
        var ccIDs = null;
        if ($("#AssignToCC").val() != undefined) {
            ccIDs = $("#AssignToCC").val();
        }

        var formData = new FormData();
        formData.append("file", $("#attach-file")[0].files[0]);
        formData.append("managerid", $("#AssignTo").val());
        formData.append("cc", ccIDs);
        formData.append("subject", $("#TicketSubject").val());
        formData.append("message", $("#TicketMessage").val());
        formData.append("priority", $("#TicketPriority").val());

        $("#TicketMessageText").val("");
        $(".file-preview .close").trigger("click");

        $.ajax({
            url: "/Management/Support/AddTicket",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                $(".close").trigger("click");
                GenerateGridTable("GridViewSupportTickets",
                    "/Management/Support/GetSupportTickets?buyerid=" + SupportTicketsParams.str,
                    "ID, Subject, Reporter, Assigned To, Priority, Status, Date/Time");
            }
        });

        /*
                var url = "/Management/Support/AddTicket";
                var data = "managerid=" +
                    $("#AssignTo").val() +
                    "&cc=" +
                    ccIDs +
                    "&subject=" +
                    $("#TicketSubject").val() +
                    "&message=" +
                    $("#TicketMessage").val() +
                    "&priority=" +
                    $("#TicketPriority").val();

                $.post(url, data).done(function(retData) {
                    $(".close").trigger("click");
                    GenerateGridTable("GridViewSupportTickets",
                        "/Management/Support/GetSupportTickets?buyerid=" + SupportTicketsParams.str,
                        "ID, Subject, Reporter, Assigned To, Priority, Status, Date/Time");
                });
        */
    });
});