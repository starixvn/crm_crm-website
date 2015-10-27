(function () {
    "use strict";
    /* jshint unused: vars */
    angular.module('feedback-manage-application').directive('numberOnlyInput', function () {
        return {
            restrict: 'A',
            template: '<input type="text" id="{{inputId}}" class="{{inputClass}}" ng-model="inputValue" />',
            scope: {
                inputValue: '=',
                inputId: '@',
                inputClass: '@'
            },
            link: function (scope, element, attrs) {
                scope.$watch('inputValue', function (newValue, oldValue) {
                    var arr = String(newValue).split('');
                    var length = arr.length;
                    if (length === 0) { return; }
                    if (length === 1 && (arr[0] === '-' || arr[0] === '.')) { return; }
                    if (length === 2 && newValue === '-.') { return; }
                    if (isNaN(newValue)) {
                        scope.inputValue = oldValue;
                    }
                });
            }
        };
    });
})();