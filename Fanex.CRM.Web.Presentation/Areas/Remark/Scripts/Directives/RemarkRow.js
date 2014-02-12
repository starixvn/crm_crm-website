"use strict";
/* global remarkApplication */
remarkApplication.directive('remarkRow', function () {
    return {
        restrict: 'E',
        scope: true,
        template: '<li class=""><input type="text" class="textbox" data-ng-model="" /></li>',
        link: function (scope, element, attrs, controller) {
        }
    };
});