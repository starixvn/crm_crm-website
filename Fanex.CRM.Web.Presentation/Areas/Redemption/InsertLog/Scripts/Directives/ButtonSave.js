(function () {
    "use strict";
    angular.module('redemption-insertlog-application').directive('buttonSave', function () {
        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element) {
                /* jshint unused: vars */
                element.on('click', function (e) {
                    scope.insertLog();
                });
            }
        };
    });
})();