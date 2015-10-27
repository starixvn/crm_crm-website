(function () {
    "use strict";
    angular.module('redemption-viewlog-application').directive('defaultFocus', function () {
        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    if (newval && !oldval) {
                        /* jshint unused: vars */
                        element.focus();
                        scope.completeSetFocus();
                    }
                });
            }
        };
    });
})();