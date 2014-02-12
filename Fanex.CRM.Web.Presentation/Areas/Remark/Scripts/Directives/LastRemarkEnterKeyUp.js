"use strict";
/* global remarkApplication */
remarkApplication.directive('onEnterKeyup', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            // get Function of onEnterKeyup attribute
            var keyupFn = scope.$eval(attrs.onEnterKeyup);

            element.bind('keyup', function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        keyupFn.call(scope);
                    });
                }
            });
        }
    };
});