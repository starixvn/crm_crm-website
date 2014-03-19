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
        var isAlreadyApply = false;
        function ngModelChange(scope, element, attrs, newVal, oldVal) {
            var options = scope[registerParams.OptionName];
            selfScope = scope;
            var picker = element.find("#" + registerParams.PickerId);
            picker.on('change', function () {
                picker.weekpicker('onSelect', onChangeDateEvent);
            });

            picker.weekpicker('onSelect', onChangeDateEvent);
            //            isAlreadyApply = true;
            //            element.find("#" + registerParams.PickerId).weekpicker('updateInitDate', null);
            //            isAlreadyApply = false;
        }

        function onChangeDateEvent(fromDate, toDate, picker) {
            var options = selfScope[registerParams.OptionName];
            if (options.OnChangeDateEvent &&
                typeof (options.OnChangeDateEvent) === "function") {
                options.OnChangeDateEvent(fromDate, toDate, picker, isAlreadyApply);
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
