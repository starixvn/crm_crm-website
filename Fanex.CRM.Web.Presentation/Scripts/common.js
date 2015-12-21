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