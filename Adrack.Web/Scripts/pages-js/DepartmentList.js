$(document).ready(function () {
    $("#add_new").click(function () {
        window.location = "/Management/Department/Item";
    });

    GenerateGridTable("GridView1", "/getdepartments", "ID, Name");
});