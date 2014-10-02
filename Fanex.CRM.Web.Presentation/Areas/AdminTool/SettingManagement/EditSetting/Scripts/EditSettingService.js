'use strict';
/* global editSettingApplication,
sharedScriptManager */
editSettingApplication.factory('editSettingService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.editSetting = function (params) {
            var url = window.rootUrl + "SettingManagement/EditSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
