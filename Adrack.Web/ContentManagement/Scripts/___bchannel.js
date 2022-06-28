var AdrackBChannel = {
    selectedNode: null,
    editMode: false,
    xmlTpl: '',
    fieldsSelect: [],
    buyerChannelId: 0,
    campaignFields: [],
    allowedAffiliateChannels: [],

    currentMatchingButton: null,

    getBuyerChannelInfoUrl: '',

    init: function (getBuyerChannelInfoUrl) {
        this.getBuyerChannelInfoUrl = getBuyerChannelInfoUrl;
    },

    editZipCode: function (e) {
        var form = $('#zip-form');

        form.find('#RedirectUrl').val($(e).data('url'));
        form.find('#ZipCode').val($(e).data('zip'));
        form.find('#Title').val($(e).data('title'));
        form.find('#Description').val($(e).data('desc'));
        form.find('#Address').val($(e).data('address'));
        form.find('#ZipCodeRedirectId').val($(e).data('id'));
        form.find('.btn-info').removeAttr('disabled');

        $('#add_new_zipcode').trigger('click');
    },

    allowedChanged: function (e) {
        for (var i = 0; i < this.allowedAffiliateChannels.length; i++) {
            if (this.allowedAffiliateChannels[i][0] == $(e).data('id')) {
                this.allowedAffiliateChannels[i][1] = $(e).is(':checked');
                return;
            }
        }

        this.allowedAffiliateChannels.push([$(e).data('id'), $(e).is(':checked')]);
    },

    addFilterRow: function (field, condition, value, value2, operator) {
        var html = '<tr>';
        html += '<td>';
        html += '<select class="fields form-control" style="width: 400px">';
        var selected = '';
        if (fieldsSelect.length == 0) {
            @foreach(var t in Model.CampaignTemplate)
            {
                @Html.Raw("selected = ''; if (field == " + t.Id.ToString() + ") selected = 'selected';");
                @Html.Raw("html += '<option value=\"" + t.Id.ToString() + "\" ' + selected +'>" + t.TemplateField + "</option>';");
            }
        }
        else {
            $.each(fieldsSelect, function (id, option) {
                selected = '';
                if (option.id == field) selected = 'selected';
                html += '<option value="' + option.id + '" ' + selected + '>' + option.name + "</option>";
            });
        }
        html += '</select>';
        html += '</td>';
        html += '<td>';
        html += '<select class="form-control">';
        html += '<option value="1" ' + (condition == '1' ? ' selected' : '') + '>CONTAINS</option>';
        html += '<option value="2" ' + (condition == '2' ? ' selected' : '') + '>DOES NOT CONTAIN</option>';
        html += '<option value="3" ' + (condition == '3' ? ' selected' : '') + '>STARTS WITH</option>';
        html += '<option value="4" ' + (condition == '4' ? ' selected' : '') + '>ENDS WITH</option>';
        html += '<option value="5" ' + (condition == '5' ? ' selected' : '') + '>EQUAL</option>';
        html += '<option value="6" ' + (condition == '6' ? ' selected' : '') + '>NOT EQUAL</option>';
        html += '<option value="7" ' + (condition == '7' ? ' selected' : '') + '>GREATER</option>';
        html += '<option value="8" ' + (condition == '8' ? ' selected' : '') + '>GREATER EQUAL</option>';
        html += '<option value="9" ' + (condition == '9' ? ' selected' : '') + '>LESS</option>';
        html += '<option value="10" ' + (condition == '10' ? ' selected' : '') + '>LESS EQUAL</option>';
        html += '<option value="11" ' + (condition == '11' ? ' selected' : '') + '>RANGE</option>';
        html += '<option value="12" ' + (condition == '12' ? ' selected' : '') + '>NO SAME DIGITS</option>';
        html += '</select>';
        html += '</td>';

        html += '<td>';
        html += '<textarea class="first-value form-control">' + value + '</textarea>';
        html += '</td>';

        html += '<td><div class="filter_remove"><i class="glyphicon glyphicon-remove red"></i></div></td>';
        html += '</tr>';

        $('#conditions tbody').append(html);

        $('.filter_remove').off('click').on('click', function () {
            $(this).parent().parent().remove();
        });
    },

    editRow: function () {
        if (selectedNode == null) {
            alert('Please select the node');
            return;
        }

        editMode = true;

        $('#node_name').val(selectedNode.title);

        $('#modal_add_node').modal('show');
    },

    deleteRow: function () {
        if (selectedNode == null) {
            alert('Please select the node');
            return;
        }

        selectedNode.remove();
    },

    loadTree: function (source) {
        var tree = $('.tree-table').fancytree('getTree');
        tree.reload(source);
    },

    loadMatchingValues: function (e) {
        currentMatchingButton = $(e);

        var m = $(currentMatchingButton).data('matchings');

        $("#matching_values").find('tbody').html('');

        for (var i = 0; i < m.length; i++) {
            $("#matching_values").find('tbody').append($("<tr><td><input type='text' class='form-control' value='" + m[i].input + "' /></td><td><input type='text' class='form-control' value='" + m[i].output + "' /></td><td><button onclick='deleteMathingRow(this)' style='height:36px'>Delete</button></td></tr>"));
        }

        $.ajax({
            cache: false,
            async: false,
            type: "POST",
            url: "",
            data: {},
            success: function (data) {
                //setTimeout(function () { $('#modal_default').hide(); }, 500);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    },

    deleteMathingRow: function (e) {
        $(e).parent().parent().remove();
    },

    initTree: function () {
        $(".tree-table").fancytree({
            extensions: ["table", "dnd"],
            checkbox: true,
            keyboard: false,
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

                $tdList.eq(0).data('name', "n");
                $tdList.eq(1).data('name', "campaign field");
                $tdList.eq(2).data('name', "template field");
                $tdList.eq(3).data('name', "default value");
                $tdList.eq(4).data('name', "matchings");

                if (!node.folder) {
                    var sel = '';

                    var templateField = '<select class="cmp form-control" style="width: 150px">';

                    @foreach(var item in Model.ListCampaignField)
        {
            @Html.Raw("if (node.data.CampaignTemplateId == " + item.Value + ") { sel = 'selected'; } else { sel = ''; }");
            @Html.Raw("templateField += '<option value=\"" + item.Value + "\" ' + sel + '>" + item.Text + "</option>';")
        }

        templateField += '</select>';

        $tdList.eq(2).addClass('text-center').html(templateField);

        var dataList = '<datalist id="' + node.data.TemplateField + '_list">' +
            '<option value="[DATE GUID]">Date GUID</option>' +
            '<option value="[CUR DATE]">Current date</option>' +
            '<option value="[OPTIONAL]">Optional</option>' +
            '<option value="[REQUIRED]">Required</option>' +
            '</datalist>';
        $tdList.eq(3).addClass('text-center').html("<input type='text' class='form-control' name='default_value' value='" + node.data.DefaultValue + "' list='" + node.data.TemplateField + "_list'>" + dataList);

        var btn = $("<button type='button' class='btn btn-default btn-sm' data-toggle='modal' data-target='#matching_values_modal' onclick='loadMatchingValues(this)' data-matchings=''>Matching value</button>")

        $tdList.eq(4).addClass('text-center').append($(btn));

        try {
            $(btn).data('matchings', JSON.parse(node.data.Matchings));
        }
        catch (err) {
        }

        // Style checkboxes
        $(node.tr).find(".styled").uniform({ radioClass: 'choice' });

        FillCampaignDropdown($(node.tr).find(".cmp"), node.title);

        //$(node.tr).find(".select-search").select2();
    }
    //$(".select-search").off("select2:select").on("select2:select", function (e) { $(this).parent().data('value', e.params.data.id); console.log(e); });
},
    activate: function(event, data) {
        selectedNode = data.node;
        },
select: function(event, data) {
    console.log(data);
    // Display list of selected nodes
    //var s = data.tree.getSelectedNodes().join(", ");
    //$("#echoSelection1").text(s);
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
            console.log("dragStart with SHIFT");
        }
        // allow dragging `node`:
        return true;
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
            alert('dd');
            return;
        }
        data.otherNode.moveTo(node, data.hitMode);
        $(node.tr).data('folder', true);
    }
}
      });
    },

getBuyerChannelInfo: function(id) {
    AdrackCommon.ajaxCall(this.getBuyerChannelInfoUrl, { id: id, xml: $('#xml_template').val() }, function (result) {
        self.loadTree(result.items);
    });
},

getCampaignFields: function (campaignid) {
    var self = this;
    AdrackCommon.ajaxCall(this.getCampaignInfoUrl, { campaignid: campaignid }, function (result) {
        self.campaignFields = [];
        $.each(result.fields, function (id, option) {
            self.campaignFields.push({ id: option.id, name: option.name });
        });
    });
},

FillCampaignDropdown: function(e, campaigntemplateid) {
    $(e).html('');
    $(e).append($('<option></option>').val(0).html("NONE").attr('selected', true));
    $.each(campaignFields, function (id, option) {
        if (option.name != campaigntemplateid)
            $(e).append($('<option></option>').val(option.id).html(option.name));
        else {
            $(e).find('option').removeAttr('selected');
            $(e).append($('<option></option>').attr('selected', true).val(option.id).html(option.name));
        }
    });
},

getNodes: function(tree, parent, parentName, ar) {
    if (parent.children != null) {
        if (parent.children.length > 0 && parent.title != parentName) {
            ar.push([]);

            ar[ar.length - 1].push(parent.title);
            ar[ar.length - 1].push("0");
            ar[ar.length - 1].push("");
            ar[ar.length - 1].push('');
            ar[ar.length - 1].push(parentName);
        }

        for (var i = 0; i < parent.children.length; i++) {
            if (parent.children[i].children == null || parent.children[i].children.length == 0) {
                ar.push([]);

                $(parent.children[i].tr).children('td').each(function () {
                    var value = null;

                    if ($(this).data('name') == "campaign field") {
                        value = $(this).find(".fancytree-title").text();
                    }
                    else
                        if ($(this).data('name') == "template field") {
                            value = $(this).find("select").val();
                        }
                        else
                            if ($(this).data('name') == "default value") {
                                value = $(this).find("input").val();
                            }
                            else
                                if ($(this).data('name') == "matchings") {
                                    value = $(this).find("button").data('matchings');
                                }

                    if (value != null)
                        ar[ar.length - 1].push(value);
                });

                ar[ar.length - 1].push(parent.title);
            }
            else
                ar = getNodes(tree, parent.children[i], parent.title, ar);
        }
    }

    return ar;
}
}