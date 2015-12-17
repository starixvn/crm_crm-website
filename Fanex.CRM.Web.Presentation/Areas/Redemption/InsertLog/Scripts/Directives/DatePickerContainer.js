(function () {
    "use strict";
    /* global crmConstants,
    currentDate,
    sharedScriptManager */
    angular.module('redemption-insertlog-application').directive('datePickerContainer', function () {
        var oldValue = null;
        function renderDatePicker(scope, element) {          
            var txtCalendar = element.find(".txtcalendar");
            txtCalendar.datepicker({
                dateFormat: crmConstants.Enums.FormatConstant.M_DD_YY_STRING,
                changeMonth: true,
                changeYear: true,
                minDate: null,
                maxDate: null,
                showOtherMonths: true,
                selectOtherMonths: true,
                onClose: function () {
                    $(this).blur();
                }
            });

            txtCalendar.datepicker('setDate', new Date(currentDate));
        }
        function selectedDateChange(scope, element) {           
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
            link: function (scope, element) {
                var txtCalendar = element.find(".txtcalendar");
                renderDatePicker(scope, element);

                oldValue = new Date(currentDate);
                element.on('change', function () {
                    var selectedDate = selectedDateChange(scope, element);
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
})();