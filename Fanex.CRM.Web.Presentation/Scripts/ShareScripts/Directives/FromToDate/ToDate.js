"use strict";
/* global DialogManager,
crmConstants,
sharedScriptManager */
var directiveManager = directiveManager || {};
directiveManager.registerFromDateCalendar = function (angularApplication) {
    angularApplication.directive('datePickerFromDateContainer', function () {
        var oldValue = null;
        var oldMaxValue = null;
        var isRenderedCalendar = false;
        function renderDatePicker(scope, element, attrs) {
            if (scope.fromDateOptions === null) {
                return;
            }

            /* jshint unused: vars */
            var fromDatePicker = element.find("#fromDate");
            fromDatePicker.datepicker({
                dateFormat: crmConstants.Enums.FormatConstant.M_DD_YY_STRING,
                changeMonth: true,
                changeYear: true,
                minDate: null,
                maxDate: new Date(scope.fromDateOptions.maxFromDate),
                showOtherMonths: true,
                selectOtherMonths: true,
                onClose: function () {
                    $(this).blur();
                }
            });

            fromDatePicker.datepicker('setDate', new Date(scope.fromDateOptions.initFromDate));
            oldValue = new Date(scope.fromDateOptions.initFromDate);
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
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    /* jshint unused: vars */
                    if (newval && !oldval) {
                        if (!isRenderedCalendar) {
                            renderDatePicker(scope, element, attrs);
                        } else {
                            oldMaxValue = new Date(newval);
                            fromDatePicker.datepicker("option", "maxDate", new Date(scope.fromDateOptions.maxFromDate));
                        }

                        scope.isupdateFromDatePicker = false;
                    }
                });
            }
        };
    });
};

