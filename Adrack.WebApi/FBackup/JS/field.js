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
    button: 18
};



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


        var labAlign = `<label class="col-md-@labsize labb" style='color:@TextLabelColor;'>@Label</label>`;
        if (t2)
            labAlign = `@br<label style='color:@TextLabelColor;'>@Label</label>`;

        if (!this.Properties.Label)
            labAlign = "";
        var labSize = 6;
        if (!this.Layout.ColumnLabelAlign)
            this.Layout.ColumnLabelAlign = 0;

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

        gen = gen.replaceAll("@labsize", labSize);


        var spanAlign = '<span class="col-md-6 col-sm-6 col-xs-12" style="padding-left:@LabelAndInputSpacingpx;">';

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
            tag = "select";

        //
        if (this.Properties.DataFormat == EnumDataFormat.image)
            tag = "img";

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
                        <div class="col-md-12 col-sm-12 col-xs-12  padddr " align=center >
                                @labAlignLeft
                                @spanAlign
                                    <@tag src="@ImageURL" value="@DefaultValue" onclick="@ButtonAction" style='background-color: @BackgroundColor; border-color:@BorderColor; border-width:@BorderWidthpx; width:@Widthpx; height:@Heightpx;  font-weight:@Bold; font-style:@Italic; color:@TextInputColor; font-size:@TextSizepx;' type='@DataFormat' required=@Required id="@ID" pattern="@ValidationRule" title="@HelperText" placeholder="@PlaceHolderText" class="@AdditionalClass">
					                    @buttonlabel
                                    </@tag>
                                  </span>
                                    @labAlignRight
                            </div>
                        </div>`;


    this.template2 = `@labAlignLeft
                      @spanAlign
                      <@tag src="@ImageURL" value="@DefaultValue" onclick="@ButtonAction" style='background-color: @BackgroundColor; border-color:@BorderColor; border-width:@BorderWidthpx; width:@Widthpx; height:@Heightpx;  font-weight:@Bold; font-style:@Italic; color:@TextInputColor; font-size:@TextSizepx;' type='@DataFormat' required=@Required id="@ID" pattern="@ValidationRule" title="@HelperText" placeholder="@PlaceHolderText" class="@AdditionalClass">
					    @buttonlabel
                        </@tag>
                      </span>
                    @labAlignRight                        
                        `;
}