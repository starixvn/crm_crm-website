'use strict';
/* global insertSettingApplication,
sharedScriptManager */
insertSettingApplication.factory('insertSettingService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getParentSetting = function (params) {
            var url = window.rootUrl + "SettingManagement/GetParentSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.insertSetting = function (params) {
            var url = window.rootUrl + "SettingManagement/InsertSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
