"use strict";
/* global insertLogApplication */
insertLogApplication.directive('optionStatus', function () {
    return {
        restrict: 'A',
        scope: {
            status: '='
        },
        link: function (scope, element, attrs) {
            /* jshint unused: vars */
            element.on('click', function (e) {
                scope.$parent.updateSelectedStatus(scope.status);
            });
        }
    };
});