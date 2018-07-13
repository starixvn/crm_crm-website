"use strict";
/* global directiveManager: true */
/* jshint unused: false */
directiveManager = directiveManager || {};
directiveManager.registerWeekPickerContainer = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        var selfScope;
        var isAlreadyApply = false;
        function ngModelChange(scope, element, attrs, newVal, oldVal) {
            var options = scope[registerParams.OptionName];
            selfScope = scope;
            var picker = element.find("#" + registerParams.PickerId);            
            picker.weekpicker('onSelect', onChangeDateEvent);
            setLimitDate(scope, options, picker);
            isAlreadyApply = false;
        }

        function onChangeDateEvent(fromDate, toDate, picker) {
            var options = selfScope[registerParams.OptionName];
            if (options.OnChangeDateEvent &&
                typeof (options.OnChangeDateEvent) === "function") {
                options.OnChangeDateEvent(fromDate, toDate, picker, isAlreadyApply);
            }
        }

        function setLimitDate(scope, options, picker) {
            if (options && options.MinDate) {
                picker.updateWeekPicker("minDate", new Date(options.MinDate))
            }

            if (options && options.MaxDate) {
                picker.updateWeekPicker("maxDate", new Date(options.MaxDate))
            }

            if (options && options.CurrentDate) {
                picker.setWeekPicker(new Date(options.CurrentDate));                
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
