$(document).ready(function () {
    $("#add_new").click(function () {
        window.location = "/Management/Campaign/TemplateItem";
    });

    GenerateGridTable("GridView1", "/GetCampaignTemplates", "ID, Name, Vertical");
});