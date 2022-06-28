$(document).ready(function () {
    $("#MinProcessingMode").change(function () {
        if ($(this).val() == 1) {
            b = confirm("This function will stop dublicated leads checking, credits and schedules processing. This function is only for testing purposes.\nAre you sure?");
            if (b == false) {
                $("#MinProcessingMode").val("0");
            }
        }
    });

    $("#SystemOnHold").change(function () {
        if ($(this).val() == 1) {
            b = confirm("This function will put system on hold.\nAre you sure you want to continue?");
            if (b == false) {
                $("#SystemOnHold").val("0");
            }
        }
    });
});