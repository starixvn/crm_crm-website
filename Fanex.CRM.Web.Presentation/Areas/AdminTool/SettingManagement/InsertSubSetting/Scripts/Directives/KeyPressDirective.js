"use strict";
/* global insertSubSettingApplication,
sharedScriptManager */
/* jshint camelcase: false,
unused: vars */
insertSubSettingApplication.directive('keyPressDirective', function () {
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