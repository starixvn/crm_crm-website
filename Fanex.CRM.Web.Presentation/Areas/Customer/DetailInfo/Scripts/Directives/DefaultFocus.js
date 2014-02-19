"use strict";
/* global customerDetailApplication */
customerDetailApplication.directive('defaultFocus', function () {
    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngIssetfocus, function (newval, oldval) {
                if (newval && !oldval) {
                    /* jshint unused: vars */
                    element.focus();
                    scope.completeSetFocus();
                }
            });
        }
    };
});