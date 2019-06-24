(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('addmintool-settingrelation-addrelation-application').factory('addRelationService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getMainSettingForAddRelation = function (params) {
            var url = window.rootUrl + "SettingRelationCN88/GetMainSettingForAddRelation";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.getMainSettingRelations = function (params) {
            var url = window.rootUrl + "SettingRelationCN88/MainSettingRelations";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.addSettingRelations = function (params) {
            var url = window.rootUrl + "SettingRelationCN88/AddSettingRelations";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();
