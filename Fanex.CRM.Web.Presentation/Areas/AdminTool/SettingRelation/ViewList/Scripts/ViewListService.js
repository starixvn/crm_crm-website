(function () {
    "use strict";
    /* global
    sharedScriptManager */
    angular.module('addmintool-settingrelation-application').factory('viewListService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getMainSetting = function (params) {
            var url = window.rootUrl + "SettingRelation/MainSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.getSettingRelations = function (params) {
            var url = window.rootUrl + "SettingRelation/SettingByType";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();
