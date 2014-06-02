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
            var isAlt = false;
            var ctrlKeyCode = keyboardManager.Enums.KeyCode.Ctrl,
                enterKeyCode = keyboardManager.Enums.KeyCode.Enter;
            element.bind('keyup', function (event) {
                if (event.which === ctrlKeyCode) {
                    isAlt = false;
                }
            })
            .bind('keydown', function (event) {
                var keyCode = event.which;
                if (keyCode === ctrlKeyCode) {
                    isAlt = true;
                }
                else if (keyCode === enterKeyCode && isAlt) {
                    scope.$apply(function () {
                        scope.onEnterKeyup();
                    });
                }
            });
        }
    };
});