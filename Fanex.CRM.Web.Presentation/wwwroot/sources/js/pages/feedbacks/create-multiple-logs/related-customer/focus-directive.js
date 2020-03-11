(function (angular) {
    'use strict';

    angular.module('createMultipleLogsModule').directive('focusMe', function ($timeout, $parse) {
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
    });
})(angular);