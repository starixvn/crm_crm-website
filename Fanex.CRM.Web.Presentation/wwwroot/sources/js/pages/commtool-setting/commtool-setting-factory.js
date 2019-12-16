(function (angular) {
    'use strict';
    /* global sharedScriptManager */

    angular.module('commToolSettingModule').factory('commToolSettingFactory', commToolSettingFactory);
    commToolSettingFactory.$inject = ['$http', '$q'];

    function commToolSettingFactory($http, $q) {
        function verifyUsername(username) {
            const url = window.rootUrl + 'customerSetting/verifyUsername';
            return sharedScriptManager.registerAngularService($http, $q, url, { username: username });
        }

        function getAllSettings() {
            var url = window.rootUrl + "communicationSetting/getAllSettings";
            return sharedScriptManager.registerAngularService($http, $q, url, {});
        }

        function getCommToolSettings(params) {
            const url = window.rootUrl + 'communicationSetting/getCommunicationToolSetting';
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        function getCommToolSettingById(id) {
            const url = window.rootUrl + 'communicationSetting/getById';
            return sharedScriptManager.registerAngularService($http, $q, url, { id: id });
        }

        function createCommToolSetting(params) {
            const url = window.rootUrl + 'communicationSetting/create';
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        function editCommToolSetting(params) {
            const url = window.rootUrl + 'communicationSetting/edit';
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        function deleteCommToolSetting(id) {
            const url = window.rootUrl + 'communicationSetting/delete';
            return sharedScriptManager.registerAngularService($http, $q, url, { id: id });
        }

        return {
            verifyUsername: verifyUsername,
            getAllSettings: getAllSettings,
            getCommToolSettings: getCommToolSettings,
            getCommToolSettingById: getCommToolSettingById,
            createCommToolSetting: createCommToolSetting,
            editCommToolSetting: editCommToolSetting,
            deleteCommToolSetting: deleteCommToolSetting
        };
    }
})(angular);