"use strict";
/* global directiveManager: true */
directiveManager = directiveManager || {};
directiveManager.registerDefaultFocus = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    if (newval && !oldval) {
                        var picker = registerParams.PickerId ?
                             element.find("#" + registerParams.PickerId) :
                             element;
                        picker.focus();

                        var options = scope[registerParams.OptionName];
                        if (options === null ||
                            options === undefined) {
                            return;
                        }

                        if (options.OnChangeEvent &&
                            typeof (options.OnChangeEvent) === "function") {
                            options.OnChangeEvent();
                        }
                    }
                });
            }
        };
    });
};