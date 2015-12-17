(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('newInformApplication').factory('newInformService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getPositions = function () {
            return [
                { "Id": 4, "Name": "Super" },
                { "Id": 3, "Name": "Master" },
                { "Id": 2, "Name": "Agent" },
                { "Id": 1, "Name": "Member" }
            ];
        };

        serviceInstance.getInforms = function (params) {
            var url = window.rootUrl + "InformHeadline/GetNewInforms";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        serviceInstance.getAllSettings = function () {
            var url = window.rootUrl + 'InformHeadline/AllSettings';
            return sharedScriptManager.registerAngularService($http, $q, url);
        };

        serviceInstance.insertInforms = function (params) {
            var url = window.rootUrl + 'InformHeadline/InsertInforms';
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();
