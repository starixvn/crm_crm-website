"use strict";
/* global DialogManager,
crmConstants,
sharedScriptManager */
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
        var oldMaxValue = null;
        var isRenderedCalendar = false;
        var isOnSelect = false;

        function renderDatePicker(scope, element, attrs) {
            var options = scope[registerParams.OptionName];
            if (options === null) {
                return;
            }

            /* jshint unused: vars */
            var datePicker = element.find(registerParams.PickerId);
            datePicker.datepicker({
                dateFormat: crmConstants.Enums.FormatConstant.M_DD_YY_STRING,
                changeMonth: true,
                changeYear: true,
                minDate: options.MinDate ? new Date(options.MinDate) : null,
                maxDate: options.MaxDate ? new Date(options.MaxDate) : null,
                showOtherMonths: true,
                selectOtherMonths: true,
                currentDate: options.InitDate ? new Date(options.InitDate) : null,
                onSelect: function () {
                    isOnSelect = true;
                    $(this).change();
                    isOnSelect = false;
                },
                onClose: function () {
                    $(this).blur();
                }
            });

            oldValue = new Date(options.InitDate);
            element.on('change', function () {
                onChangeDateEvent();

            });
        }

        function onChangeDateEvent() {
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

        function setLimitDate(scope, selectedDate, options) {
            if (options.MinDate &&
                selectedDate < options.MinDate) {
                selectedDate = options.MinDate;
                datePicker.datepicker("option", "minDate", new Date(options.MinDate));
            }

            if (options.MaxDate &&
                options.MaxDate < selectedDate) {
                selectedDate = options.MaxDate;
                datePicker.datepicker("option", "maxDate", new Date(options.MaxDate));
            }
        }

        function selectedDateChange(scope, element, attrs) {
            /* jshint unused: vars */
            var datePicker = element.find(registerParams.PickerId);
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

        function ngModelChange(newVal, oldVal) {
            /* jshint unused: vars */
            var options = scope[registerParams.OptionName];
            var datePicker = element.find(registerParams.PickerId);
            if (newval && !oldval) {
                if (!isRenderedCalendar) {
                    renderDatePicker(scope, element, attrs);
                } else {
                    oldMaxValue = new Date(newval);
                    setLimitDate(scope, selectedDate, options);
                }

                scope.IsRerenderPicker = false;
            }
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    ngModelChange(newval, oldval);
                });
            }
        };
    });
};

