'use strict';
/* global insertSubSettingApplication,
sharedScriptManager */
insertSubSettingApplication.factory('insertSubSettingService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getParentSetting = function (params) {
            var url = window.rootUrl + "SettingManagement/ParentSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.insertSubSetting = function (params) {
            var url = window.rootUrl + "SettingManagement/InsertSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
