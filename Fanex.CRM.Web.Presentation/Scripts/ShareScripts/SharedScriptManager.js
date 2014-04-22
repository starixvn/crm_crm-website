"use strict";
/* global crmConstants */
/* jshint unused: vars */
var sharedScriptManager;
var SharedScriptManager = function () {
    var self = this;
    self.cachedData = {
        pagingOptions: [50, 100, 200],
        defaultItemsPerPage: 50,
        maxFeedbackSubjectTemplateNumberOnLoad: 100000,
        yearRangeDatePicker: "-100:+0",
        numberOfYearPerSelection: 10,
        viewChartIcon: '<input id="button-viewchart" class="btnCharting" type="button" value="">',

        monthPagingOptions: [3, 6, 9, 12],
        monthPagingDefaultItemsPerPage: 12,
        smallPagingOptions: [1, 5, 7, 10],
        smallPagingDefaultItemsPerPage: 10
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
                if (successFunction && typeof (successFunction) === 'function') {
                    successFunction.call(currentHandler, rs);
                }
            },
            error: function (jqXHR, event, ajaxSettings) {
                if (errorFunction && typeof (errorFunction) === 'function') {
                    errorFunction.call(currentHandler);
                }
            },
            complete: function () {
                if (completeFunction && typeof (completeFunction) === 'function') {
                    completeFunction();
                }
            }
        });

    };

    self.urlDecode = function (s) {
        var o = s;
        var t;
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
            while (strValue.indexOf(formatIndex) !== -1) {
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
        } ]);
    };
    self.setCommonHeaderHttpRequest = function (application) {
        application.config(['$httpProvider', function ($httpProvider) {
            $httpProvider.defaults.headers.common["FROM-ANGULAR"] = "true";
            $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        } ]);
    };

    self.insertEditDescriptionBlock = function (username, editText, currentDate) {
        currentDate = new Date();
        if (editText === "") {
            editText = "&nbsp;";
        }

        var textTemplate =
            '<div>\
                <div style="background-color: #FFFFD0; padding-left: 10px; border-left: 1px solid #CCCCCC;">\
                    <div style="font-size: 9px;">by <span style="color: #105289; font-weight: bold; font-size: 11px;">{0}</span> on {1}</div>\
                    <div style="border-top: 1px solid #CCCCCC; padding: 5px 0pt 0pt;">{2}</div>\
                 </div>\
              </div>';

        return sharedScriptManager.stringFormat(textTemplate,
            [username, currentDate.toLocaleString(), editText]);
    };

    self.getIeVersion = function () {
        var version = -1; // Return value assumes failure.
        if (window.navigator.userAgent.indexOf("MSIE") > 0) {
            var usrAgent = navigator.userAgent;
            var reg = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
            if (reg.exec(usrAgent) != null) {
                version = parseFloat(RegExp.$1);
            }
        }

        return version;
    };

    self.formatAjaxResult = function (ajaxResult) {
        if ((ajaxResult.Id === undefined ||
            ajaxResult.Id === null) &&
            (ajaxResult.Code === undefined ||
            ajaxResult.Code === null)) {
            ajaxResult = {
                Id: crmConstants.Enums.Error.Forbidden,
                Code: crmConstants.Enums.Error.Forbidden,
                Name: "You don't have permission on this function!",
                Message: "You don't have permission on this function!"
            };
        }

        return ajaxResult;
    };

    self.safeApply = function ($scope, fn) {
        $scope.safeApply = function (fn) {
            var phase = this.$root.$$phase;
            if (phase == '$apply' || phase == '$digest') {
                if (fn && (typeof (fn) === 'function')) {
                    fn();
                }
            } else {
                this.$apply(fn);
            }
        };
    };
    self.handleErrorMessage = function (ajaxResult, $resultMessageService) {
        var isSuccess = ajaxResult.error === 0;
        $resultMessageService.showMessage({
            messageType: isSuccess ?
                crmConstants.Enums.MessageType.Success :
                crmConstants.Enums.MessageType.Error,
            message: ajaxResult.message
        });
    };
    self.getEndDateOfMonth = function (dateInMonth) {
        return new Date(dateInMonth.getFullYear(), dateInMonth.getMonth() + 1, 0);
    };
    self.getStartDateOfMonth = function (dateInMonth) {
        return new Date(dateInMonth.getFullYear(), dateInMonth.getMonth(), 1);
    };

    self.getEndDateOfYear = function (year) {
        return new Date(year, 12, 0);
    };
    self.getStartDateOfYear = function (year) {
        return new Date(year, 0, 1);
    };
};

sharedScriptManager = new SharedScriptManager();
