var GridSelectedID = 0;
var GridParams = null;
var isClockAjax = false;

var today = new Date("01/01/2016");

// Cookies

function readCookieIndex(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(";");
    for (var i = ca.length - 1; i >= 0; i--) {
        var c = ca[i];
        while (c.charAt(0) == " ") c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return i;
    }
    return null;
}

function createCookie(name, value, days) {
    var cookStr = document.cookie;
    /*
        var index = cookStr.indexOf(name + "=");
        if (index >= 0)
        {
            var index2 = cookStr.indexOf(";", index);
            if (index2 < 0) index2 = cookStr.length;
            var outStr = cookStr.substring(0, index + name.length + 1) + value + cookStr.substring(index2);
            document.cookie = outStr;
        }
    */
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    } else var expires = "";

    document.cookie = name + "=" + value + expires + "; path=/;";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(";");
    for (var i = ca.length - 1; i >= 0; i--) {
        var c = ca[i];
        while (c.charAt(0) == " ") c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}

function clock() {
    today.setSeconds(today.getSeconds() + 1);

    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    if (h < 10) {
        h = "0" + h;
    }
    if (m < 10) {
        m = "0" + m;
    }
    if (s < 10) {
        s = "0" + s;
    }

    var y = today.getFullYear();
    var mn = today.getMonth() + 1;
    var d = today.getDate();

    if (mn < 10) mn = "0" + mn;

    $("#clock").html(mn + "/" + d + "/" + y + " <i class='fa fa-clock-o'> </i> " + h + ":" + m + ":" + s);
}

$(document).ready(function () {
    $("#clock").hide();

    /*$(document).ajaxStart(function () {
        if (isClockAjax == true) {
            return;
        }
        var d1 = new Date();
        d1.setMinutes(d1.getMinutes() + parseInt($("#cookieExpireMinutes").val()));
        $.cookie('cookieExpireMinutes', d1.getTime());
    });*/

    /*AZ
        setInterval(function () {
            if ($.cookie('cookieExpireMinutes') != null) {
                var now = new Date();
                if (parseInt($.cookie('cookieExpireMinutes')) < now.getTime()) {
                    $.cookie('cookieExpireMinutes', null);
                    document.location = "/logout";
                }
            }
        }, 10 * 1000);
    */
    $(".buyer-submenu li a").removeClass("active");

    $(".buyer-submenu li a").each(function () {
        console.log($(this).attr("href") + " " + (document.location.pathname));

        if ($(this).attr("href").indexOf(document.location.pathname) != -1) {
            $(this).addClass("active");
        }
    });

    $.post("/Management/common/GetTimezoneNow").done(function (retData) {
        today = new Date(retData);
        $("#clock").show();
    });

    setInterval(function () {
        isClockAjax = true;
        $.post("/Management/common/GetTimezoneNow").done(function (retData) {
            today = new Date(retData);
            $("#clock").show();
            isClockAjax = false;
        });
    },
        2 * 60 * 1000);

    setInterval(function () {
        clock();
    },
        1000);

    if (readCookie("OpenMenu") != undefined && readCookie("OpenMenu") == 1) {
        $("body").removeClass("sidebar-xs");
    }

    $(".sidebar-main-toggle").click(function () {
        if (readCookie("OpenMenu") == undefined) {
            createCookie("OpenMenu", 0, 365);
        }

        if (readCookie("OpenMenu") == 1) {
            createCookie("OpenMenu", 0, 365);
        } else {
            createCookie("OpenMenu", 1, 365);
        }
    });
});

function GenerateGridPagination() {
    var data = ""; // 'params=' + $('.daterange-ranges span').attr("data-date");
    $.post("/Management/Lead/GetLeadsCount", data).done(function (retData) {
        $("#GridViewLeads_paginate").html("");
        var str = "<span>";
        for (i = 1; i <= 10; i++) {
            str += '<a class="paginate_button ' +
                (i == $("#GridPageNumber").val() ? "first-page current" : "") +
                '" aria-controls="tbl_GridViewLeads" data-dt-idx="' +
                i +
                '" tabindex="0">' +
                i +
                "</a>";
        }
        str += "</span>";
        $("#GridViewLeads_paginate").html(str);
    }
    );
}

function GenerateGridTable(objID,
    dataPath,
    Fields,
    ActionsArr,
    Page,
    Pagesize,
    Params,
    FilterSort,
    ordercolumn,
    orderdir) {
    if (FilterSort == undefined) {
        FilterSort = true;
    }

    if (ordercolumn == undefined) {
        ordercolumn = 0;
    }

    if (orderdir == undefined) {
        orderdir = "desc";
    }

    $("#GridPageNumber").val(Page);

    Pagesize = Pagesize == null ? 25 : Pagesize;

    GridParams = { _objID: objID, _dataPath: dataPath, _Fields: Fields, _ActionsArr: ActionsArr };

    $(document).ready(function () {
        var fields = Fields.split(",");
        var tableStr = '<table id="tbl_' +
            objID +
            '" class="display table datatable-html dataTable"  role="grid" aria-describedby="DataTables_Table_0_info" cellspacing="0" width="100%" style="text-align: center"><thead><tr>';
        fields.forEach(function (item, i, arr) {
            tableStr += "<th>" + item.trim() + "</th>";
        });

        tableStr += "</tr></thead></table>";

        $("#" + objID).empty();
        $("#" + objID).append(tableStr);

        var contentHeight = $(window).height() -
            $("body > .navbar").outerHeight() -
            $("body > .navbar-fixed-top:not(.navbar)").outerHeight() -
            $("body > .navbar-fixed-bottom:not(.navbar)").outerHeight() -
            $("body > .navbar + .navbar").outerHeight() -
            $("body > .navbar + .navbar-collapse").outerHeight() -
            260;

        var data_table =
            $("#tbl_" + objID).dataTable({
                autoWidth: true,
                /* scrollY: contentHeight, */
                "order": [[ordercolumn, orderdir]],
                "searching": FilterSort,
                "ordering": FilterSort,
                "processing": true,
                "serverSide": false,
                "iDisplayLength": Pagesize,
                "paging": Page != null ? false : FilterSort,
                "info": Page != null ? false : FilterSort,

                ajax: {
                    url: dataPath,
                    data: {
                        "actions": JSON.stringify(ActionsArr),
                        "page": Page,
                        "pagesize": Pagesize,
                        "params": Params
                    },
                    processData: true,
                    dataType: "json",
                    type: "POST"
                },

                "fnDrawCallback": function (oSettings) {
                    var api = this.api();
                    var jsonD = api.ajax.json();

                    if (jsonD != undefined && jsonD.recordsSum != undefined && jsonD.recordsSum > 0) {
                        console.log(jsonD.recordsSum);
                        footerStr = "";
                        fields.forEach(function (item, i, arr) {
                            if (i == 0) footerStr += "<td><b>Total:</b> </td>";
                            if (item.trim() == "Total" || item.trim() == "Amount") {
                                footerStr += '<td><div style="font-weight:bold; " class="TotalSum_' +
                                    item.trim() +
                                    '">' +
                                    jsonD.recordsSum +
                                    "</td>";
                            } else {
                                footerStr += '<td style="padding: 5px;">&nbsp;</td>';
                            }
                        });

                        $("#tbl_" + objID + "_info")
                            .html(
                                '<table width="100%" style="font-size:15px; background-color: #eeeeee; margin: 5px 0px"><tfoot>' +
                                footerStr +
                                "</tfoot></table>");
                    }
                }
            });

        if (Page != null) {
            GenerateGridPagination();
        }

        $(".datatable-html tbody").on("click",
            "tr",
            function () {
                $(".datatable-html tbody tr").removeClass("selected");
                $(this).toggleClass("selected");

                GridSelectedID = $(this).find("td:eq(0)").html();

                if (isNaN(parseInt(GridSelectedID))) {
                    GridSelectedID = $(this).find("td:eq(1)").html();
                }
            });
    });
}

function GenerateGridTableActions(objID, dataPath, Fields, actions) {
    return;
    $(document).ready(function () {
        var fields = Fields.split(",");
        var tableStr = '<table id="tbl_' +
            objID +
            '" class="display table datatable-html dataTable"  role="grid" aria-describedby="DataTables_Table_0_info" cellspacing="0" width="100%"><thead><tr>';
        fields.forEach(function (item, i, arr) {
            tableStr += "<th>" + item.trim() + "</th>";
        });
        tableStr += "</tr></thead></table>";

        $("#" + objID).empty();
        $("#" + objID).append(tableStr);

        $("#tbl_" + objID).DataTable({
            "processing": true,
            "serverSide": false,
            "order": [[0, "desc"]],
            select: true,
            "ajax": dataPath,

            "initComplete": function (settings, json) {
                /*
                                $('#tbl_' + objID + ' thead tr').each(function () {
                                    $(this).append('<th style="width: 112px; align: center;" tabindex="0" aria-controls="tbl_GridViewPayments" rowspan="1" colspan="1">Actions</th>');
                                });

                                $('#tbl_' + objID + ' tbody tr').each(function () {
                                    $(this).addClass("ID_" + $(this).find('td:eq(0)').html());

                                    var ActionsStr = '<td><ul class=\"icons-list\"><li class=\"dropdown\"><a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\"><i class=\"icon-menu9\"></i></a><ul class=\"dropdown-menu dropdown-menu-right\">';
                                    for (i = 0; i < actions.length; i++) {
                                        var SelfClass = '';
                                        if (actions[i].Class !== undefined) {
                                            SelfClass = 'class="' + actions[i].Class + '"';
                                        }
                                        var SelfData = '';
                                        if (actions[i].Modal !== undefined) {
                                            SelfData = 'data-target="#' + actions[i].Modal + '" data-toggle="modal"';
                                        }

                                        ActionsStr += '<li><a href=\"#\" ' + SelfData +
                                            (actions[i].Url !='' ? (' onclick="doButtonAction(' + $(this).find('td:eq(0)').html() + ', \'' + actions[i].Url + '\', ' + actions[i].Confirm + ');" '): '')
                                            + SelfClass + '><i class=\"glyphicon ' + actions[i].IconClass + '\"></i> ' + actions[i].Name + '</a></li>';
                                    }
                                    ActionsStr += '</ul></li></ul></td>';

                                    $(this).append(ActionsStr);
                                });
                */
            }
        });

        $(".datatable-html tbody").on("click",
            "tr",
            function () {
                $(".datatable-html tbody tr").removeClass("selected");
                $(this).toggleClass("selected");
            });
    });
}

function doButtonAction(id, url, confirmation) {
    //    alert(id + ' ' + url + ' ' + confirm);
    if (!url)
        return;

    if (confirmation == 1) {
        if (!confirm("Are You sure?"))
            return false;
    }

    var data = "id=" + GridSelectedID;
    $.post(url, data).done(function (retData) {
        if (GridParams) {
            GenerateGridTable(GridParams._objID, GridParams._dataPath, GridParams._Fields, GridParams._ActionsArr);
        }
        // var jsonData = $.parseJSON(retData);
    });
}

function JsonToTable(data) {
    var tbl_body = "";
    var odd_even = false;
    $.each(data,
        function () {
            var tbl_row = "";
            $.each(this,
                function (k, v) {
                    // tbl_row += "<td>" + v + "</td>";
                    console.log(v);
                });
            // tbl_body += "<tr class=\"" + (odd_even ? "odd" : "even") + "\">" + tbl_row + "</tr>";
            // odd_even = !odd_even;
        });

    //    $("#target_table_id tbody").html(tbl_body);
    //console.log(tbl_body);
}

function ConvertToMoney(nStr) {
    nStr += "";
    x = nStr.split(".");
    x1 = x[0];
    x2 = x.length > 1 ? "." + x[1] : "";
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, "$1" + "," + "$2");
    }

    return (x1 + x2);
}

var m_strUpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
var m_strLowerCase = "abcdefghijklmnopqrstuvwxyz";
var m_strNumber = "0123456789";
var m_strCharacters = "!@#$%^&*()_+~`|}{[]\:;?><,./-=";

var passwordScore = 0;

function checkPassword(strPassword) {
    // Reset combination count
    var nScore = 0;

    if (strPassword.length < 8) {
        //nScore += 5;
    } else if (strPassword.length >= 8 && strPassword.length < 10) {
        nScore += 15;
    } else if (strPassword.length >= 10) {
        nScore += 20;
    }

    // Letters
    var nUpperCount = countContain(strPassword, m_strUpperCase);
    var nLowerCount = countContain(strPassword, m_strLowerCase);
    var nLowerUpperCount = nUpperCount + nLowerCount;
    // -- Letters are all lower case
    if (nUpperCount > 0 || nLowerCount > 0) {
        nScore += 15;
    }
    // Numbers
    var nNumberCount = countContain(strPassword, m_strNumber);
    // -- 1 number
    if (nNumberCount >= 1) {
        nScore += 15;
    }

    // Characters
    var nCharacterCount = countContain(strPassword, m_strCharacters);
    // -- 1 character
    if (nCharacterCount == 1) {
        nScore += 10;
    }
    // -- More than 1 character
    if (nCharacterCount > 1) {
        nScore += 20;
    }

    return nScore;
}

// Runs password through check and then updates GUI

function runPassword(strPassword, strFieldID) {
    // Check password
    var nScore = checkPassword(strPassword);

    passwordScore = nScore;

    // Get controls
    var ctlBar = document.getElementById(strFieldID + "_bar");
    if (!ctlBar)
        return;

    // Set new width
    ctlBar.style.width = (nScore * 1.25 > 100) ? 100 : nScore * 1.25 + "%";
    ctlBar.style.height = "5px";

    // Color and text
    // -- Very Secure
    /*if (nScore >= 90)
    {
        var strText = "Very Secure";
        var strColor = "#0ca908";
    }
    // -- Secure
    else if (nScore >= 80)
    {
        var strText = "Secure";
        vstrColor = "#7ff67c";
    }
    // -- Very Strong
    else
    */
    if (nScore >= 80) {
        var strText = "Very Strong";
        var strColor = "#008000";
    }
    // -- Strong
    else if (nScore >= 60) {
        var strText = "Strong";
        var strColor = "#006000";
    }
    // -- Average
    else if (nScore >= 40) {
        var strText = "Average";
        var strColor = "#e3cb00";
    }
    // -- Weak
    else if (nScore >= 20) {
        var strText = "Weak";
        var strColor = "#Fe3d1a";
    }
    // -- Very Weak
    else {
        var strText = "Very Weak";
        var strColor = "#e71a1a";
    }

    if (strPassword.length == 0) {
        ctlBar.style.backgroundColor = "";
    } else {
        ctlBar.style.backgroundColor = strColor;
    }

    return nScore;
}

// Checks a string for a list of characters
function countContain(strPassword, strCheck) {
    // Declare variables
    var nCount = 0;

    for (i = 0; i < strPassword.length; i++) {
        if (strCheck.indexOf(strPassword.charAt(i)) > -1) {
            nCount++;
        }
    }

    return nCount;
}

function validatePassword() {
    if (passwordScore >= 50) return true;

    alert("Password strength is not matching the requirements");

    return false;
}

var Password = {
    generate: function (len) {
        var length = (len) ? (len) : (10);
        var string = "abcdefghijklmnopqrstuvwxyz"; //to upper
        var numeric = "0123456789";
        var punctuation = "!@#$%^&*()_+~`|}{[]\:;?><,./-=";
        var password = "";
        var character = "";
        var crunch = true;
        while (password.length < length) {
            entity1 = Math.ceil(string.length * Math.random() * Math.random());
            entity2 = Math.ceil(numeric.length * Math.random() * Math.random());
            entity3 = Math.ceil(punctuation.length * Math.random() * Math.random());
            hold = string.charAt(entity1);
            hold = (entity1 % 2 == 0) ? (hold.toUpperCase()) : (hold);
            character += hold;
            character += numeric.charAt(entity2);
            character += punctuation.charAt(entity3);
            password = character;
        }
        return password;
    }
};

function RoundNum(num, length) {
    var number = Math.round(num * Math.pow(10, length)) / Math.pow(10, length);
    return number;
}

function NotificationPopup(title, text, mode = 'bg-success') {
    new PNotify({
        title: title,
        text: text,
        addclass: mode
    });
}

function NotificationHelperPopup(type, title, text) {
    var stack_custom_top = { "dir1": "down", "dir2": "right", "push": "top", "spacing1": 1 };

    var opts = {
        title: title,
        text: text,
        width: "100%",
        cornerclass: "no-border-radius",
        addclass: "stack-custom-top bg-primary",
        stack: stack_custom_top
    };
    switch (type) {
        case 'error':
            opts.title = title;
            opts.text = text;
            opts.addclass = "stack-custom-top bg-danger";
            opts.type = "error";
            break;

        case 'info':
            opts.title = title;
            opts.text = text;
            opts.addclass = "stack-custom-top bg-info";
            opts.type = "info";
            break;

        case 'success':
            opts.title = title;
            opts.text = text;
            opts.addclass = "stack-custom-top bg-success";
            opts.type = "success";
            break;
    }
    new PNotify(opts);
}

function ShowLoader() {
    //$(".loader").fadeIn(50);
    $(".loader").show();
}

function HideLoader() {
    $(".loader").fadeOut(50);
}