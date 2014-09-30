'use strict';
/* global viewListApplication,
sharedScriptManager */
viewListApplication.factory('viewListService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getMainSetting = function (params) {
            var url = window.rootUrl + "SettingRelation/GetMainSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.getSettingRelations = function (params) {
            var url = window.rootUrl + "SettingRelation/SettingByType";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
