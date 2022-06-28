var actionsArr = [
    {
        Name: "Change Status",
        Url: "",
        Class: "ActionEditBtn",
        Modal: "modal_form_change_status",
        IconClass: "glyphicon glyphicon-edit green",
        Confirm: 0
    },
    { Name: "Detete", Url: "/Accounting/DeleteRefundedLeds", IconClass: "glyphicon glyphicon-remove red", Confirm: 1 }
];

$(function () {
    $(".ChangeStatusModal").click(function () {
        if (!GridSelectedID) {
            alert("Plaese Select a Row");
            return false;
        }

        var Status = $("tbody .selected td:nth-child(10)").text();
        switch (Status) {
            case "Pending":
                {
                    $("#RefundedStatus option[value=0]").attr("selected", "selected");
                    break;
                }
            case "Approved":
                {
                    $("#RefundedStatus option[value=1]").attr("selected", "selected");
                    break;
                }
            case "Reject":
                {
                    $("#RefundedStatus option[value=2]").attr("selected", "selected");
                    break;
                }
        }
        $("#ReviewNote").val("");
    });

    $("#Btn_ChangeStatus").click(function () {
        var data = "id=" + GridSelectedID + "&status=" + $("#RefundedStatus").val() + "&note=" + $("#ReviewNote").val();
        $.post("/Accounting/ChangeRefundedStatus", data).done(function (retData) {
            GenerateGridTable("GridViewRefunded",
                "/Management/Accounting/GetRefundedLeads",
                "ID, Lead ID, Date Created, APrice, BPrice, AInvoiceId, BInvoiceId, Reason, ReviewNote, Status, Action");
        });
    });

    GenerateGridTable("GridViewRefunded",
        "/Management/Accounting/GetRefundedLeads",
        "ID, Lead ID, Date Created, APrice, BPrice, AInvoiceId, BInvoiceId, Reason, ReviewNote, Status, Action");
});