"use strict";
/* global settingLogApplication,
crmConstants,
currentDate,
sharedScriptManager */
settingLogApplication.directive('datePickerToDateContainer', function () {
    var oldValue = null;
    var oldMinValue = null;
    function renderDatePicker(scope, element, attrs) {
        /* jshint unused: vars */
        var toDatePicker = element.find("#toDate");
        toDatePicker.datepicker({
            dateFormat: crmConstants.Enums.FormatConstant.M_DD_YY_STRING,
            changeMonth: true,
            changeYear: true,
            minDate: null,
            maxDate: new Date(currentDate),
            showOtherMonths: true,
            selectOtherMonths: true,
            onClose: function () {
                $(this).blur();
            }
        });

        toDatePicker.datepicker('setDate', new Date(currentDate));
        toDatePicker.datepicker("option", "minDate", new Date(currentDate));
    }
    function selectedDateChange(scope, element, attrs) {
        /* jshint unused: vars */
        var toDatePicker = element.find("#toDate");
        var selectedDate = toDatePicker.val();
        var date = new Date(selectedDate);
        if (!isNaN(date.valueOf())) {
            selectedDate = date;
        } else {
            selectedDate = oldValue;
            oldValue = date;
        }

        return selectedDate;
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            var toDatePicker = element.find("#toDate");
            renderDatePicker(scope, element, attrs);
            scope.$watch(attrs.ngModel, function (newval, oldval) {
                /* jshint unused: vars */
                oldMinValue = new Date(newval);
                toDatePicker.datepicker("option", "minDate", new Date(oldMinValue));
            });

            oldValue = new Date(currentDate);
            element.on('change', function () {
                var selectedDate = selectedDateChange(scope, element, attrs);
                if (isNaN(oldValue.valueOf()) ||
                    !sharedScriptManager.datesEqual(selectedDate, new Date(oldValue))) {
                    oldValue = selectedDate;
                    if (!scope.isUpdatingDate) {
                        scope.isUpdatingDate = true;
                        scope.$apply(function () {
                            scope.updateToDate(selectedDate);
                            scope.isUpdatingDate = false;
                        });
                    }

                    toDatePicker.datepicker('setDate', selectedDate);
                }
            });
        }
    };
});