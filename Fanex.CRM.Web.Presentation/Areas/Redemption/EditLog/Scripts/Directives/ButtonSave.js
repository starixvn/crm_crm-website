"use strict";
/* global editLogApplication */
editLogApplication.directive('buttonSave', function () {
    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            /* jshint unused: vars */
            element.on('click', function (e) {
                scope.updateLog();
            });
        }
    };
});