var AdrackCommon = {
    ajaxCall: function (url, data, func, method, async, cache) {
        if (method === undefined) method = "POST";
        if (async == undefined) async = false;
        if (cache == undefined) cache = false;

        $.ajax({
            async: async,
            cache: cache,
            type: method,
            url: url,
            data: data,
            success: function (result) {
                if (func !== undefined)
                    func(result);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    },

    formValidation: function (selector, rules) {
        $(selector).formwizard({
            disableUIStyles: true,
            validationEnabled: true,
            inDuration: 150,
            outDuration: 150,
            remoteAjax: remoteAjax,
            validationOptions: {
                ignore: 'input[type=hidden], .select2-search__field', // ignore hidden fields
                errorClass: 'validation-error-label',
                successClass: 'validation-valid-label',
                highlight: function (element, errorClass) {
                    $(element).removeClass(errorClass);
                },
                unhighlight: function (element, errorClass) {
                    $(element).removeClass(errorClass);
                },

                // Different components require proper error label placement
                errorPlacement: function (error, element) {

                    // Styled checkboxes, radios, bootstrap switch
                    if (element.parents('div').hasClass("checker") ||
                        element.parents('div').hasClass("choice") ||
                        element.parent().hasClass('bootstrap-switch-container')) {
                        if (element.parents('label').hasClass('checkbox-inline') ||
                            element.parents('label').hasClass('radio-inline')) {
                            error.appendTo(element.parent().parent().parent().parent());
                        } else {
                            error.appendTo(element.parent().parent().parent().parent().parent());
                        }
                    }
                    // Unstyled checkboxes, radios
                    else if (element.parents('div').hasClass('checkbox') ||
                        element.parents('div').hasClass('radio')) {
                        error.appendTo(element.parent().parent().parent());
                    }
                    // Input with icons and Select2
                    else if (element.parents('div').hasClass('has-feedback') ||
                        element.hasClass('select2-hidden-accessible')) {
                        error.appendTo(element.parent());
                    }
                    // Inline checkboxes, radios
                    else if (element.parents('label').hasClass('checkbox-inline') ||
                        element.parents('label').hasClass('radio-inline')) {
                        error.appendTo(element.parent().parent());
                    }
                    // Input group, styled file input
                    else if (element.parent().hasClass('uploader') || element.parents().hasClass('input-group')) {
                        error.appendTo(element.parent().parent());
                    } else {
                        error.insertAfter(element);
                    }
                },
                rules: rules
            }
        });
    }



}