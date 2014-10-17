"use strict";
/* global crmConstants,
keyboardManager,
moment */
/* jshint camelcase: false,
unused: vars */
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
        smallPagingDefaultItemsPerPage: 10,

        largePagingOptions: [100, 200, 300, 400, 500],
        largeDefaultItemsPerPage: 100,
        informHeadlinePagingOptions: [10, 15, 20],
        alertSuccessTime: 3000
    };

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
            m.length > 1 && m[1] !== '') {
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
        //currentDate = new Date();
        if (editText === "") {
            editText = "&nbsp;";
        }

        var textTemplate =
            '<div>\
                <div style="background-color: #FFFFD0; padding-left: 10px; border-left: 1px solid #CCCCCC;">\
                    <div style="font-size: 9px;">by <span style="color: #105289; font-weight: bold; font-size: 11px;">{0}</span> on {1}</div>\
                    <div style="border-top: 1px solid #CCCCCC; padding: 5px 0pt 0pt;">{2}</div>\
                 </div>\
              </div>\
              <br>';

        return sharedScriptManager.stringFormat(textTemplate,
            [username, moment(currentDate).format("MM/DD/YYYY, hh:mm:ss A"), editText]);
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
                Name: "Oppss... An error occurred while processing your request.",
                Message: "Oppss... An error occurred while processing your request."
            };
        }

        return ajaxResult;
    };

    self.safeApplyFunction = function ($scope, fn) {
        $scope.safeApplyFunction = function (fn) {
            var phase = this.$root.$$phase;
            if (phase === '$apply' || phase === '$digest') {
                if (fn && (typeof (fn) === 'function')) {
                    fn();
                }
            } else {
                this.$apply(fn);
            }
        };
    };
    self.safeApply = function ($scope) {
        $scope.safeApply = function () {
            if (!$scope.$$phase) {
                $scope.$apply();
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
    self.replaceLineBreak = function (text) {
        return text ? text.replace(/\r/g, '<br />') : '';
    };
    self.encodeHtml = function (text) {
        return String(text)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
    };
    self.alreadyEncodedHtml = function (text) {
        return text.indexOf('&amp;quot;') !== -1 ||
            text.indexOf('&amp;#39;') !== -1 ||
            text.indexOf('&amp;lt;') !== -1 ||
            text.indexOf('&amp;gt;') !== -1;
    };

    self.newFeedbackTime = function (selectedTime) {
        var dateTime = new Date(selectedTime);
        return new Date(dateTime.getFullYear(), dateTime.getMonth(), dateTime.getDate(), dateTime.getHours(), dateTime.getMinutes(), 0);
    };
    self.getDefaultCustomerLangue = function (languageList) {
        var languageLength = languageList.length;
        var defaultId = [crmConstants.Enums.Language.English, crmConstants.Enums.Language.Mandarin, crmConstants.Enums.Language.Cantonese];

        var defaultLanguage = null;
        for (var i = 0; i < languageLength; i++) {
            var langue = languageList[i];
            if ($.inArray(langue.Id, defaultId) &&
                (defaultLanguage == null || defaultLanguage.Id > langue.Id)) {
                defaultLanguage = langue;
            }
        }

        defaultLanguage = defaultLanguage || languageList[0];
        return defaultLanguage;
    };

    self.registerAngularService = function ($http, $q, serviceLink, params) {
        var deferred = $q.defer();
        $http.post(serviceLink, params)
            .success(function (data, status, headers, config) {
                deferred.resolve({
                    Result: data,
                    Status: status,
                    Headers: headers,
                    Config: config
                });
            }).error(function (data, status, headers, config) {
                deferred.reject({
                    Result: data,
                    Status: status,
                    Headers: headers,
                    Config: config
                });
            });

        return deferred.promise;
    };
    self.formatTimeToSendToServer = function (dateTime) {
        return moment(new Date(dateTime)).format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_String_Moment);
    };
    self.forceNumberInput = function (e) {
        var code = "";
        if (e.which) {
            code = e.which;
        }
        else if (e.keyCode) {
            code = e.keyCode;
        }

        if (code === keyboardManager.Enums.KeyCode.End ||
            code === keyboardManager.Enums.KeyCode.Home ||
            code === keyboardManager.Enums.KeyCode.LeftArrow ||
            code === keyboardManager.Enums.KeyCode.RightArrow) {
            return true;
        }

        if ((code < keyboardManager.Enums.KeyCode.Number_0 || keyboardManager.Enums.KeyCode.Number_9 < code) &&
            code !== keyboardManager.Enums.KeyCode.Backspace &&
            code !== keyboardManager.Enums.KeyCode.Enter &&
            code !== keyboardManager.Enums.KeyCode.None &&
            (code !== keyboardManager.Enums.KeyCode.Delete || e.which) &&
            code !== keyboardManager.Enums.KeyCode.Tab) {
            return false;
        }

        return true;
    };

    self.deepCompare = function (x, y) {
        // if both x and y are null or undefined and exactly the same
        if (x === y) {
            return true;
        }

        // if they are not strictly equal, they both need to be Objects
        if (!(x instanceof Object) || !(y instanceof Object)) {
            return false;
        }

        // they must have the exact same prototype chain, the closest we can do is
        // test there constructor.
        if (x.constructor !== y.constructor) {
            return false;
        }

        for (var p in x) {
            // other properties were tested using x.constructor === y.constructor
            if (!x.hasOwnProperty(p)) {
                continue;
            }

            // allows to compare x[ p ] and y[ p ] when set to undefined
            if (!y.hasOwnProperty(p)) {
                return false;
            }

            // if they have the same strict value or identity then they are equal
            if (x[p] === y[p]) {
                continue;
            }

            // Numbers, Strings, Functions, Booleans must be strictly equal
            if (typeof (x[p]) !== "object") {
                return false;
            }

            // Objects and Arrays must be tested recursively
            if (!sharedScriptManager.deepCompare(x[p], y[p])) {
                return false;
            }
        }

        for (p in y) {
            // allows x[ p ] to be set to undefined
            if (y.hasOwnProperty(p) && !x.hasOwnProperty(p)) {
                return false;
            }
        }

        return true;
    };

    self.pad2 = function (number) {
        return (number < 10 ? '0' : '') + number;
    };
};

sharedScriptManager = new SharedScriptManager();
