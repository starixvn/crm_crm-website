"use strict";
/* global editLogApplication */ 
editLogApplication.directive('dropdownCommunicationTool', function () {
    return {
        restrict: 'A',
        template: '<option data-ng-repeat="communicationTool in communicationToolList track by $index"' +
                        ' data-ng-click="updateSelectedCommunicationTool(communicationTool);"' +
                        ' value="{{ communicationTool.Id }}"' +
                        ' data-ng-selected="communicationTool.Id === communicationLog.CommunicationTool.Id">{{ communicationTool.Name }}</option>',
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