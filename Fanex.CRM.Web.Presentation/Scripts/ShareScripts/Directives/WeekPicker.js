"use strict";
/* global directiveManager: true */
/* jshint unused: vars */
directiveManager = directiveManager || {};
directiveManager.registerWeekPickerContainer = function (angularApplication, registerParams) {
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

        var selfScope;
        function ngModelChange(scope, element, attrs, newVal, oldVal) {
            var options = scope[registerParams.OptionName];
            if (options === null) {
                return;
            }

            selfScope = scope;
            element.find("#" + registerParams.PickerId).weekpicker('onSelect', onChangeDateEvent);
        }

        function onChangeDateEvent(fromDate, toDate) {
            var options = selfScope[registerParams.OptionName];
            if (options.OnChangeDateEvent &&
                typeof (options.OnChangeDateEvent) === "function") {
                options.OnChangeDateEvent(fromDate, toDate);
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
