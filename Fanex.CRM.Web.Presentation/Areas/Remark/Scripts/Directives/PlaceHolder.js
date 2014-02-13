"use strict";
/* global remarkApplication */
remarkApplication.directive('customPlaceHolder', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            // get Function of onEnterKeyup attribute
            var placeholder = attrs.defaulttext;

            element.bind('focus', function () {
                var text = element.val();
                if (text === placeholder) {
                    element.val('');
                }
            });

            element.bind('blur', function () {
                var text = element.val();
                if (text === '' || text === placeholder) {
                    element.val(placeholder);
                }
            });

            angular.element(document).ready(function () {
                element.val(placeholder);
            });
        }
    };
});