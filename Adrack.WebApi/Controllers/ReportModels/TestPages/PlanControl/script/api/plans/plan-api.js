var LZString = function () { function o(o, r) { if (!t[o]) { t[o] = {}; for (var n = 0; n < o.length; n++)t[o][o.charAt(n)] = n } return t[o][r] } var r = String.fromCharCode, n = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", e = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-$", t = {}, i = { compressToBase64: function (o) { if (null == o) return ""; var r = i._compress(o, 6, function (o) { return n.charAt(o) }); switch (r.length % 4) { default: case 0: return r; case 1: return r + "==="; case 2: return r + "=="; case 3: return r + "=" } }, decompressFromBase64: function (r) { return null == r ? "" : "" == r ? null : i._decompress(r.length, 32, function (e) { return o(n, r.charAt(e)) }) }, compressToUTF16: function (o) { return null == o ? "" : i._compress(o, 15, function (o) { return r(o + 32) }) + " " }, decompressFromUTF16: function (o) { return null == o ? "" : "" == o ? null : i._decompress(o.length, 16384, function (r) { return o.charCodeAt(r) - 32 }) }, compressToUint8Array: function (o) { for (var r = i.compress(o), n = new Uint8Array(2 * r.length), e = 0, t = r.length; t > e; e++) { var s = r.charCodeAt(e); n[2 * e] = s >>> 8, n[2 * e + 1] = s % 256 } return n }, decompressFromUint8Array: function (o) { if (null === o || void 0 === o) return i.decompress(o); for (var n = new Array(o.length / 2), e = 0, t = n.length; t > e; e++)n[e] = 256 * o[2 * e] + o[2 * e + 1]; var s = []; return n.forEach(function (o) { s.push(r(o)) }), i.decompress(s.join("")) }, compressToEncodedURIComponent: function (o) { return null == o ? "" : i._compress(o, 6, function (o) { return e.charAt(o) }) }, decompressFromEncodedURIComponent: function (r) { return null == r ? "" : "" == r ? null : (r = r.replace(/ /g, "+"), i._decompress(r.length, 32, function (n) { return o(e, r.charAt(n)) })) }, compress: function (o) { return i._compress(o, 16, function (o) { return r(o) }) }, _compress: function (o, r, n) { if (null == o) return ""; var e, t, i, s = {}, p = {}, u = "", c = "", a = "", l = 2, f = 3, h = 2, d = [], m = 0, v = 0; for (i = 0; i < o.length; i += 1)if (u = o.charAt(i), Object.prototype.hasOwnProperty.call(s, u) || (s[u] = f++ , p[u] = !0), c = a + u, Object.prototype.hasOwnProperty.call(s, c)) a = c; else { if (Object.prototype.hasOwnProperty.call(p, a)) { if (a.charCodeAt(0) < 256) { for (e = 0; h > e; e++)m <<= 1, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++; for (t = a.charCodeAt(0), e = 0; 8 > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1 } else { for (t = 1, e = 0; h > e; e++)m = m << 1 | t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t = 0; for (t = a.charCodeAt(0), e = 0; 16 > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1 } l-- , 0 == l && (l = Math.pow(2, h), h++), delete p[a] } else for (t = s[a], e = 0; h > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1; l-- , 0 == l && (l = Math.pow(2, h), h++), s[c] = f++ , a = String(u) } if ("" !== a) { if (Object.prototype.hasOwnProperty.call(p, a)) { if (a.charCodeAt(0) < 256) { for (e = 0; h > e; e++)m <<= 1, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++; for (t = a.charCodeAt(0), e = 0; 8 > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1 } else { for (t = 1, e = 0; h > e; e++)m = m << 1 | t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t = 0; for (t = a.charCodeAt(0), e = 0; 16 > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1 } l-- , 0 == l && (l = Math.pow(2, h), h++), delete p[a] } else for (t = s[a], e = 0; h > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1; l-- , 0 == l && (l = Math.pow(2, h), h++) } for (t = 2, e = 0; h > e; e++)m = m << 1 | 1 & t, v == r - 1 ? (v = 0, d.push(n(m)), m = 0) : v++ , t >>= 1; for (; ;) { if (m <<= 1, v == r - 1) { d.push(n(m)); break } v++ } return d.join("") }, decompress: function (o) { return null == o ? "" : "" == o ? null : i._decompress(o.length, 32768, function (r) { return o.charCodeAt(r) }) }, _decompress: function (o, n, e) { var t, i, s, p, u, c, a, l, f = [], h = 4, d = 4, m = 3, v = "", w = [], A = { val: e(0), position: n, index: 1 }; for (i = 0; 3 > i; i += 1)f[i] = i; for (p = 0, c = Math.pow(2, 2), a = 1; a != c;)u = A.val & A.position, A.position >>= 1, 0 == A.position && (A.position = n, A.val = e(A.index++)), p |= (u > 0 ? 1 : 0) * a, a <<= 1; switch (t = p) { case 0: for (p = 0, c = Math.pow(2, 8), a = 1; a != c;)u = A.val & A.position, A.position >>= 1, 0 == A.position && (A.position = n, A.val = e(A.index++)), p |= (u > 0 ? 1 : 0) * a, a <<= 1; l = r(p); break; case 1: for (p = 0, c = Math.pow(2, 16), a = 1; a != c;)u = A.val & A.position, A.position >>= 1, 0 == A.position && (A.position = n, A.val = e(A.index++)), p |= (u > 0 ? 1 : 0) * a, a <<= 1; l = r(p); break; case 2: return "" }for (f[3] = l, s = l, w.push(l); ;) { if (A.index > o) return ""; for (p = 0, c = Math.pow(2, m), a = 1; a != c;)u = A.val & A.position, A.position >>= 1, 0 == A.position && (A.position = n, A.val = e(A.index++)), p |= (u > 0 ? 1 : 0) * a, a <<= 1; switch (l = p) { case 0: for (p = 0, c = Math.pow(2, 8), a = 1; a != c;)u = A.val & A.position, A.position >>= 1, 0 == A.position && (A.position = n, A.val = e(A.index++)), p |= (u > 0 ? 1 : 0) * a, a <<= 1; f[d++] = r(p), l = d - 1, h--; break; case 1: for (p = 0, c = Math.pow(2, 16), a = 1; a != c;)u = A.val & A.position, A.position >>= 1, 0 == A.position && (A.position = n, A.val = e(A.index++)), p |= (u > 0 ? 1 : 0) * a, a <<= 1; f[d++] = r(p), l = d - 1, h--; break; case 2: return w.join("") }if (0 == h && (h = Math.pow(2, m), m++), f[l]) v = f[l]; else { if (l !== d) return null; v = s + s.charAt(0) } w.push(v), f[d++] = s + v.charAt(0), h-- , s = v, 0 == h && (h = Math.pow(2, m), m++) } } }; return i }(); "function" == typeof define && define.amd ? define(function () { return LZString }) : "undefined" != typeof module && null != module && (module.exports = LZString);
var totalSize = 0;
var adrackAPI = new AdrackAPI();

function hexToRgb(hex) {
    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
        r: parseInt(result[1], 16),
        g: parseInt(result[2], 16),
        b: parseInt(result[3], 16)
    } : null;
}

function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};


function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function getWeekNumber(date) {

    var d = new Date(1990, 11, 31, 0, 0, 0);
    //  d.setTime(d.getTime() + d.getTimezoneOffset() * 60 * 1000);

    var fromMoment = moment(d);

    var toMoment = moment(date);
    var duration = moment.duration(toMoment.diff(fromMoment));
    return Math.trunc(duration.asWeeks()) + 1;

}



function getWeekFirstDay(date) {

    var d = new Date(1990, 11, 31, 0, 0, 0);
    return dateAdd(d, "week", getWeekNumber(date) - 1);

}

function getDayNumber(date) {
    var fromMoment = moment(29361600000);
    var toMoment = moment(date);
    var duration = moment.duration(toMoment.diff(fromMoment));
    return Math.trunc(duration.asDays()) + 1;
}

function dateAdd(date, interval, units) {
    if (!(date instanceof Date))
        return undefined;
    if (units === 0) return date;
    var ret = new Date(date); //don't change original date
    var checkRollover = function () { if (ret.getDate() !== date.getDate()) ret.setDate(0); };
    switch (String(interval).toLowerCase()) {
        case 'year': ret.setFullYear(ret.getFullYear() + units); checkRollover(); break;
        case 'quarter': ret.setMonth(ret.getMonth() + 3 * units); checkRollover(); break;
        case 'month': ret.setMonth(ret.getMonth() + units); checkRollover(); break;
        case 'week': ret.setDate(ret.getDate() + 7 * units); break;
        case 'day': ret.setDate(ret.getDate() + units); break;
        case 'hour': ret.setTime(ret.getTime() + units * 3600000); break;
        case 'minute': ret.setTime(ret.getTime() + units * 60000); break;
        case 'second': ret.setTime(ret.getTime() + units * 1000); break;
        default: ret = undefined; break;
    }
    return ret;
}

String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}

function DefaultRanges(minDate, maxDate) {


    var obj = {
        'Today': [moment().toDate().setHours(0, 0, 0, 0), moment().toDate().setHours(23, 59, 59, 999)],
        'Yesterday': [moment().subtract(1, 'days').toDate().setHours(0, 0, 0, 0), moment().subtract(1, 'days').toDate().setHours(23, 59, 59, 999)],
        'Last 7 Days': [moment().subtract(6, 'days').toDate().setHours(0, 0, 0, 0), moment().toDate().setHours(23, 59, 59, 999)],
        'Last 30 Days': [moment().subtract(29, 'days').toDate().setHours(0, 0, 0, 0), moment().toDate().setHours(23, 59, 59, 999)],
        'This Month': [moment().startOf('month').toDate().setHours(0, 0, 0, 0), moment().endOf('month').toDate().setHours(23, 59, 59, 999)],
        'Last Month': [moment().subtract(1, 'month').startOf('month').toDate().setHours(0, 0, 0, 0), moment().subtract(1, 'month').endOf('month').toDate().setHours(23, 59, 59, 999)]
    };

    if (minDate !== undefined) {
        obj['Whole Range'] = [minDate.setHours(0, 0, 0, 0), maxDate.setHours(23, 59, 59, 999)];
    }
    return obj;
}

function isNumeric(num) {
    if (num.match(/^-{0,1}\d+$/)) {
        return true;
    } else if (num.match(/^\d+\.\d+$/)) {
        return true;
    }
    return false;
}


function getParameterByName(name) {
    var regexS = "[\\?&]" + name + "=([^&#]*)",
        regex = new RegExp(regexS),
        results = regex.exec(window.location.search);
    if (results === null) {
        return "";
    } else {
        return decodeURIComponent(results[1].replace(/\+/g, " "));
    }
}

function DateTimeInfo() {

}


function DateString(date, format) {

    if (!format) format = "MM/DD/YYYY HH:mm:ss (UTC)";

    if (date === null || date === undefined || date === "")
        return moment(new Date(Date.now())).format(format);

    if (date < new Date(2000, 1, 1))
        return "N/A";


    var res = moment(date).format(format);
    return res;
}


function DateStringOnly(date, format) {
    if (!format) format = "YYYY-MM-DD"; //YYYY-MM-DD
    if (date === null || date === undefined || date === "")
        return moment(new Date(Date.now())).format(format);
    if (date < new Date(2000, 1, 1))
        return "N/A";
    var res = moment(date).format(format);
    return res;
}

function ParseDate(date, ignoreutc) {
    if (date === null || date === undefined) {
        return null;
    }
    ignoreutc = true;

    if (date instanceof Date)
        return date;

    date = date.replace("(UTC)", "");

    var minus = date.indexOf("-");
    if (minus > 0 && minus < 4) {
        var parsed = Date.parse(date);
        return new Date(parsed);
    }
    var plus = date.indexOf("+");
    if (plus > 0)
        date = date.substring(0, plus);
    //if (date.indexOf("T") < 0)
    //  return new Date(Date.parse(date));
    var split = date.split("T");

    if (date.indexOf("T") < 0) {
        split = date.split(" ");
    }

    var dateString = split[0].split("-");
    if (dateString[0].indexOf("/") >= 0) {
        dateString = split[0].split("/");
        var strs = new Array();
        strs[0] = dateString[2];
        strs[1] = dateString[0];
        strs[2] = dateString[1];
        dateString = strs;
    }


    if (split.length > 1) {
        var timeString = split[1].split(":");
        if (!timeString[2])
            timeString[2] = 0;
        if (ignoreutc)
            dt = new Date(dateString[0], parseInt(dateString[1]) - 1, dateString[2], timeString[0], timeString[1], timeString[2]);
        else
            dt = new Date(Date.UTC(dateString[0], parseInt(dateString[1]) - 1, dateString[2], timeString[0], timeString[1], timeString[2]));
    }
    else {
        if (ignoreutc)
            dt = new Date(dateString[0], parseInt(dateString[1]) - 1, dateString[2]);
        else
            dt = new Date(Date.UTC(dateString[0], parseInt(dateString[1]) - 1, dateString[2]));
    }
    return dt;
}


function AdrackAPI() {

    this.userName = null;
    this.password = null;

    this.serverURL = "https://localhost:44331/api/report/";
    
    this.donorId = null;

    this.writeCache = true;
    this.ignoreCacheNextTime = false;
    this.cacheList = new Object();

    this.cacheTimeoutSeconds = 3600 * 5; //5 hours timeout
  
    this.saveLoginInfo = function () {
        $.cookie('user', this.userName);
        $.cookie('pass', this.password);
    };

    this.readLoginInfo = function () {
        this.userName = "-";
        this.password = "-";
    };

    this.getCommand = function (action) {        
        var obj = new Object();
        obj.action = action;
        obj.params = new Object();
        return obj;
    };


    this.clearCache = function () {
        for (var key in this.cacheList) {
            window.localStorage[key] = "null";
        }
        this.cacheList = new Object();
    };



    this.runCommand = function (command, successHandler,
        errorHandler, datatype) {
        var scope = this;

        return this.runCommandCache(command, successHandler, errorHandler, datatype);
    };

    this.runCommandCache = function (command, successHandler,
        errorHandler, datatype) {
        if (!datatype)
            datatype = "json";

        


        //this.writeCache = false;

        if (this.writeCache && !this.ignoreCacheNextTime) {
            this.ignoreCacheNextTime = false;
            var storageKey = JSON.stringify(command);

            this.cacheList[storageKey] = 1;
            if (window.localStorage[storageKey] !== undefined && window.localStorage[storageKey] !== "null") {
                var decompressed = LZString.decompress(window.localStorage[storageKey].toString());
                
                var result = JSON.parse(decompressed);
                console.log("DECOMPRESS " + decompressed.length);
                if (Date.now() - result.dateCached < this.cacheTimeoutSeconds * 1000) {
                    successHandler(result);
                    return;
                }
            }
        }

        this.ignoreCacheNextTime = false;

        /*$.ajaxSetup({
                xhrFields: {
                    withCredentials: true
                }
            });*/

        var serverURL = this.serverURL;


        var sendCommand = JSON.stringify(command);
        var cloneCommand = JSON.parse(sendCommand);

        
        $.ajax({
            type: "POST",
            data: JSON.stringify(cloneCommand.params),
            dataType: datatype,
            crossDomain: true,
            url: serverURL + command.action
        }
        ).done(function (result, textstatus, request) {
            if (datatype === 'json') {
                    result.dateCached = Date.now();
                    try {
                        //if (request.responseText.length > 1024 * 1024 * 8)
                        //  window.localStorage[storageKey] = null;
                        //else 
                        {
                            var storageKey = JSON.stringify(command);
                            var resStr = JSON.stringify(result);
                            totalSize += resStr.length;
                            var compressed = LZString.compress(resStr);
                            
                            

                            try {
                                window.localStorage[storageKey] = compressed;
                            }
                            catch (e) {
                                window.localStorage[storageKey] = null;
                            }
                        }
                    }
                    catch (ex) {

                    }
                    successHandler(result);
               
            }
            else
                successHandler(result);
        }).fail(function (error) {
            errorHandler(error.responseText);
        });
    };

    

    this.runCustomAction = function (actionName, params, successHandler, errorHandler) {
        this.readLoginInfo();
        var command = this.getCommand(actionName);
        if (!params)
            adrackAPI.getWrappedParameters(command);
        else
            command.params = params;
        this.runCommand(command, successHandler, errorHandler);
    };
    
    
    this.getLeadsReport = function (startDate,endDate,successHandler, errorHandler) {
        this.readLoginInfo();
        var command = this.getCommand("dashboard/leadsreport");
        command.params.DateFrom = DateStringOnly(startDate);
        command.params.DateTo = DateStringOnly(endDate);        
        this.runCommand(command, successHandler,errorHandler);
    };

    this.getCurrentPlan = function (startDate, endDate, successHandler, errorHandler) {
        this.readLoginInfo();
        var command = this.getCommand("plan/getcurrentplan");
        command.params.DateFrom = DateStringOnly(startDate);
        command.params.DateTo = DateStringOnly(endDate);
        this.runCommand(command, successHandler, errorHandler);
    };

    this.getPlanReport = function (startDate, endDate, successHandler, errorHandler) {
        this.readLoginInfo();
        var command = this.getCommand("plan/getplanreport");
        command.params.DateFrom = DateStringOnly(startDate);
        command.params.DateTo = DateStringOnly(endDate);
        this.runCommand(command, successHandler, errorHandler);
    };

    this.getTopStates = function (startDate, endDate, successHandler, errorHandler) {
        this.readLoginInfo();
        var command = this.getCommand("dashboard/topstates");
        command.params.DateFrom = DateStringOnly(startDate);
        command.params.DateTo = DateStringOnly(endDate);
        this.runCommand(command, successHandler, errorHandler);
    };

    this.getTopCountries = function (startDate, endDate, successHandler, errorHandler) {
        this.readLoginInfo();
        var command = this.getCommand("dashboard/topcountries");
        command.params.DateFrom = DateStringOnly(startDate);
        command.params.DateTo = DateStringOnly(endDate);
        this.runCommand(command, successHandler, errorHandler);
    };
 
}








