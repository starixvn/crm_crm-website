"use strict";
/* global directiveManager: true,
crmConstants,
sharedScriptManager */
/* jshint unused: false */
directiveManager = directiveManager || {};
directiveManager.registerDatePickerContainer = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        var oldValue = null;
        var isRenderedCalendar = false;
        var isOnSelect = false;

        function renderDatePicker(scope, element, attrs) {
            var options = scope[registerParams.OptionName];
            if (!options) {
                return;
            }

            var picker = element.find("#" + registerParams.PickerId);
            picker.datepicker({
                dateFormat: crmConstants.Enums.FormatConstant.M_DD_YY_STRING,
                changeMonth: true,
                changeYear: true,
                minDate: options.MinDate ? new Date(options.MinDate) : null,
                maxDate: options.MaxDate ? new Date(options.MaxDate) : null,
                showOtherMonths: true,
                selectOtherMonths: true,
                yearRange: options.YearRange,
                onSelect: function () {
                    isOnSelect = true;
                    $(this).change();
                    isOnSelect = false;
                },
                onClose: function () {
                    $(this).blur();
                }
            });

            picker.datepicker("setDate", options.InitDate ? new Date(options.InitDate) : null);
            oldValue = options.InitDate ? new Date(options.InitDate) : null;
            element.on('change', function () {
                onChangeDateEvent(scope, element, attrs);
            });

            isRenderedCalendar = true;
        }

        function onChangeDateEvent(scope, element, attrs, isAlreadyApply) {
            var picker = element.find("#" + registerParams.PickerId);
            var options = scope[registerParams.OptionName];
            if (options == null) {
                return;
            }

            var selectedDate = selectedDateChange(scope, element, attrs, options);
            if (oldValue === null || oldValue === "" ||
                isNaN(oldValue.valueOf()) ||
                !sharedScriptManager.datesEqual(selectedDate, new Date(oldValue))) {
                oldValue = selectedDate;
                if (!scope.isUpdatingDate) {
                    scope.IsUpdatingDate = true;
                    if (options.OnChangeDateEvent &&
                        typeof (options.OnChangeDateEvent) === "function") {
                        options.OnChangeDateEvent(selectedDate);
                    }

                    if (!isAlreadyApply) {
                        scope.$apply();
                    }

                    scope.IsUpdatingDate = false;
                }

                if (!isOnSelect) {
                    picker.datepicker('setDate', selectedDate);
                }
            }
        }

        function setLimitDate(scope, selectedDate, options, picker) {
            if (options.IsAllowNullDate === true && options.MinDate === null) {
                picker.datepicker("option", "minDate", null);
            } else if (options.MinDate) {
                picker.datepicker("option", "minDate", new Date(options.MinDate));
            }

            if (options.IsAllowNullDate === true && options.MaxDate === null) {
                picker.datepicker("option", "maxDate", null);
            } else if (options.MaxDate) {
                picker.datepicker("option", "maxDate", new Date(options.MaxDate));
            }

            if (options.IsSetDate &&
                options.InitDate !== undefined &&
                options.InitDate !== null) {
                picker.datepicker("setDate", new Date(options.InitDate));
                options.IsSetDate = false;
            }
        }

        function selectedDateChange(scope, element, attrs, options) {
            var picker = element.find("#" + registerParams.PickerId);
            var selectedDate = picker.val();
            if (options.IsAllowNullDate === true &&
                (selectedDate === null || selectedDate === '')) {
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

        function ngModelChange(scope, element, attrs, newVal, oldVal) {
            var options = scope[registerParams.OptionName];
            var picker = element.find("#" + registerParams.PickerId);
            if (newVal && !oldVal) {
                if (!isRenderedCalendar) {
                    renderDatePicker(scope, element, attrs);
                } else {
                    setLimitDate(scope, oldValue, options, picker);
                }

                updateInitValue(scope, element, attrs);
                if (options.CompleteRender &&
                    typeof (options.CompleteRender) === "function") {
                    options.CompleteRender();
                } else {
                    options.IsRerenderPicker = false;
                }
            }
        }

        function updateInitValue(scope, element, attrs) {
            var options = scope[registerParams.OptionName];
            if (options == null) {
                return;
            }

            var selectedDate = selectedDateChange(scope, element, attrs, options);
            if (!scope.isUpdatingDate) {
                scope.IsUpdatingDate = true;
                oldValue = selectedDate;
                if (options.OnChangeDateEvent &&
                    typeof (options.OnChangeDateEvent) === "function") {
                    options.OnChangeDateEvent(selectedDate, true);
                }

                scope.IsUpdatingDate = false;
            }
        }

        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newVal, oldVal) {
                    ngModelChange(scope, element, attrs, newVal, oldVal);
                });
            }
        };
    });
};

