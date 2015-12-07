(function () {
    'use strict';
    /* global casinoDailyApplication */
    angular.module('log-statistic-application').factory('LogStatisticService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getAllSetting = function (params) {
            var url = window.rootUrl + "LogStatistic/GetAllSettings";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        serviceInstance.getMainReport = function (params) {
            var url = window.rootUrl + "LogStatistic/GetMainReport";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        serviceInstance.getDetailReport = function (params) {
            var url = window.rootUrl + "LogStatistic/GetDetailReport";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        return serviceInstance;
    } ]);
})();