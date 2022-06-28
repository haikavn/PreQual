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
    //return cookies[cname];
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

function ClearCacheAndReload() {
    window.localStorage.clear();
    window.location.reload();
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