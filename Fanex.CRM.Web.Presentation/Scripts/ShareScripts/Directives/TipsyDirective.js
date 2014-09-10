"use strict";
/* global directiveManager: true,
sharedScriptManager */
/* jshint unused: vars */
var directiveManager = directiveManager || {};
directiveManager.registerTipsyDirective = function (angularApplication, registerParams) {
    function isOptionReady(options) {
        return options !== null &&
            options !== undefined;
    }

    angularApplication.directive("tipsyDirective", function () {
        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newVal, oldVal) {
                    var options = scope[registerParams.OptionName];
                    if (!isOptionReady(options)) {
                        return;
                    }

                    element.tipsy({
                        gravity: options.Gravity,
                        title: function () {
                            return options.Title;
                        }
                    });
                });
            }
        };
    })
};