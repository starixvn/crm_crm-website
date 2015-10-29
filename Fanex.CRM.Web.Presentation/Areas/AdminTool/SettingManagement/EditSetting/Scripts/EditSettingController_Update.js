(function () {
    "use strict";
    /* jshint unused: vars,
    camelcase: false */
    /* exported registerUpdateFunctions */
    window.registerUpdateFunctions = function ($scope) {
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };
    }
})();