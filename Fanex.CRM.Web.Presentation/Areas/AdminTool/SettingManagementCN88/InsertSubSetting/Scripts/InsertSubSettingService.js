(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('addmintool-settingmanagement-insertsubsetting-application').factory('insertSubSettingService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getParentSetting = function (params) {
            var url = window.rootUrl + "SettingManagementCN88/ParentSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.insertSubSetting = function (params) {
            var url = window.rootUrl + "SettingManagementCN88/InsertSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();
