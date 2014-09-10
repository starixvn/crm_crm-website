"use strict";
/* global directiveManager: true,
sharedScriptManager */
/* jshint unused: vars */
var directiveManager = directiveManager || {};
directiveManager.registerRawHtmlDirective = function (angularApplication, registerParams) {
    angularApplication.directive("rawHtmlDirective", function () {
        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newVal, oldVal) {
                    element.html(newVal);
                });
            }
        };
    })
};