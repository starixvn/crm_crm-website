(function () {
    "use strict";
    /* global
    crmConstants,
    currentDate,
    sharedScriptManager */
    angular.module('activitylog-application').directive('datePickerToDateContainer', function () {
        var oldValue = null;
        var oldMinValue = null;
        var isOnSelect = false;
        function renderDatePicker(scope, element) {
            var toDatePicker = element.find("#toDate");
            toDatePicker.datepicker({
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

            toDatePicker.datepicker('setDate', new Date(currentDate));
            toDatePicker.datepicker("option", "minDate", new Date(currentDate));
        }

        function selectedDateChange(scope, element) {
            var toDatePicker = element.find("#toDate");
            var selectedDate = toDatePicker.val();
            var date = new Date(selectedDate);
            if (!isNaN(date.valueOf())) {
                selectedDate = date;
            } else {
                selectedDate = oldValue;
                oldValue = date;
            }

            if (selectedDate < scope.searchCriteria.MinToDate) {
                selectedDate = scope.searchCriteria.MinToDate;
            }

            return selectedDate;
        }

        function datePickerOnChange(scope, element) {
            var toDatePicker = element.find("#toDate");
            var selectedDate = selectedDateChange(scope, element);
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

                if (!isOnSelect) {
                    toDatePicker.datepicker('setDate', selectedDate);
                }
            }
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                var toDatePicker = element.find("#toDate");
                renderDatePicker(scope, element);
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    /* jshint unused: false */
                    oldMinValue = new Date(newval);
                    toDatePicker.datepicker("option", "minDate", new Date(oldMinValue));
                });

                oldValue = new Date(currentDate);
                element.on('change', function () {
                    datePickerOnChange(scope, element);
                });
            }
        };
    });
})();