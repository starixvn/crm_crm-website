'use strict';
/* global casinoDailyApplication */
casinoDailyEnhancementApplication.factory('CasinoDailyEnhancementService', ['$http', '$q', function ($http, $q) {
    var serviceInstance = {};

    serviceInstance.getAllSetting = function (params) {
        var url = window.rootUrl + "CasinoReportEnhancement/GetAllSettings";
        return sharedScriptManager.registerAngularService($http, $q, url, params);
    }

    return serviceInstance;
} ]);