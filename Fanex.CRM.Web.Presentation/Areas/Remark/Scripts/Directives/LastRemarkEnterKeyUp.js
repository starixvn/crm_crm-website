"use strict";
/* global remarkApplication,
keyboardManager */
/* jshint unused: vars */
remarkApplication.directive('onEnterKeyup', function () {
    return {
        restrict: 'A',
        scope: {
            onEnterKeyup: '&onEnterKeyup'
        },
        link: function (scope, element, attrs) {
            element.bind('keyup', function (event) {
                if (event.which === keyboardManager.Enums.KeyCode.Enter) {
                    scope.$apply(function () {
                        scope.onEnterKeyup();
                    });
                }
            });
        }
    };
});