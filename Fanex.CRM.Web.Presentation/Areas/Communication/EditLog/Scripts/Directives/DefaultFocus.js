(function () {
    "use strict";
    angular.module('communicationlog-edit-application').directive('defaultFocus', function () {
        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    if (newval && !oldval) {
                        element.focus();
                        scope.completeSetFocus();
                    }
                });
            }
        };
    });
})();