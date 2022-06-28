
function AdrackFormNextPage()
{

 $($(".next-step")[FbCurrentPage-1]).trigger("click");
}

function AdrackFormPrevPage()
{
 $($(".previous-step")[FbCurrentPage-1]).trigger("click");
}

function fbPageDefaultSelectorIn(item)
{
   $(".topItems-wrapper__item").css("opacity","0.5");
   $(item).css("opacity","1.0");
}

function fbPageDefaultSelectorOut(item)
{
   $(".topItems-wrapper__item").css("opacity","1.0");
}

function FormPage()
{
    this.Name = "Page";
    this.Fields = new Array();

    this.template = `<fieldset class="formFieldset">


                        <div class="col-md-12 col-sm-12 col-xs-12 " id="">

                        <div class="page-title @TitleClass" style="font-size:24px; @TitleStyle;">@TitleText</div>
                        <div class="page-description @DescriptionClass" style="@DescriptionStyle">@DescriptionText</div>

                  <div class="progress">
                        <div class="progress-bar"></div>
                    </div>

                            <div class="col-md-12 tableshadow divbgwhite" id="circlegraf" style="">

			<div class="col-md-12 col-sm-12 col-xs-12 topItems-wrapper" align=center>			
				@topzone
			</div>

                                <div class="col-md-12 col-sm-12 col-xs-12 fieldItems-wrapper">
                                    @Fields

                                </div>

			<div class="col-md-12 col-sm-12 col-xs-12 " align=center>			
				@bottomzone
			</div>

                            </div>

                        </div>

                        <input style="@NextStepDisplay" type="button" name="previous-step"
                               class="previous-step"
                               value="Previous" />

                        <input style="@NextStepDisplay" type="button" name="next-step"
                               class="next-step" value="Next" />
                    </fieldset>`;


    this.isSpecialField = function (fld) {
	if (!fld.Layout.ColumnGrid) return false;
        return fld.Layout.ColumnGrid.toString().toLowerCase() == "bottom" || fld.Layout.ColumnGrid.toString().toLowerCase() == "top";
    };


    this.GetPageXML = function (withIP)
    {        

        var out = "";

	if (withIP)
		out+="<IPADDRESS>"+withIP+"</IPADDRESS>";

        for (var i = 0; i < this.Fields.length; i++)
        {
            var fld = this.Fields[i];
            var tag = fld.GetXMLTag();

            if (tag)
            tag+ "\n";
            out += tag;
        }

        return out;
    };

    this.GenerateSpecialSection = function (gen,section) {
        var fields = new Array();
        for (var i = 0; i < this.Fields.length; i++) {
            var fld = this.Fields[i];
            if (fld.Layout.ColumnGrid && fld.Layout.ColumnGrid.toString().toLowerCase() == section) {
                fields.push(fld);
            }
        }

        var s = "";
        for (var i = 0; i < fields.length; i++) {
            var fld = fields[i];
	    if (fld.Properties.DataFormat==16)
            	s+="<div style='display:table-cell; cursor:pointer;' class='topItems-wrapper__item' onmouseout='fbPageDefaultSelectorOut(this)' onmouseover='fbPageDefaultSelectorIn(this)'>"+fld.Generate(true)+"</div>";
	    else
		s+="<div style='display:table-cell; cursor:pointer;'>"+fld.Generate(true)+"</div>";
        }
        gen = gen.replace("@" + section + "zone", s);
        return gen;
    };

    this.Generate = function (PageId)
    {
        var gen = this.template;

        for (var key in this.Properties) {
            if (key == "Step") continue;
            if (this.Properties[key])
                gen = gen.replace("@" + key, this.Properties[key]);
            else
                gen = gen.replace("@" + key, "");
        }

        gen = gen.replace("@TitleText", "");
        gen = gen.replace("@DescriptionText", "");

        var name = this.Properties.Title;
        
        gen = gen.replace("@Title", name);
        name = this.Properties.Description;
        if (!name)
            name = "";
        gen = gen.replace("@Description", name);

        this.Properties.Title;
        var fields = "";

        var gridSize = 1;
        var gridFields=new Array();

	var rowCalc=0;
        for (var i = 0; i < this.Fields.length; i++) {
            var fld = this.Fields[i];

            fld.IdGen = PageId + "_field_" + i;
            if (this.isSpecialField(fld)) continue;
            gridFields.push(fld);

	    if (fld.Layout.RowGrid!==undefined)
		if (rowCalc<parseInt(fld.Layout.RowGrid))
			rowCalc=parseInt(fld.Layout.RowGrid);

            if (fld.Layout.ColumnGrid > gridSize)
                gridSize = fld.Layout.ColumnGrid;
        }

        var rows = gridFields.length / gridSize;

	if (rowCalc)
		rows=rowCalc;

        var colSize = 12 / gridSize;
        var s = "";
        for (var i = 0; i < rows; i++)
        {
	    s+="<div class='col-md-12'>";
            for (var col = 0; col < gridSize; col++) {
                var code = i.toString() + col.toString();
                s += `<div class="col-md-` + colSize + `">
                @Field`+ code + `
                </div>`;
            }
	    s+="</div>";
        }

        gen = gen.replace("@Fields", s);
        var rowNumber = 0;

       for (var i=0; i<gridFields.length; i++) 
		console.log(gridFields[i].Layout.RowGrid+" "+gridFields[i].Layout.ColumnGrid)

	if (gridFields.length>0)
	{
        var oldNumber = gridFields[0].Layout.ColumnGrid;
        if (!oldNumber)
            oldNumber = 1;

        for (var i = 0; i < gridFields.length; i++) {
            fld = gridFields[i];

            var grid = fld.Layout.ColumnGrid;
	    var rowNum = fld.Layout.RowGrid;

   	    if (!rowNum)
		{	
            if (!grid)
                grid = 1;


		}

            grid--;


            var fldGen = fld.Generate();

	    if (rowNum)
	    {
		rowNumber=parseInt(rowNum)-1;	
	    }
	    else
            if (gen.indexOf("@Field" + code) < 0) {
                rowNumber++;
                i--;

                if (rowNumber > 1000) break;
                continue;
            }
            var code = rowNumber.toString() + grid.toString();
            gen = gen.replace("@Field" + code, fldGen);
        }
	}

        for (var i = 0; i < rows; i++) {
            for (var col = 0; col < gridSize; col++) {
                var code = i.toString() + col.toString();
                gen = gen.replace("@Field" + code, "<div class='col-md-12'>&nbsp;</div>");
            }
        }

        gen = this.GenerateSpecialSection(gen, "top");
        gen = this.GenerateSpecialSection(gen, "bottom");
        return gen;
    };
}

//

