(function () {
    "use strict";
    angular.module('redemption-editlog-application').directive('buttonSave', function () {
        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element) {
                /* jshint unused: false */
                element.on('click', function (e) {
                    scope.updateLog();
                });
            }
        };
    });
})();