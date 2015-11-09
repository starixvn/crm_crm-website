(function () {
    "use strict";
    /* exported registerUpdateFunctions */
    window.registerUpdateFunctions = function ($scope) {
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };
    };
})();