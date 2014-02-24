"use strict";
/* global currentTime:true, 
insertLogApplication,
sharedScriptManager,
crmConstants,
moment */
insertLogApplication.directive('timePickerEndTimeContainer', function () {
    var isSettingDate = false;
    var oldValue = null;
    function renderDatePicker(scope, element, attrs) {
        /* jshint unused: vars */
        var datePicker = element.find(".timepicker");
        /* jshint camelcase: false */
        datePicker.datetimepicker(
        {
            timeFormat: crmConstants.Enums.FormatConstant.HH_MM_TT_TimePicker,
            stepHour: 1,
            stepMinute: 1,
            stepSecond: 1,
            showSecond: false,
            ampm: true
        });

        var initTime = scope.communicationLog.EndTime ?
            new Date(scope.communicationLog.EndTime) :
            new Date(currentTime);
        datePicker.datepicker('setDate', initTime);
    }

    function updateCurrentTime() {
        var clientTime = new Date();
        if (clientTime > currentTime) {
            currentTime = clientTime;
        }
    }
    function setTimeWithDate(dateInfo, datePicker) {
        /* jshint unused: vars */
        var arrayDate = dateInfo.split("/");
        var isValidDate = sharedScriptManager.isValidDate(new Date(dateInfo));
        if (arrayDate.length < 3 || !isValidDate) {
            return new Date(currentTime);
        }

        /* jshint camelcase: false */
        var newDate = new Date(sharedScriptManager.stringFormat(
            crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_StringFormat,
            [arrayDate[2], arrayDate[0], arrayDate[1], 0, 0, crmConstants.Enums.DateTime.AM]));
        return newDate;
    }
    function setTimeWithDateAndTime(dateInfo, timeInfo, amPM, datePicker)
    /* jshint unused: vars */{
        var arrayDate = dateInfo.split("/");
        var isValidDate = sharedScriptManager.isValidDate(new Date(dateInfo));
        if (arrayDate.length < 3 || !isValidDate) {
            return new Date(currentTime);
        }

        var arrayTime = timeInfo.split(":");
        if (arrayDate.length < 2) {
            arrayTime = [0, 0];
        }

        var hour = parseInt(arrayTime[0]);
        var minute = parseInt(arrayTime[1]);
        amPM = amPM &&
            (amPM === crmConstants.Enums.DateTime.AM || amPM === crmConstants.Enums.DateTime.PM) ?
            amPM :
            crmConstants.Enums.DateTime.AM;
        if (hour >= 12) {
            crmConstants.Enums.DateTime.PM;
        }

        /* jshint camelcase: false */
        var newDate = new Date(sharedScriptManager.stringFormat(
            crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_StringFormat,
            [arrayDate[2], arrayDate[0], arrayDate[1], hour, minute, amPM]));
        return newDate;
    }
    function selectedTimeChange(scope, element, attrs)
    /* jshint unused: vars */{
        updateCurrentTime();
        var datePicker = element.find(".timepicker");
        var selectedTime = datePicker.val();
        var arrayTime = selectedTime.split(' ');
        selectedTime = new Date(currentTime);
        switch (arrayTime.length) {
            case 1:
                selectedTime = setTimeWithDate(arrayTime[0], datePicker);
                break;
            case 2:
                selectedTime = setTimeWithDateAndTime(arrayTime[0], arrayTime[1], crmConstants.Enums.DateTime.AM, datePicker);
                break;
            case 3:
                if (!isSettingDate) {
                    isSettingDate = true;
                    selectedTime = setTimeWithDateAndTime(arrayTime[0], arrayTime[1], arrayTime[2], datePicker);
                    isSettingDate = false;
                }

                break;
            default:
                selectedTime = new Date(currentTime);
                break;
        }

        return selectedTime;
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            var datePicker = element.find(".timepicker");
            renderDatePicker(scope, element, attrs);
            scope.$watch(attrs.ngModel, function (newval, oldval) {
                /* jshint unused: vars */
                if (!sharedScriptManager.datesEqual(new Date(oldValue), new Date(newval))) {
                    oldValue = new Date(newval);
                    scope.isUpdatingTime = true;
                    /* jshint camelcase: false */
                    datePicker.val(moment(new Date(newval)).format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_String_Moment));
                    scope.isUpdatingTime = false;
                }
            });

            var initEndTime = new Date(currentTime);
            initEndTime.setMinutes(new Date(currentTime).getMinutes() + 1);
            scope.updateEndTime(scope.communicationLog.EndTime ?
                new Date(scope.communicationLog.EndTime) :
                initEndTime);
            oldValue = new Date(currentTime);
            element.on('change', function () {
                var selectedTime = selectedTimeChange(scope, element, attrs);
                if (!sharedScriptManager.datesEqual(selectedTime, new Date(oldValue))) {
                    oldValue = selectedTime;
                    if (!scope.isUpdatingTime) {
                        scope.isUpdatingTime = true;
                        scope.$apply(function () {
                            scope.updateEndTime(selectedTime);
                            scope.isUpdatingTime = false;
                        });
                    }

                    datePicker.datepicker('setDate', selectedTime);
                } else {
                    scope.$apply(function () {
                        scope.updateRemarkPrefix();
                    });
                }
            });
        }
    };
});