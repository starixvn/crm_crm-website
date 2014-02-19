"use strict";
/* global settingLogApplication,
crmConstants,
currentDate,
sharedScriptManager,
initFromDate */
settingLogApplication.directive('datePickerFromDateContainer', function () {
    var oldValue = null;
    var oldMaxValue = null;
    function renderDatePicker(scope, element, attrs) {
        /* jshint unused: vars */
        var fromDatePicker = element.find('#' + attrs.datePickerFromDateContainer);
        fromDatePicker.datepicker({
            dateFormat: attrs.dateformat,
            changeMonth: true,
            changeYear: true,
            minDate: attrs.mindate ? new Date(attrs.mindate) : null,
            maxDate: new Date(attrs.maxdate),
            showOtherMonths: true,
            selectOtherMonths: true
        });

        fromDatePicker.datepicker('setDate', new Date(attrs.initvalue));
        return fromDatePicker;
    };

    function selectedDateChange(datepicker) {
        /* jshint unused: vars */
        //var fromDatePicker = element.find('#' + attrs.datePickerFromDateContainer);
        var selectedDate = datepicker.val();
        var date = new Date(selectedDate);
        if (!isNaN(date.valueOf())) {
            selectedDate = date;
        } else {
            selectedDate = oldValue;
            oldValue = date;
        }

        return selectedDate;
    };

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            var fromDatePicker = renderDatePicker(scope, element, attrs);
            scope.$watch(attrs.ngModel, function (newvalue, oldvalue) {
                /* jshint unused: vars */
                if (oldMaxValue === null ||
                    new Date(newvalue) < oldMaxValue) {
                    oldMaxValue = new Date(newvalue);
                    fromDatePicker.datepicker('option', 'maxDate', new Date(oldMaxValue));
                }
            });

            oldValue = new Date(attrs.initvalue);
            element.on('change', function () {
                var selectedDate = selectedDateChange(fromDatePicker);
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