"use strict";
/* global directiveManager: true */
/* jshint unused: false */
directiveManager = directiveManager || {};
directiveManager.registerToggleElement = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        var cachedData = {
            Options: null,
            Picker: null
        };

        function bindingEvent(scope, element, attrs) {
            var options = scope[registerParams.OptionName];
            cachedData.Options = options;
            cachedData.Picker = registerParams.PickerId ?
                element.find("#" + registerParams.PickerId) :
                element;
            if (options === null) {
                return;
            }

            showHideEvent();
            if (cachedData.Options.OnChangeEvent &&
                typeof (cachedData.Options.OnChangeEvent) === "function") {
                cachedData.Picker.click(function () {
                    cachedData.Options.OnChangeEvent();
                    showHideEvent();
                });
            }
        }

        function showHideEvent() {
            if (cachedData.Options === null) {
                return;
            }

            if (cachedData.Options.IsHideBorderBottom) {
                if (cachedData.Options.IsShowTargetElement) {
                    cachedData.Picker.removeClass("border-bottom-none");
                } else {
                    cachedData.Picker.addClass("border-bottom-none");
                }
            }

            var activeElement = $(cachedData.Options.ActiveElement);
            var disableElement = $(cachedData.Options.DisableElement);
            if (cachedData.Options.IsCollapse) {
                disableElement.addClass("display-none");
                activeElement.removeClass("display-none");
            } else {
                activeElement.addClass("display-none");
                disableElement.removeClass("display-none");
            }
        }

        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                bindingEvent(scope, element, attrs);

                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    if (cachedData.Options === null) {
                        bindingEvent(scope, element, attrs);
                    }

                    if (cachedData.Options === null) {
                        return;
                    }

                    showHideEvent();
                    cachedData.Options.IsUpdate = false;
                    scope.$apply();
                });
            }
        };
    });
};