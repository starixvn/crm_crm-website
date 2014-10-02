"use strict";
/* global insertSettingApplication,
sharedScriptManager */
/* jshint camelcase: false,
unused: vars */
insertSettingApplication.directive('keyPressDirective', function () {
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