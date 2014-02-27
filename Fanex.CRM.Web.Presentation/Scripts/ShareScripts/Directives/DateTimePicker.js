"use strict";
/* global directiveManager */
/* jshint unused: vars */
/* jshint camelcase: false */
directiveManager = directiveManager || {};
directiveManager.registerDateTimePicker = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        //        var defaultRegisterParams = {
        //            PlugInName: null,
        //            OptionName: null,
        //            PickerId: null
        //        };

        var isSettingDate = false;
        var oldValue = null;
        var selfOptions = null;
        var isRenderedPicker = false;

        function renderDatePicker(scope, element, attrs) {
            var datePicker = element.find("#" + registerParams.PickerId);
            datePicker.datetimepicker(
            {
                timeFormat: crmConstants.Enums.FormatConstant.HH_MM_TT_TimePicker,
                stepHour: 1,
                stepMinute: 1,
                stepSecond: 1,
                showSecond: false,
                ampm: true
            });

            var initTime = selfOptions.SelectedTime ?
                new Date(selfOptions.SelectedTime) :
                new Date(selfOptions.CurrentTime);
            datePicker.datepicker('setDate', initTime);
        }
        function updateCurrentTime() {
            var clientTime = new Date();
            if (clientTime > selfOptions.CurrentTime) {
                selfOptions.CurrentTime = clientTime;
            }
        }
        function setTimeWithDate(dateInfo, datePicker) {
            var arrayDate = dateInfo.split("/");
            var isValidDate = sharedScriptManager.isValidDate(new Date(dateInfo));
            if (arrayDate.length < 3 || !isValidDate) {
                return new Date(selfOptions.CurrentTime);
            }

            var newDate = new Date(sharedScriptManager.stringFormat(
                crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_StringFormat,
                [arrayDate[2], arrayDate[0], arrayDate[1], 0, 0, crmConstants.Enums.DateTime.AM]));
            return newDate;
        }
        function setTimeWithDateAndTime(dateInfo, timeInfo, amPM, datePicker) {
            var arrayDate = dateInfo.split("/");
            var isValidDate = sharedScriptManager.isValidDate(new Date(dateInfo));
            if (arrayDate.length < 3 || !isValidDate) {
                return new Date(selfOptions.CurrentTime);
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

            var newDate = new Date(sharedScriptManager.stringFormat(
                crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_StringFormat,
                [arrayDate[2], arrayDate[0], arrayDate[1], hour, minute, amPM]));
            return newDate;
        }
        function selectedTimeChange(scope, element, attrs) {
            updateCurrentTime();
            var datePicker = element.find("#" + registerParams.PickerId);
            var selectedTime = datePicker.val();
            var arrayTime = selectedTime.split(' ');
            selectedTime = new Date(selfOptions.CurrentTime);
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
                    selectedTime = new Date(selfOptions.CurrentTime);
                    break;
            }

            return selectedTime;
        }
        function onChangeTimeEvent(scope, element, attrs) {
            var datePicker = element.find("#" + registerParams.PickerId);
            var selectedTime = selectedTimeChange(scope, element, attrs);
            if (!sharedScriptManager.datesEqual(selectedTime, new Date(oldValue))) {
                oldValue = selectedTime;
                if (!scope.IsUpdatingTime) {
                    scope.IsUpdatingTime = true;
                    if (selfOptions.OnChangeEvent &&
                        typeof (selfOptions.OnChangeEvent) === "function") {
                        selfOptions.OnChangeEvent(selectedTime);
                    }

                    scope.IsUpdatingTime = false;
                }

                datePicker.datepicker('setDate', selectedTime);
            }
        }
        function getInitEndTime() {
            var initEndTime = new Date(selfOptions.CurrentTime);
            initEndTime.setMinutes(new Date(selfOptions.CurrentTime).getMinutes() + 1);
            return initEndTime;
        }

        function initPicker(scope, element, attrs) {
            renderDatePicker(scope, element, attrs);
            oldValue = selfOptions.SelectedTime ?
                selfOptions.SelectedTime :
                selfOptions.CurrentTime;
            if (registerParams.PickerType === crmConstants.Enums.CalendarPickerType.ToDate) {
                oldValue = getInitEndTime();
            }

            if (selfOptions.OnChangeEvent &&
                typeof (selfOptions.OnChangeEvent) === "function") {
                selfOptions.OnChangeEvent(oldValue);
            }
        }

        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                var datePicker = element.find("#" + registerParams.PickerId);
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    if (!isRenderedPicker) {
                        selfOptions = scope[registerParams.OptionName];
                        if (selfOptions === null) {
                            return;
                        }

                        initPicker(scope, element, attrs);
                    }

                    if (!sharedScriptManager.datesEqual(new Date(oldValue), new Date(newval))) {
                        oldValue = new Date(newval);
                        scope.isUpdatingTime = true;
                        datePicker.val(moment(new Date(newval)).format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_String_Moment));
                        scope.IsUpdatingTime = false;
                    }
                });

                element.on('change', function () {
                    onChangeTimeEvent(scope, element, attrs);
                });
            }
        };
    });
};