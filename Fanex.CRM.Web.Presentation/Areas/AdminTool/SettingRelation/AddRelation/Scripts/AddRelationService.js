(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('addmintool-settingrelation-addrelation-application').factory('addRelationService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getMainSettingForAddRelation = function (params) {
            var url = window.rootUrl + "SettingRelation/GetMainSettingForAddRelation";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.getMainSettingRelations = function (params) {
            var url = window.rootUrl + "SettingRelation/MainSettingRelations";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.addSettingRelations = function (params) {
            var url = window.rootUrl + "SettingRelation/AddSettingRelations";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();
