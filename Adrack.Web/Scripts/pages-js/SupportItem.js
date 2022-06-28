var SupportItemParams = {
    str: "",
    init: function (_str) {
        this.str = _str;
    }
};

function GetSupportTicketsMessages() {
    var url = "/Management/Support/GetSupportTicketsMessages";
    var data = "ticketid=" + SupportItemParams.str;
    $.post(url, data).done(function (retData) {
        $("#MessagesContainer").html(retData);

        $(".chat-list").stop().animate({
            'scrollTop': $("#MessagesContainer").height()
        },
            300,
            "swing",
            function () {
            });
    });
}

$(document).ready(function () {
    GetSupportTicketsMessages();

    window.setInterval(function () {
        GetSupportTicketsMessages();
    },
        4000);

    $("#attach-file").bind("change",
        function () {
            if (this.files[0].size / 1024 / 1024 > 2) {
                alert("This file size is: " + this.files[0].size / 1024 / 1024 + "MB,\nwitch is larger than allowed");
                $(".file-preview .close").trigger("click");
            }
        });

    $("#AddTicketMessage").click(function () {
        if ($.trim($("#TicketMessageText").val()) == "" && $("#attach-file")[0].files[0] === undefined)
            return;

        var formData = new FormData();
        formData.append("file", $("#attach-file")[0].files[0]);
        formData.append("ticketid", SupportItemParams.str);
        formData.append("message", $("#TicketMessageText").val());

        $("#TicketMessageText").val("");
        $(".file-preview .close").trigger("click");

        $.ajax({
            url: "/Management/Support/AddTicketsMessages",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                GetSupportTicketsMessages();
            }
        });
    });

    // $("input[name=changestatusradio]:radio").change(function () {
    $(".support-action-btn button").click(function () {
        var url = "/Management/Support/ChangeTicketsStatus";
        var data = "ticketid=" + SupportItemParams.str + "&status=" + $(this).data("id");

        $.post(url, data).done(function (retData) {
            document.location.reload();
            /*
            GetSupportTicketsMessages();

            if ($(this).data("id") == 1) {
                $("#messaging-container").fadeOut();
                $(this).addClass("btn-warning");
            }
            else {
                $("#messaging-container").fadeIn();
                $(this).addClass("btn-success");
            }
            */
        });
    });

    $(document).keypress(13,
        function (e) {
            if (e.ctrlKey) {
                $("#AddTicketMessage").trigger("click");
            }
        });
});