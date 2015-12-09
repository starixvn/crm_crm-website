(function () {
    'use strict';
    /* global casinoDailyApplication */
    angular.module('log-statistics-application').factory('LogStatisticsService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getAllSetting = function (params) {
            var url = window.rootUrl + "LogStatistics/GetAllSettings";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        serviceInstance.getMainReport = function (params) {
            var url = window.rootUrl + "LogStatistics/GetMainReport";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        serviceInstance.getDetailReport = function (params) {
            var url = window.rootUrl + "LogStatistics/GetReportDetail";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        return serviceInstance;
    } ]);
})();