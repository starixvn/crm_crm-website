// Success=0, Error=1, Warning=2
function showAlertMessage(containerid, msg, type) {
    switch (type) {
        case 0:
            document.getElementById(containerid).innerHTML = "<div class='alertboxscc'><div>" + msg + "</div></div>";
            break;
        case 1:
            document.getElementById(containerid).innerHTML = "<div class='alertboxerr'><div>" + msg + "</div></div>";
            break;
        case 2:
            document.getElementById(containerid).innerHTML = "<div class='alertboxwrn'><div>" + msg + "</div></div>"; ;
            break;
        default:
            break;
    }
    var t = setTimeout("clearMessage('" + containerid + "')", 8000);
    window.scroll(0, 0);
    parent.scroll(0, 0);
}

function clearMessage(containerid) {
    document.getElementById(containerid).innerHTML = "";
}

function gup(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return results[1];
}


function CheckDecNumber(e) {

    var code = "";
    if (e.which)
        code = e.which;
    else if (e.keyCode)
        code = e.keyCode;

    if (code == 37 || code == 39 || code == 36 || code == 35) //e or E or "-"
        return true;
    if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 9)
        return false;
    return true;
}

function convertUnitToLocalDatetime(objDatime) {
    var okDate = /Date\(([-+]?\d+[-+]?\d+)\)/.exec(objDatime);
    var date; 
    if (okDate) {
        var when = new Date(eval(okDate[1]));
        date = when; 
    } else {
        var currdate = new Date();
        date = currdate;
    }
    return date;
}

///// <reference path="stdlib.js" />
function formatDate(objDatime, strFormat) {
    objDatime.format(strFormat);
}

function convertUnitToLocalDatetime(objDatime, strFormat) {
    var okDate = /Date\(([-+]?\d+[-+]?\d+)\)/.exec(objDatime);
    var date;
    if (okDate) {
        var when = new Date(eval(okDate[1]));
        date = when;
    } else {
        var currdate = new Date();
        date = currdate;
    }
    return date.format(strFormat);
}

//add n number of days
Date.prototype.goto = function (n) {
    //this.setTime(this.getTime() + n * 86400000);    
    this.setDate(this.getDate() + n);
    return this;
};

function findMainWindow(wnd) {
    var mainWnd = wnd;
    //Main frame name = content
    if (mainWnd.name == "content") {
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
            if (m != null && m.length > 1 && m[1] != '') {
                o += m[1]; x += m[1].length;
            } else {
                if (c[x] == ' ') o += '+'; else {
                    var d = c.charCodeAt(x); var h = d.toString(16);
                    o += '%' + (h.length < 2 ? '0' : '') + h.toUpperCase();
                } x++;
            }
        } return o;
    },
    URLDecode: function (s) {
        var o = s; var binVal, t; var r = /(%[^%]{2})/;
        while ((m = r.exec(o)) != null && m.length > 1 && m[1] != '') {
            b = parseInt(m[1].substr(1), 16);
            t = String.fromCharCode(b); o = o.replace(m[1], t);
        } return o;
    }
});


function trimAllSpace(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
};

String.prototype.trim = function () {
    return this.replace(/^\s*/, "").replace(/\s*$/, "");
};

function replaceSpecialHtmlTag(content) {
    content = removeSpecialHtmlTag(content);    
    return content.replace(/(table)|(tr)|(td)|(div)|(colgroup)|(col)/g, "span");
};

function removeSpecialHtmlTag(content) {    
    return content.replace(/(<\s*script\s*([^>]*)>((.|\n)*?)<\s*\/\s*script>)|(<\s*style\s*([^>]*)>((.|\n)*?)<\s*\/\s*style>)/g, "");

}

function emailListValidate(emailList) {
    if (emailList.trim() == '')
        return 1;
    else {
        var email = emailList.split(',');
        for (var i = 0; i < email.length; i++) {
            if (!emailValidate(email[i]))
                return 2;
        }
    }
    return 0;
};

function emailValidate(email) {

    var reg = /^([a-zA-Z0-9])(([a-zA-Z0-9])*([\._-])?([a-zA-Z0-9]))*@(([a-zA-Z0-9\-])+(\.))+([a-zA-Z]{2,4})+$/;
    if (email.search(reg) == -1)
        return false;
    return true;
};

function replaceSpaceForIE9(text) {
    var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');
    var html = text.replace(expr, '><');
    return html;
}