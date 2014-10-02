'use strict';
/* global deleteSettingApplication,
sharedScriptManager */
deleteSettingApplication.factory('deleteSettingService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.deleteSetting = function (params) {
            var url = window.rootUrl + "SettingManagement/DeleteSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
