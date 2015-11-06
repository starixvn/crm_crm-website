(function () {
    "use strict";
    /* global crmConstants,
    currentDate,
    sharedScriptManager,
    initFromDate */
    angular.module('communication-application').directive('datePickerFromDateContainer', function () {
        var oldValue = null;
        var oldMaxValue = null;
        var isOnSelect = false;
        function renderDatePicker(scope, element) {
            var fromDatePicker = element.find("#fromDate");
            fromDatePicker.datepicker({
                dateFormat: crmConstants.Enums.FormatConstant.M_DD_YY_STRING,
                changeMonth: true,
                changeYear: true,
                minDate: null,
                maxDate: new Date(currentDate),
                showOtherMonths: true,
                selectOtherMonths: true,
                onSelect: function () {
                    isOnSelect = true;
                    $(this).change();
                    isOnSelect = false;
                },
                onClose: function () {
                    $(this).blur();
                }
            });

            fromDatePicker.datepicker('setDate', new Date(initFromDate));
        }
        function selectedDateChange(scope, element) {
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

            if (selectedDate > scope.searchCriteria.MaxFromDate) {
                selectedDate = scope.searchCriteria.MaxFromDate;
            }

            return selectedDate;
        }

        function datePickerOnChange(scope, element) {
            var fromDatePicker = element.find("#fromDate");
            var selectedDate = selectedDateChange(scope, element);
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

                if (!isOnSelect) {
                    fromDatePicker.datepicker('setDate', selectedDate);
                }
            }
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                var fromDatePicker = element.find("#fromDate");
                renderDatePicker(scope, element);
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    /* jshint unused: false */
                    oldMaxValue = new Date(newval);
                    fromDatePicker.datepicker("option", "maxDate", new Date(oldMaxValue));
                });

                oldValue = new Date(initFromDate);
                element.on('change', function () {
                    datePickerOnChange(scope, element);
                });
            }
        };
    });
})();