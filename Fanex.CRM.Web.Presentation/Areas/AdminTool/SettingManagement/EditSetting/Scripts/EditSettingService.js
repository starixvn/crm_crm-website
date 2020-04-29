(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('addmintool-settingmanagement-application').factory('editSettingService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.editSetting = function (params) {
            var url = window.rootUrl + "SettingManagement/EditSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.getSourceSettingMapping = function (params) {
            var url = window.rootUrl + "Setting/GetSourceSettingMappings";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        return serviceInstance;
    } ]);
})();
