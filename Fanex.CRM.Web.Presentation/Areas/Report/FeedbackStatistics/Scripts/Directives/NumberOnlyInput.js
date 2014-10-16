'use strict';
/* global fbStatisticApplication */
/* jshint unused: vars */
fbStatisticApplication.directive('numberOnlyInput', function () {
    return {
        restrict: 'A',
        template: '<input type="text" class="{{inputClass}}" ng-model="inputValue" /> {{inputLabel}}',
        scope: {
            inputValue: '=',
            inputClass: '@',
            inputLabel: '@'
        },
        link: function (scope, element, attrs) {
            scope.$watch('inputValue', function (newValue, oldValue) {
                var arr = String(newValue).split('');
                var length = arr.length;
                if (length === 0) { return; }
                if (isNaN(newValue)) {
                    scope.inputValue = oldValue;
                }
            });
        }
    };
});