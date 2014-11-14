"use strict";
/* global dashboardApplication,
sharedScriptManager */
/* jshint unused: vars */
dashboardApplication.directive("rawHtmlDirective", function () {
    return {
        restrict: "C",
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngText, function (newVal, oldVal) {
                element.html(sharedScriptManager.decodeHtml(newVal));
            });
        }
    };
});