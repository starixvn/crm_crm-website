(function () {
    "use strict";
    /* global sharedScriptManager */
    /* jshint unused: vars */
    angular.module('dashboard-application').directive("rawHtmlDirective", function () {
        return {
            restrict: "C",
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngText, function (newVal, oldVal) {
                    /* jshint unused: false */
                    element.html(sharedScriptManager.decodeHtml(newVal));
                });
            }
        };
    });
})();