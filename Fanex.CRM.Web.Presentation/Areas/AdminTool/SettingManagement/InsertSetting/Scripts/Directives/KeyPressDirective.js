(function () {
    "use strict";
    /* global sharedScriptManager */
    /* jshint camelcase: false,
    unused: vars */
    angular.module('addmintool-settingmanagement-insertnewsetting-application').directive('keyPressDirective', function () {
        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element) {               
                element.unbind("keypress");
                element.keypress(function (e) {
                    return sharedScriptManager.forceNumberInput(e);
                });
            }
        };
    });
})();