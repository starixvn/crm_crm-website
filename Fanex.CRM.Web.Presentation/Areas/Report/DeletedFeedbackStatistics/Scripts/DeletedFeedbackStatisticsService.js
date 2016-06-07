/*global sharedScriptManager*/
(function () {
    "use strict";
    angular.module('deleted-feedback-statistics-application')
    .factory('DeletedFeedbackStatisticsService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getReport = function (params) {
            var url = window.rootUrl + "DeletedFeedbackStatistics/GetReport";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    }])
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }]);
})();