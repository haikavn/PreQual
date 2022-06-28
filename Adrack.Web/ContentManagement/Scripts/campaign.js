var AdrackCampaign = {
    campaignId: 0,
    canRedirect: false,
    canDragDrop: true,
    xmlTpl: '',
    rowNum: 1,
    systemFields: [],
    dataTypes: [],
    blackList: [],
    pingTreeList: [],

    getCampaignInfoUrl: '',
    getCampaignsByVerticalIdUrl: '',
    getCampaignTemplatesByVerticalIdUrl: '',
    loadFromXmlUrl: '',
    currentFilterSettingsButton: null,

    init: function (campaignId, getCampaignInfoUrl, getCampaignsByVerticalIdUrl, getCampaignTemplatesByVerticalIdUrl, loadFromXmlUrl, _canDragDrop) {
        this.campaignId = campaignId;
        this.getCampaignInfoUrl = getCampaignInfoUrl;
        this.getCampaignsByVerticalIdUrl = getCampaignsByVerticalIdUrl;
        this.getCampaignTemplatesByVerticalIdUrl = getCampaignTemplatesByVerticalIdUrl;
        this.loadFromXmlUrl = loadFromXmlUrl;
        this.canDragDrop = _canDragDrop;

        var self = this;

        $("#item-form").bind("keypress", function (e) {
            if (e.keyCode == 13) {
                return false;
            }
        });

        if (location.hash != "") {
            aElement = location.hash.replace('#', '');
            $("." + aElement).trigger("click");
        }

        $(".nav-tabs li").click(function () {
            location.hash = $(this).find("a").attr("href");
        });

        $('#add_new_filter').click(function () {
            window.location = '/Management/Filter/Item/-' + self.campaignId;
        });

        $('#add_new_buyer').click(function () {
            window.location = '/Management/BuyerChannel/Create?campaignid=' + self.campaignId;
            return false;
        });

        $('#add_new_affiliate').click(function () {
            window.location = '/Management/AffiliateChannel/Create?campaignid=' + self.campaignId;
            return false;
        });

        $("#campaigns").change(function () {
            var selectedItem = $(this).val();
            if (selectedItem > 0)
                self.getInfo(selectedItem, true);
        });

        $("#campaignstpl").change(function () {
            var selectedItem = $(this).val();
            if (selectedItem > 0)
                self.getInfo(selectedItem, true);
        });

        $("#VerticalId").change(function () {
            var selectedItem = $(this).val();

            self.getCampaignsByVerticalId(selectedItem);
            self.getCampaignTemplatesByVerticalId(selectedItem);
        });

        $('.select').select2();

        $('#btnSubmitClose').on('click', function () {
            self.canRedirect = true;
            return true;
        });

        $('#item-form').on('submit', function (e) {
            e.preventDefault();

            var totalPercent = 0;
            var order = -1;

            if ($('#PingTreeCycle').val() !== '0')
            if (self.pingTreeList.length > 0) {
                for (var i = 0; i < self.pingTreeList.length; i++) {
                    if (order != parseInt(self.pingTreeList[i].order)) {
                        if (self.pingTreeList[i].rate > 0) {
                            if (totalPercent > 100) {
                                alert('The sum of percents of ping tree group "' + (order == -1 ? 0 : order) + '" can not be greater than 100%');
                                return;
                            }
                            else if (totalPercent < 100 && order >= 0) {
                                alert('The sum of percents of ping tree group "' + (order == -1 ? 0 : order) + '" can not be less than 100%');
                                return;
                            }
                        }

                        order = parseInt(self.pingTreeList[i].order);
                        totalPercent = 0;
                    }

                    totalPercent += parseInt(self.pingTreeList[i].rate);
                }

                if (totalPercent > 100) {
                    alert('The sum of percents of ping tree group "' + (order == -1 ? 0 : order) + '" can not be greater than 100%');
                    return;
                }
                else if (totalPercent < 100) {
                    alert('The sum of percents of ping tree group "' + (order == -1 ? 0 : order) + '" can not be less than 100%');
                    return;
                }
            }

            $('#tab-main').trigger('click');

            var disabledElements = $(this).find(':disabled');
            disabledElements.removeAttr('disabled');

            if ($('#VerticalId').val() == '0') {
                alert('Vertical is not selected');
                return;
            }

            var tpl = [];

            var sn = null;
            var lastsn = "";

            tpl = [];

            var tree = $(".tree-table").fancytree("getTree");

            if (tree != undefined && tree.rootNode != undefined) {
                tpl = self.getNodes(tree, tree.rootNode, "root", tpl);

                if (tpl.length == 0) {
                    alert('Campaign template is not defined');
                    return;
                }
            }

            var tpljson = JSON.stringify(tpl);

            var btn = $(":input[type=submit]:focus");

            ShowLoader();

            
            ///
            $.ajax({
                url: $(this).attr('action'),
                type: "POST",
                async: true,
                data: $(this).serialize() + '&json=' + tpljson + '&xml=' + self.xmlTpl + '&pingTree=' + JSON.stringify(self.pingTreeList),
                success: function (data) {
                    HideLoader();
                    if (data.error != undefined) {
                        alert(data.error);
                    }
                    else {
                        self.campaignId = data.id;

                        $('#add_new_filter').prop('disabled', false);
                        $('#campaignid').val(self.campaignId);

                        $('.panel-title').text(self.campaignId + '-' + data.name);

                        if (self.canRedirect)
                            window.location = '/management/campaign/list';
                    }

                    self.canRedirect = false;

                    disabledElements.attr('disabled', 'disabled');
                },
                error: function (jXHR, textStatus, errorThrown) {
                    disabledElements.attr('disabled', 'disabled');
                    HideLoader();
                }
            });

            return false;
        });

        this.initTree();

        if (self.campaignId > 0) {
            setTimeout(function () { self.getInfo(self.campaignId, false); }, 500);
        }

        $('#load_template_btn').click(function () {
            $('.modal-body').block({
                message: '<i class="icon-spinner2 spinner"></i>',
                overlayCSS: {
                    backgroundColor: '#1B2024',
                    opacity: 0.85,
                    cursor: 'wait'
                },
                css: {
                    border: 0,
                    padding: 0,
                    backgroundColor: 'none',
                    color: '#fff'
                }
            });

            xmlTpl = $('#xml_template').val();

            self.loadFromXml(xmlTpl);
        });
    },

    loadFromXml: function (xmlTpl) {
        var self = this;

        $.ajax({
            cache: false,
            type: "POST",
            url: self.loadFromXmlUrl,
            data: { xml: xmlTpl, campaignid: self.campaignId },
            success: function (data) {
                self.loadTree(data);
                $('.modal-body').unblock();
                $('#modal_default').modal('hide');
                //setTimeout(function () { $('#modal_default').hide(); }, 500);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    },

    addSystemField: function (name, value) {
        this.systemFields.push({ value: value, name: name });
    },

    addDataType: function (name, value) {
        this.dataTypes.push({ value: value, name: name });
    },

    addBlackList: function (name, value) {
        this.blackList.push({ value: value, name: name });
    },

    getInfo: function (campaignid, isClone) {
        var self = this;
        AdrackCommon.ajaxCall(this.getCampaignInfoUrl, { campaignid: campaignid, xml: $('#xml_template').val(), isClone: isClone }, function (result) {
            self.xmlTpl = result.xml;
            console.log(result.items.children.length);
            if (result.items.children.length > 0)
                self.loadTree(result.items);
            else {
                self.loadFromXml("", campaignid);
            }
        });
    },

    getCampaignsByVerticalId: function (verticalid) {
        var ddlStates = $("#campaigns");
        AdrackCommon.ajaxCall(this.getCampaignsByVerticalIdUrl, { verticalId: verticalid }, function (result) {
            ddlStates.html('');
            $.each(result, function (id, option) {
                ddlStates.append($('<option></option>').val(option.id).html(option.name));
            });
        });
    },

    getCampaignTemplatesByVerticalId: function (verticalid) {
        var ddlStates2 = $("#campaignstpl");
        AdrackCommon.ajaxCall(this.getCampaignTemplatesByVerticalIdUrl, { verticalId: verticalid }, function (result) {
            ddlStates2.html('');
            $.each(result, function (id, option) {
                ddlStates2.append($('<option></option>').val(option.id).html(option.name));
            });
        });
    },

    getValidatorType: function (type, element, select) {
        AdrackCommon.ajaxCall("/GetValidatorType/" + type, null, function (result) {
            var obj = null;

            try {
                obj = jQuery.parseJSON(result);
            }
            catch (err) {
            }

            $(element).html('');
            $(element).hide();

            if (obj == null) {
                console.log($(element));
                $(element).html(result);
                $(element).show();
            }
            else {
                var res = obj.validators.split(',');
                //select.parent().parent().children(".validator-select option").removeAttr('disabled');
                //select.parent().parent().children(".validator-select option:first").attr('selected','selected');
                //$('#data-types-' + select.data('row')).children("option").prop('disabled', false);
                //$('#data-types-' + select.data('row')).children("option").prop('selected', false);
                //$('#data-types-' + select.data('row')).children("option:first").prop('selected', true);

                if (obj.validators != '') {
                    var gfound = false;
                    //select.parent().parent().children(".validator-select option").each(function ()
                    $('#data-types-' + select.data('row')).children("option").each(function () {
                        $(this).prop('selected', false);
                        $(this).prop('disabled', false);

                        var found = false;

                        for (var i = 0; i < res.length; i++) {
                            if (res[i] == $(this).val()) {
                                if (!gfound) {
                                    $(this).prop('selected', true);
                                }
                                gfound = true;
                                found = true;
                                break;
                            }
                        }

                        if (!found && $(this).val() != 0) {
                            $(this).prop('disabled', true);
                        }
                    });

                    $('#data-types-' + select.data('row')).trigger('change');
                }
                else {
                    $(element).html('');

                    $('#data-types-' + select.data('row')).children("option").each(function () {
                        $(this).prop('selected', false);
                        $(this).prop('disabled', false);
                    });

                    $('#data-types-' + select.data('row')).find("option:first").prop('selected', true);
                }

                //select.parent().parent().children(".validator-select").trigger('change');

                //$(".validator-select").select2();
                select.parent().parent().find('.field-description').val(obj.format);
            }
        }, 'GET');
    },

    loadTree: function (source) {
        var tree = $('.tree-table').fancytree('getTree');
        if ($.isFunction(tree.reload)) {
            tree.reload(source);
            $(".fancytree-container tr td:first-child").hide();
            $(".fancytree-container tr th:first-child").hide();

            $('#required-all-check').change(function () {
                $('.required-check').prop('checked', $(this).is(':checked'));
                $.uniform.update();
            });

            $('#hash-all-check').change(function () {
                $('.hash-check').prop('checked', $(this).is(':checked'));
                $.uniform.update();
            });

            $('#filter-all-check').change(function () {
                $('.filter-check').prop('checked', $(this).is(':checked'));
                $.uniform.update();
            });

            $('#hidden-all-check').change(function () {
                $('.hidden-check').prop('checked', $(this).is(':checked'));
                $.uniform.update();
            });
        }
    },

    initTree: function () {
        var self = this;
        this.rowNum = 1;

        $(".tree-table").fancytree({
            extensions: ["table", "dnd"],
            checkbox: true,
            keyboard: false,
            selectMode: 1,
            table: {
                indentation: 20,      // indent 20px per node level
                nodeColumnIdx: 1,     // render the node title into the 2nd column
                checkboxColumnIdx: 0  // render the checkboxes into the 1st column
            },
            source: [],
            lazyLoad: function (event, data) {
                data.result = { url: "ajax-sub2.json" }
            },
            renderColumns: function (event, data) {
                var node = data.node,
                    $tdList = $(node.tr).find(">td");

                $(node.tr).data('folder', node.folder);
                $(node.tr).data('tplid', node.data.Id);

                $tdList.eq(0).data('name', "n");
                $tdList.eq(1).data('name', "template field");
                $tdList.eq(2).data('name', "system field");
                $tdList.eq(3).data('name', "validator");
                $tdList.eq(4).data('name', "comments");
                $tdList.eq(5).data('name', "possible");
                $tdList.eq(6).data('name', "required");
                $tdList.eq(7).data('name', "hash");
                $tdList.eq(8).data('name', "hidden");
                $tdList.eq(9).data('name', "filterable");
                $tdList.eq(10).data('name', "filtersettings");

                /* $tdList.eq(1).data('value', node.parent.title);
                 $tdList.eq(2).data('value', node.data.DatabaseField);
                 $tdList.eq(3).data('value', node.data.Validator);
                 $tdList.eq(5).data('value', node.data.Required);*/

                // (index #0 is rendered by fancytree by adding the checkbox)

                if (!node.folder) {
                    var sel = '';

                    var systemFields = '<select id="system-fields-' + self.rowNum + '" class="select-search form-control" data-row="' + self.rowNum + '">';

                    for (var i = 0; i < self.systemFields.length; i++) {
                        systemFields += '<option value="' + self.systemFields[i].value + '" ' + (self.systemFields[i].value == node.data.DatabaseField ? 'selected' : '') + '>' + self.systemFields[i].name + "</option>";
                    }

                    systemFields += '</select>';

                    var dataTypes = '<select id="data-types-' + self.rowNum + '" class="select-search validator-select form-control" data-row="' + self.rowNum + '">';

                    for (var i = 0; i < self.dataTypes.length; i++) {
                        dataTypes += '<option value="' + self.dataTypes[i].value + '" ' + (self.dataTypes[i].value == node.data.Validator ? 'selected' : '') + '>' + self.dataTypes[i].name + "</option>";
                    }

                    dataTypes += '</select>';

                    //dataTypes += '<div style="display: ' + (node.data.Validator == 1 ? 'block' : 'none') + '"><table><tr><td>Min len.</td><td>Max len.</td></tr><tr><td><input type="text" class="minLength" value="' + node.data.MinLength + '"></td><td><input type="text" class="maxLength" value="' + node.data.MaxLength + '"></td></tr></table></div>';

                    dataTypes += '<div id="data-format-html-' + self.rowNum + '" style="display: block;">' + node.data.DataFormatHtml + '</div>';

                    $tdList.eq(2).addClass('text-center').html(systemFields);
                    $tdList.eq(3).addClass('text-center').html(dataTypes);

                    //Pogran upravlenie

                    // (index #2 is rendered by fancytree)
                    $tdList.eq(4).addClass('text-center').html("<input type='text' class='field-description form-control'  maxlength='300' name='comment' value='" + (node.data.Description != 'null' ? node.data.Description : '') + "'>");
                    $tdList.eq(6).addClass('text-center').html("<input type='checkbox' class='styled required-check' " + (node.data.Required ? "checked='checked'" : "") + " >");
                    $tdList.eq(8).addClass('text-center').html("<input type='checkbox' class='styled hidden-check' " + (node.data.IsHidden ? "checked='checked'" : "") + " style='display: none' >");
                    $tdList.eq(7).addClass('text-center').html("<input type='checkbox' class='styled hash-check' " + (node.data.IsHash ? "checked='checked'" : "") + " >");

                    var blackLists = '<select class="select-search form-control">';

                    for (var i = 0; i < self.blackList.length; i++) {
                        blackLists += '<option value="' + self.blackList[i].value + '">' + self.blackList[i].name + "</option>";
                    }

                    blackLists += '</select>';

                    //$tdList.eq(6).addClass('text-center').html(blackLists);

                    var actions = '<div class="btn-group">' +
                        '<button type="button" class="btn btn-primary btn-icon dropdown-toggle" data-toggle="dropdown">' +
                        '<i class="icon-menu7"></i> &nbsp;<span class="caret"></span>' +
                        '</button>' +

                        '<ul class="dropdown-menu dropdown-menu-right">' +
                        '<li><a href="javascript:void(0)" onclick="addNodeDialog()"><i class="icon-screen-full"></i>Add</a></li>' +
                        '<li><a href="javascript:void(0)" onclick="editRow()"><i class="icon-menu7"></i>Edit</a></li>' +
                        '<li><a href="javascript:void(0)" onclick="deleteRow()"><i class="icon-screen-full"></i>Delete</a></li>' +
                        '</ul>'
                    '</div>';

                    $tdList.eq(9).addClass('text-center').html("<input type='checkbox' class='styled filter-check' " + (node.data.IsFilterable ? "checked='checked'" : "") + " >");
                    //$tdList.eq(10).addClass('text-center').html(actions);
                    $tdList.eq(5).addClass('text-center').html("<input type='text' name='possible' class='form-control' value='" + (node.data.PossibleValue != 'null' ? node.data.PossibleValue : '') + "'>");

                    var filterSettingIndicator = "";

                    try {
                        var filterSettingsObj = $.parseJSON(node.data.FieldFilterSettings);
                        if (filterSettingsObj.filterType == 1) {
                            filterSettingIndicator = "<b>(T)</b>";
                        }
                        else if (filterSettingsObj.filterType == 2) {
                            filterSettingIndicator = "<b>(D)</b>";
                        }
                    }
                    catch(ex) {}

                    $tdList.eq(10).addClass('text-center').html("<button type='button' class='btn btn-default btn-sm' data-toggle='modal' data-target='#filter_settings_modal' onclick='AdrackCampaign.loadFilterSetings(this)' data-settings='" + node.data.FieldFilterSettings + "'>Filter settings " + filterSettingIndicator + "</button>");

                    /*$(node.tr).find(".select-search").select2();
                    $(node.tr).find(".select-search").off("select2:select").on("select2:select", function (e) {
                        if (e.params.data.id == "1")
                            $(e.params.data.element).parent().next().next().show();
                        else
                            $(e.params.data.element).parent().next().next().hide();

                        getValidatorType(e.params.data.id, $(e.params.data.element).parent().next().next(), $(this));
                    });*/

                    $(node.tr).find("#system-fields-" + self.rowNum).change(function (e) {
                        self.getValidatorType($(this).find(":selected").val(), $('#data-format-html-' + $(this).data('row')), $(this));
                    });

                    $(node.tr).find("#data-types-" + self.rowNum).change(function (e) {
                        self.getValidatorType($(this).find(":selected").val(), $('#data-format-html-' + $(this).data('row')), $(this));
                    });

                    self.rowNum++;
                }
                else {
                    $tdList.eq(8).addClass('text-center').html("<input type='checkbox' class='styled hidden-check' " + (node.data.IsHidden ? "checked='checked'" : "") + " >");
                }
                $(node.tr).find(".styled").uniform({ radioClass: 'choice' });
                //$(".select-search").off("select2:select").on("select2:select", function (e) { $(this).parent().data('value', e.params.data.id); console.log(e); });
            },
            activate: function (event, data) {
                selectedNode = data.node;
            },
            select: function (event, data) {
            },
            dnd: {
                preventVoidMoves: true, // Prevent dropping nodes 'before self', etc.
                preventRecursiveMoves: true, // Prevent dropping nodes on own descendants
                autoExpandMS: 400,
                draggable: {
                    //zIndex: 1000,
                    // appendTo: "body",
                    // helper: "clone",
                    scroll: false,
                    revert: "invalid"
                },
                dragStart: function (node, data) {
                    if (data.originalEvent.shiftKey) {
                    }
                    // allow dragging `node`:
                    return slef.canDragDrop;
                },
                dragEnter: function (node, data) {
                    // Prevent dropping a parent below another parent (only sort
                    // nodes under the same parent)
                    /* 					if(node.parent !== data.otherNode.parent){
                                            return false;
                                        }
                                        // Don't allow dropping *over* a node (would create a child)
                                        return ["before", "after"];
                    */
                    return true;
                },
                dragDrop: function (node, data) {
                    if (!data.otherNode) {
                        // It's a non-tree draggable
                        var title = $(data.draggable.element).text() + " (" + (count)++ + ")";
                        node.addNode({ title: title }, data.hitMode);
                        return;
                    }
                    data.otherNode.moveTo(node, data.hitMode);
                    $(node.tr).data('folder', true);
                }
            }
        });
    },

    getNodes: function (tree, parent, parentName, ar) {
        var self = this;

        if (parent.children != null) {
            if (parent.children.length > 0 && parent.title != parentName) {
                $tdList = $(parent.tr).find(">td");

                ar.push([]);

                ar[ar.length - 1].push($(parent.tr).data('tplid'));
                ar[ar.length - 1].push(parent.title);
                ar[ar.length - 1].push("NONE");
                ar[ar.length - 1].push("0");
                ar[ar.length - 1].push("");
                ar[ar.length - 1].push("");
                ar[ar.length - 1].push(false);
                ar[ar.length - 1].push(false);
                if ($tdList.eq(8).data('name') == "hidden") {
                    ar[ar.length - 1].push($tdList.eq(8).find("input").is(':checked'));
                }
                ar[ar.length - 1].push(false);
                ar[ar.length - 1].push("");
                ar[ar.length - 1].push(parentName);
            }

            for (var i = 0; i < parent.children.length; i++) {
                if (parent.children[i].children == null || parent.children[i].children.length == 0) {
                    ar.push([]);

                    ar[ar.length - 1].push($(parent.children[i].tr).data('tplid'));

                    $(parent.children[i].tr).children('td').each(function () {
                        var value = null;

                        if ($(this).data('name') == "template field") {
                            value = $(this).find(".fancytree-title").text();
                        }
                        else
                            if ($(this).data('name') == "system field" || $(this).data('name') == "validator" || $(this).data('name') == "blacklist") {
                                value = $(this).find("select").val();
                                if ($(this).data('name') == "validator") {
                                    $(this).find("select").parent().find("[name^='df_']").each(function () {
                                        value += ';' + $(this).val();
                                    });

                                    if (value == null) {
                                        value = "";
                                    }

                                    //value += ';' + $(this).find("select").next().next().find('.minLength').val();
                                    //value += ';' + $(this).find("select").next().next().find('.maxLength').val();
                                }
                            }
                            else
                                if ($(this).data('name') == "comments") {
                                    value = $(this).find("input").val();
                                }
                                else
                                    if ($(this).data('name') == "required") {
                                        value = $(this).find("input").is(':checked');
                                    }
                                    else
                                        if ($(this).data('name') == "possible") {
                                            value = $(this).find("input").val();
                                        }
                                        else
                                            if ($(this).data('name') == "hash") {
                                                value = $(this).find("input").is(':checked');
                                            }
                                            else
                                                if ($(this).data('name') == "hidden") {
                                                    value = $(this).find("input").is(':checked');
                                                }
                                                else
                                                    if ($(this).data('name') == "filterable") {
                                                        value = $(this).find("input").is(':checked');
                                                    }
                                                    else
                                                        if ($(this).data('name') == "filtersettings") {
                                                            value = $(this).find("button").data('settings');
                                                        }
                        if (value != null)
                            ar[ar.length - 1].push(value);
                    });

                    ar[ar.length - 1].push(parent.title);
                }
                else
                    ar = self.getNodes(tree, parent.children[i], parent.title, ar);
            }
        }

        return ar;
    },
    loadFilterSetings: function (e) {
        this.currentFilterSettingsButton = $(e);
        $('#filter-type').val(1);
        $('#filter-type-value').val('');

        try {
            var obj = $(e).data('settings');

            if (obj !== undefined && obj !== null && obj.constructor != Object) {
                obj = $.parseJSON($(e).data('settings'));
            }

            $('#filter-type').val(obj.filterType);
            $('#filter-type-value').val(obj.filterTypeValue);
        }
        catch (ex) {

        }
    }
}