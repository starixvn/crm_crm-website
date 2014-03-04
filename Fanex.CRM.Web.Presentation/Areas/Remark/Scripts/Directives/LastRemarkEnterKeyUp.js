"use strict";
/* global remarkApplication */
remarkApplication.directive('onEnterKeyup', function () {
    return {
        restrict: 'A',
        scope: {
            onEnterKeyup: '&onEnterKeyup'
        },
        link: function (scope, element, attrs) {
            element.bind('keyup', function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.onEnterKeyup();
                    });
                }
            });
        }
    };
});