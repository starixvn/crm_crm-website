(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('report-dailystatistics-application').factory('dailyStatisticsReportService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getDailyStatisticsReport = function (params) {
            var url = window.rootUrl + "DailyStatisticsReport/MainReport";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.getDailyStatisticsDetailReport = function (params) {
            var url = window.rootUrl + "DailyStatisticsReport/DetailReport";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();