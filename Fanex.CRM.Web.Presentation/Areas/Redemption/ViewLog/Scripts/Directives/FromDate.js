"use strict";
/* global redemptionViewLogApplication,
crmConstants,
currentDate,
sharedScriptManager,
initFromDate */
redemptionViewLogApplication.directive('datePickerFromDateContainer', function () {
    var oldValue = null;
    var oldMaxValue = null;
    function renderDatePicker(scope, element, attrs) {
        /* jshint unused: vars */
        var fromDatePicker = element.find("#fromDate");
        fromDatePicker.datepicker({
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

        fromDatePicker.datepicker('setDate', new Date(initFromDate));
    }
    function selectedDateChange(scope, element, attrs) {
        /* jshint unused: vars */
        var fromDatePicker = element.find("#fromDate");
        var selectedDate = fromDatePicker.val();
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
            var fromDatePicker = element.find("#fromDate");
            renderDatePicker(scope, element, attrs);
            scope.$watch(attrs.ngModel, function (newval, oldval) {
                /* jshint unused: vars */
                oldMaxValue = new Date(newval);
                fromDatePicker.datepicker("option", "maxDate", new Date(oldMaxValue));
            });

            oldValue = new Date(initFromDate);
            element.on('change', function () {
                var selectedDate = selectedDateChange(scope, element, attrs);
                if (isNaN(oldValue.valueOf()) ||
                    !sharedScriptManager.datesEqual(selectedDate, new Date(oldValue))) {
                    oldValue = selectedDate;
                    if (!scope.isUpdatingDate) {
                        scope.isUpdatingDate = true;
                        scope.$apply(function () {
                            scope.updateFromDate(selectedDate);
                            scope.isUpdatingDate = false;
                        });
                    }

                    fromDatePicker.datepicker('setDate', selectedDate);
                }
            });
        }
    };
});