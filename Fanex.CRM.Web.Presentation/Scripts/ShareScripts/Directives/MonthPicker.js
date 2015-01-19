"use strict";
/* global directiveManager: true,
crmConstants */
/* jshint unused: vars */
directiveManager = directiveManager || {};
directiveManager.registerMonthPickerContainer = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        //    var defaultRegisterParams = {
        //            PlugInName: null,
        //            OptionName: null,
        //            PickerId: null
        //        };
        //    var defaultOption = {
        //            MinDate: null,
        //            MaxDate: null,
        //            InitDate: null,
        //            IsRerenderPicker: null,
        //            OnChangeEvent: null
        //        };

        var isRenderedPicker = false;
        var cachedData = {
            yearDropdown: null,
            monthDropdown: null
        };

        function getYearOptions(options) {
            var date = new Date(options.MaxDate);
            var minYear = options.MinYear;
            if (minYear == null) {
                minYear = new Date(options.MinDate).getFullYear();
            }

            var maxYear = options.MaxYear;
            if (maxYear == null) {
                maxYear = date.getFullYear();
            }

            var currentYear = parseInt(options.SelectedYear);
            if (currentYear == null) {
                currentYear = maxYear;
            }

            var yearOptions = '';
            for (var i = minYear; i <= maxYear; i++) {
                if (i === currentYear) {
                    yearOptions += '<option selected="selected" value="' + i + '">' + i + '</option>';
                }
                else {
                    yearOptions += '<option value="' + i + '">' + i + '</option>';
                }
            }

            return yearOptions;
        }

        function getMonthOptions(options) {
            var date = new Date(options.MaxDate);
            var currentMonth = date.getMonth() + 1;
            var currentYear = date.getFullYear();
            if (options.SelectedYear < currentYear && options.selectedMonth) {
                currentMonth = options.SelectedMonth;
            }

            var year = parseInt(options.SelectedYear);
            if (options.MinMonth === null ||
                options.MinMonth === undefined) {
                if (options.MinDate &&
                    options.MinDate.getFullYear() === options.SelectedYear) {
                    options.MinMonth = options.MinDate.getMonth() + 1;
                } else {
                    options.MinMonth = 1;
                }
            }

            if (options.MaxMonth === null ||
                options.MaxMonth === undefined) {
                options.MaxMonth = currentMonth;
                if (options.SelectedYear && year !== currentYear) {
                    options.MaxMonth = 12;
                }
            }

            var monthOptions = '';
            for (var i = options.MinMonth - 1; i <= options.MaxMonth - 1; i++) {
                if (i === (options.SelectedMonth ? options.SelectedMonth : currentMonth) - 1) {
                    monthOptions += '<option selected="selected" value="' + (i + 1) + '">' +
                        crmConstants.MonthNames[i] + '</option>';
                }
                else {
                    monthOptions += '<option value="' + (i + 1) + '">' +
                        crmConstants.MonthNames[i] + '</option>';
                }
            }

            return monthOptions;
        }

        function ngModelChange(scope, element, attrs, newVal, oldVal) {
            var options = scope[registerParams.OptionName];
            if (options === null) {
                return;
            }

            var yearOptions = getYearOptions({
                MaxDate: options.MaxDate,
                MinDate: options.MinDate,
                SelectedYear: options.SelectedYear
            });

            var yearDropdown = options.IsOutScopePicker ?
                $("#" + registerParams.PickerId.Year) :
                element.find("#" + registerParams.PickerId.Year);
            cachedData.yearDropdown = yearDropdown;
            yearDropdown.empty().append(yearOptions);

            var monthDropdown = options.IsOutScopePicker ?
                $("#" + registerParams.PickerId.Month) :
                element.find("#" + registerParams.PickerId.Month);
            cachedData.monthDropdown = monthDropdown;
            var monthOptions = getMonthOptions({
                MaxDate: options.MaxDate,
                MinDate: options.MinDate,
                SelectedYear: parseInt(yearDropdown.val()),
                SelectedMonth: options.SelectedMonth,
                MaxMonth: options.MaxMonth
            });
            monthDropdown.empty().append(monthOptions);

            if (!isRenderedPicker) {
                monthDropdown.unbind("change");
                monthDropdown.change(function () {
                    onChangeEvent(scope, monthDropdown.val(), yearDropdown.val());
                    ngModelChange(scope, element, attrs, newVal, oldVal);
                });

                yearDropdown.unbind("change");
                yearDropdown.change(function () {
                    onChangeEvent(scope, 1/*parseInt(monthDropdown.val())*/, parseInt(yearDropdown.val()));
                    ngModelChange(scope, element, attrs, newVal, oldVal);
                });
            }

            isRenderedPicker = true;
            options.IsRerenderPicker = false;
            updateInitValue(scope, element, attrs);
        }

        function onChangeEvent(scope, month, year, isAlreadyApply) {
            var options = scope[registerParams.OptionName];
            if (options === null) {
                return;
            }

            if (options.OnChangeEvent &&
                typeof (options.OnChangeEvent) === "function") {
                options.OnChangeEvent(month, year, isAlreadyApply);
            }
        }

        function updateInitValue(scope, element, attrs) {
            var month = parseInt(cachedData.monthDropdown.val());
            var year = parseInt(cachedData.yearDropdown.val());
            onChangeEvent(scope, month, year, true);
        }

        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newVal, oldVal) {
                    ngModelChange(scope, element, attrs, newVal, oldVal);
                }, true);
            }
        };
    });
};