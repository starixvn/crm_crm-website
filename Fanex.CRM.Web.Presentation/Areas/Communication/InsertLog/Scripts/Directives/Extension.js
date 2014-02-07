"use strict";
/* global insertLogApplication */
 insertLogApplication.directive('dropdownExtension', function () {
    return {
        restrict: 'A',
        template: '<option data-ng-repeat="extention in extensionList track by $index"' +
            ' data-ng-click="updateSelectedExtension(extention);"' +
            ' value="{{ extention.Id }}">{{ extention.Value }}</option>',
        replace: false,
        priority: 0,
        transclude: false,
        scope: true,
        terminal: false,
        require: false,
        compile: function compile(tElement, tAttrs, transclude) {
            /* jshint unused: vars */
        },
        link: function (scope, elem, attrs) {
            /* jshint unused: vars */
        }
    };
});