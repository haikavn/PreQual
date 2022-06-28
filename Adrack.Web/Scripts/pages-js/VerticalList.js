$(document).ready(function () {
    $("#add_new").click(function () {
        window.location = "/Management/Vertical/Item";
    });
    GenerateGridTable("GridView1", "/GetVerticals", "ID, Name");
});