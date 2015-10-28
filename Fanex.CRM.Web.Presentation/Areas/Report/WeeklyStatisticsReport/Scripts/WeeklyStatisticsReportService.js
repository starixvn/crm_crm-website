(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('report-weeklystatistics-application').factory('weeklyStatisticsReportService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getWeeklyStatisticsReport = function (params) {
            var url = window.rootUrl + "WeeklyStatisticsReport/MainReport";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.getWeeklyStatisticsDetailReport = function (params) {
            var url = window.rootUrl + "WeeklyStatisticsReport/DetailReport";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();