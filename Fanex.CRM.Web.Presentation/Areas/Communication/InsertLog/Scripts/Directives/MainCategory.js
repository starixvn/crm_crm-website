"use strict";
/* global insertLogApplication */
insertLogApplication.directive('dropdownMainCategory', function () {
    return {
        restrict: 'A',
        template: '<option data-ng-repeat="mainCategory in mainCategoryList track by $index"' +
                        ' data-ng-click="updateSelectedMainCategory(mainCategory);"' +
                        ' value="{{ mainCategory.Id }}">{{ mainCategory.Name }}</option>',
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