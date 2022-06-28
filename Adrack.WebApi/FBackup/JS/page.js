function FormPage()
{
    this.Name = "Page";
    this.Fields = new Array();
	
    this.template = `<fieldset>


                        <div class="col-md-12 col-sm-12 col-xs-12 " id="">


                                <div class="circlegraftopn">
                                    <span>@Page</span>
                                </div>
                            

                            <div class="col-md-12 tableshadow divbgwhite" id="circlegraf" style="">

			<div class="col-md-12 col-sm-12 col-xs-12 " align=center>			
				@topzone
			</div>

                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    @Fields

                                </div>

			<div class="col-md-12 col-sm-12 col-xs-12 " align=center>			
				@bottomzone
			</div>

                            </div>

                        </div>


                        <input type="button" name="next-step"
                               class="next-step" value="Next" />
                        <input type="button" name="previous-step"
                               class="previous-step"
                               value="Previous" />
                    </fieldset>`;


    this.isSpecialField = function (fld) {
        return fld.Layout.ColumnGrid == "bottom" || fld.Layout.ColumnGrid == "top";
    };

    this.GenerateSpecialSection = function (gen,section) {
        var fields = new Array();
        for (var i = 0; i < this.Fields.length; i++) {
            var fld = this.Fields[i];
            if (fld.Layout.ColumnGrid.toString().toLowerCase() == section) {
                fields.push(fld);
            }
        }

        var s = "";
        for (var i = 0; i < fields.length; i++) {
            var fld = fields[i];
            s+=fld.Generate(true);
        }
        gen = gen.replace("@" + section + "zone", s);
        return gen;
    };

    this.Generate = function ()
    {
        var gen = this.template;
        gen = gen.replace("@Page", this.Name);
        var fields = "";

        var gridSize = 1;
        var gridFields=new Array();
        for (var i = 0; i < this.Fields.length; i++) {
            var fld = this.Fields[i];
            if (this.isSpecialField(fld)) continue;
            gridFields.push(fld);
            if (fld.Layout.ColumnGrid > gridSize)
                gridSize = fld.Layout.ColumnGrid;
        }

        var rows = gridFields.length / gridSize;

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
        
        var oldNumber = gridFields[0].Layout.ColumnGrid;
        if (!oldNumber)
            oldNumber = 1;

        for (var i = 0; i < gridFields.length; i++) {
            fld = gridFields[i];            
            
            var grid = fld.Layout.ColumnGrid;
            
            if (!grid)
                grid = 1;
            
            grid--;

            
            var code = rowNumber.toString() + grid.toString();

            var fldGen = fld.Generate();

            if (gen.indexOf("@Field" + code) < 0) {
                rowNumber++;
                i--;

                if (rowNumber > 1000) break;
                continue;
            }

            gen = gen.replace("@Field" + code, fldGen);
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