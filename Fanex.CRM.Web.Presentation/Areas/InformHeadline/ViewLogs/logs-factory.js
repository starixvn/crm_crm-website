(function (angular) {
    'use strict';

    angular.module('informHeadlineApplication').factory('logsFactory', logsFactory);
    logsFactory.$inject = ['$http', '$q'];

    function logsFactory($http, $q) {
        function getRemovedAccountLogs(params) {
            var url = window.rootUrl + 'InformHeadline/GetRemovedAccountLogs';
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        return {
            getRemovedAccountLogs: getRemovedAccountLogs
        };
    }
})(angular);