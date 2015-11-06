(function () {
    "use strict";
    /* global sharedScriptManager */
    /* jshint camelcase: false,
    unused: vars */
    angular.module('addmintool-settingmanagement-editsetting-application').directive('keyPressDirective', function () {
        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element) {
                //jshint unused:false             
                element.unbind("keypress");
                element.keypress(function (e) {
                    return sharedScriptManager.forceNumberInput(e);
                });
            }
        };
    });
})();