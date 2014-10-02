'use strict';
/* global viewListApplication,
sharedScriptManager */
viewListApplication.factory('viewListService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getMainSetting = function (params) {
            var url = window.rootUrl + "SettingManagement/GetMainSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.getSettingRelations = function (params) {
            var url = window.rootUrl + "SettingManagement/SettingByType";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
