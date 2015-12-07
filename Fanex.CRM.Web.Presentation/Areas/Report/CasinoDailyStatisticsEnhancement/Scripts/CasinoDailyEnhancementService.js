(function () {
    'use strict';
    /* global casinoDailyApplication */
    angular.module('report-casinodaily-enhancement-application').factory('CasinoDailyEnhancementService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getAllSetting = function (params) {
            var url = window.rootUrl + "CasinoReportEnhancement/GetAllSettings";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        serviceInstance.getCasinoDailyEnhancementMainReport = function (params) {
            var url = window.rootUrl + "CasinoReportEnhancement/GetMainReport";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        return serviceInstance;
    } ]);
})();