"use strict";
/* global editSettingApplication,
sharedScriptManager */
/* jshint camelcase: false,
unused: vars */
editSettingApplication.directive('keyPressDirective', function () {
    return {
        restrict: 'C',
        scope: true,
        link: function (scope, element, attrs) {
            element.unbind("keypress");
            element.keypress(function (e) {
                return sharedScriptManager.forceNumberInput(e);
            });
        }
    };
});