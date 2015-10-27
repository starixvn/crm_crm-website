(function () {
    "use strict";
    /* jshint unused: vars */
    angular.module('redemption-editlog-application').directive('numberOnlyInput', function ($filter) {
        return {
            restrict: 'A',
            template: '<input type="text" id="{{inputId}}" class="{{inputClass}}" ng-model="inputValue" />',
            scope: {
                inputValue: '=',
                inputId: '@',
                inputClass: '@'
            },
            link: function (scope, element, attrs) {
                element.keydown(function (e) {
                    var code = e.keyCode;
                    // Allow: backspace, delete, tab, escape, enter and '.'
                    // Allow: Ctrl + A
                    // Allow: home, end, left, right
                    if ($.inArray(code, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                    (code === 65 && e.ctrlKey === true) ||
                    (code >= 35 && code <= 39)) {
                        return;
                    }

                    // Ensure that it is a number and stop the keypress
                    if ((e.shiftKey || (code < 48 || code > 57)) && (code < 96 || code > 105)) {
                        e.preventDefault();
                    }
                });

                scope.$watch('inputValue', function (newValue, oldValue) {
                    scope.inputValue = scope.inputValue.toString().replace(/,/g, '').replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                });
            }
        };
    });
})();