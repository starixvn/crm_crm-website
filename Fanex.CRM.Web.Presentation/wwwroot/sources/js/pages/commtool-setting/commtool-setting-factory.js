(function (angular) {
    'use strict';
    /* global sharedScriptManager */

    angular.module('commToolSettingModule').factory('commToolSettingFactory', commToolSettingFactory);
    commToolSettingFactory.$inject = ['$http', '$q'];

    function commToolSettingFactory($http, $q) {
        function getAllSettings() {
            var url = window.rootUrl + "communicationSetting/getAllSettings";
            return sharedScriptManager.registerAngularService($http, $q, url, {});
        }

        function getCommToolSettings(params) {
            const url = window.rootUrl + 'communicationSetting/getCommunicationToolSetting';
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        function verifyCustomers(customers) {
            const url = window.rootUrl + 'communicationSetting/verifyUsername';
            return sharedScriptManager.registerAngularService($http, $q, url, { username: customers });
        }

        function createCommToolSetting(params) {
            const url = window.rootUrl + 'communicationSetting/createCommunicationToolSetting';
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        return {
            getAllSettings: getAllSettings,
            getCommToolSettings: getCommToolSettings,
            verifyCustomers: verifyCustomers,
            createCommToolSetting: createCommToolSetting
        };
    }
})(angular);