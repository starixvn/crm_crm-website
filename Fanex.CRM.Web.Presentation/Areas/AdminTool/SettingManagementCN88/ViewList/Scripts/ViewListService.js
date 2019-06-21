(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('addmintool-settingmanagement-application').factory('viewListService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getMainSetting = function (params) {
            var url = window.rootUrl + "SettingManagementCN88/MainSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.getSettingRelations = function (params) {
            var url = window.rootUrl + "SettingManagementCN88/SettingByType";
            return sharedScriptManager.registerAngularService($http, $q, url,
                {SettingName: params.MainSetting.SettingType});
        };

        return serviceInstance;
    } ]);
})();
