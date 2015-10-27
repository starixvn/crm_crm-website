(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('addmintool-settingmanagement-application').factory('viewListService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getMainSetting = function (params) {
            var url = window.rootUrl + "SettingManagement/MainSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.getSettingRelations = function (params) {
            var url = window.rootUrl + "SettingManagement/SettingByType";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();
