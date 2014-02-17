"use strict";
/* global customerDetailApplication,
crmConstants,
currentDate,
sharedScriptManager */
customerDetailApplication.directive('datePickerContainer', function () {
    var oldValue = null;
    function renderDatePicker(scope, element, attrs) {
        /* jshint unused: vars */
        var txtCalendar = element.find(".txtcalendar");
        txtCalendar.datepicker({
            dateFormat: crmConstants.Enums.FormatConstant.M_DD_YY_STRING,
            changeMonth: true,
            changeYear: true,
            minDate: null,
            maxDate: new Date(currentDate),
            showOtherMonths: true,
            selectOtherMonths: true
        });

        txtCalendar.datepicker('setDate', new Date(scope.customerDetail.BirthDay));
    }
    function selectedDateChange(scope, element, attrs) {
        /* jshint unused: vars */
        var fromDatePicker = element.find(".txtcalendar");
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
            var txtCalendar = element.find(".txtcalendar");
            renderDatePicker(scope, element, attrs);

            oldValue = new Date(scope.customerDetail.BirthDay);
            element.on('change', function () {
                var selectedDate = selectedDateChange(scope, element, attrs);
                if (isNaN(oldValue.valueOf()) ||
                    !sharedScriptManager.datesEqual(selectedDate, new Date(oldValue))) {
                    oldValue = selectedDate;
                    if (!scope.isUpdatingDate) {
                        scope.isUpdatingDate = true;
                        scope.$apply(function () {
                            scope.updateSelectedDate(selectedDate);
                            scope.isUpdatingDate = false;
                        });
                    }

                    txtCalendar.datepicker('setDate', selectedDate);
                }
            });
        }
    };
});