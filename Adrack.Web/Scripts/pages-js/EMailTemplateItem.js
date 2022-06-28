$(document).ready(function () {
    $(".template-token").click(function () {
        var wysihtml5Editor = $("#TemplateBody").data("wysihtml5").editor;
        wysihtml5Editor.composer.commands.exec("insertHTML", " " + $(this).html() + " ");
    });
});