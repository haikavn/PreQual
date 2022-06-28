$(document).ready(function () {
    $("#add_new").click(function () {
        window.location = "/Management/Role/Item";
    });
    GenerateGridTable("GridView1", "/getroles", "ID, Name, Role");
});