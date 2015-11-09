"use strict";
/* global directiveManager: true */
/* jshint unused: false */
directiveManager = directiveManager || {};
directiveManager.registerYearPickerContainer = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        var isRenderedPicker = false;

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

            var currentYear = options.SelectedYear;
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

            var yearDropdown = element.find("#" + registerParams.PickerId);
            yearDropdown.empty().append(yearOptions);

            if (!isRenderedPicker) {
                yearDropdown.unbind("change");
                yearDropdown.change(function () {
                    onChangeEvent(scope, parseInt(yearDropdown.val()));
                    ngModelChange(scope, element, attrs, newVal, oldVal);
                });
            }

            isRenderedPicker = true;
            options.IsRerenderPicker = false;
            updateInitValue(scope, element, attrs);
        }

        function onChangeEvent(scope, year, isAlreadyApply) {
            var options = scope[registerParams.OptionName];
            if (options === null) {
                return;
            }

            if (options.OnChangeEvent &&
                typeof (options.OnChangeEvent) === "function") {
                options.OnChangeEvent(year, isAlreadyApply);
            }
        }

        function updateInitValue(scope, element, attrs) {
            var year = parseInt(element.find("#" + registerParams.PickerId).val());
            onChangeEvent(scope, year, true);
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