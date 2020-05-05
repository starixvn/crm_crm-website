(function (angular) {
    "use strict";
/* global sharedScriptManager */
    angular.module('addmintool-settingmanagement-application').factory('admintoolSettingFactory', admintoolSettingFactory);
    admintoolSettingFactory.$inject = ['$http', '$q'];

    function admintoolSettingFactory($http, $q) {
        function getMainSetting(params) {
            var url = window.rootUrl + "SettingManagement/MainSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        function getSettingRelations(params) {
            var url = window.rootUrl + "SettingManagement/SettingByType";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        function getSourceSettingMapping(params) {
            var url = window.rootUrl + "Setting/GetSourceSettingMappings";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return {
            getMainSetting: getMainSetting,
            getSettingRelations: getSettingRelations,
            getSourceSettingMapping: getSourceSettingMapping
        };
    }
})(angular);
