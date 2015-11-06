(function () {
    "use strict";
    /* jshint unused: vars */
    angular.module('feedback-statistic-application').directive('numberOnlyInput', function () {
        return {
            restrict: 'A',
            template: '<input type="text" class="{{inputClass}}" ng-model="inputValue" /> {{inputLabel}}',
            scope: {
                inputValue: '=',
                inputClass: '@',
                inputLabel: '@'
            },
            link: function (scope, element) {
                //jshint unused:false
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
})();