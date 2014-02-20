"use strict";

var SharedScriptManager = function () {
    var self = this;
    self.cachedData = {
        pagingOptions: [50, 100, 200],
        defaultItemsPerPage: 50,
        maxFeedbackSubjectTemplateNumberOnLoad: 100000
    },

    self.CallAjax = function (currentHandler,
        url,
        requestParams,
        successFunction,
        errorFunction,
        completeFunction) {
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: requestParams ? JSON.stringify(requestParams) : null,
            beforeSend: function () {
            },
            success: function (rs) {
                if (successFunction && typeof (successFunction) == 'function') {
                    successFunction.call(currentHandler, rs);
                }
            },
            error: function (jqXHR, event, ajaxSettings) {
                if (errorFunction && typeof (errorFunction) == 'function') {
                    errorFunction.call(currentHandler);
                }
            },
            complete: function () {
                if (completeFunction && typeof (completeFunction) == 'function') {
                    completeFunction();
                }
            }
        });

    };

    self.urlDecode = function (s) {
        var o = s;
        var binVal, t;
        var r = /(%[^%]{2})/;
        var m;
        while ((m = r.exec(o)) != null &&
            m.length > 1 && m[1] != '') {
            var b = parseInt(m[1].substr(1), 16);
            t = String.fromCharCode(b);
            o = o.replace(m[1], t);
        }

        return o;
    };
    self.getURLParam = function (paramName) {
        paramName = paramName.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + paramName + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.href);
        if (results == null) {
            return "";
        }
        else {
            return results[1];
        }
    };
    self.stringFormat = function (strValue, paramArr) {
        var len = paramArr.length;
        for (var i = 0; i < len; i++) {
            var paramInfo = paramArr[i];
            var formatIndex = '{' + i + '}';
            while (strValue.indexOf(formatIndex) != -1) {
                strValue = strValue.replace(formatIndex, paramInfo);
            }
        }

        return strValue;
    };
    self.isValidDate = function (date) {
        if (Object.prototype.toString.call(date) !== "[object Date]") {
            return false;
        }

        return !isNaN(date.getTime());
    };
    self.datesEqual = function (dateA, dateB) {
        return !(dateA > dateB || dateB > dateA);
    };

    self.setNoCachingHttpRequest = function (application) {
        application.config(['$httpProvider', function ($httpProvider) {
            //initialize get if not there
            if (!$httpProvider.defaults.headers.get) {
                $httpProvider.defaults.headers.get = {};
            }

            //disable IE ajax request caching
            $httpProvider.defaults.headers.get['If-Modified-Since'] = '0';
        }]);
    };
};

var sharedScriptManager = new SharedScriptManager();
