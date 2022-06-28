var FormBuilderDataViewerInstances = new Object();
var FormBuilderDataViewerInstancesId = 0;

var base64Img;


function ResetDBViewer() {
    for (var key in FormBuilderDataViewerInstances) {
        var instance = FormBuilderDataViewerInstances[key];
        $("#" + instance.headerOfTable).html("");
    }
    FormBuilderDataViewerInstances = new Object();
}


function DefaultRanges(minDate, maxDate) {


    var obj = {
        'Today': [moment().toDate().setHours(0, 0, 0, 0), moment().toDate().setHours(23, 59, 59, 999)],
        'Yesterday': [moment().subtract(1, 'days').toDate().setHours(0, 0, 0, 0), moment().subtract(1, 'days').toDate().setHours(23, 59, 59, 999)],
        'Last 7 Days': [moment().subtract(6, 'days').toDate().setHours(0, 0, 0, 0), moment().toDate().setHours(23, 59, 59, 999)],
        'Last 30 Days': [moment().subtract(29, 'days').toDate().setHours(0, 0, 0, 0), moment().toDate().setHours(23, 59, 59, 999)],
        'This Month': [moment().startOf('month').toDate().setHours(0, 0, 0, 0), moment().endOf('month').toDate().setHours(23, 59, 59, 999)],
        'Last Month': [moment().subtract(1, 'month').startOf('month').toDate().setHours(0, 0, 0, 0), moment().subtract(1, 'month').endOf('month').toDate().setHours(23, 59, 59, 999)],
        'Last 6 Month': [moment().subtract(6, 'month').startOf('month').toDate().setHours(0, 0, 0, 0), moment().subtract(1, 'month').endOf('month').toDate().setHours(23, 59, 59, 999)],
        'Last 12 Month': [moment().subtract(12, 'month').startOf('month').toDate().setHours(0, 0, 0, 0), moment().subtract(1, 'month').endOf('month').toDate().setHours(23, 59, 59, 999)]
    };

    if (minDate !== undefined) {
        obj['Whole Range'] = [minDate.setHours(0, 0, 0, 0), maxDate.setHours(23, 59, 59, 999)];
    }
    return obj;
}


function InitCalendarById(dateKey,dataSource,id,keyup) {

    var calendarHTML = `<span class="calenda">
                        <table border=0 colspan=0 cellspacing=0><tr><td><input onkeyup="<KEYUP>" id=<ID> type="text" style="width:100%; min-width:80px;" value="" /></td><td>
                        <i  style="float:right;" id=<IDCAL> class="fa fa-calendar pointercursor"><img src="img/calendar005.svg" width=17></i></td></tr></table>          
                    </span>`;

    var maxDate = 0;
    var minDate = new Date(2050, 1, 1).getTime();

    for (var i = 0; i < dataSource.length; i++) {
        var row = dataSource[i];
        var str = DateStringOnly(row[dateKey]);
        if (str == "Invalid date")
            continue;
        if (row[dateKey].getTime() > maxDate)
            maxDate = row[dateKey].getTime();
        if (row[dateKey].getTime() < minDate)
            minDate = row[dateKey].getTime();
    }

    calendarHTML = calendarHTML.replace("<KEYUP>", keyup);
    calendarHTML = calendarHTML.replace("<ID>", id);
    calendarHTML = calendarHTML.replace("<IDCAL>", id + "_calicon");
    $('#' + id + "_caldiv").html(calendarHTML);

    minDate = new Date(minDate);
    maxDate = new Date(maxDate);
    //+"_calicon"
    $("#" + id + "_calicon").daterangepicker({
        startDate: minDate,
        endDate: maxDate,
        minDate: minDate,
        maxDate: maxDate,
        ranges: DefaultRanges(minDate, maxDate),
        opens: 'left'
    }, function (start, end, label) {
            if (start == end)
                $('#' + id).val(DateStringOnly(start));
            else
                $('#' + id).val(DateStringOnly(start) + "-" + DateStringOnly(end));
            eval(keyup);
        console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));        
    });
}


function imgToBase64(src, callback) {
    var outputFormat = src.substr(-3) === 'png' ? 'image/png' : 'image/jpeg';
    var img = new Image();
    img.crossOrigin = 'Anonymous';
    img.onload = function () {
        var canvas = document.createElement('CANVAS');
        var ctx = canvas.getContext('2d');
        var dataURL;
        canvas.height = this.naturalHeight;
        canvas.width = this.naturalWidth;
        ctx.drawImage(this, 0, 0);
        dataURL = canvas.toDataURL(outputFormat);
        callback(dataURL);
    };
    img.src = src;
    if (img.complete || img.complete === undefined) {
        img.src = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==";
        img.src = src;
    }
}

const PRINT_DIALOG = `<div id="PrintViewDialog" class="modal fade" role="dialog" >
    <div class="modal-dialog" style="width:90% !important;">
        <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close closebutton" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title"><strong> PDF Output</strong></h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group col-md-12 col-sm-12 col-xs-12">
                            <iframe id="PrintContent" width="100%" height=500px></iframe>                        
                        </div>
                    </div>
                    <div class="modal-footer">

                    </div>
                </div>
    
    </div>
</div>`;

function InitPrintDialog() {
    var element = document.getElementById("PrintViewDialog1");
    if (!element) {
        //var issueElement = $(issueButton);
        element = document.createElement("div");
        element.id = "PrintViewDialog1";
        document.body.appendChild(element);
        //document.body.appendChild(issueElement[0]);
    }
    element.innerHTML = PRINT_DIALOG;
}

function PrintDefault() {
    for (var key in FormBuilderDataViewerInstances) {
        FormBuilderDataViewerInstances[key].print("PDF Output");
        return;
    }
}

function FormBuilderRedrawDataViewers(clearFilters) {
    for (var key in FormBuilderDataViewerInstances) {
        if (clearFilters)
            FormBuilderDataViewerInstances[key].ref = new Object();
        FormBuilderDataViewerInstances[key].Draw();

    }
}

function download(data, strFileName, strMimeType) {

    var self = window, // this script is only for browsers anyway...
        u = "application/octet-stream", // this default mime also triggers iframe downloads
        m = strMimeType || u,
        x = data,
        D = document,
        a = D.createElement("a"),
        z = function (a) { return String(a); },


        B = self.Blob || self.MozBlob || self.WebKitBlob || z,
        BB = self.MSBlobBuilder || self.WebKitBlobBuilder || self.BlobBuilder,
        fn = strFileName || "download",
        blob,
        b,
        ua,
        fr;

    //if(typeof B.bind === 'function' ){ B=B.bind(self); }

    if (String(this) === "true") { //reverse arguments, allowing download.bind(true, "text/xml", "export.xml") to act as a callback
        x = [x, m];
        m = x[0];
        x = x[1];
    }



    //go ahead and download dataURLs right away
    if (String(x).match(/^data\:[\w+\-]+\/[\w+\-]+[,;]/)) {
        return navigator.msSaveBlob ?  // IE10 can't do a[download], only Blobs:
            navigator.msSaveBlob(d2b(x), fn) :
            saver(x); // everyone else can save dataURLs un-processed
    }//end if dataURL passed?

    try {

        blob = x instanceof B ?
            x :
            new B([x], { type: m });
    } catch (y) {
        if (BB) {
            b = new BB();
            b.append([x]);
            blob = b.getBlob(m); // the blob
        }

    }



    function d2b(u) {
        var p = u.split(/[:;,]/),
            t = p[1],
            dec = p[2] == "base64" ? atob : decodeURIComponent,
            bin = dec(p.pop()),
            mx = bin.length,
            i = 0,
            uia = new Uint8Array(mx);

        for (i; i < mx; ++i) uia[i] = bin.charCodeAt(i);

        return new B([uia], { type: t });
    }

    function saver(url, winMode) {


        if ('download' in a) { //html5 A[download] 			
            a.href = url;
            a.setAttribute("download", fn);
            a.innerHTML = "downloading...";
            D.body.appendChild(a);
            setTimeout(function () {
                a.click();
                D.body.removeChild(a);
                if (winMode === true) { setTimeout(function () { self.URL.revokeObjectURL(a.href); }, 250); }
            }, 66);
            return true;
        }

        //do iframe dataURL download (old ch+FF):
        var f = D.createElement("iframe");
        D.body.appendChild(f);
        if (!winMode) { // force a mime that will download:
            url = "data:" + url.replace(/^data:([\w\/\-\+]+)/, u);
        }


        f.src = url;
        setTimeout(function () { D.body.removeChild(f); }, 333);

    }//end saver 


    if (navigator.msSaveBlob) { // IE10+ : (has Blob, but not a[download] or URL)
        return navigator.msSaveBlob(blob, fn);
    }

    if (self.URL) { // simple fast and modern way using Blob and URL:
        saver(self.URL.createObjectURL(blob), true);
    } else {
        // handle non-Blob()+non-URL browsers:
        if (typeof blob === "string" || blob.constructor === z) {
            try {
                return saver("data:" + m + ";base64," + self.btoa(blob));
            } catch (y) {
                return saver("data:" + m + "," + encodeURIComponent(blob));
            }
        }

        // Blob but not URL:
        fr = new FileReader();
        fr.onload = function (e) {
            saver(this.result);
        };
        fr.readAsDataURL(blob);
    }
    return true;
}

function ExportExcel() {
    for (var key in FormBuilderDataViewerInstances) {
        FormBuilderDataViewerInstances[key].exportExcel();
        return;
    }
}

function FormBuilderDataViewer() {
    FormBuilderDataViewerInstances[FormBuilderDataViewerInstancesId] = this;

    this.id = FormBuilderDataViewerInstancesId;
    FormBuilderDataViewerInstancesId++;
    this.bodyOfTable = null;
    this.headerWidthInfo = new Object();
    this.headerOfTable = null;
    this.summaryOfTable = null;
    this.dataSource = new Array();
    this.displayFields = new Object();
    this.editFields = new Object();
    this.tableName = "";
    this.showTotal = true;
    this.showDisplayed = true;

    this.editedData = new Object();
    this.includePeriod = true;
    this.includeSorting = true;
    this.includePaging = true;
    this.currentPage = 0;
    this.pageSize = 100;

    this.callbackIsCellClickable = function (item) {
        return false;
    };


    this.callackValidateAllFields = function (item) {
      
    };

    this.callbackIsObjectClickable = function (item) {
        return false;
    };

    this.callbackIsRowVisible = function () {
        return true;
    };

    this.callbackGetCellStyle = function () {
        return "";
    };

    this.callbackGetCellColor = function () {
        return null;
    };

    this.callbackGetCellTextColor = function () {
        return null;
    };

    this.callbackMapValueHtmlEncode = function () {
        return false;
    };

    this.callbackMapValue = function (key, val) {
        return val;
    };


    this.print = function (title) {

        if (!title)
            title = "Print Table";

        InitPrintDialog();

        /*var div = document.createElement("div");

        div.innerHTML = `<div style='font-weight:normal; font-size:10px;' id="summaryOfPrint"></div>
                <table id="printTable" class="table table-bordered table-hover table-responsive" style="max-width:841px !important;";>
                            <thead>
                                <tr>
                                    <td id="summaryOfTablePrint" colspan="100"></td>
                                </tr>
                                <tr id="headOfTablePrint"></tr>
                            </thead>
                            <tbody id="bodyOfIssuesPrint"></tbody>
                        </table>`;

        document.body.appendChild(div);
        
        var old1 = this.bodyOfTable;
        var old2 = this.headerOfTable;
        var old3 = this.summaryOfTable;

        this.bodyOfTable = "bodyOfIssuesPrint";
        this.headerOfTable = "headOfTablePrint";
        this.summaryOfTable = "summaryOfTablePrint";
        

        div.style.position = "absolute";
        
        div.style.left = "0px";
        div.style.width = "841px";
        div.style.minWidth = "841px";
        div.style.maxWidth = "841px";
        div.style.paddingLeft = "30px";
        div.style.paddingRight = "30px";
        div.style.zIndex = 0;
        //this.callbackGetCellStyle = "";
        var oldLen = this.maxLength;
        var oldPageSize = this.pageSize;
        this.pageSize = 10;

        this.maxLength = 30;
        var oldP = this.includePrintButton;
        this.includePrintButton = false;

        var oldE = this.includeExcelButton;
        this.includeExcelButton = false;
        this.printMode = true;
        
        var s1 = this.displayFields["signature"];
        var s2 = this.displayFields["session_id"];

        this.displayFields["signature"] = null;
        this.displayFields["session_id"] = null;

        this.Draw();
        this.maxLength = oldLen;
        this.pageSize = oldPageSize;

        this.includePrintButton = oldP;
        this.includeExcelButton = oldE;

        this.displayFields["signature"] = s1;
        this.displayFields["session_id"] = s2;

        
        $(div).find("td").css("font-family", "Segoe UI, Tahoma, Geneva, Verdana, sans-serif");
        $(div).find("th").css("font-family", "Segoe UI, Tahoma, Geneva, Verdana, sans-serif");
        $(div).find("td").css("font-size", "8px");

        
        $(div).find("td").css("max-width", "100px;");
        $(div).find("th").css("width", "100px;");
        $(div).find("td").css("word-wrap", "break-all");        
        $(div).find("th").css("word-wrap", "break-all");

        $(div).find("th").css("font-size", "8px");

        $(div).find("td").css("font-weight", "normal");
        $(div).find("th").css("font-weight", "bold");


        $(div).find("input").remove();
        $(div).find("img").remove();


        var scope = this;

        this.printMode = false;

        var divSum = document.getElementById("summaryOfPrint");

        if (this.includePeriod && this.displayParams.startDate) {
            divSum.innerHTML = "<br><br>" + title + "<br>Period:"
                + DateStringOnly(this.displayParams.startDate, this.dateFormat) + " - "
                + DateStringOnly(this.displayParams.endDate, this.dateFormat) + "<br><br>";
        }
        else
            divSum.innerHTML = "<br><br>" + title+"<br>";

        var newstr = div.innerHTML;
        //document.body.removeChild(div);

        //document.body.innerHTML = newstr;

       
        */

        var iframe = document.getElementById("PrintContent");
        iframe.src = "";

        var pdf = new jsPDF('landscape', 'pt', 'letter');

        var originalData = new Array();

        var header = new Array();


        var headArray = this.exportFieldsArray;
        if (!headArray)
            headArray = this.displayFieldsArray;


        for (var displayOrderKey = 0; displayOrderKey < headArray.length; displayOrderKey++) {
            let key = headArray[displayOrderKey];
            if (key !== "signature" && key !== "session_id") {
                header.push(this.displayFields[key].replaceAll("<br>", "\n").replaceAll("&nbsp;", " ").replaceAll("<sub>", "").replaceAll("</sub>", ""));
                
            }
        }


        for (var k = 0; k < this.displayParams.visArray.length; k++) {
            var info = this.displayParams.visArray[k];
            var arr = new Array();
            for (let displayOrderKey = 0; displayOrderKey < headArray.length; displayOrderKey++) {
                let key = headArray[displayOrderKey];
                if (key !== "signature" && key !== "session_id") {
                    var dataString = this.callbackMapValue(key, info[key], info, k);
                    if (!this.callbackMapValueHtmlEncode(key))
                        dataString = extractContent(dataString);

                    arr.push(dataString);
                }
            }
            originalData.push(arr);
        }

        /*imgToBase64('examples/document.jpg', function (base64) {
            base64Img = base64;

        });*/

        var scope = this;
        var totalPagesExp = "{total_pages_count_string}";
        pdf.autoTable({
            head: [header],
            body: originalData,
            //tableWidth: 'wrap',            
            //styles: { cellWidth: 'wrap' },
            styles: { font: 'helvetica', fontSize: 9, overflow: 'linebreak', cellWidth: 'wrap', minCellWidth: 50 },
            //columnStyles: { text: { cellWidth: 'auto' } },
            didParseCell: function (data) {
                var color = scope.callbackGetCellColor(data.column.dataKey, data.row.raw[data.column.dataKey]);
                if (color)
                    data.cell.styles.fillColor = color;
                color = scope.callbackGetCellTextColor(data.column.dataKey, data.row.raw[data.column.dataKey]);
                if (color)
                    data.cell.styles.textColor = color;
            },
            didDrawPage: function (data) {
                var doc = pdf;
                // Header
                doc.setFontSize(10);
                doc.setTextColor(40);
                doc.setFontStyle('normal');
                if (base64Img) {
                    doc.addImage(base64Img, 'JPEG', data.settings.margin.left, 15, 10, 10);
                }

                if (scope.includePeriod && scope.displayParams.startDate) {

                    var now = new Date();
                    var utc_timestamp = new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(),
                        now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds(), now.getUTCMilliseconds());

                    var text = title + " | Period:"
                        + DateStringOnly(scope.displayParams.startDate, scope.dateFormat) + " - "
                        + DateStringOnly(scope.displayParams.endDate, scope.dateFormat) + " | Date: " + DateStringOnly(utc_timestamp, scope.dateFormat);

                    doc.text(text, data.settings.margin.left + 15, 22);
                }

                // Footer
                var str = "Page " + doc.internal.getNumberOfPages()
                // Total page number plugin only available in jspdf v1.0+
                if (typeof doc.putTotalPages === 'function') {
                    str = str + " of " + totalPagesExp;
                }
                doc.setFontSize(10);

                // jsPDF 1.4+ uses getWidth, <1.4 uses .width
                var pageSize = doc.internal.pageSize;
                var pageHeight = pageSize.height ? pageSize.height : pageSize.getHeight();
                doc.text(str, data.settings.margin.left, pageHeight - 25);
            },
            margin: { top: 30 }
        });

        // Total page number plugin only available in jspdf v1.0+
        if (typeof pdf.putTotalPages === 'function') {
            pdf.putTotalPages(totalPagesExp);
        }

        $("#PrintViewDialog").modal('show');
        iframe = document.getElementById("PrintContent");
        //iframe.setAttribute('style', 'position:fixed; top:0; bottom:0; height:100%; width:100%');                
        iframe.src = pdf.output('datauristring');
        iframe.height = (window.innerHeight - 200) + "px";

        //pdf.save('table.pdf');

        /*pdf.html(div, {
            callback: function (pdf)
            {
                document.body.removeChild(div);
                scope.bodyOfTable=old1;
                scope.headerOfTable=old2;
                scope.summaryOfTable=old3;
                //pdf.autoPrint();
                $("#PrintViewDialog").modal('show');
                var iframe = document.getElementById("PrintContent");
                //iframe.setAttribute('style', 'position:fixed; top:0; bottom:0; height:100%; width:100%');                
                iframe.src = pdf.output('datauristring');
                iframe.height = (window.innerHeight-200)+"px";
                //pdf.output('dataurl');
                //pdf.save("Report.pdf");
            }
        });*/

    };


    this.nextPage = function () {



        this.currentPage++;
        if (this.currentPage >= this.totalPages)
            this.currentPage = this.totalPages - 1;
        this.Draw();

        if (this.linkedTable) {
            var h = $("#" + this.headerOfTable).outerHeight();
            $("#" + this.linkedTable.headerOfTable).css("height", h);

            this.linkedTable.nextPage();
        }
        this.Draw(); //double draw for checkboxes
    };

    this.prevPage = function () {

        this.currentPage--;
        if (this.currentPage < 0)
            this.currentPage = 0;
        this.Draw();

        if (this.linkedTable) {
            var h = $("#" + this.headerOfTable).outerHeight();
            $("#" + this.linkedTable.headerOfTable).css("height", h);

            this.linkedTable.prevPage();
        }

        this.Draw(); //double draw for checkboxes

    }

    function extractContent(s) {
        var span = document.createElement('span');
        span.innerHTML = s;
        return span.textContent || span.innerText;
    };

    this.exportExcel = function (title) {


        if (!title)
            title = this.tableName;
        if (this.exportTitle)
            title = this.exportTitle;
        else
            if (title === undefined || title === "")
                title = "ExportExcel";

        var widthObject = new Object();
        var widthArray = new Array();
        function calcWidth(key, info) {
            if (info === undefined || info === null)
                info = "0";

            if (widthObject[key] === undefined)
                widthObject[key] = info.length;
            if (widthObject[key] < info.length)
                widthObject[key] = info.length;
        }

        var workbook = ExcelBuilder.Builder.createWorkbook();
        var worksheet = workbook.createWorksheet({ name: title });
        var stylesheet = workbook.getStyleSheet();

        var originalData = new Array();

        var header = new Array();

        var headArray = this.exportFieldsArray;
        if (!headArray)
            headArray = this.displayFieldsArray;

        for (let displayOrderKey = 0; displayOrderKey < headArray.length; displayOrderKey++) {
            let key = headArray[displayOrderKey];
            if (!this.exportFieldsArray)
                if (this.displayFields[key] == null) continue;
            calcWidth(key, this.displayFields[key]);
            header.push(extractContent(this.displayFields[key]));
        }

        originalData.push(header);
        for (var k = 0; k < this.dataSource.length; k++) {
            var info = this.dataSource[k];
            var arr = new Array();
            for (let displayOrderKey = 0; displayOrderKey < headArray.length; displayOrderKey++) {
                let key = headArray[displayOrderKey];
                if (!this.exportFieldsArray)
                    if (this.displayFields[key] == null) continue;
                var dataString = this.callbackMapValue(key, info[key], info, k);
                if (!this.callbackMapValueHtmlEncode(key))
                    dataString = extractContent(dataString);

                calcWidth(key, dataString);
                arr.push(dataString);
            }
            originalData.push(arr);
        }

        /*var originalData = [
            ['Artist', 'Album', 'Price'],
            ['Buckethead', 'Albino Slug', 8.99],
            ['Buckethead', 'Electric Tears', 13.99],
            ['Buckethead', 'Colma', 11.34],
            ['Crystal Method', 'Vegas', 10.54],
            ['Crystal Method', 'Tweekend', 10.64],
            ['Crystal Method', 'Divided By Night', 8.99]
        ];*/



        var albumTable = new ExcelBuilder.Table();
        albumTable.styleInfo.themeStyle = "TableStyleLight1";
        albumTable.setReferenceRange([1, 1], [header.length, originalData.length]);

        albumTable.setTableColumns(header);

        /*albumTable.setTableColumns([
            'Artist',
            'Album',
            'Price'
        ]);*/

        var props = new Object();
        props.column_widths_auto = new Array();
        for (var i = 0; i < header.length; i++)
            props.column_widths_auto.push({ bestFit: true });


        //worksheet.setColumnFormats(props.column_widths_auto);

        //worksheet.sheetView.showGridLines = false;

        for (key in widthObject) {
            widthArray.push({ width: widthObject[key] + 3 });
        }
        worksheet.setColumns(widthArray);

        worksheet.setData(originalData);
        workbook.addWorksheet(worksheet);

        worksheet.addTable(albumTable);
        workbook.addTable(albumTable);



        base64toBlob = function (base64Data, contentType) {
            contentType = contentType || '';
            var sliceSize = 1024;
            var byteCharacters = atob(base64Data);
            //var byteCharacters = decodeURIComponent(escape(window.atob(base64Data)))
            var bytesLength = byteCharacters.length;
            var slicesCount = Math.ceil(bytesLength / sliceSize);
            var byteArrays = new Array(slicesCount);

            for (var sliceIndex = 0; sliceIndex < slicesCount; ++sliceIndex) {
                var begin = sliceIndex * sliceSize;
                var end = Math.min(begin + sliceSize, bytesLength);

                var bytes = new Array(end - begin);
                for (var offset = begin, i = 0; offset < end; ++i, ++offset) {
                    bytes[i] = byteCharacters[offset].charCodeAt(0);
                }
                byteArrays[sliceIndex] = new Uint8Array(bytes);
            }
            return new Blob(byteArrays, { type: contentType });
        }

        ExcelBuilder.Builder.createFile(workbook).then(function (data) {
            //var data = new Blob([blob], { type: 'base64' });
            //download(data, title + ".xlsx");

            /*var myA = document.createElement('a');
            myA.setAttribute('href', "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + data);
                myA.setAttribute('download', title+".xlsx");
            document.body.appendChild(myA);
            myA.click();
            document.body.removeChild(myA);*/

            var blob = base64toBlob(data);
            if (window.navigator.msSaveBlob) { // // IE hack; see http://msdn.microsoft.com/en-us/library/ie/hh779016.aspx
                window.navigator.msSaveOrOpenBlob(blob, title + ".xlsx");
            }
            else {
                var a = window.document.createElement("a");
                a.href = window.URL.createObjectURL(blob, { type: "text/plain" });
                a.download = title + ".xlsx";
                document.body.appendChild(a);
                a.click();  // IE: "Access is denied"; see: https://connect.microsoft.com/IE/feedback/details/797361/ie-10-treats-blob-url-as-cross-origin-and-denies-access
                document.body.removeChild(a);
            }

        });
    };

    this.callackGetSearchByInsidePattern = function () {
        return "";
    };

    this.callackGetSearchByDescriptionPattern = function () {
        return ""
    };





    this.callbackOpenItem = null;
    this.defaultSortingField = null;
    this.defaultSortingDirection = -1;
    this.defaultDateField = null;
    this.includeAutoFilters = false;
    this.calendarMap = new Object();
    //this.dateFormat = "YYYY-MM-DD HH:mm:ss";
    this.dateFormat = "MM/DD/YYYY (UTC)";

    this.IsValid = function () {
        return this.callackGetSearchByDescriptionPattern &&
            this.callackGetSearchByInsidePattern &&
            this.callackValidateAllFields &&
            this.callbackIsCellClickable &&
            this.callbackIsObjectClickable &&
            this.callbackMapValue &&
            this.bodyOfTable &&
            defaultSortingField &&
            this.defaultDateField;
    };

    this.imgArrowDown = "<img class='uparrow' style='position:absolute; right:5px; top:5px; float:right; display:none;' src='img/sort_topicon.svg'>";
    this.imgArrowUp = "<img class='downarrow' style='position:absolute; right:5px; top:5px; float:right; display:none;' src = 'img/sort_bottomicon.svg' >";
    this.imgRowHasSubItemsImage = "<img style = 'float:right;' height = 20 src = 'img/folderclosed2.png'>";

    var scope = this;

    var sortingTable = new Object();
    sortingTable[this.defaultSortingField] = this.defaultSortingDirection;


    var lastArrowSource = null;

    this.displayParams = new Object();


    this.maxLength = 550;

    this.Draw = function () {
        this.DisplayTable(
            this.displayParams.sortField,
            this.displayParams.reverseSorting,
            this.displayParams.filterDescription,
            this.displayParams.filterInside,
            this.displayParams.startDate,
            this.displayParams.endDate);
    };

    this.OpenItem = function (source, fieldId) {
        if (this.callbackOpenItem) {
            this.callbackOpenItem(source, this.displayParams.ref[fieldId]);
        }
    };

    this.FilterByField = function (source, field) {
        var scope = this;
        if (scope.locked) return;
        scope.locked = true;

        var len = scope.dataSource.length;
        var time = 1000;
        if (len < 5000)
            time = 10;
        window.setTimeout(function () {
            scope.Draw();

            scope.locked = false;
            if (scope.CheckBoxSelected)
                scope.CheckBoxSelected();
        }, time);
    };

    this.EditField = function (source, field, id) {
        if (!this.editedData[id])
            this.editedData[id] = new Object();
        if (this.displayParams.ref[id][field] !== source.value) {
            source.style.color = "#FF0000";
            this.editedData[id][field] = source.value;
        }
        else {
            source.style.color = "#000000";
            this.editedData[id][field] = null;
        }
    };

    this.ShowPattern = function (source, key, pattern) {
        pattern = pattern.replaceAll("\^", "");
        pattern = pattern.replaceAll("$", "");
        pattern = pattern.replaceAll(")", "");
        pattern = pattern.replaceAll("(", "");
        pattern = pattern.replaceAll("|", ", ");
        this.FillSummary(-1, key, pattern);
    };

    this.SortByField = function (source, field) {


        if (sortingTable[field] === undefined)
            sortingTable[field] = -1;

        var lastArrowSourceItem = null;
        if (lastArrowSource) {
            lastArrowSourceItem = $("#" + lastArrowSource);
            lastArrowSourceItem.find(".uparrow").hide();
            lastArrowSourceItem.find(".downarrow").hide();
        }

        lastArrowSource = source.id;

        sortingTable[field] = sortingTable[field] * -1;

        if (this.sortCallBack) {
            this.sortCallBack(field, sortingTable[field]);
        }


        if (sortingTable[field] === -1)
            $(source).find(".downarrow").show();
        else
            $(source).find(".uparrow").show();

        this.displayParams.sortField = field;
        this.displayParams.reverseSorting = sortingTable[field];
        this.Draw();
    };



    this.secondSortFieldPriority = null;

    this.DisplayTable = function (sortField, reverseSorting, filterDescription, filterInside, fromDate, toDate) {
        if (!this.displayFieldsArray) {
            this.displayFieldsArray = new Array();
            for (var key in this.displayFields) {
                this.displayFieldsArray.push(key);
            }
        }
        var scope = this;
        var dataSource = this.dataSource;
        if (filterDescription)
            filterDescription = filterDescription.toLowerCase();

        if (filterInside)
            filterInside = filterInside.toLowerCase();

        //////////////////// CHECK FILTERS FOR SINGLE ITEM
        function compareFunction(dataSourceItem_a, dataSourceItem_b, sort_field_ref) {

            if (!sort_field_ref) sort_field_ref = sortField;

            if (dataSourceItem_a[sort_field_ref] == "N/A" && dataSourceItem_b[sort_field_ref]!="N/A")
                return -1 * reverseSorting;
            if (dataSourceItem_b[sort_field_ref] == "N/A" && dataSourceItem_a[sort_field_ref] != "N/A")
                return 1 * reverseSorting;
            if (dataSourceItem_b[sort_field_ref] == "N/A" && dataSourceItem_a[sort_field_ref] == "N/A")
                return 0;

            if (dataSourceItem_a[sort_field_ref] == dataSourceItem_b[sort_field_ref]) {
                if (sort_field_ref == scope.secondSortFieldPriority)
                    return 0;
                if (sort_field_ref == sortField && scope.secondSortFieldPriority)
                    return compareFunction(dataSourceItem_a, dataSourceItem_b, scope.secondSortFieldPriority);
            }
            if (dataSourceItem_a[sort_field_ref] < dataSourceItem_b[sort_field_ref])
                return -1 * reverseSorting;
            else
                if (dataSourceItem_a[sort_field_ref] > dataSourceItem_b[sort_field_ref])
                    return 1 * reverseSorting;

            return 0;
        }

        var predefinedFilters = new Object();

        function predefindDataSourceItemInFilters(dataSourceItem) {
            predefinedFilters = new Object();
            if (scope.includeAutoFilters) {
                for (var key in dataSourceItem) {
                    if (key === "filterDescriptionLowerCase" || key === "filterInsideLowerCase")
                        continue;
                    var fieldFilter = "fieldFilter" + scope.id.toString() + key;

                    fieldFilter = fieldFilter.replaceAll(".", "_");

                    var value = $("#" + fieldFilter).val();
                    if (value != "" && value != null && value.trim() != "")
                        predefinedFilters[key] = value;
                }
            }
        }

        function dataSourceItemInFilters(dataSourceItem) {
            if (Object.keys(predefinedFilters).length == 0) return true;
            if (scope.includeAutoFilters) {
                for (var key in dataSourceItem) {
                    if (key === "filterDescriptionLowerCase" || key === "filterInsideLowerCase")
                        continue;
                    /*var fieldFilter = "fieldFilter" + scope.id.toString() + key;
 
                    fieldFilter = fieldFilter.replaceAll(".", "_");
 
                    var value = $("#" + fieldFilter).val();*/

                    var value = predefinedFilters[key];
                    if (value !== undefined && value !== null && value !== "") {

                        let exactMatch = false;
                        //exactMatch = true;

                        if (key == "gender")
                            exactMatch = true;

                        if (value.startsWith('~')) {
                            value = value.substring(1);
                            exactMatch = true;
                        }
                        let excludeMatch = false;
                        if (value.startsWith('!')) {
                            value = value.substring(1);
                            excludeMatch = true;
                        }


                        if (dataSourceItem[key] !== null && dataSourceItem[key] !== undefined)
                            if (dataSourceItem[key] instanceof Date) {


                                if (scope.calendarMap[key] && value.indexOf("-") > 0) {
                                    var dates = value.split("-");
                                    var dtMin = ParseDate(dates[0]);
                                    var dtMax = ParseDate(dates[1]);
                                    dtMax.setHours(23, 59, 59,999);
                                    return dataSourceItem[key] >= dtMin && dataSourceItem[key] <= dtMax;
                                };

                                let k = DateString(dataSourceItem[key], scope.dateFormat).toLowerCase().indexOf(value.toLowerCase());
                                if (k < 0)
                                    return excludeMatch;
                                if (exactMatch)
                                    if (k !== 0)
                                        return excludeMatch;
                                if (excludeMatch)
                                    return false;
                            }
                            else {
                                let k = scope.callbackMapValue(key, dataSourceItem[key], dataSourceItem).toString().toLowerCase().indexOf(value.toLowerCase());
                                if (k < 0)
                                    return excludeMatch;
                                if (exactMatch)
                                    if (k !== 0)
                                        return excludeMatch;
                                if (excludeMatch)
                                    return false;
                            }
                    }
                }
            }



            if (filterDescription)
                if (dataSourceItem.filterDescriptionLowerCase.indexOf(filterDescription) < 0) return false;

            if (filterInside)
                if (dataSourceItem.filterInsideLowerCase.indexOf(filterInside) < 0) return false;

            if (!fromDate)
                return true;

            if (scope.defaultDateField === null)
                return true;

            try {
                return dataSourceItem[scope.defaultDateField].getTime() >= fromDate && dataSourceItem[scope.defaultDateField].getTime() <= toDate;
            }
            catch (e) {

                return true;
            }
        }

        ////////////////////

        if (!sortField) {
            sortField = scope.defaultSortingField;
        }

        if (!reverseSorting) {
            sortingTable[sortField] = scope.defaultSortingDirection;
            reverseSorting = scope.defaultSortingDirection;
        }

        this.displayParams.sortingField = sortField;
        this.displayParams.reverseSorting = reverseSorting;

        var resultTable = "";

        function HTMLEncode(str) {
            var i = str.length,
                aRet = [];

            while (i--) {
                var iC = str[i].charCodeAt();
                if (iC < 65 || iC > 127 || (iC > 90 && iC < 97)) {
                    aRet[i] = '&#' + iC + ';';
                } else {
                    aRet[i] = str[i];
                }
            }
            return aRet.join('');
        }

        function cell(value, name, img, cellStyle, key, dataSourceItem, htmlEncode) {


            if (value === undefined || value === null) value = "None";
            value = value.toString();

            if (value[0] !== '<' && value.length > scope.maxLength)
                value = value.substring(0, scope.maxLength) + "...";

            if (!img) img = "";
            if (scope.editFields[key]) {

                var regEx = scope.editFields[key];
                if (regEx === "DATE")
                    regEx = "";
                regEx = regEx.replaceAll("/", "");
                var pat = "";
                var regExShow = regEx;
                if (regEx === "DATE") {
                    regEx = "([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))";
                    regExShow = "(DATE) YYYY-MM-DD";
                }

                if (regEx === "STRING") {
                    regExShow = "ANY TEXT";
                }

                if (regEx !== "" && regEx !== "STRING")
                    pat = "pattern= '" + regEx + "'";

                var req = "required";
                var val = value.replaceAll("'", "''");
                if (val == "" || !val)
                    req = "";
                if (regEx === "STRING")
                    resultTable += "<td style='" + cellStyle + "' title='" + name + "'><textarea onfocus=\"FormBuilderDataViewerInstances[" + scope.id + "].ShowPattern(this,'" + key + "', '" + regExShow + "'," + dataSourceItem.id + ");\" onkeyup=\"FormBuilderDataViewerInstances[" + scope.id + "].EditField(this, '" + key + "'," + dataSourceItem.id + ");\" " + req + " cols=50 rows=5 type=text " + pat + " '>" + val + "</textarea>" + img + "</td>";
                else
                    resultTable += "<td style='" + cellStyle + "' title='" + name + "'><input onfocus=\"FormBuilderDataViewerInstances[" + scope.id + "].ShowPattern(this,'" + key + "', '" + regExShow + "'," + dataSourceItem.id + ");\" onkeyup=\"FormBuilderDataViewerInstances[" + scope.id + "].EditField(this, '" + key + "'," + dataSourceItem.id + ");\" " + req + " type=text " + pat + " value='" + val + "'>" + img + "</td>";
            }
            else {
                var cbox = "";
                if (key == scope.checkBoxKey) {

                    var idVal = value;//.substr(0, value.indexOf(" "));
                    var checked = scope.checkBoxStatus[idVal];
                    idVal = "dbViewerCheck" + idVal;
                    if (!checked) checked = "";
                    else
                        checked = "checked='" + checked + "'";

                    cbox = "<input style='height:10px !important; margin:0px !important; padding:0px !important;' " + checked + " onchange='FormBuilderDataViewerInstances[" + scope.id + "].CheckBoxSelected();' id='" + idVal + "' type=checkbox>&nbsp;";
                }

                var title = name.replaceAll("<br>", " ");
                title = title.replaceAll("<sub>", " ");
                title = title.replaceAll("</sub>", " ");
                if (scope.getCellHint)
                    title = scope.getCellHint(key, value, dataSourceItem);
                resultTable += "<td style='" + cellStyle + "' title='" + title + "'>" + cbox + (htmlEncode ? HTMLEncode(value) : value) + img + "</td>";
            }
        }


        //////////////////////// INITIAL PARSING
        var maxDate = 0;
        var minDate = new Date(2050, 1, 1).getTime();
        this.displayParams.ref = new Object();
        this.displayParams.visArray = new Array();

        //var len = Math.min(100, dataSource.length);

        var len = dataSource.length;

        for (let i = 0; i < len; i++) {

            let dataSourceItem = dataSource[i];

            this.callackValidateAllFields(dataSourceItem);

            if (this.defaultDateField !== null) {
                if (typeof (dataSourceItem[this.defaultDateField]) === "string")
                    dataSourceItem[this.defaultDateField] = new Date(dataSourceItem[this.defaultDateField]);


                if (dataSourceItem[this.defaultDateField] > maxDate)
                    maxDate = dataSourceItem[this.defaultDateField].getTime();

                if (dataSourceItem[this.defaultDateField] < minDate)
                    minDate = dataSourceItem[this.defaultDateField].getTime();
            }

            if (dataSourceItem.filterDescriptionLowerCase === undefined) {
                dataSourceItem.filterDescriptionLowerCase = this.callackGetSearchByDescriptionPattern(dataSourceItem);
            }

            if (dataSourceItem.filterInsideLowerCase === undefined) {
                dataSourceItem.filterInsideLowerCase = this.callackGetSearchByInsidePattern(dataSourceItem);
            }
        }

        //////////////////////////

        dataSource.sort(compareFunction);

        if (this.maxDate === undefined) {
            this.maxDate = new Date(maxDate);
            this.minDate = new Date(minDate);
        }


        //////////////////////// HEADER CREATION

        var headerString = "";

        var calendarFields = new Object();

        function header(field, title) {

            var widthInfo = scope.headerWidthInfo[field];
            if (!widthInfo)
                widthInfo = "";

            var titleString = '';
            if (scope.getCustomHeaderHint) {
                titleString = scope.getCustomHeaderHint(field, title);
            }

            var fieldId = field.replaceAll(".", "_");
            fieldId = fieldId.replaceAll(" ", "_");
            fieldId = fieldId.replaceAll("(", "_");
            fieldId = fieldId.replaceAll(")", "_");

            if (scope.includeSorting)
                headerString += "<th title='" + titleString + "' style='position:relative; " + widthInfo + "' id='thItem" + scope.id + fieldId + "' onclick=\"FormBuilderDataViewerInstances[" + scope.id + "].SortByField(this, '" + field + "');\" class='clickable_row noselect'>";
            else
                headerString += "<th title='" + titleString + "' style='position:relative; " + widthInfo + "' id='thItem" + scope.id + fieldId + "' class='clickable_row noselect'>";

            if (field === sortField) {
                lastArrowSource = "thItem" + scope.id + field;
                if (scope.defaultSortingDirection === 1)
                    headerString += title + scope.imgArrowDown.replace("none", "block") + scope.imgArrowUp;
                else
                    headerString += title + scope.imgArrowUp.replace("none", "block") + scope.imgArrowDown;
            }
            else
                headerString += title + scope.imgArrowUp + scope.imgArrowDown;
            if (scope.includeAutoFilters) {

                if (scope.calendarMap && scope.calendarMap[field]) {
                    var idCalendar = "fieldFilter" + scope.id + field.replaceAll(".", "_");
                    headerString += "<br><div onclick='event.stopPropagation();'  id='" + idCalendar + "_caldiv' style='width:100%; min-width:10px;'></div>";
                    calendarFields[idCalendar] = field;
                    
                }
                else
                headerString += "<br><input onclick='event.stopPropagation();' id='fieldFilter" + scope.id + field.replaceAll(".", "_") + "' onkeyup=\"FormBuilderDataViewerInstances[" + scope.id + "].FilterByField(this, '" + field + "');\" size='1' style='width:100%; min-width:10px;' type=text>";
            }
            headerString += "</th>";
        }



        if ($("#" + scope.headerOfTable).html().trim() === "") {
            for (let displayOrderKey = 0; displayOrderKey < scope.displayFieldsArray.length; displayOrderKey++) {
                let key = scope.displayFieldsArray[displayOrderKey];
                let displayName = key;
                if (scope.displayFields !== null)
                    displayName = scope.displayFields[key];

                if (displayName !== undefined && displayName !== null)
                    header(key, displayName);
            }
            $("#" + scope.headerOfTable).html(headerString);

            for (var ckey in calendarFields)
            {
                let id = ckey;//calendarFields[c];
                InitCalendarById(calendarFields[ckey],dataSource,id, "FormBuilderDataViewerInstances[" + scope.id + "].FilterByField(this, '" + id + "');");
                
                
            }
        }

        var visCount = 0;

        //len = Math.min(100, dataSource.length);
        var startFrom = 0;

        if (this.includePaging) {
            startFrom = this.pageSize * this.currentPage;
        }

        var endIndex = (this.pageSize) * (this.currentPage + 1);
        if (endIndex > len)
            endIndex = len;

        if (startFrom >= len) {
            startFrom = 0;
            currentPage = 0;
        }


        var totalCount = 0;

        if (dataSource.length > 0)
            predefindDataSourceItemInFilters(dataSource[0]);

        for (let i = 0; i < len; i++) {
            let dataSourceItem = dataSource[i];

            if (!this.callbackIsRowVisible(dataSourceItem)) continue;
            totalCount++;

            if (!dataSourceItemInFilters(dataSourceItem)) continue;

            visCount++;
            if (visCount - 1 < startFrom || visCount > endIndex)
                continue;

            if (this.callbackIsObjectClickable(dataSourceItem)) {
                resultTable += "<tr onclick='FormBuilderDataViewerInstances[" + this.id + "].OpenItem(this," + dataSourceItem.id + ");' class='issue_row_hover'>";
            }
            else
                resultTable += "<tr class='issue_row'>";

            if (this.rowClass)
                resultTable = resultTable.replace("class='issue_row", "class='" + this.rowClass + " issue_row");

            this.displayParams.visArray.push(dataSourceItem);
            this.displayParams.ref[dataSourceItem.id] = dataSourceItem;

            for (let displayOrderKey = 0; displayOrderKey < this.displayFieldsArray.length; displayOrderKey++) {
                let key = this.displayFieldsArray[displayOrderKey];

                let displayName = key;
                if (scope.displayFields !== null)
                    displayName = scope.displayFields[key];

                if (displayName !== undefined && displayName !== null)
                    if (this.callbackIsCellClickable(key, dataSourceItem))
                        cell(this.callbackMapValue(key, dataSourceItem[key], dataSourceItem, i), displayName, scope.imgRowHasSubItemsImage,
                            this.callbackGetCellStyle(key, dataSourceItem), key, dataSourceItem, this.callbackMapValueHtmlEncode(key));
                    else
                        cell(this.callbackMapValue(key, dataSourceItem[key], dataSourceItem, i), displayName, undefined,
                            this.callbackGetCellStyle(key, dataSourceItem), key, dataSourceItem, this.callbackMapValueHtmlEncode(key));
            }

            resultTable += "</tr>";
        }



        $("#" + this.bodyOfTable).html(resultTable);
        this.totalPages = Math.ceil(visCount / this.pageSize);

        if (this.totalPages > 0)
            if (this.currentPage >= this.totalPages) {
                this.currentPage = 0;
                this.Draw();
            }

        this.FillSummary(visCount, null, null, totalCount);

    };

    this.FillLoading = function () {
        var summary = "Loading data:  <span style='color:darkred;'>please wait...</span>";
        $("#" + this.summaryOfTable).html(summary);
    };

    this.FillSummary = function (visCount, field, pattern, totalCount) {
        if (this.summaryOfTable) {
            if (!this.tableName)
                this.tableName = "";

            var summary = "";
            var summaryPrintData = "";
            if (this.tableName !== "")
                summary += "<span style='vertical-align:middle; font-size:12px; color:gray;'>Showing data for:&nbsp</span><span style='vertical-align:middle; color:rgb(8, 78, 135);'>" + this.tableName + '</span>';

            if (this.showTotal)
                summaryPrintData += "Total:  <span style='vertical-align:middle; color:rgb(8, 78, 135);'>" + totalCount + '</span>&nbsp;&nbsp;';

            if (this.showDisplayed) {
                if (visCount >= 0)
                    if (visCount < totalCount || !this.showTotal) {
                        if (!this.showTotal) {
                            summaryPrintData += " Displayed:<span style='color:darkred;'>" + visCount + '</span>';
                        }
                        else
                            summaryPrintData += " | Displayed:<span style='color:darkred;'>" + visCount + '</span>';
                    }
            }
            else
                summary = summary.replace("|", "");

            if (pattern && pattern !== "")
                summary += " Value format <span style='color:green;'>" + field + ":</span> <span style='color:darkred;'>" + pattern + '</span>';

          
            if (this.includePrintButton && this.includePrintFunction)
                summaryPrintData += '<a href="#" onclick="' + this.includePrintFunction + '; return false;" class="issue_button2"><img width="17" src="img/print.png"></a>';
            else
                if (this.includePrintButton)
                    summaryPrintData += ' <a href="#" onclick="FormBuilderDataViewerInstances[' + this.id + '].print(); return false;" class="issue_button2" style="opacity:0.6;"><img width="25" src="img/print.png"></a>';

            if (this.includeExcelButton)
                summaryPrintData += ' <a href="#" onclick="FormBuilderDataViewerInstances[' + this.id + '].exportExcel(); return false;" class="issue_button2" style="opacity:0.6;"><img width="25" src="img/excel.png"></a>';

            if (this.includePaging) {

                summaryPrintData += '<span style="font-size:12px;">Page <span class="paginationnew"><a href = "#" onclick = "FormBuilderDataViewerInstances[' + this.id + '].prevPage(); return false;" class="leftarrow"><img src="img/Path 4598.svg"></a> ' + (this.currentPage + 1) + '/' + this.totalPages + ' <a href="#" onclick="FormBuilderDataViewerInstances[' + this.id + '].nextPage(); return false;" class="rightarrow"><img src="img/Path 4599.svg"> </a></span></span>';
                //summary += ' Page ' + (this.currentPage + 1) + '/' + this.totalPages + ' <a href = "#" onclick = "FormBuilderDataViewerInstances[' + this.id + '].prevPage(); return false;" class="" >&lt;</a >&nbsp;';
                //summary += '<a href="#" onclick="FormBuilderDataViewerInstances[' + this.id + '].nextPage(); return false;" class="">&gt; </a>';
            }


            if (this.showTableNameOnly)
                $("#" + this.summaryOfTable).html(this.tableName);
            else
                $("#" + this.summaryOfTable).html("<div style='vertical-align:middle; width:100%;'><div style='width:50%; display:inline-block; vertical-align:middle;'>" + summary + "</div><span style='text-align:right; width:50%; display:inline-block;'>" + summaryPrintData +"</span></div>");

        }
    }

}

