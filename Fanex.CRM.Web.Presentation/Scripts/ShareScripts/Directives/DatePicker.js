"use strict";
/* global DialogManager,
crmConstants,
sharedScriptManager */
/* jshint unused: vars */
var directiveManager = directiveManager || {};
directiveManager.registerDatePickerContainer = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        var defaultRegisterParams = {
            PlugInName: null,
            OptionName: null,
            PickerId: null
        };

        var defaultOption = {
            MinDate: null,
            MaxDate: null,
            InitDate: null,
            IsRerenderPicker: null,
            OnChangeDateEvent: null
        };

        var oldValue = null;
        var isRenderedCalendar = false;
        var isOnSelect = false;

        function renderDatePicker(scope, element, attrs) {
            var options = scope[registerParams.OptionName];
            if (options === null) {
                return;
            }

            var datePicker = element.find("#" + registerParams.PickerId);
            datePicker.datepicker({
                dateFormat: crmConstants.Enums.FormatConstant.M_DD_YY_STRING,
                changeMonth: true,
                changeYear: true,
                minDate: options.MinDate ? new Date(options.MinDate) : null,
                maxDate: options.MaxDate ? new Date(options.MaxDate) : null,
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

            datePicker.datepicker("setDate", options.InitDate ? new Date(options.InitDate) : null);
            oldValue = new Date(options.InitDate);
            element.on('change', function () {
                onChangeDateEvent(scope, element, attrs);
            });

            isRenderedCalendar = true;
        }

        function onChangeDateEvent(scope, element, attrs) {
            var options = scope[registerParams.OptionName];
            var selectedDate = selectedDateChange(scope, element, attrs);
            if (isNaN(oldValue.valueOf()) ||
                !sharedScriptManager.datesEqual(selectedDate, new Date(oldValue))) {
                oldValue = selectedDate;
                if (!scope.isUpdatingDate) {
                    scope.IsUpdatingDate = true;
                    scope.$apply(function () {
                        if (options.OnChangeDateEvent &&
                            typeof (options.OnChangeDateEvent) === "function") {
                            options.OnChangeDateEvent(selectedDate);
                        }
                    });

                    scope.IsUpdatingDate = false;
                }

                if (!isOnSelect) {
                    datePicker.datepicker('setDate', selectedDate);
                }
            }
        }

        function setLimitDate(scope, selectedDate, options, datePicker) {
            if (options.MinDate) {
                selectedDate = options.MinDate;
                datePicker.datepicker("option", "minDate", new Date(options.MinDate));
            }

            if (options.MaxDate) {
                selectedDate = options.MaxDate;
                datePicker.datepicker("option", "maxDate", new Date(options.MaxDate));
            }
        }

        function selectedDateChange(scope, element, attrs) {
            var datePicker = element.find("#" + registerParams.PickerId);
            var selectedDate = datePicker.val();
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
            var datePicker = element.find("#" + registerParams.PickerId);
            if (newVal && !oldVal) {
                if (!isRenderedCalendar) {
                    renderDatePicker(scope, element, attrs);
                } else {
                    setLimitDate(scope, oldValue, options, datePicker);
                }

                options.IsRerenderPicker = false;
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

