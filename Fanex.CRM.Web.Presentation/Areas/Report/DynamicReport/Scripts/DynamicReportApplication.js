(function () {
    "use strict";
    angular.module('report-dynamicreport-application', [])
        .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }]);
})();