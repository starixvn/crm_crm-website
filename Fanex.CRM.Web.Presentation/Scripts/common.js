"use strict";
/* jshint unused: false */
/* exported addDays, CheckDecNumber, convertUnitToLocalDatetime, emailListValidate, findMainWindow, formatDate,
   gup, replaceSpaceForIE9, replaceSpecialHtmlTag, reRenderGrid, trimAllSpace */
/* jshint maxlen: 200 */
function gup(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]"); // jshint ignore:line
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null) {
        return "";
    }
    else {
        return results[1];
    }
}


function CheckDecNumber(e) {
    var code = e.which || e.keyCode;
    if ((code < 48 || code > 57) && code !== 8 && code !== 13 && code !== 0 && code !== 46 && code !== 9) {
        return false;
    }

    return true;
}

///// <reference path="stdlib.js" />
function formatDate(objDatime, strFormat) {
    objDatime.format(strFormat);
}

function convertUnitToLocalDatetime(objDatime, strFormat) {
    var okDate = /Date\(([-+]?\d+[-+]?\d+)\)/.exec(objDatime);
    var date;
    if (okDate) {
        var when = new Date(eval(okDate[1])); // jshint ignore:line
        date = when;
    } else {
        var currdate = new Date();
        date = currdate;
    }

    return strFormat ? date.format(strFormat) : date;
}

//add n number of days
Date.prototype.goto = function (n) {  // jshint ignore:line
    this.setDate(this.getDate() + n);
    return this;
};

function addDays(startDate, n) {
    var endDate = new Date();
    endDate.setDate(startDate.getDate() + n);
    return endDate;
}

function findMainWindow(wnd) {
    var mainWnd = wnd;
    //Main frame name = content
    if (mainWnd.name === "content") {
        return mainWnd;
    }
    else {
        mainWnd = mainWnd.parent;
        if (mainWnd.parent != null) {
            mainWnd = findMainWindow(mainWnd);
            return mainWnd;
        }
        else {
            return mainWnd;
        }
    }
}

$.extend({
    URLEncode: function (c) {
        var o = ''; var x = 0; c = c.toString(); var r = /(^[a-zA-Z0-9_.]*)/;
        while (x < c.length) {
            var m = r.exec(c.substr(x));
            if (m != null && m.length > 1 && m[1] !== '') {
                o += m[1]; x += m[1].length;
            } else {
                if (c[x] === ' ') {
                    o += '+';
                } else {
                    var d = c.charCodeAt(x); var h = d.toString(16);
                    o += '%' + (h.length < 2 ? '0' : '') + h.toUpperCase();
                } x++;
            }
        } return o;
    },
    URLDecode: function (s) {
        // fixed -- + char decodes to space char
        var o = s;
        var binVal, t, b;
        var regex = /(%[^%]{2}|\+)/;
        var m = regex.exec(o);
        while (m != null && m.length > 1 && m[1] !== '') {
            if (m[1] === '+') {
                t = ' ';
            } else {
                b = parseInt(m[1].substr(1), 16);
                t = String.fromCharCode(b);
            }
            o = o.replace(m[1], t);
        }
        return o;
    }
});


function trimAllSpace(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

String.prototype.trim = function () { // jshint ignore:line
    return this.replace(/^\s*/, "").replace(/\s*$/, "");
};

function removeSpecialHtmlTag(content) {
    return content.replace(/(<\s*script\s*([^>]*)>((.|\n)*?)<\s*\/\s*script>)|(<\s*style\s*([^>]*)>((.|\n)*?)<\s*\/\s*style>)/g, "");

}

function replaceSpecialHtmlTag(content) {
    content = removeSpecialHtmlTag(content);
    return content.replace(/(table)|(tr)|(td)|(div)|(colgroup)|(col)/g, "span");
}

function emailValidate(email) {
    var reg = /^([a-zA-Z0-9])(([a-zA-Z0-9])*([\._-])?([a-zA-Z0-9]))*@(([a-zA-Z0-9\-])+(\.))+([a-zA-Z]{2,4})+$/;
    if (email.search(reg) === -1) {
        return false;
    }
    return true;
}

function emailListValidate(emailList) {
    if (emailList.trim() === '') {
        return 1;
    }
    else {
        var email = emailList.split(',');
        for (var i = 0; i < email.length; i++) {
            if (!emailValidate(email[i])) {
                return 2;
            }
        }
    }
    return 0;
}

function replaceSpaceForIE9(text) {    
    if ($.browser.msie && $.browser.version < 9 || !$.browser.msie) {
        return text;
    }

    var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');
    var html = text.replace(expr, '><');

    return html;
}

function reRenderGrid(gridID, frozenID, scrollID, gridHeight, gridFrozenWidth, totalFrozenCols, fristColWidth) {
    var trFrozen = $('#' + frozenID + ' tbody tr');
    var totalRows = trFrozen.length;

    for (var i = 0; i < totalRows; i++) {
        var frozenHeight = $('#' + frozenID + ' tbody tr:eq(' + i + ') td:eq(0)').height();
        var scrollHeight = $('#' + scrollID + ' tbody tr:eq(' + i + ') td:eq(0)').height();

        var maxHeight = 18;
        if (frozenHeight > scrollHeight) {
            maxHeight = frozenHeight + 1;
        }
        else if (scrollHeight > frozenHeight) {
            maxHeight = scrollHeight + 1;
        }
        else {
            maxHeight = frozenHeight;
        }
        $('#' + scrollID + ' tbody tr:eq(' + i + ') td:eq(0) div').css('height', maxHeight).append(' ');
        $('#' + frozenID + ' tbody tr:eq(' + i + ') td:eq(0)>div').attr('style', ' width:' + fristColWidth + 'px; display:table-cell; vertical-align: middle; height:' + maxHeight + 'px');
    }

    var mainreport = $("#" + gridID);

    var gridOpts = {
        tblContentId: scrollID,
        manualCopyHeader: true,
        fullWidth: true,
        fullHeight: true,
        gridHeight: gridHeight,
        freezeWidth: gridFrozenWidth,
        numberFreezeCol: totalFrozenCols
    };
    mainreport.gridScroller(gridOpts);

}

// String Format
String.prototype.format = function () { // jshint ignore:line
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] !== 'undefined' ? args[number] : match;
    });
};