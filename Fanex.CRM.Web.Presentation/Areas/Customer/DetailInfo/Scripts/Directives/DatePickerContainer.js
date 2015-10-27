(function () {
    "use strict";
    /* global crmConstants,
    currentDate,
    sharedScriptManager */
    /* jshint unused: vars */
    angular.module('customerdetail-application').directive('datePickerContainer', function () {
        var oldValue = null;
        var isOnSelect = false;

        function renderDatePicker(scope, element, attrs) {
            var txtCalendar = element.find(".txtcalendar");
            txtCalendar.datepicker({
                dateFormat: crmConstants.Enums.FormatConstant.M_DD_YY_STRING,
                changeMonth: true,
                changeYear: true,
                minDate: null,
                maxDate: new Date(currentDate),
                showOtherMonths: true,
                selectOtherMonths: true,
                yearRange: sharedScriptManager.cachedData.yearRangeDatePicker,
                onSelect: function (dateText, inst) {
                    isOnSelect = true;
                    $(this).change();
                    isOnSelect = false;
                },
                onClose: function () {
                    $(this).blur();
                }
            });

            if (scope.customerDetail && scope.customerDetail.BirthDayString) {
                txtCalendar.datepicker('setDate', new Date(scope.customerDetail.BirthDayString));
            }
        }
        function selectedDateChange(scope, element, attrs) {
            /* jshint unused: vars */
            var fromDatePicker = element.find(".txtcalendar");
            var selectedDate = fromDatePicker.val();
            if (selectedDate === null ||
            selectedDate === '') {
                return selectedDate;
            }

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

                oldValue = scope.customerDetail && scope.customerDetail.BirthDayString ?
                new Date(scope.customerDetail.BirthDayString) :
                new Date(currentDate);
                element.on('change', function () {
                    var selectedDate = selectedDateChange(scope, element, attrs);
                    if (oldValue === "" ||
                    isNaN(oldValue.valueOf()) ||
                    !sharedScriptManager.datesEqual(selectedDate, new Date(oldValue))) {
                        oldValue = selectedDate;
                        if (!scope.isUpdatingDate) {
                            scope.isUpdatingDate = true;
                            scope.$apply(function () {
                                scope.updateSelectedDate(selectedDate);
                                scope.isUpdatingDate = false;
                            });
                        }

                        if (!isOnSelect) {
                            txtCalendar.datepicker('setDate', selectedDate);
                        }
                    }
                });
            }
        };
    });
})();