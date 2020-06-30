(function (angular) {
    'use strict';

    angular.module('focusMeModule', []).directive('focusMe', focusMeDirective);
    focusMeDirective.$inject = ['$timeout'];

    function focusMeDirective($timeout) {
        return {
            scope: {
                trigger: '=focusMe',
                onBlur: '='
            },
            link: function (scope, element) {
                scope.$watch('trigger', function (value) {
                    if (value === true) {
                        $timeout(function () {
                            element[0].focus();
                            scope.trigger = false;
                        });
                    }
                });

                element.bind('blur', function () {
                    scope.onBlur();
                });
            }
        };
    }
})(angular);