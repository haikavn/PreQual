
EnumColumnLabelAlign = {
    Left: 0,
    MiddleTop: 1,
    Right: 2,
    MiddleBottom: 3
};

EnumDataFormat = {
    text: 0,
    time: 1,
    file: 2,
    month: 3,
    week: 4,
    date: 5,
    "datetime-local": 6,
    email: 7,
    url: 8,
    search: 9,
    tel: 10,
    color: 11,
    number: 12,
    password: 13,
    checkbox: 14,
    radio: 15,
    image: 16,
    select: 17,
    button: 18,
    hidden: 19
};


var IdGenValues = new Object();

function FormField() {
    this.Name = "Default";
    this.columnSize = "12";

    this.Layout = {
        "ColumnGrid": null,
        "ColumnGap": null,
        "FieldsGap": null,
        "ColumnLabelAlign": null,
        "LabelAndInputSpacing": null
    };

    this.Properties =
    {
        "DefaultValue": null,
        "DataFormat": 0,
        "Required": true,
        "ValidationRule": "",
        "Label": "BLabel",
        "HelperText": "BHelper",
        "PlaceHolderText": "BPlaceHolder",
        "ImageBase64": null,
        "Width": 100,
        "Height": 50
    };

    this.Style = {
        "Italic": null,
        "Bold": null,
        "TextSize": "11",
        "TextInputColor": "black",
        "TextLabelColor": "black",
        "BorderColor": "black",
        "BackgroundColor": "white",
        "BorderWidth": "1"
    };

    this.GetValue = function () {
        if (this.image) {
            var val = IdGenValues[this.Step][this.IdGen];
            if (!val)
                return null;
            val = IdGenValues[this.IdGen].Properties.DefaultValue;
            return val;
        }
        if ($("#" + this.IdGen).length == 0)
            return null;
        return $("#" + this.IdGen).val();
    };
    this.GetXMLTag = function ()
    {
        var val = this.GetValue();
        if (!val)
            return "";
        var tag = this.Name.toUpperCase();
        return "<" + tag + ">" + val + "</" + tag + ">";
    };


    this.GetFiledTag = function () {
        if (this.Properties.DataType == EnumDataType.Text)
            return "<input type='text'";
        if (this.Properties.DataType == EnumDataType.CountryCode)
            return "<select ";
        if (this.Properties.DataType == EnumDataType.Number)
            return "<input type='number' ";
        if (this.Properties.DataType == EnumDataType.NumberFloat)
            return "<input type='number' ";
        if (this.Properties.DataType == EnumDataType.List)
            return "<select type='number' ";
    };

    this.GetFieldClass = function () {
        return "class='dashboardselect newselect'";
    };
    this.Generate = function (t2) {
        
        var gen = this.template;
        if (t2)
            gen = this.template2;
        var key;
	if (this.Properties.DataFormat == EnumDataFormat.hidden || this.Properties.DataFormat == "hidden") {
	   gen = this.template3;
	}
        var labAlign = `<label class="col-md-@labsize labb" style='text-align:left; color:@TextLabelColor;'>@DefaultValue@Label</label>`;
        if (t2)
            labAlign = `@br<label style='color:@TextLabelColor;'>@DefaultValue@Label</label>`;

        
        if (this.Properties.DataFormat == EnumDataFormat.image)
	{
		        if (!this.Properties.Label && !this.Properties.DefaultValue)
			              labAlign = "";
	}
	else
        if (!this.Properties.Label)
            labAlign = "";
        var labSize = 6;
        if (!this.Layout.ColumnLabelAlign)
            this.Layout.ColumnLabelAlign = 0;

        if (this.Properties.DataFormat == EnumDataFormat.image)
	{
		if (this.Properties.DefaultValue)
		    labAlign=labAlign.replace("@DefaultValue",this.Properties.DefaultValue+"<br>");
		else
		    labAlign=labAlign.replace("@DefaultValue","");
	}
	else
		labAlign=labAlign.replace("@DefaultValue","");

	gen = gen.replace("@NAME", this.Name.toLowerCase());

	gen = gen.replaceAll("@ID", this.IdGen);
	try
	{
	if (this.Properties.Width=="big" || this.Properties.Width.toString()==="1")
	{
		this.Properties.Width="64";
		this.Properties.Height="64";
	}

	if (this.Properties.Width=="small" || this.Properties.Width.toString()==="0") 
	{
		this.Properties.Width="32";
		this.Properties.Height="32";
	}
	}
	catch (ex) {}



        if (this.Layout.ColumnLabelAlign == "Left" || this.Layout.ColumnLabelAlign == EnumColumnLabelAlign.Left) {
            gen = gen.replace("@labAlignRight", "");
            gen = gen.replace("@labAlignLeft", labAlign);
            gen = gen.replace("@br", "");
            labSize = 6;
        }

        if (this.Layout.ColumnLabelAlign == "Right" || this.Layout.ColumnLabelAlign == EnumColumnLabelAlign.Right) {
            gen = gen.replace("@labAlignLeft", "");
            gen = gen.replace("@labAlignRight", labAlign);
            gen = gen.replace("@br", "");
            labSize = 6;
        }

        if (this.Layout.ColumnLabelAlign == "MiddleTop" || this.Layout.ColumnLabelAlign == EnumColumnLabelAlign.MiddleTop) {
            gen = gen.replace("@labAlignLeft", labAlign);
            gen = gen.replace("@labAlignRight", "");
            gen = gen.replace("@br", "");
            labSize = 12;
        }

        if (this.Layout.ColumnLabelAlign == "MiddleBottom" || this.Layout.ColumnLabelAlign == EnumColumnLabelAlign.MiddleBottom) {
            gen = gen.replace("@labAlignLeft", "");
            gen = gen.replace("@labAlignRight", labAlign);
            gen = gen.replace("@br", "<br>");
            labSize = 12;
        }

        if (this.Properties.DataFormat == EnumDataFormat.image || this.Properties.DataFormat == EnumDataFormat.button)
        {
            tag = "img";
            this.image = true;
            gen = gen.replace("@ButtonAction", "IdGenValues[" + this.Step + "]=new Object(); IdGenValues[" + this.Step + "]['" + this.IdGen + "']=true; @ButtonAction");
        }


        if (!this.Style.ButtonAction)
            gen = gen.replace("@ButtonAction", "");

        var spanAlign = '<span class="col-md-@labsize" style="padding-left:@LabelAndInputSpacingpx;">';

        gen = gen.replaceAll("@labsize", labSize);
        spanAlign = spanAlign.replaceAll("@labsize", labSize);

	if (!this.Layout.ColumnGap)
		this.Layout.ColumnGap="30";


        if (!this.Properties.Label)
            spanAlign = '<span class="col-md-12">';

        if (t2)
            spanAlign = '<span style="padding:@ColumnGappx; margin:@FieldsGappx;">';

        gen = gen.replaceAll("@spanAlign", spanAlign);

        var isset = false;
        for (key in EnumDataFormat) {
            if (EnumDataFormat[key] == this.Properties.DataFormat) {
                gen = gen.replace("@DataFormat", key);
                isset = true;
            }
        }
        if (!isset)
            gen = gen.replace("@DataFormat", this.Properties.DataFormat);

        for (key in this.Layout) {
            gen = gen.replace("@" + key, this.Layout[key]);
        }

        for (key in this.Style) {
            if (this.Style[key] !== null && this.Style[key] !== undefined)
                gen = gen.replace("@" + key, this.Style[key]);
            else
                gen = gen.replace("@" + key, "");

        }

        for (key in this.Properties) {
            if (this.Properties[key] !== null && this.Properties[key] !== undefined)
                gen = gen.replace("@" + key, this.Properties[key]);
            else
                gen = gen.replace("@" + key, "");

        }





        var tag = "input";
        if (this.Properties.DataFormat == EnumDataFormat.select || this.Properties.DataFormat == "Select")
	{
            tag = "select";
	    var list=this.Properties.InlineList;
	    if (list) 
		{
			list=list.split(";");
			var opt="";
			for (var im=0; im<list.length; im++)
			{
				if (list[im]==this.Properties.DefaultValue)
					opt+="<option selected>"+list[im]+"</option>";
				else
					opt+="<option value='"+list[im]+"'>"+list[im]+"</option>";
			}
			gen = gen.replace("@buttonlabel", opt);
		}
	}


	if (this.Properties.DataFormat == EnumDataFormat.image)
            tag = "img";

        IdGenValues[this.IdGen] = this;

        //
        //if (this.Properties.DataFormat == EnumDataFormat.image)
          //  tag = "img";

        if (this.Properties.DataFormat == EnumDataFormat.button || this.Properties.DataFormat == "Button") {
            tag = "button";
            gen = gen.replace("@buttonlabel", this.Style.ButtonText);
            gen = gen.replace("@click", this.Style.ButtonAction);
        }
        else
            gen = gen.replace("@buttonlabel", "");



        gen = gen.replace("@tag", tag);
        gen = gen.replace("@tag", tag); //closetag

        gen = gen.replaceAll("@size", this.columnSize);

        return gen;
    };

    var mclass = "dashboardselect newselect";
    this.template = `<div class="col-md-@size col-sm-@size col-xs-@size studylevel padddr" style='padding:@ColumnGappx; margin:@FieldsGappx;'>
                        <div class="col-md-12 col-sm-12 col-xs-12  padddr input-wrapper" align=center >
                                @spanAlign
                                @labAlignLeft
                                    <@tag src="@ImageURL" value="@DefaultValue" onclick="@ButtonAction" style='background-color: @BackgroundColor; border-color:@BorderColor; border-width:@BorderWidthpx; width:@Widthpx; height:@Heightpx;  font-weight:@Bold; font-style:@Italic; color:@TextInputColor; font-size:@TextSizepx;' type='@DataFormat' required=@Required id="@ID" name="@NAME" pattern="@ValidationRule" title="@HelperText" placeholder="@PlaceHolderText" class="@AdditionalClass formInput">
					                    @buttonlabel
                                    </@tag>
                                  </span>
                                    @labAlignRight
                            </div>
                            <script>
                                 $("input[name='routingnumber").keyup(function() {  
                                    var request = $.ajax({
                                        type: 'GET',
                                        data: "rn=" + $(this).val(),
                                        url: "https://www.routingnumbers.info/api/data.json",
                                        success: function(data) {
                                            // console.log(data);
                                            $("[name='bankphone").val(data.telephone);
                                            $("[name='bankname").val(data.customer_name);
                                        },
                                        error: function(jqXHR, textStatus, errorThrown) {
                                            
                                            $("[name='bankphone").val('');
                                            $("[name='bankname").val('');
                                            console.log(jqXHR, textStatus, errorThrown);
                                            request.abort();
                                        }
                                    });
                    
                  
                                    // $.get("demo_test.asp", function(data, status){
                                    //     alert("Data: " + data + "\\nStatus: " + status);
                                    // });
                                });   
                                function is_int(value) {
                                    if ((parseFloat(value) == parseInt(value)) && !isNaN(value)) {
                                        return true;
                                    } else {
                                        return false;
                                    }
                                }
                              $("input[name='zip").keyup(function() {              
                                    var el = $(this);
                                    // Did they type five integers?
                                    if ((el.val().length == 5) && (is_int(el.val()))) {
                            
                                        // Call Ziptastic for information
                                        $.ajax({
                                            url: "https://zip.getziptastic.com/v2/US/" + el.val(),
                                            cache: false,
                                            dataType: "json",
                                            type: "GET",
                                            success: function(result, success) {
                                                $(".zip-error, .instructions").slideUp(200);
                                                
                                                $("[name='city'").val(result.city);   
                                                // $("#Page0_field_4").val(result.city);
                                                $("[name='state").val(result.state);
                                                // $("#Page2_field_1").blur();
                            
                                            },
                                            error: function(result, success) {
                                                // $(".zip-error").slideDown(300);
                                            }
                                        });
                                    } else if (el.val().length < 5) {
                                        $(".zip-error").slideUp(200);
                                                 $("[name='city'").val('');
                                                $("[name='state").val('Alabama');
                                    };
                                });
                            </script>
                      </div>`;

    this.template2 = `@labAlignLeft
                      @spanAlign
                      <@tag src="@ImageURL" value="@DefaultValue" onclick="@ButtonAction" style='background-color: @BackgroundColor; border-color:@BorderColor; border-width:@BorderWidthpx; width:@Widthpx; height:@Heightpx;  font-weight:@Bold; font-style:@Italic; color:@TextInputColor; font-size:@TextSizepx;' type='@DataFormat' required=@Required id="@ID" pattern="@ValidationRule" title="@HelperText" placeholder="@PlaceHolderText" class="@AdditionalClass">
					    @buttonlabel
                        </@tag>
                        
                      </span>
                    @labAlignRight                        
                        `;

    this.template3 = `              <@tag src="@ImageURL" value="@DefaultValue" onclick="@ButtonAction" style='background-color: @BackgroundColor; border-color:@BorderColor; border-width:@BorderWidthpx; width:@Widthpx; height:@Heightpx;  font-weight:@Bold; font-style:@Italic; color:@TextInputColor; font-size:@TextSizepx;' type='@DataFormat' required=@Required id="@ID" name="@ID" pattern="@ValidationRule" title="@HelperText" placeholder="@PlaceHolderText" class="@AdditionalClass formInput">
					                    @buttonlabel
                                    </@tag>
                `;

}

//